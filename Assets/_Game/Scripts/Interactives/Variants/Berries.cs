using System;
using System.Collections;
using _Game.Scripts.DaySystem;
using _Game.Scripts.HelpSystem;
using _Game.Scripts.Interactives;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class Berries : Interactive, IReactToTheDay
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _berriesSpot;
        [SerializeField] private Sprite _emptyBerriesSpot;
        
        private bool _active;

        public bool Active => _active;
        
        private void Awake()
        {
            Activate();
        }

        public override string GetName()
        {
            return "Berries";
        }

        public override IEnumerator Interact()
        {
            if (!_active)
            {
                G.Get<HelpTooltipController>().SpawnHelp(transform.position, "No berries");
                yield break;
            }
            
            yield return new WaitForSeconds(0.1f);
            G.Get<InventoryController>().AddItem("berries");
            G.Get<DayController>().NextTimeDay();
            Deativate();
        }
        
        private void Activate()
        {
            _active = true;
            _spriteRenderer.sprite = _berriesSpot;
        }
        
        public void Deativate()
        {
            _active = false;
            _spriteRenderer.sprite = _emptyBerriesSpot;
        }

        public void ReactToTheDay()
        {
            Activate();
        }
    }
}