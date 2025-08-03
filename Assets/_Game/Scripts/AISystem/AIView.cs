using System;
using _Game.Scripts.Dialogues;
using UnityEngine;

namespace _Game.Scripts.AISystem
{
    public class AIView : Speaker
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _goodSprite;
        [SerializeField] private Sprite _badSprite;

        private void Awake()
        {
            SetDialogue(GetDialogue("talk"));
        }

        public void EnableBad()
        {
            _audioSource.Play();
            _spriteRenderer.sprite = _badSprite;
        }

        public void EnableGood()
        {
            _audioSource.Play();
            _spriteRenderer.sprite = _goodSprite;
        }
    }
}