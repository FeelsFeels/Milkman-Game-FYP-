using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamManager : MonoBehaviour
{
    public static LaserBeamManager instance = null;
    public GameObject theLaser;

    [System.Serializable]
    public struct LaserInformation
    {
        public string LaserPositionOnClock;

        public Transform shootOrigin;
        public Transform shootDestination;


        [HideInInspector]
        public Transform warningOrigin;
        public Transform warningDestination;
        public float timeTillActive;
    }

    public LaserInformation[] lasers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {

    }

    private void Update()
    {

    }
    
    //// For loop activate laser.
    //public void ActivateLaser()
    //{
    //    for (int i = 0; i < lasers.Length; i++)
    //    {

    //        LaserBeamCast startBeam = Instantiate(theLaser, lasers[i].shootOrigin.position, Quaternion.LookRotation(lasers[i].shootDestination.transform.position - lasers[i].shootOrigin.transform.position)).GetComponent<LaserBeamCast>();
    //        print("Shoot laser!" + i);

    //        // Start and end position of the shot laser beam.
    //        startBeam.laserStartPos = lasers[i].shootOrigin;
    //        startBeam.laserEndPos = lasers[i].shootDestination;

    //        // Sets the warning beam positions to the laser beam positions.
    //        lasers[i].warningOrigin = lasers[i].shootOrigin;
    //        lasers[i].warningDestination = lasers[i].shootDestination;

    //        // Start and end position of the warning laser beam.
    //        startBeam.warningStartPos = lasers[i].warningOrigin;
    //        startBeam.warningEndPos = lasers[i].shootDestination;
            
    //        // Starts the laser beam by shooting warning first.
    //        startBeam.LaserWarning(i);
            
    //        // Laser beam shooting direction.
    //        startBeam.shootLaserDir = (lasers[i].shootDestination.transform.position - lasers[i].shootOrigin.transform.position);

    //    }
    //}
    //public void ActivateLaser()
    //{
    //    for (int i = 0; i < lasers.Length; i++)
    //    {
    //        lasers[i].timeTillActive -= Time.deltaTime;
    //        if (lasers[i].timeTillActive <= 0)
    //        {
    //            LaserBeamCast beam = Instantiate(theLaser, lasers[i].shootOrigin.position, Quaternion.identity).GetComponent<LaserBeamCast>();
    //            beam.LaserWarning(i);
    //        }
    //    }
    //}
}