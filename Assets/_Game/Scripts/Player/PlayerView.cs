using System;
using System.Collections;
using System.Text.RegularExpressions;
using _Game.Scripts.AISystem;
using _Game.Scripts.CameraSystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.Interactives;
using _Game.Scripts.Interactives.Variants;
using _Game.Scripts.Inventory;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerView : MonoBehaviour, IItemNeeder, IReactToTheDay
    {
        [SerializeField] private AudioSource _eatSource;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _speed;
        [SerializeField]private SpriteRenderer _spriteRenderer;
        private int _direction;
        private Action _onEndMoveCallback;
        private Sequence _moveSequence;
        private float _target;
        private bool _isMove;
        private float _lastXPos;
        private Interactive _lastInteractive;
        private bool _canDeath;        
        private int _health;
        private int _years = 1;
        private bool _ddeath;

        public int Direction => _direction;
        public bool CanDeath => _canDeath;
        
        private void Awake()
        {
            _health = 2;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                G.Get<DayController>().NextTimeDay();
            }
        }
#endif

        public void SetTargetPosition(float xPosition, Interactive interactive)
        {
            _moveSequence?.Kill();

            if (interactive != null)
            {
                xPosition = interactive.Position.x;

                if (transform.position.x > xPosition)
                {
                    xPosition += 1.5f;
                }
                else
                {
                    xPosition -= 1.5f;
                }
            }
            
            if (transform.position.x > xPosition)
            {
                _direction = -2;
            }
            else
            {
                _direction = 2;
            }

            Rotate(xPosition);
            _lastXPos = xPosition;

            _lastInteractive = interactive;
            Vector2 targetPos = new Vector2(xPosition, _rb.position.y);
            float distance = Mathf.Abs(_rb.position.x - xPosition);
            float duration = distance / _speed;

            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(
            DOTween.To(
                    () => _rb.position,
                    pos => _rb.MovePosition(pos),
                    targetPos,
                    duration
                )
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_lastInteractive != null)
                    {
                        G.Get<RoutineStarter>().StartCoroutine(_lastInteractive.Interact());
                        Rotate(_lastInteractive.Position.x);
                    }
                }));
        }

        public void Stop()
        {
            _moveSequence?.Kill();
        }

        public void SetTargetOnTakeItem(float xPosition, IItemNeeder interactive, string itemName)
        {
            _moveSequence?.Kill();

            if (interactive != null && interactive != this)
            {
                xPosition = interactive.NeederPosition.x;

                if (transform.position.x > xPosition)
                {
                    xPosition += 1.5f;
                }
                else
                {
                    xPosition -= 1.5f;
                }
            }
                
            if (transform.position.x > xPosition)
            {
                _direction = -2;
            }
            else
            {
                _direction = 2;
            }
            
            Rotate(xPosition);
            _lastXPos = xPosition;

            Vector2 targetPos = new Vector2(xPosition, _rb.position.y);
            float distance = Mathf.Abs(_rb.position.x - xPosition);
            float duration = distance / _speed;

            _moveSequence = DOTween.Sequence();
            _moveSequence.Append(
                DOTween.To(
                        () => _rb.position,
                        pos => _rb.MovePosition(pos),
                        targetPos,
                        duration
                    )
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (interactive != null)
                        {
                            G.Get<InventoryController>().RemoveItem(itemName);

                            interactive.TakeItem(itemName);
                            Rotate(interactive.NeederPosition.x);
                        }
                    }));
        }

        private void Rotate(float x)
        {
            if (transform.position.x > x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public Vector2 NeederPosition => transform.position;
        public bool TakeItem(string item)
        {
            if (item == "gift" )
            {
                AIView aiView = FindObjectOfType<AIView>();
                Dialogue dialogue = aiView.GetDialogue("scoreinfo");
                G.Get<DialogueSystem>().StartDialogue(dialogue, aiView);
                return true;
            }
            if (_canDeath && item == "berries")
            {
                _health++;
                _eatSource.Play();
                return true;
            }
            
            return false;
        }

        public bool NeedItem(string item)
        {
            if (item == "gift")
            {
                return true;
            }

            if (_canDeath && item == "berries")
            {
                return true;
            }
            
            return false;
        }

        public void EnableProcessOfDeath()
        {
            _canDeath = true;
        }
        
        public void Dead()
        {
            G.Get<RootInput>().PauseSystem();
            G.Get<RootInput>().CanUnpause = false;
            if (_ddeath)
            {
                return;
            }

            _ddeath = true;
            SpawnSkeleton();
            G.Get<RoutineStarter>().StartCoroutine(WaitThrowSkeleton());
            gameObject.SetActive(false);
        }

        private IEnumerator WaitThrowSkeleton()
        {
            if (G.Get<YangBotController>().YangBotView == null)
            {
                AIView aiView = FindObjectOfType<AIView>();
                Dialogue dialogue = aiView.GetDialogue("alone_death");
                G.Get<DialogueSystem>().StartDialogue(dialogue, aiView);
                
                yield break;
            }
            
            G.Get<CameraController>().SetTarget(G.Get<YangBotController>().YangBotView.transform);
            yield return new WaitForSeconds(0.5f);
            G.Get<YangBotController>().ThrowPlayerSkeleton();
        }   
        
        private void SpawnSkeleton()
        {
            var asset = Resources.Load<Skelet>("Prefabs/Dead");
            Instantiate(asset, transform.position, Quaternion.identity);
        }
        
        public void ReactToTheDay()
        {
            if (_canDeath)
            {
                _health--;
            }

            if (_health <= 0)
            {
                Dead();
                return;
            }
            
            string nme = _spriteRenderer.sprite.name;
            Match match = Regex.Match(nme, @"\d+");

            int years = 0;
            
            if (match.Success)
            {
                years = int.Parse(match.Value);
            }
            
            _spriteRenderer.sprite = SpriteLoader.LoadOlderSprite(years);

            if (years >= 10)
            {
                Dead();
            }
        }
    }
}
