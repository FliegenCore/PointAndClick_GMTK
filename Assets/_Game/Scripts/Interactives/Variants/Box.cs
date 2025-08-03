using System;
using System.Collections;
using _Game.Scripts.AISystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.HelpSystem;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Interactives.Variants
{
    public class Box : Interactive, IItemNeeder
    {
        private AIView _aiView;
        [SerializeField] private bool _haveBox;
        [SerializeField] private GameObject _box;
        
        public bool HaveBox => _haveBox;
        public Vector2 NeederPosition => transform.position;

        private void Awake()
        {
            _aiView = FindObjectOfType<AIView>();
        }

        public override string GetName()
        {
            return "Box place";
        }

        public void DisableBox()
        {
            _box.SetActive(false);
            _haveBox = false;
        }

        public void EnableBox()
        {
            _box.SetActive(true);
            _haveBox = true;
        }
        
        public override IEnumerator Interact()
        {
            yield return null;
            
            if (_haveBox)
            {
                G.Get<InventoryController>().AddItem("box");
                _box.SetActive(false);
                _haveBox = false;
                //плюс время ко дню
            }
            else
            {
                G.Get<HelpTooltipController>().SpawnHelp(new Vector2(transform.position.x, transform.position.y + 2), 
                    "There is nothing");
            }
        }

        public bool TakeItem(string item)
        {
            if ("box" == item)
            {
                _box.SetActive(true);
                _haveBox = true;
                G.Get<DayController>().NextTimeDay();
                G.Get<DialogueSystem>().StartDialogue(_aiView.GetDialogue("scoreinfo"), _aiView);
                return true;
            }
            
            return false;
        }

        public bool NeedItem(string item)
        {
            if ("box" == item)
            {
                return true;
            }
            
            return false;
        }
    }
}