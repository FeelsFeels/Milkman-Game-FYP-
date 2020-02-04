using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewBeans.InstructionsScreen
{
    public abstract class BaseSimulation : MonoBehaviour
    {
        public AIPlayerInputController player1;
        public AIPlayerInputController player2;

        public abstract void StartSimulation();
        public abstract void ResetSimulation();
    }
}
