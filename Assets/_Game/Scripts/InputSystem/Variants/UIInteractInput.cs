using System.Collections.Generic;
using _Game.Scripts.Dialogues;
using _Game.Scripts.HelpSystem;
using _Game.Scripts.Inventory;
using _Game.Scripts.Player;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Game.Scripts.PlayerInput.Variants
{
    public class UIInteractInput : Interactor
    {
        private Canvas _canvas;
        private UIItem _draggedItem;
        private Raycaster _raycaster;
        private Vector2 _offset;
        private PlayerController _playerController;
        
        public UIInteractInput(Raycaster raycaster,Canvas canvas)
        {
            _raycaster = raycaster;
            _canvas = canvas;
            _playerController = G.Get<PlayerController>();
        }
        
        public override void SelfUpdate()
        {
            if (Input.GetMouseButtonUp(0) && _draggedItem != null)
            {
                EndDrag();
            }

            DraggingItem();
        }
        
        public override bool TryInteract()
        {
            if (Input.GetMouseButton(0))
            {
                if (TryGetUIItemUnderMouse(out var item))
                {
                    _draggedItem = item;
                    
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _canvas.transform as RectTransform,
                        Input.mousePosition,
                        _canvas.worldCamera,
                        out Vector2 localPoint);
                    
                    _offset = (Vector2)_draggedItem.transform.localPosition - localPoint;
                    return true;
                }
            }
            
            return false;
        }

        private void EndDrag()
        {
            if (TryGiveItem())
            {
                _draggedItem.transform.localPosition = Vector3.zero;
                _draggedItem = null;
                return;
            }
            
            if (_draggedItem != null)
            {
                _draggedItem.transform.localPosition = Vector3.zero;
            }
            
            _draggedItem = null;
        }

        private bool TryGiveItem()
        {
            if (_raycaster.TryGetItemNeederObject(Input.mousePosition, out IItemNeeder needer, out Vector2 worldClickPosition))
            {
                if (needer.NeedItem(_draggedItem.Id))
                {
                    MoveAndGiveItem(needer, _draggedItem.Id);
                    return true;
                }
                else
                {
                    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    G.Get<HelpTooltipController>().SpawnHelp(pos, "Nothing will come of it.");
                    
                    return false;
                }
                
            }
            
            return false;
        }

        private void MoveAndGiveItem(IItemNeeder needer, string itemName)
        {
            _playerController.MoveForTakeItem(needer.NeederPosition.x, needer, itemName);
        }
        
        private void DraggingItem()
        {
            if (_draggedItem == null) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                Input.mousePosition,
                _canvas.worldCamera,
                out Vector2 localPoint);
                
            _draggedItem.transform.localPosition = localPoint + _offset;
        }
        
        private bool TryGetUIItemUnderMouse(out UIItem item)
        {
            item = null;
            
            if (EventSystem.current == null) return false;
            
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                item = result.gameObject.GetComponent<UIItem>();
                if (item != null)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}