using System.Collections;
using _Game.Scripts.Interactives;
using UnityEngine;

namespace _Game.Scripts.CapsuleSystem
{
    public class Capsule : Interactive
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private GameObject _decor;

        public void EnableSound()
        {
            _audioSource.Play();
        }
        
        public void EnableDecor()
        {
            _decor.SetActive(true);
        }

        public void DisableDecor()
        {
            _decor.SetActive(false);
        }
        
        public override string GetName()
        {
            return "Capsule";
        }

        public override IEnumerator Interact()
        {
            yield return null;
            
            //check player years if > 50 interact
        }
    }
}
