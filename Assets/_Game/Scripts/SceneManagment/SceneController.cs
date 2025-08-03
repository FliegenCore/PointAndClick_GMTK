using System.Collections;
using _Game.Scripts.Fader;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.SceneManagment
{
    public class SceneController : IService
    {
        public const string CORE_SCENE = "CoreScene";
        public const string BOOTSTRAP_SCENE = "BootstrapScene";

        private FadeController _fadeController;
        
        public void Initialize()
        {
            _fadeController = G.Get<FadeController>();
        }
        
        public void ChangeScene(string sceneName)
        {
            G.UnregisterAllDestroyed();
            
            _fadeController.FadeIn(2,callback: () =>
            {
                SceneManager.LoadScene(sceneName);

                G.Get<RoutineStarter>().StartCoroutine(WaitFadeOut());
            });
        }

        private IEnumerator WaitFadeOut()
        {
            yield return new WaitForSeconds(0.5f);
            _fadeController.FadeOut(1f);
        }
    }
}