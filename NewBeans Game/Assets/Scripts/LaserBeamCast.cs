using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCast : MonoBehaviour
{
    public LayerMask layersToHit;
    public bool hitDetect = false;
    public float maxDistance = 1000f;

    private RaycastHit[] hit;
    private Collider collider;
    
    public Transform laserStartPos;
    public Transform laserEndPos;

    public LineRenderer warningLine;
    public LineRenderer laserLine;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        warningLine = GetComponent<LineRenderer>();
        laserLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaserBeam();
    }

    public void LaserWarning()
    {

    }

    public void ShootLaserBeam()
    {
        Vector3 colliderScale = new Vector3(2f, 2f, 2f);
        Vector3 shootLaserDir = (laserStartPos.transform.position + transform.forward * maxDistance);

        float distanceDifference; // Distance between object that is hit and the laser start point.

        float closestRockDistance = 1000f;
        float playerDistance = 0f;

        bool rockExists = false;

        Debug.DrawRay(transform.position, shootLaserDir, Color.red);

        RaycastHit[] hits = Physics.BoxCastAll(collider.bounds.center, colliderScale, shootLaserDir, transform.rotation, maxDistance, layersToHit);

        foreach (RaycastHit hit in hits)
        {
            distanceDifference = (laserStartPos.transform.position - hit.collider.transform.position).magnitude;
            Debug.DrawRay(hit.point, hit.point + Vector3.up * 5, Color.green);
            hitDetect = true;

            print("Laser hit:" + hit.collider.name);

            // ----------- If hit rock
            if (hit.collider.tag == "Rock")
            {
                rockExists = true;
                Vector3 nearestRock = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
                closestRockDistance = Vector3.Distance(laserStartPos.transform.position, nearestRock);

            }
            // ---------- If hit player
            if (hit.collider.tag == "Player")
            {
                playerDistance = distanceDifference;

                if ((rockExists == true) && (playerDistance <= closestRockDistance))
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().Hit();
                }

                else if (rockExists == false)
                {
                    hit.collider.gameObject.GetComponent<PlayerController>().Hit();
                }
            }
        }
    }
}