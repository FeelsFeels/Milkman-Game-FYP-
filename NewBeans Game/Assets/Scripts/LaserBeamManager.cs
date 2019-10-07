using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamManager : MonoBehaviour
{
    public LayerMask layersToHit;
    public ParticleSystem laserEffect;

    public Transform shootStartPosition;
    public Transform shootEndPosition;
    public float timeTillStartLaser = 2f;

    private LineRenderer laserLine;
    private RaycastHit hit;

    private PlayerController playerHit;
    
    public bool startLineRender;

    // Start is called before the first frame update
    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserEffect = GetComponent<ParticleSystem>();
    }

    public void Update()
    {

        timeTillStartLaser -= Time.deltaTime;

        WarnLaserBeam();
        return;
    }

    public void WarnLaserBeam()
    {

        laserLine.SetWidth(0.5f, 0.5f);
        laserLine.SetPosition(0, shootStartPosition.position);
        laserLine.SetPosition(1, shootEndPosition.position);


        if (timeTillStartLaser <= 0)
        {
            ShootLaserBeam();
            return;
        }
    }

    public void ShootLaserBeam()
    {
        laserLine.SetWidth(5.0f, 5.0f);

         Destroy(laserLine, 0.4f);

        Vector3 shootDirection = (shootEndPosition.position - shootStartPosition.position);

        startLineRender = true;

        laserLine.SetPosition(0, shootStartPosition.position);
        laserLine.SetPosition(1, shootEndPosition.position);

        if (startLineRender == true)
        {
            if (Physics.Raycast(shootStartPosition.position, shootDirection, out hit, Mathf.Infinity, layersToHit))
            {
                Debug.DrawRay(shootStartPosition.position, shootDirection, Color.green);


                //------- Check what object the laser hits.
                if (hit.collider.tag == "Rock")
                {
                    hit.point = hit.transform.position;
                    laserLine.SetPosition(1, hit.transform.position);
                    print("Laser hit a rock");

                }
                else if (hit.collider.tag == "Player")

                {
                    hit.collider.gameObject.GetComponent<PlayerController>().Hit();
                    print("Laser hit at least 1 player");

                }
            }
            
        }
    }
}
