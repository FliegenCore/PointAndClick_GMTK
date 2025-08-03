using System.Collections;
using _Game.Scripts.AISystem;
using _Game.Scripts.CameraSystem;
using _Game.Scripts.CapsuleSystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.GrandpaSystem;
using _Game.Scripts.Interactives.Variants;
using _Game.Scripts.Player;
using _Game.Scripts.PlayerInput;
using DG.Tweening;
using Game.ServiceLocator;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Scripts.DaySystem
{
    public class DayHandler
    {
        private AIView _aiView;
        private PlayerSpawnPoint _playerSpawnPoint;
        private Capsule _capsule;

        public DayHandler()
        {
            Init();
        }

        private void Init()
        {
            _capsule = Object.FindObjectOfType<Capsule>();
            _aiView = Object.FindObjectOfType<AIView>();
            _playerSpawnPoint = Object.FindObjectOfType<PlayerSpawnPoint>();
        }
        
        public void OnDayChanged(int currentDay)
        {
            if (currentDay <= 5)
            {
                Skelet findSkelet = Object.FindObjectOfType<Skelet>();
                if (findSkelet != null)
                {
                    PlayerView playerView = Object.FindObjectOfType<PlayerView>();
                    playerView.Dead();
                }
            }
            
            if (currentDay == 4)
            {
                G.Get<RoutineStarter>().StartCoroutine(BaseDeathEvent());
            }
            else if (currentDay == 5)
            {
                DisableCapsule();
            }
            else if (currentDay == 6)
            {
                G.Get<RoutineStarter>().StartCoroutine(WhenPlayerOld());
            }
            else if(currentDay == 7)
            {
                G.Get<RoutineStarter>().StartCoroutine(SpawnCapsuleEvent());
            }
            
            
        }

        private void DisableCapsule()
        {
            _capsule.DisableDecor();
            _capsule.gameObject.SetActive(false);
            SkyCapsulePoint point = Object.FindObjectOfType<SkyCapsulePoint>();
            _capsule.transform.position = point.transform.position;
        }

        private IEnumerator SpawnCapsuleEvent()
        {
            yield return new WaitForSeconds(2f);
            G.Get<CameraController>().SetTarget(_capsule.transform);
            G.Get<RootInput>().PauseSystem();
            _capsule.gameObject.SetActive(true);
            CapsulePoint capsulePoint = Object.FindObjectOfType<CapsulePoint>();
            
            _capsule.transform.DOMove(capsulePoint.transform.position, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    G.Get<CameraController>().Shake();
                    _capsule.EnableSound();
                    G.Get<YangBotController>().Spawn();
                    _capsule.EnableDecor();
                });
            
            yield return new WaitForSeconds(0.75f);
            
            G.Get<CameraController>().SetTarget(G.Get<YangBotController>().YangBotView.transform);
            
            yield return new WaitForSeconds(0.75f);
            G.Get<CameraController>().SetTarget(G.Get<PlayerController>().PlayerView.transform);
            G.Get<RootInput>().UnpauseSystem();
        }
        
        private IEnumerator WhenPlayerOld()
        {
            G.Get<PlayerController>().PlayerView.transform.position = _playerSpawnPoint.transform.position;
            yield return new WaitForSeconds(2f);
            Dialogue aiDialogue = _aiView.GetDialogue("need_eat");
            G.Get<DialogueSystem>().StartDialogue(aiDialogue, _aiView);
            G.Get<PlayerController>().PlayerView.gameObject.layer = LayerMask.NameToLayer("Default");
            G.Get<PlayerController>().PlayerView.EnableProcessOfDeath();
        }

        private IEnumerator BaseDeathEvent()
        {
            Grandpa grandpa = Object.FindObjectOfType<Grandpa>();
            if (grandpa == null)
            {
                yield break;
            }
            
            grandpa.Died();
            
            yield return new WaitForSeconds(2f);
            
            AIView aiView = Object.FindObjectOfType<AIView>();
            Dialogue dialogue = aiView.GetDialogue("base_grandpa_death");
            G.Get<DialogueSystem>().StartDialogue(dialogue, aiView);
        }
    }
}