using UnityEngine;

namespace VarjoExample
{
    public class Interactable : MonoBehaviour
    {
        [HideInInspector]
        public Hand activeHand = null;
    }
}