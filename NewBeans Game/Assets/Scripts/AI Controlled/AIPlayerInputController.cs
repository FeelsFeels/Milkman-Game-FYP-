using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public class AIPlayerInputController : MonoBehaviour
    {
        public AIPlayerController playerController;
        public AIShoot playerShoot;

        public enum Direction
        {
            N, NE, E, SE, S, SW, W, NW, Still
        }

        private void Awake()
        {
            playerController = GetComponent<AIPlayerController>();
            playerShoot = GetComponent<AIShoot>();
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.N:
                    playerController.Move(new Vector3(0, 0, 1).normalized);
                    break;
                case Direction.NE:
                    playerController.Move(new Vector3(1, 0, 1).normalized);
                    break;
                case Direction.E:
                    playerController.Move(new Vector3(1, 0, 0).normalized);
                    break;
                case Direction.SE:
                    playerController.Move(new Vector3(1, 0, -1).normalized);
                    break;
                case Direction.S:
                    playerController.Move(new Vector3(0, 0, -1).normalized);
                    break;
                case Direction.SW:
                    playerController.Move(new Vector3(-1, 0, -1).normalized);
                    break;
                case Direction.W:
                    playerController.Move(new Vector3(-1, 0, 0).normalized);
                    break;
                case Direction.NW:
                    playerController.Move(new Vector3(-1, 0, 1).normalized);
                    break;
                case Direction.Still:
                    playerController.Move(Vector3.zero);
                    break;
                default:
                    break;
            }
        }

        public void Move(Vector3 direction)
        {
            playerController.Move(direction);
        }

        public void Turn(Vector3 direction)
        {
            playerController.Turn(direction);
        }

        public void HoldShootButton()
        {
            playerShoot.HoldShootButton();
        }
        public void ReleaseShootButton()
        {
            playerShoot.ReleaseShootButton();
        }
        public void HoldPullButton()
        {
            playerShoot.HoldPullButton();
        }
        public void ReleasePullButton()
        {
            playerShoot.ReleasePullButton();
        }
    }
}