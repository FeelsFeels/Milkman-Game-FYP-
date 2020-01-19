using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastLaserBeam : MonoBehaviour
{
    public LayerMask layersToHit; // Sets what layers the laser beam should detect and hit.

    public bool hitDetect = false; // Detects if laser beam hits anything.

    public float maxDistance = 1000f; // Distance of raycast.
    private float laserEndTime = 0.4f; // How long the laser beam should last.

    public GameObject warningDirection;
    public LineRenderer warningLine; // Line Renderer to show the range of laser beam.
    public float lineWidth = 2.5f; // Line Renderer's width.

    private RaycastHit[] hit;
    private Collider collider; // Collider of BoxCastAll.

    public Transform laserStartPos; // Start position of the laser beam.
    public Transform laserEndPos; // End position of the laser beam.

    public float warningTime = 3f; // The time that the warning is active, before the real laser beam is active.

    public ParticleSystem laserEffects;
    public Color32 normalColor;
    public Color32 blinkColor;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();

        gameObject.transform.position = laserStartPos.transform.position;

        normalColor = warningLine.startColor;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, new Vector3(5f, 5f, 5f));
    //}

    // ----- Activate the warning laser beam.
    public void LaserWarning()
    {
        warningLine.gameObject.SetActive(true);
        warningLine.SetWidth(2.5f, 2.5f);
        warningLine.startColor = normalColor;
        warningLine.endColor = normalColor;
        warningLine.SetPosition(0, new Vector3(laserStartPos.transform.position.x, 0, laserStartPos.transform.position.z));
        //warningLine.SetPosition(1, new Vector3(laserEndPos.transform.position.x, 0, laserEndPos.transform.position.z));


        bool foundPos = false;
        float distance = 1f;
        int iterations = 0;
        
        ///This sends a raycast downwards,
        ///and if it hits ground, instantiates a warning indication on top of ground.
        //for (int i = 0; i < 15; i++)
        //{
        //    Vector3 newPos = laserStartPos.position + (distance * (laserEndPos.position - laserStartPos.position).normalized);
        //    Debug.DrawLine(newPos, newPos - Vector3.down * 100, Color.yellow, 5f);
        //    distance++;
        //}
        //while (foundPos == false)
        //{
        //    iterations++;
        //    if(iterations >= 50)
        //    {
        //        break;
        //    }
        //    RaycastHit hit;
        //    Vector3 newPos = laserStartPos.position + (distance * (laserEndPos.position - laserStartPos.position).normalized);
        //    Debug.DrawLine(newPos, newPos - Vector3.down * 100, Color.yellow, 5f);
        //    if (Physics.Raycast(newPos, Vector3.down, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
        //    {
        //        print("Name: " + hit.collider.gameObject.name);
        //        foundPos = true;
        //        GameObject indication = Instantiate(warningDirection, newPos, Quaternion.identity);
        //        Destroy(indication, warningTime);
        //        break;

        //    }
        //    else
        //    {
        //        distance += 1;
        //        continue;
        //    }
        //}

        Debug.DrawRay(laserStartPos.transform.position, laserEndPos.transform.position, Color.blue, 3.0f, false);
        StartCoroutine("WarningIndicationRoutine");
        Invoke("ShootLaserBeam", warningTime);

    }

    // ----- Activate the real laser beam.
    public void ShootLaserBeam()
    {
        warningLine.gameObject.SetActive(false);
        Vector3 colliderScale = new Vector3(1.5f, 1.5f, 1.5f);
        Vector3 laserShootDirection = (laserEndPos.transform.position - laserStartPos.transform.position);

        transform.LookAt(laserEndPos);
        laserEffects.transform.LookAt(laserEndPos);
        laserEffects.Play();

        float distanceDifference; // Distance between object that is hit and the laser start point.

        float closestRockDistance = Mathf.Infinity;
        float playerDistance = 0f;
        bool rockExists = false;

        Debug.DrawRay(laserEffects.transform.position, laserEffects.transform.forward * 100f, Color.red, 3f);
        
        //RaycastHit[] hits = Physics.BoxCastAll(laserStartPos.transform.position, colliderScale, laserShootDirection, transform.rotation, maxDistance, layersToHit);
        //foreach (RaycastHit hit in hits)
        //{
        //    print("Hit");
        //    distanceDifference = (laserStartPos.transform.position - hit.collider.transform.position).magnitude;
        //    Debug.DrawRay(hit.point, hit.point + Vector3.up * 5, Color.green);
        //    hitDetect = true;

        //    print("Laser hit:" + hit.collider.name);

        //    // ----------- If hit rock
        //    if (hit.collider.tag == "Rock")
        //    {
        //        rockExists = true;

        //        Vector3 nearestRock = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, hit.collider.transform.position.z);
        //        closestRockDistance = Vector3.Distance(laserStartPos.transform.position, nearestRock);

        //    }
        //    // ---------- If hit player
        //    if (hit.collider.tag == "Player")
        //    {
        //        playerDistance = distanceDifference;

        //        if ((rockExists == true) && (playerDistance <= closestRockDistance))
        //        {

        //            hit.collider.gameObject.GetComponent<PlayerController>().Hit(3);
        //        }

        //        else if (rockExists == false)
        //        {
        //            hit.collider.gameObject.GetComponent<PlayerController>().Hit(3);
        //        }
        //    }
        //}
    }

    IEnumerator WarningIndicationRoutine()
    {
        float warningTime = 0f;
        Vector3 tailEndPos;

        while (warningTime < 1.5f)
        {
            warningTime += Time.deltaTime;
            tailEndPos = Vector3.Lerp(laserStartPos.position, laserEndPos.position, warningTime / 1.5f);
            //print(tailEndPos.y);  //Dunno why this is always 2.5f instead of 0
            warningLine.SetPosition(1, new Vector3(tailEndPos.x, 0, tailEndPos.z));
            yield return null;
        }

        int iteration = 0;
        while (warningTime < 3f)
        {
            yield return null;

            warningTime += Time.deltaTime;
            if (warningTime < 2f)
                continue;

            //Blinks the indicator
            iteration++;
            if(iteration % 4 == 0)
            {
                if (warningLine.startColor == normalColor)
                {
                    warningLine.startColor = blinkColor;
                    warningLine.endColor = blinkColor;
                }
                else
                {
                    warningLine.startColor = normalColor;
                    warningLine.endColor = normalColor;
                }
            }
        }
        warningLine.startColor = normalColor;
        warningLine.endColor = normalColor;
    }
}