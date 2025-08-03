using System;
using _Game.Scripts.SceneManagment;
using Game.ServiceLocator;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts.Utils
{
    public class ReloadScene : MonoBehaviour, IService
    {
        public bool GameEnded;
        
        public void Initialize()
        {
            
        }
        

        public void Reload()
        {
            G.Get<SceneController>().ChangeScene(SceneController.BOOTSTRAP_SCENE);
        }
    }
}