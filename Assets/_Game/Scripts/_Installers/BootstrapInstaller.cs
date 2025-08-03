using _Game.Scripts.Fader;
using _Game.Scripts.SceneManagment;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts._Installers
{
    public class BootstrapInstaller : MonoBehaviour
    {
        private void Awake()
        {
            Register();
            InitializeServices();
        }

        private void Register()
        {
            SceneLoader.WasLoaded = true;
            G.InstantiateAndRegisterService<RoutineStarter>(ServiceLifetime.Singleton);
            G.Register(new FadeController(), ServiceLifetime.Singleton);
            G.Register(new SceneController(), ServiceLifetime.Singleton);

        }

        private void InitializeServices()
        {
            G.InitializeServices();
            
            G.Get<SceneController>().ChangeScene(SceneController.CORE_SCENE);
        }
    }
}