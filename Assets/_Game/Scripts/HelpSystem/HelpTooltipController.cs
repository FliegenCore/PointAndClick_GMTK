using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.HelpSystem
{
    public class HelpTooltipController : IService
    {
        private HelpTooltipView _prefab;
        
        public void Initialize()
        {
            _prefab = Resources.Load<HelpTooltipView>("Prefabs/UI/HelpTooltipView");
            
        }
        
        public void SpawnHelp(Vector2 position, string text)
        {
            HelpTooltipView view = InstantiateTooltip(position);
            view.DoAnimation(text);
        }

        private HelpTooltipView InstantiateTooltip(Vector2 position)
        {
            return Object.Instantiate(_prefab, position, Quaternion.identity);
        }
    }
}