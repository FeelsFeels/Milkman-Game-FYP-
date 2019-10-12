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

    private ParticleSystem laserEffects;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        laserEffects = GetComponentInChildren<ParticleSystem>();


        LaserWarning();
        Invoke("ShootLaserBeam", 2);
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LaserWarning()
    {
        warningLine.SetWidth(2f, 2f);
        warningLine.SetPosition(0, laserStartPos.transform.position);
        warningLine.SetPosition(1, laserEndPos.transform.position);
    }

    public void ShootLaserBeam()
    {
        Vector3 colliderScale = new Vector3(2f, 2f, 2f);
        Vector3 shootLaserDir = (laserStartPos.transform.position + transform.forward * maxDistance);

        laserLine.SetWidth(2f, 2f);
        laserLine.SetPosition(0, laserStartPos.transform.position);
        laserLine.SetPosition(1, laserEndPos.transform.position);

        laserEffects.Play();

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