using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Menu
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        
        public Button StartButton => _startButton;
    }
}