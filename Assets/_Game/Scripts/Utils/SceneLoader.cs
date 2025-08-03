using _Game.Scripts.SceneManagment;
using Game.ServiceLocator;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static bool WasLoaded;
    
    public static void LoadFirstScene()
    {
        if (WasLoaded)
        {
            return;
        }
        
        WasLoaded = true;
        G.UnregisterAllDestroyed();
        SceneManager.LoadScene(SceneController.BOOTSTRAP_SCENE);
    }

}