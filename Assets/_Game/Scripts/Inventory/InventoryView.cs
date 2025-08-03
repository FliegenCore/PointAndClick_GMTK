using System.Collections.Generic;
using _Game.Scripts.Dialogues;
using UnityEngine;

namespace _Game.Scripts.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private List<InventoryCell> _cells;

        public IReadOnlyList<InventoryCell> Cells => _cells;
    }
}