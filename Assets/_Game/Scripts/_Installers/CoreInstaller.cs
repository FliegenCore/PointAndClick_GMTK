using _Game.Scripts.CameraSystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.HelpSystem;
using _Game.Scripts.Interactives;
using _Game.Scripts.Inventory;
using _Game.Scripts.Menu;
using _Game.Scripts.Player;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using _Game.Scripts.WhiteScreen;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class CoreInstaller : MonoBehaviour
    {
        [SerializeField] private InventoryController _inventoryController;
        [SerializeField] private DialogueSystem _dialogueSystem;
        
        private void Awake()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
#if UNITY_EDITOR
            if (SceneLoader.WasLoaded == false)
            {
                SceneLoader.LoadFirstScene();
                return;
            }
#endif
            
            G.Register(new PlayerController(), ServiceLifetime.Transient);
            G.Register(new InteractiveHandler(), ServiceLifetime.Transient);
            G.Register(_inventoryController, ServiceLifetime.Transient);
            G.Register(_dialogueSystem, ServiceLifetime.Transient);
            G.Register(new WhiteScreenController(), ServiceLifetime.Transient);
            G.Register(new YangBotController(), ServiceLifetime.Transient);
            G.Register(new HelpTooltipController(), ServiceLifetime.Transient);
            G.Register(new DayController(), ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<RootInput>(ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<CameraController>(ServiceLifetime.Transient);
            G.InstantiateAndRegisterService<ReloadScene>(ServiceLifetime.Transient);
            G.Register(new MenuController(), ServiceLifetime.Transient);
        }

        private void InitializeServices()
        {
            G.InitializeServices();
        }
    }
}