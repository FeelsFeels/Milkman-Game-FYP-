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
        latching,
        none
    }

    ///Latching is currently obsoltete
    /// shooting: Going Forwards
    /// takeback: Coming back towards player
    /// reverse: Bringing player towards the hook leader(the pointier part)
    /// latching: hooked onto an object.


    public HookStatus hookStatus = HookStatus.shooting;

    public GameObject hookOwner;        //This refers to the "first node", usually refers to player. 
    public GameObject player;           //Player gameobject. Does NOT change.
    public GameObject latchedObject;

    public GameObject nodePrefab;       //This is the nodes

    public Vector3 direction;           //where the hook leader is going towards. Initialised in Shoot.cs when hook is fired.
    public float speed;                 //How fast hook leader goes when travelling forwards
    
    public float maxNodes;              //Max number of nodes before hook returns
    public float lastNodeUpdateTime;    //The time that a node was last spawned
    public float nodeUpdateTime;        //THE RATE in which that nodes will spawn
    public GameObject nodeToMoveTo;     //When retracting, the node that the hook leader will travel towards.
    public float nodeBondDistance;      //The distance between each node
    public float nodeBondDamping;       //The damping for each connected node
    public float renodeDelay;
    private float currentRenodeDelay;

    public bool willReturn;             //Bool for if hook is supposed to come back.

    private bool releaseOnNext;         //If true and an object is latched, release it the next time a node is removed.

    public List<GameObject> nodes = new List<GameObject>();

    public bool useLineRenderer;
    private LineRenderer lineRenderer;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(!useLineRenderer)
            lineRenderer.enabled = false;

        player = hookOwner;
    }

    private void FixedUpdate()
    {
        HookLogic();

        //Update the position of each hook node.
        for (int i = 0; i < nodes.Count; i++)
        {
            //used when pulling player towards pillar
            if (hookStatus == HookStatus.reverse)
            {
                FollowPreviousNode(i == 0 ? hookOwner.transform : nodes[i - 1].transform, nodes[i].transform);
                //AdjustNodes((i == nodes.Count - 1) ? transform : nodes[i + 1].transform, i == 0 ? hookOwner.transform : nodes[i - 1].transform, nodes[i].transform);
            }
            else
            {
                //The nodes follows the player
                FollowPreviousNode(i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
                //AdjustNodes((i == nodes.Count - 1) ? transform : nodes[i + 1].transform, i == 0 ? player.transform : nodes[i - 1].transform, nodes[i].transform);
            }
        }

        if (useLineRenderer)
        {
            lineRenderer.positionCount = nodes.Count;

            for (int i = 0; i < nodes.Count; i++)
            {
                lineRenderer.SetPosition(i, nodes[i].transform.position);
            }
        }
    }

    private void HookLogic()
    {
        if (hookStatus == HookStatus.shooting)
        {
            //Movement. Direction is set by Shoot.cs
            transform.Translate(direction * speed);
            //If the hook nodes are at its maximum, bring back the hook.
            if(nodes.Count >= maxNodes)
            {
                if (willReturn)
                    StartTakeBack();
                else
                {
                    FinishHookSequence();
                    return;
                }
            }
            //Continue adding nodes while the maximum is not reached.
            if (nodes.Count < maxNodes)
            {
                //If the hook travels [nodeBondDistance] away from the previous node, add another node.
                if (Vector3.Distance(transform.position, LastNode().position) > nodeBondDistance)
                {
                    AddNode();
                }
            }

            //float angle = Quaternion.Angle();

        }

        if (hookStatus == HookStatus.takeback)
        {
            if (Time.time - lastNodeUpdateTime > nodeUpdateTime)
            {
                if (nodeToMoveTo != null)
                {
                    //This block controls when to let go of a player if hooked
                    if (latchedObject != null)
                    {
                        if (!releaseOnNext)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f, 1 << LayerMask.NameToLayer("Ground")))
                            {
                                Tile tile = hit.collider.GetComponent<Tile>();
                                if (tile)
                                {
                                    if (tile.tileState == Tile.TileState.down)
                                    {
                                        releaseOnNext = true;
                                        StartCoroutine(Release());
                                    }
                                }
                            }
                        }
                    }

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

        if (hookStatus == HookStatus.reverse)
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

        if (hookStatus != HookStatus.reverse && hookStatus != HookStatus.takeback)
        {
            //use angle and dot product to decide if player is moving towards hook.
            //then, adjust nodes position
            float angle = Quaternion.Angle(player.transform.rotation, nodes[0].transform.rotation);
            float dotProduct = Vector3.Dot(player.transform.forward, transform.position - player.transform.position);

            if(player.GetComponent<PlayerController>().averageInput != 0 && angle < 250f && dotProduct > 0)
            {
                if(currentRenodeDelay < renodeDelay)
                {
                    currentRenodeDelay++;
                }
                else
                {
                    AddNode();

                    GameObject node = nodes[0];
                    nodes.Remove(node);
                    Destroy(node);

                    currentRenodeDelay = 0;
                }
            }
        }
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
        Vector3 position = previousNode.position;
        position -= previousNode.forward * nodeBondDistance;
        return position;
    }

    private Quaternion NextNodeRotation(Transform previousNode, Vector3 position)
    {
        return Quaternion.LookRotation(previousNode.position - position, previousNode.up);
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

    public void StartTakeBack()
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

    //Call before destroying hook
    private void FinishHookSequence()
    {
        hookStatus = HookStatus.none;
        if (nodes.Count >= 0)
        {
            foreach(GameObject node in nodes)
            {
                Destroy(node);
            }
        }
        player.GetComponent<Shoot>().canHook = true;
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        //Dont need to do anything if colliding with the caster
        if (other.gameObject == hookOwner)
            return;
        //Or if its hitting a shield, because the shield blocks the grapple hook anyway
        if (other.gameObject.tag == "Shield")
            return;

        //Dont need to do anything if currently reversing
        if (hookStatus == HookStatus.reverse)
            return;

        if (latchedObject == null)
        {
            if (other.tag == "Player")
            {
                other.transform.parent = transform;
                other.GetComponent<PlayerController>().lastHitBy = player; // Hooked player gets hooked by the hook owner.
                latchedObject = other.gameObject;
                StartTakeBack();

                //transform.parent = other.transform;
                return;
            }
            else if (other.tag == "GrabbableEnvironment" || other.tag == "Rock")
            {
                StartReverse();
                gameObject.transform.position = other.transform.position;
                return;
            }
            else
            {
                StartTakeBack();
            }
        }
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(0.1f);
        transform.DetachChildren();
        latchedObject.GetComponent<PlayerController>().rb.AddForce(Vector3.down * 1000);
        yield return new WaitForSeconds(0.1f);
        latchedObject = null;
        releaseOnNext = false;
    }


    //Currently unused
    private void AdjustNodes(Transform nextNode, Transform prevNode, Transform node)
    {
        Quaternion targetRotation = Quaternion.LookRotation(prevNode.position - node.position, prevNode.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        node.rotation = Quaternion.Slerp(node.rotation, targetRotation, Time.deltaTime * nodeBondDamping);

        Vector3 targetPosition = prevNode.position;
        targetPosition -= node.transform.rotation * Vector3.forward * nodeBondDistance;
        targetPosition.y = node.position.y;
        node.position = Vector3.Lerp(node.position, targetPosition, Time.deltaTime * nodeBondDamping);

        Vector3 offsetToNext = nextNode.position - node.position;
        Vector3 offsetToPrev = prevNode.position - node.position;
        Vector3 velocity = offsetToPrev * 5.0f + offsetToNext * 5.0f;
        node.position += velocity * Time.deltaTime / 1.0f;
    }
}
