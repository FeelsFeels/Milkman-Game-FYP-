using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamCast : MonoBehaviour
{
    public LayerMask layersToHit;
    public bool hitDetect = false;
    public float maxDistance = 1000f;
    private float laserEndTime = 1f;

    private RaycastHit[] hit;
    private Collider collider;

    public Transform laserStartPos;
    public Transform laserEndPos;

    public LineRenderer warningLine;
    public LineRenderer laserLine;

    public Transform warningStartPos;
    public Transform warningEndPos;

    public Vector3 shootLaserDir;


    public ParticleSystem laserEffects;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();


    }

    private void Awake()
    {
        
    }
    //// The warning that shows the hitting range of the laser.
    //public void LaserWarning()
    //{
    //    warningLine.SetWidth(5f, 5f);
    //    warningLine.SetPosition(0, laserStartPos.transform.position);
    //    warningLine.SetPosition(1, laserEndPos.transform.position);

    //    Invoke("ShootLaserBeam", 2f);
    //}

    // The warning that shows the hitting range of the laser.
    public void LaserWarning(int index)
    {
        warningStartPos = laserStartPos;
        warningEndPos = laserEndPos;

        warningLine.SetWidth(5f, 5f);
        warningLine.SetPosition(0, warningStartPos.transform.position);
        warningLine.SetPosition(1, warningEndPos.transform.position);

        Invoke("ShootLaserBeam", 2f);
        return;
    }
    // The actual laser beam that shoots out.
    public void ShootLaserBeam()
    {
        laserEffects.Play();
        warningLine.enabled = false;

        Vector3 colliderScale = new Vector3(5f, 5f, 5f);
        Vector3 shootLaserDir = (laserStartPos.transform.position + transform.forward * maxDistance);

        laserLine.SetWidth(5f, 5f);
        laserLine.SetPosition(0, laserStartPos.transform.position);
        laserLine.SetPosition(1, laserEndPos.transform.position);


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

                laserLine.SetPosition(0, laserStartPos.transform.position);
                laserLine.SetPosition(1, hit.collider.transform.position);

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
        Destroy(gameObject, laserEndTime);
    }
}

// The actual laser beam that shoots out.
//public void ShootLaserBeam()
//    {
//        laserEffects.Play();
//        warningLine.enabled = false;

//        Vector3 colliderScale = new Vector3(5f, 5f, 5f);
//        Vector3 shootLaserDir = (laserStartPos.transform.position + transform.forward * maxDistance);


//        laserLine.SetWidth(5f, 5f);
//        laserLine.SetPosition(0, laserStartPos.transform.position);
//        laserLine.SetPosition(1, laserEndPos.transform.position);


//        float distanceDifference; // Distance between object that is hit and the laser start point.

//        float closestRockDistance = 1000f;
//        float playerDistance = 0f;

//        bool rockExists = false;

//        Debug.DrawRay(transform.position, shootLaserDir, Color.red);

//        RaycastHit[] hits = Physics.BoxCastAll(collider.bounds.center, colliderScale, shootLaserDir, transform.rotation, maxDistance, layersToHit);

//        foreach (RaycastHit hit in hits)
//        {
//            distanceDifference = (laserStartPos.transform.position - hit.collider.transform.position).magnitude;
//            Debug.DrawRay(hit.point, hit.point + Vector3.up * 5, Color.green);
//            hitDetect = true;

//            print("Laser hit:" + hit.collider.name);

//            // ----------- If hit rock
//            if (hit.collider.tag == "Rock")
//            {
//                rockExists = true;

//                laserLine.SetPosition(0, laserStartPos.transform.position);
//                laserLine.SetPosition(1, hit.collider.transform.position);

//                Vector3 nearestRock = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
//                closestRockDistance = Vector3.Distance(laserStartPos.transform.position, nearestRock);

//            }
//            // ---------- If hit player
//            if (hit.collider.tag == "Player")
//            {
//                playerDistance = distanceDifference;

//                if ((rockExists == true) && (playerDistance <= closestRockDistance))
//                {

//                    hit.collider.gameObject.GetComponent<PlayerController>().Hit();
//                }

//                else if (rockExists == false)
//                {
//                    hit.collider.gameObject.GetComponent<PlayerController>().Hit();
//                }
//            }
//        }
//        Destroy(gameObject, laserEndTime);
//    }
//}