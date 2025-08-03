using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game.Scripts.HelpSystem
{
    public class HelpTooltipView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void DoAnimation(string text)
        {
            _text.text = text;
            
            transform.DOMoveY(transform.position.y + 0.5f, 1.5f);
            
            _text.DOFade(0, 1.5f)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}