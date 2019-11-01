using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private PlayerController player;
    private MeshRenderer shieldRenderer;
    private SphereCollider shieldCollider;

    public float shieldTime;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        shieldRenderer = GetComponent<MeshRenderer>();
        shieldCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        shieldRenderer.enabled = false;
        shieldCollider.enabled = false;
    }

    public void ActivateShield()
    {
        shieldRenderer.enabled = true;
        shieldCollider.enabled = true;

        //player.rb.WakeUp();
        //GetComponent<Rigidbody>().WakeUp();
        player.transform.Translate(0.0001f, 0, 0);

        StartCoroutine(TimeBeforeDeactivate());
    }

    private IEnumerator TimeBeforeDeactivate()
    {
        yield return new WaitForSeconds(shieldTime);
        shieldRenderer.enabled = false;
        shieldCollider.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        PushProjectile projectile = other.GetComponent<PushProjectile>();
        if (projectile)
        {
            if (projectile.ownerPlayer != player.gameObject)
            {
                Destroy(projectile.gameObject);
            }
        }

        GrapplingHook hook = other.GetComponent<GrapplingHook>();
        if (hook)
        {
            if(hook.player != player.gameObject)
            {
                hook.StartTakeBack();
            }
        }
    }
}
