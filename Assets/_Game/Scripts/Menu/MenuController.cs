using _Game.Scripts.Events;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Menu
{
    public class MenuController : IService
    {
        private MenuView _menuView;
        private StartEvent _startEvent;

        public void Initialize()
        {
            _menuView = CreateMenuView();
            _startEvent = new StartEvent();
            _startEvent.InitGame();
            _menuView.StartButton.onClick.AddListener(OnStartClick);
            
            Enable();
        }

        private void OnStartClick()
        {
            _startEvent.StartGame();
            Disable();
        }

        private void Enable()
        {
            _menuView.gameObject.SetActive(true);
        }

        private void Disable()
        {
            _menuView.gameObject.SetActive(false);
        }

        private MenuView CreateMenuView()
        {
            var asset = Resources.Load<MenuView>("Prefabs/UI/MenuView");
            return Object.Instantiate(asset);
        }
    }
}