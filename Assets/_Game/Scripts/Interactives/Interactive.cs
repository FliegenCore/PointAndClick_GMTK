using System.Collections;
using UnityEngine;

namespace _Game.Scripts.Interactives
{
    public abstract class Interactive : MonoBehaviour
    {
        public abstract string GetName();
        public Vector3 Position => transform.position;
        
        public abstract IEnumerator Interact();
    }
}