using _Game.Scripts.AISystem;
using _Game.Scripts.CameraSystem;
using _Game.Scripts.DaySystem;
using _Game.Scripts.GrandpaSystem;
using _Game.Scripts.Interactives.Variants;
using _Game.Scripts.Player;
using _Game.Scripts.Utils;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class DialogueTagHandler
    {
        private Grandpa _grandpa;
        private AIView _aiView;
        
        public DialogueTagHandler()
        {
            _aiView = Object.FindObjectOfType<AIView>();  
            _grandpa = Object.FindObjectOfType<Grandpa>();
        }
        
        public void HandleTags(string[] tags)
        {
            foreach (var tag in tags)
            {
                string[] split = tag.Split('_');
                string tagComposite = split[0];
                string tagObject = string.Empty;
                if(split.Length > 1)
                    tagObject = split[1];
                
                if(tagComposite == "camera")
                {
                    DoCameraAction(tagObject);
                }
                else if(tagComposite == "ai")
                {
                    DoAIAction(tagObject);
                }
                else if (tagComposite == "spawn")
                {
                    SpawnItem(tagObject);
                }
                else if (tagComposite == "tiktak")
                {
                    G.Get<DayController>().NextTimeDay();
                }
                else if (tagComposite == "bot")
                {
                    HandleBot(tagObject);
                }
                else if (tagComposite == "restart")
                {
                    G.Get<ReloadScene>().GameEnded = true;
                    G.Get<ReloadScene>().Reload();
                }
                else if(tagComposite == "checkPoints")
                {
                    CheckPoints();
                }
                else if (tagComposite == "kill")
                {
                    G.Get<YangBotController>().KillPlayer();
                }
            }
        }

        private void CheckPoints()
        {
            PlayerView playerView = Object.FindObjectOfType<PlayerView>();

            if (playerView.CanDeath)
            {
                Dialogue dialogue = _aiView.GetDialogue("checkPoints_old");
                G.Get<DialogueSystem>().StartDialogue(dialogue, _aiView);
            }
            else
            {
                Dialogue dialogue = _aiView.GetDialogue("checkPoints_yang");
                G.Get<DialogueSystem>().StartDialogue(dialogue, _aiView);
            }
        }
        
        private void HandleBot(string tagObject)
        {
            if (tagObject == "movebox")
            {
                Box[] boxes = Object.FindObjectsOfType<Box>();
                if (!boxes[0].HaveBox && !boxes[1].HaveBox)
                {
                    Dialogue dialogue = G.Get<YangBotController>().YangBotView.GetDialogue("kill_player");
                    G.Get<DialogueSystem>().StartDialogue(dialogue, G.Get<YangBotController>().YangBotView);
                }
                else
                {
                    G.Get<YangBotController>().MoveTheBoxes();
                }
            }
            else if (tagObject == "berries")
            {
                Berries berries = Object.FindObjectOfType<Berries>();
                Debug.Log("Yang bot berries");
                Debug.Log(berries.Active);
                if (berries.Active)
                {
                    G.Get<YangBotController>().BringBerries();
                }
                else
                {
                    Dialogue dialogue = G.Get<YangBotController>().YangBotView.GetDialogue("no_berries");
                    G.Get<DialogueSystem>().StartDialogue(dialogue, G.Get<YangBotController>().YangBotView);
                }
            }
        }

        private void SpawnItem(string itemName)
        {
            Vector3 spawnPos = _grandpa.transform.position;
            
            if (itemName == "gift")
            {
                spawnPos = Object.FindObjectOfType<GiftSpawnPoint>().transform.position;
            }
            
            var asset = Resources.Load<GameObject>("Prefabs/Items/" + itemName);
            Object.Instantiate(asset, spawnPos, Quaternion.identity);
        }

        private void DoAIAction(string emotion)
        {
            if (emotion == "good")
            {
                _aiView.EnableGood();
            }
            else if (emotion == "bad")
            {
                _aiView.EnableBad();
            }
        }

        private void DoCameraAction(string followObject)
        {
            switch (followObject)
            {
                case "ai":
                      G.Get<CameraController>().SetTarget(_aiView.transform);
                    break;
                case "grandpa":
                        G.Get<CameraController>().SetTarget(_grandpa.transform);
                    break;
                case "player":
                        PlayerView player = Object.FindObjectOfType<PlayerView>();
                        G.Get<CameraController>().SetTarget(player.transform);
                    break;
            }
        }
    }
}