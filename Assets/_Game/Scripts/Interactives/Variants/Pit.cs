using System.Collections;
using _Game.Scripts.Dialogues;
using _Game.Scripts.HelpSystem;
using _Game.Scripts.Interactives;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PitSystem
{
    public class Pit : Interactive, IItemNeeder
    {
        public Vector2 NeederPosition => transform.position;
        
        public bool TakeItem(string item)
        {
            G.Get<HelpTooltipController>().SpawnHelp(new Vector2(transform.position.x, transform.position.y + 2),
                $"You just threw away the {item}, lol");
            return true;
        }

        public bool NeedItem(string item)
        {
            return true;
        }

        public override string GetName()
        {
            return "Pit";
        }

        public override IEnumerator Interact()
        {
            yield return new WaitForSeconds(0.1f);
            G.Get<HelpTooltipController>().SpawnHelp(new Vector2(transform.position.x, transform.position.y + 2), $"Just pit.");
        }
    }
}
