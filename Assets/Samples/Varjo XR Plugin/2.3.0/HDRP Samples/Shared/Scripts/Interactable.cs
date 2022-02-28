using UnityEngine;

namespace VarjoExample
{
    [RequireComponent(typeof(ArticulationBody))]
    public class Interactable : MonoBehaviour
    {
        [HideInInspector]
        public Hand activeHand = null;
    }
}