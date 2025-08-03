using System.Collections;
using _Game.Scripts.Inventory;
using _Game.Scripts.PlayerInput.Variants;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.PlayerInput
{
    public class RootInput : MonoBehaviour, IService
    {
        private Camera _camera;

        private Raycaster _raycaster;
        private PlayerTargetInput _playerTargetInput;
        private UIInteractInput _uiInteractInput;
        private TooltipInput _tooltipInput;

        public bool CanUnpause;
        private Interactor _currentInteractor;
        private bool _isPaused;
        
        public void Initialize()
        {
            CanUnpause = true;
            _camera = Camera.main;
            PostInit();
        }
        
        private void PostInit()
        {
            _raycaster = new Raycaster(_camera);
            _playerTargetInput = new PlayerTargetInput(_raycaster);
            _tooltipInput = new TooltipInput(_raycaster);
            _uiInteractInput = new UIInteractInput(_raycaster, G.Get<InventoryController>().View.GetComponent<Canvas>());
        }

        private void Update()
        {
            if (_isPaused)
            {
                return;
            }
            
            _tooltipInput?.SelfUpdate();
            if(_currentInteractor != null)
                _currentInteractor.SelfUpdate();
            
            ChooseInteractor();
        }

        private void ChooseInteractor()
        {
            if (_uiInteractInput.TryInteract())
            {
                _currentInteractor = _uiInteractInput;
            }
            else if(_playerTargetInput.TryInteract())
            {
                _currentInteractor = _playerTargetInput;
            }
        }
        
        public void PauseSystem()
        {
            _isPaused = true;
        }

        public void UnpauseSystem()
        {
            if (!CanUnpause)
            {
                return;
            }
            
            StartCoroutine(WaitUnpause());
        }

        private IEnumerator WaitUnpause()
        {
            yield return new WaitForSeconds(0.05f);
            _isPaused = false;
        }
    }
}

