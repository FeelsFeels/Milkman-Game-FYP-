using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathSkill_ShootingMonkey : BaseDeathSkill
{
    public GeneralMovement generalMovement;
    public GeneralRotation generalRotation;


    public GameObject aimingCanvas;
    public Image aimingImage;
    public Image chargingImage;
    public float timeCharged;

    public Rigidbody monkey;
    public Transform shootOrigin;
    public GameObject monkeyPellets;
    public Transform groundCheckTransform;

    public float timeSinceLastShot;
    public float shootingTimeElapsed;

    bool deactivated;
    bool skillUsed;
    bool onGround;

    public override void OnFinishSkill()
    {
        if (!deactivated)
        {
            deactivated = true;
            assignedController.FinishUsingSkill();
            Destroy(gameObject);
        }
    }

    public override void OnReady()
    {
        aimingCanvas.SetActive(true);
        aimingImage.color = assignedPlayer.inputInfo.chosenCharacterData.characterColor;
        monkey.useGravity = false;
    }

    public override void OnUse()
    {
        if (!skillUsed)
        {
            skillUsed = true;
            chargingImage.fillAmount = 0;
            monkey.useGravity = true;
            StartCoroutine(MoveMonkeyDown());
        }
    }

    private void Update()
    {
        if (assignedPlayer == null || assignedController == null || deactivated)
            return;

        if (!skillUsed)
        {
            timeCharged += Time.deltaTime;
            chargingImage.fillAmount = timeCharged / 10f;
            if (timeCharged >= 10f)
            {
                OnUse();
            }

            float moveVerticalAxis = Input.GetAxisRaw(VerticalInputAxis);
            float moveHorizontalAxis = Input.GetAxisRaw(HorizontalInputAxis);
            Vector3 input = new Vector3(moveHorizontalAxis, 0, -moveVerticalAxis);
            Vector3 direction = input.normalized;

            if (moveHorizontalAxis != 0 || moveVerticalAxis != 0)
            {
                generalMovement.Move(direction);
            }
        }

        if (onGround)
        {
            //Turning the monkey and its shoot position

            float moveVerticalAxis = Input.GetAxisRaw(VerticalInputAxis);
            float moveHorizontalAxis = Input.GetAxisRaw(HorizontalInputAxis);
            Vector3 input = new Vector3(moveHorizontalAxis, 0, -moveVerticalAxis);
            Vector3 direction = input.normalized;
            generalRotation.Rotate(direction);
            //Auto Shoot
            timeSinceLastShot += Time.deltaTime;
            shootingTimeElapsed += Time.deltaTime;
            if(timeSinceLastShot >= 0.25f)
            {
                DeathSkill_ShootingMonkeyProjectile projectile = Instantiate(monkeyPellets, shootOrigin.position, Quaternion.identity).GetComponent<DeathSkill_ShootingMonkeyProjectile>();
                projectile.Shoot(shootOrigin.forward);
                projectile.monkey = monkey.gameObject;
                timeSinceLastShot = 0;
            }
            if(shootingTimeElapsed >= 5f)
            {
                OnFinishSkill();
            }

            chargingImage.fillAmount = shootingTimeElapsed / 5f;
        }

        if (Input.GetButtonDown(AButtonInput))
        {
            OnUse();
        }
    }

    IEnumerator MoveMonkeyDown()
    {
        float timeElapsed = 0;

        while (true)
        {
            timeElapsed += Time.deltaTime;
            monkey.transform.Translate(Vector3.down * 2);

            if(timeElapsed >= 10f)
            {
                //Assume the monkey fell into water liao
                Destroy(gameObject);
            }
            //Check if touch ground. Then can start shooting
            if(Physics.CheckSphere(groundCheckTransform.position, 1f, 1 << LayerMask.NameToLayer("Ground")))
            {
                onGround = true;
                break;
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        OnFinishSkill();
    }
}
