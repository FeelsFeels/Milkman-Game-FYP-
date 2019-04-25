using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public enum HookStatus
    {
        shooting,
        takeback,
        reverse
    }
    /// shooting: Going Forwards
    /// takeback: Coming back towards player
    /// reverse: Bringing player towards the hook leader(the pointier part)

    
    public HookStatus hookStatus = HookStatus.shooting;

    public GameObject hookOwner;        //This refers to the player that shot the hook;

    public GameObject nodePrefab;       //This is the nodes

    public Vector3 direction;           //where the hook leader is going towards
    public float speed;                 //How fast hook leader goes when travelling forwards
    //public float retract;
    public float maxNodes;              //Max number of nodes before hook returns
    public float lastNodeUpdateTime;    //The time that a node was last spawned
    public float nodeUpdateTime;        //THE RATE in which that nodes will spawn
    public GameObject nodeToMoveTo;     //When retracting, the node that the hook leader will travel towards.


    public List<GameObject> nodes = new List<GameObject>();



    private void Start()
    {
        nodes.Add(Instantiate(nodePrefab, transform.position, Quaternion.identity));
    }

    private void Update()
    {
        HookLogic();
    }

    private void HookLogic()
    {
        
        if (hookStatus == HookStatus.shooting)
        {
            transform.Translate(direction * speed);

            if (nodes.Count < maxNodes)
            {
                if(Time.time - lastNodeUpdateTime > nodeUpdateTime)
                {
                    AddNode();
                    lastNodeUpdateTime = Time.time;
                }
            }
            else
            {
                StartTakeBack();
            }
        }

        if(hookStatus == HookStatus.takeback)
        {
            if(Time.time - lastNodeUpdateTime > nodeUpdateTime)
            {
                if (nodeToMoveTo != null)
                {
                    nodes.Remove(nodeToMoveTo);
                    Destroy(nodeToMoveTo); //Unless we wanna add object pooling coolbeans😎🆒 stuff
                }
                if (nodes.Count == 0)
                {
                    transform.DetachChildren();
                    Destroy(gameObject);
                }
                else if (nodes.Count > 0)
                    nodeToMoveTo = NextNodePosition();
            }
            if (nodeToMoveTo != null)
            {
                transform.position = Vector3.Lerp(transform.position, nodeToMoveTo.transform.position, Time.deltaTime * 20);
            }
        }

        if(hookStatus == HookStatus.reverse)
        {
            if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
            {
                if (nodeToMoveTo != null)
                {
                    nodes.Remove(nodeToMoveTo);
                    Destroy(nodeToMoveTo); //Unless we wanna add object pooling coolbeans😎🆒 stuff
                }
                if (nodes.Count == 0)
                {
                    transform.DetachChildren();
                    Destroy(gameObject);
                }
                else if (nodes.Count > 0)
                    nodeToMoveTo = NextNodePosition();
            }
            if (nodeToMoveTo != null)
            {
                hookOwner.transform.position = Vector3.Lerp(hookOwner.transform.position, nodeToMoveTo.transform.position, Time.deltaTime * 20);
            }
        }
    }

    private void AddNode()
    {
        nodes.Add(Instantiate(nodePrefab, transform.position, Quaternion.identity));
    }

    private void StartTakeBack()
    {
        nodes.Reverse();
        hookStatus = HookStatus.takeback;
    }

    private void StartReverse()
    {
        hookStatus = HookStatus.reverse;
    }

    private GameObject NextNodePosition()
    {
        return nodes[0];
    }

    private Transform LastNode()
    {
        return transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject != hookOwner)
        {            
            StartTakeBack();
            other.transform.parent = transform;
            return;
        }
        if (other.tag == "GrabbableEnvironment")
        {
            StartReverse();
            gameObject.transform.position = other.transform.position;
            return;
        }
    }

}
