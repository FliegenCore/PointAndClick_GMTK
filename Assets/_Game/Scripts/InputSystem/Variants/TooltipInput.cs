using _Game.Scripts.Interactives;
using _Game.Scripts.TooltipUI;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class TooltipInput 
    {
        private Raycaster _raycaster;
        private Interactive _interactive;
        private TooltipView _tooltipView;
        
        public TooltipInput(Raycaster raycaster)
        {
            _raycaster = raycaster;
            
            Init();
        }

        private void Init()
        {
            _tooltipView = CreateTooltipView();
            _tooltipView.Disable();
        }

        public void SelfUpdate()
        {
            TryGetInteractable();
        }

        private void TryGetInteractable()
        {
            Vector2 screenPoint = Input.mousePosition;
            
            if (_raycaster.TryGetInteractiveObject(screenPoint, out var interactive, out Vector2 worldPosition))
            {
                Vector2 offset = new Vector2(0, 1);
                _tooltipView.transform.position = worldPosition + offset;
                _interactive = interactive;
                _tooltipView.SetText(_interactive.GetName());
                _tooltipView.Enable();
            }
            else
            {
                if (_interactive != null)
                {
                    _tooltipView.Disable();
                    _interactive = null;
                }
            }
        }

        private TooltipView CreateTooltipView()
        {
            var tooltipView = Resources.Load<TooltipView>("Prefabs/UI/TooltipView");
            return Object.Instantiate(tooltipView);
        }
    }
}