using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBomb : MonoBehaviour
{

    public float timeBeforeExploding;

    public float explosionForce;
    public float explosionRadius;
    public float groundDestroyRadius;
    //public float upwardsForce;

    public GameObject bombFX;

    private int layermask;

    private void Start()
    {
        StartCoroutine(WaitBeforeExploding());
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
                print("Tilehit");
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
        yield return new WaitForSeconds(timeBeforeExploding);
        Explode();
    }
}
