using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookNode : MonoBehaviour
{

    public Transform previousNode;

    private void Update()
    {
        FollowPreviousNode();
    }

    private void FollowPreviousNode()
    {

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
