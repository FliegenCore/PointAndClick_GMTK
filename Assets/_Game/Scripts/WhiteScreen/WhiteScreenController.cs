using System;
using System.Collections;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.WhiteScreen
{
    public class WhiteScreenController : IService
    {
        private WhiteScreenView _whiteScreenView;

        public void Initialize()
        {
            _whiteScreenView = CreateView();
            _whiteScreenView.gameObject.SetActive(false);
        }

        public void WhiteIn(Action callback = null)
        {
            _whiteScreenView.gameObject.SetActive(true);
            _whiteScreenView.Image.DOFade(1, 0.5f).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        public void WhiteOut(Action callback = null)
        {
            _whiteScreenView.Image.DOFade(0, 0.5f).OnComplete(() =>
            {
                _whiteScreenView.gameObject.SetActive(false);
                callback?.Invoke();
            });
        }

        private WhiteScreenView CreateView()
        {
            var asset = Resources.Load<WhiteScreenView>("Prefabs/UI/WhiteScreen");
            return Object.Instantiate(asset);
        }
    }
}