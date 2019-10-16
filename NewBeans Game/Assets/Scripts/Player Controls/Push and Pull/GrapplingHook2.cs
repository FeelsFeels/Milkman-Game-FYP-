using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrapplingHook2 : MonoBehaviour
{
    public enum HookStatus
    {
        shooting,
        takeback,
        reverse,
        latching
    }
    /// shooting: Going Forwards
    /// takeback: Coming back towards player
    /// reverse: Bringing player towards the hook leader(the pointier part)
    /// latching: hooked onto an object.

    
    public HookStatus hookStatus = HookStatus.shooting;

    public GameObject hookOwner;        //This refers to the "first node", can change depending on the state
    public GameObject player;           //Player gameobject. Does NOT change.
    public GameObject latchedObject;

    public GameObject nodePrefab;       //This is the nodes

    public Vector3 direction;           //where the hook leader is going towards
    public float speed;                 //How fast hook leader goes when travelling forwards
    public float latchTimeWindow;       //Time window in which you can decide to pull the enemy back
    private float latchCurrentTime;

    //public float retract;
    public float maxNodes;              //Max number of nodes before hook returns
    public float lastNodeUpdateTime;    //The time that a node was last spawned
    public float nodeUpdateTime;        //THE RATE in which that nodes will spawn
    public GameObject nodeToMoveTo;     //When retracting, the node that the hook leader will travel towards.
    public float nodeBondDistance;      //The distance between each node
    public float nodeBondDamping;       //The damping for each connected node


    public List<GameObject> nodes = new List<GameObject>();
    //public LineRenderer lineRenderer;

    private void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //lineRenderer.enabled = false;
        player = hookOwner;
    }
    
    //public void Init()
    //{

    //}

    private void Update()
    {
        HookLogic();

        //Update the position of each hook node.
        for (int i = 0; i < nodes.Count; i++)
        {
            if (hookStatus == HookStatus.reverse)
            {
                //HookOwner is now the latched onto object, the nodes will follow the grabbable wall
                FollowPreviousNode(i == 0 ? hookOwner.transform : nodes[i - 1].transform, nodes[i].transform);
            }
            else if (hookStatus == HookStatus.latching && latchedObject.tag == "GrabbableEnvironment")
            {
                if (Vector3.Distance(latchedObject.transform.position, LastNode().position) <= nodeBondDistance)
                {
                    FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
                    //FollowPreviousNode(i == 0 ? latchedObject.transform : nodes[i - 1].transform, nodes[i].transform);
                }
                else
                {
                
                }
            }
            else
            {
                //The nodes follows the player
                FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
            }
        }

        //if(nodes.Count > 5)
        //{
        //    lineRenderer.enabled = true;
        //    var points = new Vector3[nodes.Count];
        //    for (int i = 0; i < nodes.Count; i++)
        //    {
        //        points[i] = nodes[i].transform.position;
        //    }
        //    lineRenderer.SetPositions(points);
        //}
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

        if (hookStatus == HookStatus.latching)
        {
            if (Time.time - latchCurrentTime < latchTimeWindow) //Continue latching onto the target
            {
                transform.position = latchedObject.transform.position;
                //nodes[0].transform.position = player.transform.position;
                //nodes[0].transform.position = nodes[1].transform.forward * nodeBondDistance;
            }
            else
            {
                latchedObject = null;
                StartTakeBack();
            }
        }

        if (hookStatus == HookStatus.takeback)
        {
            if(Time.time - lastNodeUpdateTime > nodeUpdateTime)
            {
                if (nodeToMoveTo != null)
                {
                    //lastNodeUpdateTime = Time.time;
                    nodes.Remove(nodeToMoveTo);
                    Destroy(nodeToMoveTo);
                }
                if (nodes.Count == 0)
                {
                    transform.DetachChildren();
                    FinishHookSequence();
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
                    Destroy(nodeToMoveTo);
                }
                if (nodes.Count == 0)
                {
                    FinishHookSequence();
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
        //      //Probably when you walk towards the node, it destroys and spawns a new one at the end?
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

    //private void FollowNextNode(Transform nextNode, Transform node)
    //{

    //}

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

    //Called when the player uses the grappling hook while already latched onto something
    //Called from the Shoot Script :(
    public void PullFromLatch()
    {
        if(latchedObject.tag == "Player")
        {
            transform.parent = null;
            latchedObject.transform.parent = transform;
            StartTakeBack();
        }
        else if(latchedObject.tag == "GrabbableEnvironment")
        {
            StartReverse();
        }
    }

    //Initialised Latching
    private void StartLatching(GameObject grabbedObject)
    {
        if (latchedObject == null)
        {
            latchedObject = grabbedObject;
            hookStatus = HookStatus.latching;
            latchCurrentTime = Time.time;

            //Initialise the Hinge joint
            if (!LastNode().GetComponent<HingeJoint>())
            {
                LastNode().position = latchedObject.gameObject.transform.position;

                if (latchedObject.gameObject.tag == "Player")
                {
                    HingeJoint hinge = LastNode().gameObject.AddComponent<HingeJoint>() as HingeJoint;
                    hinge.connectedBody = latchedObject.GetComponent<Rigidbody>();
                }
                else if (latchedObject.gameObject.tag == "GrabbableEnvironment")
                {
                    //Connect a hinge joint between the grabbable wall and its closest node
                    HingeJoint hinge = latchedObject.AddComponent<HingeJoint>() as HingeJoint;
                    hinge.connectedBody = LastNode().GetComponent<Rigidbody>();

                    //Connect a hinge joint between the player and the closest node
                    //SpringJoint ownerHinge = nodes[0].AddComponent<SpringJoint>() as SpringJoint;
                    //ownerHinge.connectedBody = player.GetComponent<Rigidbody>();
                    //ownerHinge.spring = 1000;

                    //Reverse the node order such that the wall is the "owner" now
                    nodes.Reverse();
                }
            }


            ////Initialise the spring joint
            //if (!LastNode().GetComponent<SpringJoint>())
            //{
            //    SpringJoint spring = LastNode().gameObject.AddComponent<SpringJoint>() as SpringJoint;
            //    spring.maxDistance = nodeBondDistance / 2;
            //    spring.minDistance = 0;

            //    spring.connectedBody = latchedObject.GetComponent<Rigidbody>();
            //    spring.spring = 50;

            //    SpringJoint ownerSpring = nodes[0].AddComponent<SpringJoint>() as SpringJoint;
            //    ownerSpring.maxDistance = nodeBondDistance / 2;
            //    ownerSpring.minDistance = 0;

            //    ownerSpring.connectedBody = latchedObject.GetComponent<Rigidbody>();
            //    ownerSpring.spring = 50;
            //}
        }
    }

    private void StartTakeBack()
    {
        //nodes.Reverse();
        hookStatus = HookStatus.takeback;
    }

    private void StartReverse()
    {
        //nodes.Reverse();
        hookStatus = HookStatus.reverse;
        hookOwner = gameObject; 
    }

    //Call before destroying hook
    private void FinishHookSequence()
    {
        Destroy(gameObject);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (latchedObject == null)
        {
            if (other.tag == "Player" && other.gameObject != hookOwner)
            {
                StartLatching(other.gameObject);
                //StartTakeBack();
                //other.transform.parent = transform;
                transform.parent = other.transform;
                return;
            }
            if (other.tag == "GrabbableEnvironment")
            {
                StartLatching(other.gameObject);
                //StartReverse();
                gameObject.transform.position = other.transform.position;
                return;
            }
        }
    }

}


//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class GrapplingHook : MonoBehaviour
//{
//    public enum HookStatus
//    {
//        shooting,
//        takeback,
//        reverse,
//        latching
//    }
//    /// shooting: Going Forwards
//    /// takeback: Coming back towards player
//    /// reverse: Bringing player towards the hook leader(the pointier part)
//    /// latching: hooked onto an object.


//    public HookStatus hookStatus = HookStatus.shooting;

//    public GameObject hookOwner;        //This refers to the "first node", can change depending on the state
//    public GameObject player;           //Player gameobject. Does NOT change.
//    public GameObject latchedObject;

//    public GameObject nodePrefab;       //This is the nodes

//    public Vector3 direction;           //where the hook leader is going towards
//    public float speed;                 //How fast hook leader goes when travelling forwards
//    public float latchTimeWindow;       //Time window in which you can decide to pull the enemy back
//    private float latchCurrentTime;

//    //public float retract;
//    public float maxNodes;              //Max number of nodes before hook returns
//    public float lastNodeUpdateTime;    //The time that a node was last spawned
//    public float nodeUpdateTime;        //THE RATE in which that nodes will spawn
//    public GameObject nodeToMoveTo;     //When retracting, the node that the hook leader will travel towards.
//    public float nodeBondDistance;      //The distance between each node
//    public float nodeBondDamping;       //The damping for each connected node


//    public List<GameObject> nodes = new List<GameObject>();
//    //public LineRenderer lineRenderer;

//    private void Start()
//    {
//        //lineRenderer = GetComponent<LineRenderer>();
//        //lineRenderer.enabled = false;
//        player = hookOwner;
//    }

//    //public void Init()
//    //{

//    //}

//    private void Update()
//    {
//        HookLogic();

//        //Update the hook nodes position
//        if (hookStatus == HookStatus.reverse)
//        {
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                //HookOwner is now the latched onto object, the nodes will follow the grabbable wall
//                FollowPreviousNode(i == 0 ? hookOwner.transform : nodes[i - 1].transform, nodes[i].transform);
//            }
//        }
//        else if (hookStatus == HookStatus.latching && latchedObject.tag == "GrabbableEnvironment")
//        {
//            return;
//            ///The idea is to not allow player to move using a hinge joint between the first node and the player
//            ///WHen the distance between them gets too wide.
//            //if (Vector3.Distance(latchedObject.transform.position, LastNode().position) <= nodeBondDistance)
//            //{
//            //    FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
//            //    //FollowPreviousNode(i == 0 ? latchedObject.transform : nodes[i - 1].transform, nodes[i].transform);
//            //}

//            ///This Idea is to calculate the angle where the player is moving in relation to the hook rope
//            ///When angle is of the away direction, stop following
//            //bool stretched = true;
//            //for (int i = 0; i < nodes.Count; i++)
//            //{
//            //    float dot = Vector3.Dot(player.transform.forward, -nodes[i].transform.forward);
//            //    if (dot > -0.9f)
//            //    {
//            //        stretched = false;
//            //        break;
//            //    }                
//            //}
//            //if(stretched == false)
//            //{
//            //    for (int i = 0; i < nodes.Count; i++)
//            //    {
//            //        FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
//            //    }
//            //}
//        }
//        else
//        {
//            for (int i = 0; i < nodes.Count; i++)
//            {
//                //The nodes follows the player
//                FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
//            }
//        }
//    }

//    private void HookLogic()
//    {
//        if (hookStatus == HookStatus.shooting)
//        {
//            transform.Translate(direction * speed);

//            if (nodes.Count < maxNodes)
//            {
//                if (Vector3.Distance(transform.position, LastNode().position) > nodeBondDistance)
//                {
//                    AddNode();
//                }
//                //if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
//                //{
//                //    AddNode();
//                //    lastNodeUpdateTime = Time.time;
//                //}
//            }
//            else
//            {
//                StartTakeBack();
//            }
//        }

//        if (hookStatus == HookStatus.latching)
//        {
//            if (Time.time - latchCurrentTime < latchTimeWindow) //Continue latching onto the target
//            {
//                transform.position = latchedObject.transform.position;
//                //nodes[0].transform.position = player.transform.position;
//                //nodes[0].transform.position = nodes[1].transform.forward * nodeBondDistance;
//            }
//            else
//            {
//                latchedObject = null;
//                StartTakeBack();
//            }
//        }

//        if (hookStatus == HookStatus.takeback)
//        {
//            if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
//            {
//                if (nodeToMoveTo != null)
//                {
//                    //lastNodeUpdateTime = Time.time;
//                    nodes.Remove(nodeToMoveTo);
//                    Destroy(nodeToMoveTo);
//                }
//                if (nodes.Count == 0)
//                {
//                    transform.DetachChildren();
//                    FinishHookSequence();
//                }
//                else if (nodes.Count > 0)
//                    nodeToMoveTo = nodes.Last();
//            }
//            if (nodeToMoveTo != null)
//            {
//                transform.position = Vector3.Lerp(transform.position, nodeToMoveTo.transform.position, Time.deltaTime * 20);
//            }
//        }

//        if (hookStatus == HookStatus.reverse)
//        {
//            if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
//            {
//                if (nodeToMoveTo != null)
//                {
//                    nodes.Remove(nodeToMoveTo);
//                    Destroy(nodeToMoveTo);
//                }
//                if (nodes.Count == 0)
//                {
//                    FinishHookSequence();
//                }
//                else if (nodes.Count > 0)
//                    nodeToMoveTo = nodes.Last();
//            }
//            if (nodeToMoveTo != null)
//            {
//                player.transform.position = Vector3.Lerp(player.transform.position, nodeToMoveTo.transform.position, Time.deltaTime * 20);
//            }
//        }

//        //if (hookStatus != HookStatus.reverse && nodes.Count >= 5)
//        //{
//        //    float angle = Quaternion.Angle(hookOwner.transform.rotation, transform.rotation);
//        //    if (angle < 110f)
//        //    {
//        //      //Probably when you walk towards the node, it destroys and spawns a new one at the end?
//        //    }
//        //}
//    }

//    private void FollowPreviousNode(Transform prevNode, Transform node)
//    {
//        // Set node's rotation and position by the previous node

//        Quaternion targetRotation = Quaternion.LookRotation(prevNode.position - node.position, prevNode.up);
//        targetRotation.x = 0;
//        targetRotation.z = 0;
//        node.rotation = Quaternion.Slerp(node.rotation, targetRotation, Time.deltaTime * nodeBondDamping);

//        Vector3 targetPosition = prevNode.position;
//        targetPosition -= node.transform.rotation * Vector3.forward * nodeBondDistance;
//        targetPosition.y = node.position.y;
//        node.position = Vector3.Lerp(node.position, targetPosition, Time.deltaTime * nodeBondDamping);
//    }

//    //private void FollowNextNode(Transform nextNode, Transform node)
//    //{

//    //}

//    private void AddNode()
//    {
//        Transform lastNode = LastNode();

//        Vector3 position = NextNodePosition(lastNode);
//        Quaternion rotation = NextNodeRotation(lastNode, position);

//        nodes.Add(Instantiate(nodePrefab, transform.position, Quaternion.identity));
//        //nodes.Add(Instantiate(nodePrefab, position, rotation));
//    }

//    private Vector3 NextNodePosition(Transform previousNode)
//    {
//        Quaternion currentRotation = Quaternion.Euler(0, previousNode.eulerAngles.y, 0);
//        Vector3 position = previousNode.position;
//        position -= previousNode.forward * nodeBondDistance;
//        return position;
//    }

//    private Quaternion NextNodeRotation(Transform previousNode, Vector3 position)
//    {
//        return Quaternion.LookRotation(previousNode.position - position, previousNode.up);
//        //return Quaternion.Euler(0, previousNode.eulerAngles.y, 0);
//    }

//    private Transform LastNode()
//    {
//        if (nodes.Count > 0)
//        {
//            return nodes[nodes.Count - 1].transform;
//        }
//        else
//        {
//            return hookOwner.transform;
//        }
//    }

//    //Called when the player uses the grappling hook while already latched onto something
//    //Called from the Shoot Script :(
//    public void PullFromLatch()
//    {
//        if (latchedObject.tag == "Player")
//        {
//            transform.parent = null;
//            latchedObject.transform.parent = transform;
//            print("Pulling");
//            StartTakeBack();
//        }
//        else if (latchedObject.tag == "GrabbableEnvironment")
//        {
//            nodes.Reverse();
//            StartReverse();
//        }
//    }

//    //Initialised Latching
//    private void StartLatching(GameObject grabbedObject)
//    {
//        if (latchedObject == null)
//        {
//            latchedObject = grabbedObject;
//            hookStatus = HookStatus.latching;
//            latchCurrentTime = Time.time;

//            //Initialise the Hinge joint
//            //if (!LastNode().GetComponent<HingeJoint>())
//            //{
//            LastNode().position = latchedObject.gameObject.transform.position;

//            if (latchedObject.gameObject.tag == "Player")
//            {
//                //ConfigurableJoint hinge = LastNode().gameObject.AddComponent<ConfigurableJoint>() as ConfigurableJoint;
//                ConfigurableJoint hinge = LastNode().gameObject.GetComponent<ConfigurableJoint>();
//                hinge.connectedBody = latchedObject.GetComponent<Rigidbody>();
//            }
//            else if (latchedObject.gameObject.tag == "GrabbableEnvironment")
//            {
//                //Connect a hinge joint between the grabbable wall and its closest node
//                //HingeJoint hinge = latchedObject.AddComponent<HingeJoint>() as HingeJoint;
//                //hinge.connectedBody = LastNode().GetComponent<Rigidbody>();

//                //Connect a hinge joint between the player and the closest node
//                //SpringJoint ownerHinge = nodes[0].AddComponent<SpringJoint>() as SpringJoint;
//                //ownerHinge.connectedBody = player.GetComponent<Rigidbody>();
//                //ownerHinge.spring = 1000;
//                //ownerHinge.GetComponent<Rigidbody>().mass = 1000;

//                //Connect a series of configurable joints
//                for (int i = 0; i < nodes.Count; i++)
//                {
//                    if (i++ >= nodes.Count)
//                    {
//                        nodes[i].GetComponent<ConfigurableJoint>().connectedBody = latchedObject.GetComponent<Rigidbody>();
//                    }
//                    else
//                    {
//                        nodes[i].GetComponent<ConfigurableJoint>().connectedBody = nodes[i + 1].GetComponent<Rigidbody>();
//                    }
//                }
//                //Reverse the node order such that the wall is the "owner" now
//                //nodes.Reverse();
//            }
//            //}


//            ////Initialise the spring joint
//            //if (!LastNode().GetComponent<SpringJoint>())
//            //{
//            //    SpringJoint spring = LastNode().gameObject.AddComponent<SpringJoint>() as SpringJoint;
//            //    spring.maxDistance = nodeBondDistance / 2;
//            //    spring.minDistance = 0;

//            //    spring.connectedBody = latchedObject.GetComponent<Rigidbody>();
//            //    spring.spring = 50;

//            //    SpringJoint ownerSpring = nodes[0].AddComponent<SpringJoint>() as SpringJoint;
//            //    ownerSpring.maxDistance = nodeBondDistance / 2;
//            //    ownerSpring.minDistance = 0;

//            //    ownerSpring.connectedBody = latchedObject.GetComponent<Rigidbody>();
//            //    ownerSpring.spring = 50;
//            //}
//        }
//    }

//    private void StartTakeBack()
//    {
//        //nodes.Reverse();
//        hookStatus = HookStatus.takeback;
//    }

//    private void StartReverse()
//    {
//        //nodes.Reverse();
//        hookStatus = HookStatus.reverse;
//        hookOwner = gameObject;
//    }

//    //Call before destroying hook
//    private void FinishHookSequence()
//    {
//        player.GetComponent<Shoot>().canHook = true;
//        Destroy(gameObject);
//    }



//    private void OnTriggerEnter(Collider other)
//    {
//        if (latchedObject == null)
//        {
//            if (other.tag == "Player" && other.gameObject != hookOwner)
//            {
//                StartLatching(other.gameObject);
//                //StartTakeBack();
//                //other.transform.parent = transform;
//                print("Player hit");
//                transform.parent = other.transform;
//                return;
//            }
//            if (other.tag == "GrabbableEnvironment")
//            {
//                StartLatching(other.gameObject);
//                //StartReverse();
//                gameObject.transform.position = other.transform.position;
//                return;
//            }
//        }
//    }

//}