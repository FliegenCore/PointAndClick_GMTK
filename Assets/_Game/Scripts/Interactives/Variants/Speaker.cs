using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.Interactives;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.Dialogues
{
    public class Speaker : Interactive
    {
        [SerializeField] private string _name;
        [SerializeField] private List<Dialogue> _dialogues;
        
        public List<Dialogue> Dialogues => _dialogues;
        private Dialogue _currentDialogue;

        public override string GetName()
        {
            return _name;
        }

        public void SetDialogue(Dialogue dialogue)
        {
            _currentDialogue = dialogue;
        }
        
        public override IEnumerator Interact()
        {
            yield return null;
            G.Get<DialogueSystem>().StartDialogue(_currentDialogue, this);
        }

        public Dialogue GetDialogue(string id)
        {
            foreach (var dialogue in _dialogues)
            {
                if(dialogue.Id == id)
                    return dialogue;
            }
            
            return null;
        }
    }
}