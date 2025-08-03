using System;
using System.Collections;
using System.Text.RegularExpressions;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.Utils;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts.YangBot
{
    public class YangBotView : Speaker, IReactToTheDay
    {
        [SerializeField] private GameObject _skelet;
        [SerializeField] private GameObject _knife;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Rigidbody2D _rb;

        public bool Busy;
        
        private Sequence _moveSequence;

        private void Awake()
        {
            SetDialogue(GetDialogue("talk"));
        }

        public override IEnumerator Interact()
        {
            if (Busy)
            {
                yield break;
            }
            
            yield return base.Interact();
        }

        private Vector2 _lastTarget;

        public void Move(Transform transformDirection, Action callback)
        {
            _moveSequence?.Kill();

            Vector2 GetTargetPosition()
            {
                float x = transformDirection.position.x; 
                float offset = transform.position.x > transformDirection.position.x ? 1.5f : -1.5f;
                return new Vector2(x + offset, _rb.position.y);
            }

            _lastTarget = GetTargetPosition();
            float distance = Vector2.Distance(_rb.position, _lastTarget);
            float duration = distance / 5f;

            _moveSequence = DOTween.Sequence();
    
            _moveSequence.Append(
                DOTween.To(
                        () => _rb.position,
                        pos => _rb.MovePosition(pos),
                        _lastTarget,
                        duration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => 
                    {
                        Vector2 newTarget = GetTargetPosition();
                        if (Vector2.Distance(_rb.position, newTarget) > 0.5f) 
                        {
                            Move(transformDirection, callback);
                        }
                        else 
                        {
                            callback?.Invoke();
                        }
                    })
            );

            Rotate(_lastTarget.x);
        }
        
        private Vector2 GetDynamicTarget(Transform target)
        {
            float x = target.position.x;
            float offset = transform.position.x > target.position.x ? 1.5f : -1.5f;
            return new Vector2(x + offset, _rb.position.y);
        }

        private void Rotate(float xPosition)
        {
            if (transform.position.x > xPosition)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void EnableSkelet()
        {
            _skelet.gameObject.SetActive(true);
        }

        public void DisableSkelet()
        {
            _skelet.gameObject.SetActive(false);
        }

        public void ShowKnife()
        {
            _knife.SetActive(true);
        }

        public void HideKnife()
        {
            _knife.SetActive(false);
        }
        
        public void ReactToTheDay()
        {
            string nme = _spriteRenderer.sprite.name;
            Match match = Regex.Match(nme, @"\d+");

            int years = 0;
            
            if (match.Success)
            {
                years = int.Parse(match.Value);
                Debug.Log(years);
            }
            
            _spriteRenderer.sprite = SpriteLoader.LoadOlderSprite(years);
        }
    }
}