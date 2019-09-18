using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HazardBomb : MonoBehaviour
{

    public float timeBeforeExploding;

    public float explosionForce;
    public float explosionRadius;
    public float groundDestroyRadius;

    public GameObject canvas;
    public Image countdownAlpha;
    
    //public float upwardsForce;

    public GameObject bombFX;

    private int layermask;

    private void Start()
    {
        canvas.SetActive(false);
    }

    void OnDrawGizmos()
    {        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }

    public void Explode()
    {
        layermask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground");
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, explosionRadius, layermask);
        foreach(Collider hit in collidersInRange)
        {
            if (hit.tag == "Player")
            {
                Vector3 direction = (hit.transform.position - new Vector3(transform.position.x, hit.transform.position.y, transform.position.z)).normalized;
                hit.GetComponent<Rigidbody>().AddForce(direction * explosionForce);
                continue;
            }
            else
            {
                Tile tile = hit.GetComponent<Tile>();
                if (Vector3.Distance(transform.position, tile.transform.position) < groundDestroyRadius)
                {
                    if (tile.tileState == Tile.TileState.up)
                        tile.tileState = Tile.TileState.goingDown;
                }
            }
        }
        Instantiate(bombFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public IEnumerator WaitBeforeExploding()
    {
        GetComponent<Rigidbody>().drag = 1.5f;

        canvas.SetActive(true);

        countdownAlpha.fillAmount = 1;

        //Controls timing of countdown visuals
        float explodeTime = Time.time + timeBeforeExploding;

        //This loop controls the countdown visuals
        while (Time.time < explodeTime)
        {
            countdownAlpha.fillAmount = ((explodeTime - Time.time) / timeBeforeExploding);
            yield return null;
        }

        //yield return new WaitForSeconds(timeBeforeExploding);
        Explode();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(WaitBeforeExploding());
    }
}
