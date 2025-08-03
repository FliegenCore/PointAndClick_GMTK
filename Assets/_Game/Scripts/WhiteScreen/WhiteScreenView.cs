using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.WhiteScreen
{
    public class WhiteScreenView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        public Image Image => _image;
    }
}