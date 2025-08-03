using _Game.Scripts.Interactives;
using _Game.Scripts.Inventory;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class Raycaster
    {
        private Camera _camera;
        
        public Raycaster(Camera camera)
        {
            _camera = camera;
        }

        public bool TryGetInteractiveObject(Vector2 screenPosition, out Interactive interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f);
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }

        public bool TryGetItemNeederObject(Vector2 screenPosition, out IItemNeeder interactiveObject, out Vector2 worldClickPosition)
        {
            interactiveObject = null;
            worldClickPosition = Vector2.zero;
            
            worldClickPosition = _camera.ScreenToWorldPoint(screenPosition);
            
            RaycastHit2D hit = Physics2D.Raycast(worldClickPosition, Vector2.zero, 0f);
            
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out interactiveObject))
                {
                    worldClickPosition = hit.point;
                    return true;
                }
            }
            
            return false;
        }
    }
}