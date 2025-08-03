using System;
using _Game.Scripts.Interactives;
using _Game.Scripts.Inventory;
using Game.ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game.Scripts.Player
{
    public class PlayerController : IService
    {
        public event Action OnPlayerSpawn;
        
        private PlayerView _playerView;
        private PlayerSpawnPoint _playerSpawnPoint;
        
        public PlayerView PlayerView => _playerView;
        
        public void Initialize()
        {
            _playerSpawnPoint = Object.FindObjectOfType<PlayerSpawnPoint>();
        }

        public void SpawnPlayer()
        {
            _playerView = CreatePlayerView();
            OnPlayerSpawn?.Invoke();
        }
        
        public void Move(float x, Interactive interactive)
        {
            _playerView.SetTargetPosition(x, interactive);
        }

        public void MoveForTakeItem(float x, IItemNeeder itemNeeder, string itemName)
        {
            _playerView.SetTargetOnTakeItem(x, itemNeeder, itemName);
        }

        private PlayerView CreatePlayerView()
        {
            var asset = Resources.Load<PlayerView>("Prefabs/PlayerView");
            return Object.Instantiate(asset, _playerSpawnPoint.transform.position, Quaternion.identity);
        }
    }
}