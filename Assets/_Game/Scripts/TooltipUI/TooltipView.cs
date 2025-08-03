using System;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.TooltipUI
{
    public class TooltipView : MonoBehaviour
    {
        private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;

        public RectTransform RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
        
        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}