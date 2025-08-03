using _Game.Scripts.Interactives;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace _Game.Scripts.PlayerInput.Variants
{
    public class PlayerTargetInput : Interactor
    {
        private Raycaster _raycaster;
        private Interactive _lastInteractive;
        private InteractiveHandler _interactiveHandler;

        private Vector2 _lastWorldPos;
        
        public PlayerTargetInput(Raycaster raycaster)
        {
            _interactiveHandler = G.Get<InteractiveHandler>();
            _raycaster = raycaster;
        }
        
        public override void SelfUpdate()
        {
            if (OnClickUp())
            {
                _interactiveHandler.Interact(_lastInteractive, _lastWorldPos.x);
                _lastInteractive = null;
            }
        }
        
        public override bool TryInteract()
        {
            if (EventSystem.current != null && 
                       EventSystem.current.IsPointerOverGameObject())
            {
                return false;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                if (_raycaster.TryGetInteractiveObject(Input.mousePosition, 
                        out Interactive interactive, out Vector2 worldPos))
                {
                    _lastInteractive = interactive;
                    _lastWorldPos = worldPos;
                }
                
                _lastInteractive = interactive;
                _lastWorldPos = worldPos;
            }
            
            return true;
        }
    }
}