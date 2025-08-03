using System;
using System.Collections;
using System.Text.RegularExpressions;
using _Game.Scripts.AISystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.Interactives.Variants;
using _Game.Scripts.Inventory;
using _Game.Scripts.Player;
using _Game.Scripts.Utils;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.GrandpaSystem
{
    public class Grandpa : Speaker, IItemNeeder, IReactToTheDay
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioSource _eatSound;
        [SerializeField] private string[] _itemNeedName;
        
        private PlayerView _playerView;
        private int _health;
        private PlayerController _playerController;

        public Vector2 NeederPosition => transform.position;

        private void Start()
        {
            _health = 1;
            _playerController = G.Get<PlayerController>();
            _playerController.OnPlayerSpawn += Init;
        }

        private void Init()
        {
            _playerView = _playerController.PlayerView;
        }

        private void Update()
        {
            if (_playerView == null)
            {
                _playerView = _playerController.PlayerView;
                return;
            }
            
            Rotate();
        }
        
        public bool TakeItem(string item)
        {
            foreach (var nme in _itemNeedName)
            {
                if (nme == item)
                {
                    if (item == "berries")
                    {
                        G.Get<DialogueSystem>().StartDialogue(GetDialogue("eat_dialogue"), this);
                        G.Get<DayController>().NextTimeDay();
                        _health++;
                        _eatSound.Play();
                    }

                    return true;
                }
            }
            
            return false;
        }

        public bool NeedItem(string item)
        {
            foreach (var nme in _itemNeedName)
            {
                if (nme == item)
                {
                    return true;
                }
            }
            
            return false;
        }

        private void Rotate()
        {
            if (_playerView.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        public void ReactToTheDay()
        {
            _health--;

            if (_health <= 0)
            {
                G.Get<RoutineStarter>().StartCoroutine(WaitDeathEvent());
                Died();
                
                return;
            }
            
            string nme = _spriteRenderer.sprite.name;
            Match match = Regex.Match(nme, @"\d+");

            int years = 0;
            
            if (match.Success)
            {
                years = int.Parse(match.Value);
                Debug.Log(years);
            }
            else
            {
                Debug.Log("Цифры не найдены в имени спрайта");
            }
                        
            _spriteRenderer.sprite = SpriteLoader.LoadOlderSprite(years);
        }

        private IEnumerator WaitDeathEvent()
        {
            yield return new WaitForSeconds(2f);
            
            AIView aiView = FindObjectOfType<AIView>();
            Dialogue dialogue = aiView.GetDialogue("hunger_grandpa_death");
            G.Get<DialogueSystem>().StartDialogue(dialogue, aiView);
        }
        
        public void Died()
        {
            gameObject.SetActive(false);
            SpawnSkelet();
        }

        private void SpawnSkelet()
        {
            var asset = Resources.Load<Skelet>("Prefabs/Dead");
            Instantiate(asset, transform.position, Quaternion.identity);
        }
    }
}
