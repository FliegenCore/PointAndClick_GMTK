using System.Collections;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;

namespace _Game.Scripts.Interactives.Variants
{
    public class Skelet : Interactive
    {
        public override string GetName()
        {
            return "Skeleton";
        }

        public override IEnumerator Interact()
        {
            yield return null;
            G.Get<InventoryController>().AddItem("skelet");
            gameObject.SetActive(false);
        }
    }
}