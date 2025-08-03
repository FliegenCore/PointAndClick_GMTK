using Game.ServiceLocator;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.CapsuleSystem
{
    public class CapsuleController : IService
    {
        private Capsule _capsule;
        
        public void Initialize()
        {
            _capsule = Object.FindObjectOfType<Capsule>();
        }

        public void DropCapsule()
        {
            
        }

        public void JustEnableCapsule()
        {
            
        }
    }
}