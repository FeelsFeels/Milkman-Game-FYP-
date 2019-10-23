using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPatterns : MonoBehaviour
{
    
    public static LaserPatterns instance = null;

    public GameObject[] lasers;

    public int randomNum;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // -------- From 12 different positions on the clock, choose 1 position and shoot a laser beam from there.
    // --- Difficulty: 1
    public void SingularLaser()
    {
        randomNum = Random.Range(0, 12);

        lasers[randomNum].GetComponent<CastLaserBeam>().LaserWarning();
        print("Shot singular laser:" + randomNum);
    }

    // --- Difficulty: 2
    public void EasyPatterns()
    {
        UnevenCross();
    }

    // --- Difficulty: 3
    public void MediumPatterns()
    {
        CrossPattern();
    }

    // --- Difficulty: 4
    public void HardPatterns()
    {
        ThreeInOneRow();
    }
    public void ExtremePatterns()
    {

    }

    // !! --- All methods below this are the actual patterns that will be called in the above methods.

    // -------- Shoots 2 laser beam that forms an uneven cross shape.   
    public void UnevenCross()
    {
        randomNum = Random.Range(0, 3);
        print("EvenCross number: " + randomNum);

        if (randomNum == 0)
        {
            lasers[Random.Range(12, 15)].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[Random.Range(15, 18)].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 1)
        {
            lasers[Random.Range(18, 21)].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[Random.Range(21, 24)].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 2)
        {
            lasers[Random.Range(12, 15)].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[Random.Range(21, 24)].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 3)
        {
            lasers[Random.Range(15, 18)].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[Random.Range(18, 21)].GetComponent<CastLaserBeam>().LaserWarning();
        }
    }

    // -------- A pattern that shoots 2 laser beams, creating an even cross shape.  
    public void CrossPattern()
    {
        randomNum = Random.Range(0, 12); // Gets random number from 0 to 11 (Does not include 12).
        print("CrossPattern number: " + randomNum);

        if (randomNum == 0)
        {
            lasers[0].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[0+3].GetComponent<CastLaserBeam>().LaserWarning();
        }

        if (randomNum == 1)
        {
            lasers[1].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[1+3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 2)
        {
            lasers[2].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[2+3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 3)
        {
            lasers[3].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[3+3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 4)
        {
            lasers[4].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[4+3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 5)
        {
            lasers[5].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[5+3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 6)
        {
            lasers[6].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[6 + 3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 7)
        {
            lasers[7].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[7 + 3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 8)
        {
            lasers[8].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[8 + 3].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 9)
        {
            lasers[9].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[0].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 10)
        {
            lasers[10].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[1].GetComponent<CastLaserBeam>().LaserWarning();
        }
        if (randomNum == 11)
        {
            lasers[11].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[2].GetComponent<CastLaserBeam>().LaserWarning();
        }
    }
    
    // -------- A pattern that shoots 3 laser beams from the same side, towards the same direction.
    public void ThreeInOneRow()
    {
        randomNum = Random.Range(0, 4);
        print("Three in a row: " + randomNum);

        if (randomNum == 0)
        {
            lasers[12].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[13].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[14].GetComponent<CastLaserBeam>().LaserWarning();
        }

        if (randomNum == 1)
        {
            lasers[15].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[16].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[17].GetComponent<CastLaserBeam>().LaserWarning();
        }

        if (randomNum == 2)
        {
            lasers[18].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[19].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[20].GetComponent<CastLaserBeam>().LaserWarning();
        }

        if (randomNum == 3)
        {
            lasers[21].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[22].GetComponent<CastLaserBeam>().LaserWarning();
            lasers[23].GetComponent<CastLaserBeam>().LaserWarning();
        }
    }
}
