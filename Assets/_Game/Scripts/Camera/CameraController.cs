using System;
using _Game.Scripts.Player;
using _Game.Scripts.Utils;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.CameraSystem
{
    public class CameraController : MonoBehaviour, IService
    {
        private Vector3 _velocity = Vector3.zero;
        private float _smoothTime = 0.2f;
        private CameraRoot _cameraRoot;
        private Camera _camera;

        private Transform _target;

        private bool _isPlayer;
        
        public void Initialize()
        {
            _camera = Camera.main;
            _cameraRoot = FindObjectOfType<CameraRoot>();
        }

        private void LateUpdate()
        {
            Follow();
        }

        public void Shake()
        {
            _camera.DOShakePosition(0.5f, new Vector3(2, 0.1f)).SetEase(Ease.OutBounce);
        }
        
        public void SetTarget(Transform target)
        {
            _target = target;

            _isPlayer = _target.GetComponent<PlayerView>();
        }

        private void Follow()
        {
            if (_target == null)
            {
                return;
            }
            
            float x = _target.position.x;
            x = Mathf.Clamp(x, Consts.MIN_X_CAMERA_POSITION, Consts.MAX_X_CAMERA_POSITION);
            
            if (_isPlayer)
            {
                x += G.Get<PlayerController>().PlayerView.Direction;
            }
            
            Vector3 targetPos = new Vector3(x, _cameraRoot.transform.position.y, _cameraRoot.transform.position.z);
    
            _cameraRoot.transform.position = Vector3.SmoothDamp(
                _cameraRoot.transform.position, 
                targetPos, 
                ref _velocity, 
                _smoothTime);
        }
    }
}