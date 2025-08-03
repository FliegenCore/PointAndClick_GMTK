using System.Collections;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;

namespace _Game.Scripts.Interactives.Variants
{
    public class Gift : Interactive
    {
        public override string GetName()
        {
            return "Gift";
        }

        public override IEnumerator Interact()
        {
            yield return null;
            gameObject.SetActive(false);
            G.Get<InventoryController>().AddItem("gift");
        }
    }
}