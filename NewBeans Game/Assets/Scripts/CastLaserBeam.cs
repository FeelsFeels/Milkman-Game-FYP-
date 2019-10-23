using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastLaserBeam : MonoBehaviour
{
    public LayerMask layersToHit; // Sets what layers the laser beam should detect and hit.

    public bool hitDetect = false; // Detects if laser beam hits anything.

    public float maxDistance = 1000f; // Distance of raycast.
    private float laserEndTime = 0.4f; // How long the laser beam should last.
    
    public LineRenderer warningLine; // Line Renderer to show the range of laser beam.
    public float lineWidth = 5f; // Line Renderer's width.

    private RaycastHit[] hit; 
    private Collider collider; // Collider of BoxCastAll.

    public Transform laserStartPos; // Start position of the laser beam.
    public Transform laserEndPos; // End position of the laser beam.
    
    public float warningTime = 2f; // The time that the warning is active, before the real laser beam is active.

    public ParticleSystem laserEffects;

    // Start is called before the first frame update
    void Start()
    {

        collider = GetComponent<Collider>();

        gameObject.transform.position = laserStartPos.transform.position;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, new Vector3 (5f, 5f, 5f));
    }

    // ----- Activate the warning laser beam.
    public void LaserWarning()
    {
        warningLine.SetWidth(5f, 5f);
        warningLine.SetPosition(0, laserStartPos.transform.position);
        warningLine.SetPosition(1, laserEndPos.transform.position);
        Debug.DrawRay(laserStartPos.transform.position, laserEndPos.transform.position, Color.blue);

        Invoke("ShootLaserBeam", warningTime);
    }

    // ----- Activate the real laser beam.
    public void ShootLaserBeam()
    {
        warningLine.enabled = false;

        Vector3 colliderScale = new Vector3(5f, 5f, 5f);
        Vector3 laserShootDirection = (laserEndPos.transform.position - laserStartPos.transform.position);


        Vector3 offset = new Vector3(0f, 0f, 2.8f);
//        laserEffects.transform.position = laserStartPos.transform.position - offset;
       laserEffects.transform.LookAt(laserEndPos);
 //       laserEffects.transform.rotation = Quaternion.LookRotation(laserEndPos.transform.position);
        laserEffects.Play();

        float distanceDifference; // Distance between object that is hit and the laser start point.

        float closestRockDistance = 1000f;
        float playerDistance = 0f;
        bool rockExists = false;

        RaycastHit[] hits = Physics.BoxCastAll(laserStartPos.transform.position, colliderScale, laserShootDirection, transform.rotation, maxDistance, layersToHit);

        Debug.DrawRay(laserEffects.transform.position, laserEffects.transform.forward * 100f, Color.red, 5f);
        
        foreach (RaycastHit hit in hits)
        {
            print("Hit");
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
    //    StartCoroutine("DisableLaser");
    }

    // ------ Disables laser beam after the desired timing.
    private IEnumerator DisableLaser()
    {
        yield return new WaitForSeconds(laserEndTime);
        gameObject.SetActive(false);
    }
}
