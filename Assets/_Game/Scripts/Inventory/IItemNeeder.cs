using System;
using UnityEngine;

namespace _Game.Scripts.Inventory
{
    public interface IItemNeeder
    {
        Vector2 NeederPosition { get; }
        bool TakeItem(string item);
        bool NeedItem(string item);
    }
}