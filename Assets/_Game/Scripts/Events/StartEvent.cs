using System.Collections;
using _Game.Scripts.CameraSystem;
using _Game.Scripts.CapsuleSystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.GrandpaSystem;
using _Game.Scripts.Player;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using _Game.Scripts.WhiteScreen;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Events
{
    public class StartEvent
    {
        private Capsule _capsule;
        private SkyCapsulePoint _skyPoint;
        private CapsulePoint _capsulePoint;
        private Grandpa _grandpa;
        
        private int _currentLoop;
        
        public void InitGame()
        {
            G.Get<RootInput>().PauseSystem();
            
            _currentLoop = PlayerPrefs.GetInt(Consts.CURRENT_LOOP, 0);
            _capsule = Object.FindObjectOfType<Capsule>();
            _grandpa = Object.FindObjectOfType<Grandpa>();
            _skyPoint = Object.FindObjectOfType<SkyCapsulePoint>();
            _capsulePoint = Object.FindObjectOfType<CapsulePoint>();
            
            if (_currentLoop == 0)
            {
                _capsule.transform.position = _skyPoint.transform.position;
            }
            else
            {
                _capsule.EnableDecor();
                _capsule.transform.position = _capsulePoint.transform.position;
                G.Get<PlayerController>().SpawnPlayer();
            }
        }
        
        public void StartGame()
        {
            if (_currentLoop == 0)
            {
                _capsule.transform.DOMove(_capsulePoint.transform.position, 0.5f)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() =>
                    {
                        G.Get<CameraController>().Shake();
                        _capsule.EnableSound();
                    });
                
                G.Get<RoutineStarter>().StartCoroutine(WaitWhiteIn());
            }
            else
            {
                G.Get<RootInput>().UnpauseSystem();
                G.Get<CameraController>().SetTarget(G.Get<PlayerController>().PlayerView.transform);
            }
            
            _currentLoop++;
            PlayerPrefs.SetInt(Consts.CURRENT_LOOP, _currentLoop);
        }

        private IEnumerator WaitWhiteIn()
        {
            yield return new WaitForSeconds(0.45f);
            G.Get<WhiteScreenController>().WhiteIn(() =>
            {
                G.Get<PlayerController>().SpawnPlayer();
                G.Get<RoutineStarter>().StartCoroutine(WaitWhiteOut());
                _capsule.EnableDecor();
            });
        }

        private IEnumerator WaitWhiteOut()
        {
            yield return new WaitForSeconds(1.5f);
            G.Get<WhiteScreenController>().WhiteOut(() =>
            {
                G.Get<DialogueSystem>().StartDialogue(_grandpa.GetDialogue("start_dialogue"), _grandpa);
            });
        }
    }
}