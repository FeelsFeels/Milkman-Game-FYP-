using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public enum HookStatus
    {
        shooting,
        takeback,
        reverse,
        latch
    }
    /// shooting: Going Forwards
    /// takeback: Coming back towards player
    /// reverse: Bringing player towards the hook leader(the pointier part)
    /// latch: hooked onto an object.

    
    public HookStatus hookStatus = HookStatus.shooting;

    public GameObject hookOwner;        //This refers to the "first node";
    public GameObject player;

    public GameObject nodePrefab;       //This is the nodes

    public Vector3 direction;           //where the hook leader is going towards
    public float speed;                 //How fast hook leader goes when travelling forwards
    //public float retract;
    public float maxNodes;              //Max number of nodes before hook returns
    public float lastNodeUpdateTime;    //The time that a node was last spawned
    public float nodeUpdateTime;        //THE RATE in which that nodes will spawn
    public GameObject nodeToMoveTo;     //When retracting, the node that the hook leader will travel towards.
    public float nodeBondDistance;      //The distance between each node
    public float nodeBondDamping;       //The damping for each connected node


    public List<GameObject> nodes = new List<GameObject>();

    private void Start()
    {
        player = hookOwner;    
    }

    private void Update()
    {
        HookLogic();

        //Update the position of each hook node.

        for (int i = 0; i < nodes.Count; i++)
        {
            if (hookStatus == HookStatus.reverse)
            {
                FollowPreviousNode(i == 0 ? hookOwner.transform : nodes[i - 1].transform, nodes[i].transform);
            }
            else
            {
                FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
            }
        }
    }

    private void HookLogic()
    {
        
        if (hookStatus == HookStatus.shooting)
        {
            transform.Translate(direction * speed);

            if (nodes.Count < maxNodes)
            {
                if (Vector3.Distance(transform.position, LastNode().position) > nodeBondDistance)
                {
                    AddNode();
                }
                //if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
                //{
                //    AddNode();
                //    lastNodeUpdateTime = Time.time;
                //}
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
                    //lastNodeUpdateTime = Time.time;
                    nodes.Remove(nodeToMoveTo);
                    Destroy(nodeToMoveTo); //Unless we wanna add object pooling coolbeans😎🆒 stuff
                }
                if (nodes.Count == 0)
                {
                    transform.DetachChildren();
                    Destroy(gameObject);
                }
                else if (nodes.Count > 0)
                    nodeToMoveTo = nodes.Last();
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
                    Destroy(gameObject);
                }
                else if (nodes.Count > 0)
                    nodeToMoveTo = nodes.Last();
            }
            if (nodeToMoveTo != null)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, nodeToMoveTo.transform.position, Time.deltaTime * 20);
            }
        }


        //if (hookStatus != HookStatus.reverse && nodes.Count >= 5)
        //{
        //    float angle = Quaternion.Angle(hookOwner.transform.rotation, transform.rotation);
        //    if (angle < 110f)
        //    {
        //        bool updateNodeOrder = false;

        //    }
        //}
    }

    private void FollowPreviousNode(Transform prevNode, Transform node)
    {
        // Set node's rotation and position by the previous node

        Quaternion targetRotation = Quaternion.LookRotation(prevNode.position - node.position, prevNode.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        node.rotation = Quaternion.Slerp(node.rotation, targetRotation, Time.deltaTime * nodeBondDamping);

        Vector3 targetPosition = prevNode.position;
        targetPosition -= node.transform.rotation * Vector3.forward * nodeBondDistance;
        targetPosition.y = node.position.y;
        node.position = Vector3.Lerp(node.position, targetPosition, Time.deltaTime * nodeBondDamping);
    }

    private void AddNode()
    {
        Transform lastNode = LastNode();

        Vector3 position = NextNodePosition(lastNode);
        Quaternion rotation = NextNodeRotation(lastNode, position);

        nodes.Add(Instantiate(nodePrefab, transform.position, Quaternion.identity));
        //nodes.Add(Instantiate(nodePrefab, position, rotation));
    }

    private Vector3 NextNodePosition(Transform previousNode)
    {
        Quaternion currentRotation = Quaternion.Euler(0, previousNode.eulerAngles.y, 0);
        print(currentRotation);
        Vector3 position = previousNode.position;
        position -= previousNode.forward * nodeBondDistance;
        return position;
    }

    private Quaternion NextNodeRotation(Transform previousNode, Vector3 position)
    {
        return Quaternion.LookRotation(previousNode.position - position, previousNode.up);
        //return Quaternion.Euler(0, previousNode.eulerAngles.y, 0);
    }

    private Transform LastNode()
    {
        if (nodes.Count > 0)
        {
            return nodes[nodes.Count - 1].transform;
        }
        else
        {
            return hookOwner.transform;
        }
    }







    private void StartTakeBack()
    {
        //nodes.Reverse();
        hookStatus = HookStatus.takeback;
    }

    private void StartReverse()
    {
        nodes.Reverse();
        hookStatus = HookStatus.reverse;
        hookOwner = gameObject; 
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

//private void FollowPrev (Transform prevNode, Transform node)
//	{
//		// Set node's rotation and position by the previous node

//		Quaternion targetRotation = Quaternion.LookRotation(prevNode.position - node.position, prevNode.up);
//targetRotation.x = 0;
//		targetRotation.z = 0;
//		node.rotation = Quaternion.Slerp(node.rotation, targetRotation, Time.deltaTime* bondDamping);
		
//		Vector3 targetPosition = prevNode.position;
//targetPosition -= node.transform.rotation* Vector3.forward * bondDistance;
//targetPosition.y = node.position.y;
//		node.position = Vector3.Lerp(node.position, targetPosition, Time.deltaTime* bondDamping); 
//	}
