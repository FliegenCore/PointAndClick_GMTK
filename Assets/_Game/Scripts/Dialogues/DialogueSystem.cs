using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Dialogues
{
    public class DialogueSystem : MonoBehaviour, IService
    {
        [SerializeField] private DialogueView _dialogueView;
        private DialogueTagHandler _tagHandler;
        
        private Speaker _speaker;
        private Dialogue _currentDialogue;
        private List<DialogueChoice> _currentChoices = new List<DialogueChoice>();
        private Coroutine _writeCoroutine;
        private int _currentLine;
        private bool _canContinue;
        private bool _canSkip;
        private bool _dialogueEnd;
        
        public void Initialize()
        {
            _tagHandler = new DialogueTagHandler();
            _dialogueView.gameObject.SetActive(false);
            
            InitButtons();
        }
        
        private void InitButtons()
        {
            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Init();
                button.OnClick += SelectChoice;
            }
        }

        private void Update()
        {
            if (G.Get<ReloadScene>().GameEnded)
            {
                return;
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (_canContinue)
                {
                    ContinueDialogue();
                    return;
                }
                SkipDialogue();
            }
        }

        private Sequence _bottomSequence;
        private Sequence _upperSequence;
        
        public void StartDialogue(Dialogue dialogue, Speaker speaker)
        {
            _bottomSequence?.Kill();
            _upperSequence?.Kill();
            G.Get<RootInput>().PauseSystem();

            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Disable();
            }
            
            _dialogueView.gameObject.SetActive(true);
            if(dialogue != null)
                if(dialogue.StartTagsId.Count > 0)
                    UseTags(dialogue.StartTagsId.ToArray());
            
            _dialogueEnd = false;
            _dialogueView.SpeakerText.text = "";
            _dialogueView.Text.text = "";
            _bottomSequence = DOTween.Sequence();
            _upperSequence = DOTween.Sequence();
            _bottomSequence.Append(_dialogueView.BottomFrame.DOAnchorPosY(100, 0.5f)).OnComplete(() =>
            {
                _canSkip = true;
                _speaker = speaker;
                _currentDialogue = dialogue;
                _currentLine = 0;
                DialogueProcess(_currentLine);
            });

            _upperSequence.Append(_dialogueView.TopFrame.DOAnchorPosY(-100, 0.5f));
        }
        
        private void DialogueProcess(int lineNumber)
        {
            _canSkip = true;
            _canContinue = false;
            _currentChoices.Clear();
            
            if (_currentDialogue == null || lineNumber >= _currentDialogue.Lines.Count)
            {
                if (_dialogueEnd)
                {
                    return;
                }

                _canSkip = false;
                
                _dialogueEnd = true;
                if (_dialogueView != null)
                {
                    _dialogueView.BottomFrame.DOAnchorPosY(-100, 0.5f).OnComplete(() =>
                    {
                        _canSkip = false;
                        _dialogueView.gameObject.SetActive(false);
                        if(G.Get<RootInput>() != null)
                            G.Get<RootInput>().UnpauseSystem();
                    });
                    
                    _dialogueView.TopFrame.DOAnchorPosY(100, 0.5f);
                }
                

                
                return;
            }

            var line = _currentDialogue.Lines[lineNumber];
            _dialogueView.SpeakerText.text = line.Speaker;
            _currentChoices.AddRange(line.Choices);
            
            _writeCoroutine = StartCoroutine(WriteDialogue(line.Text, OnWriteComplete));
        }

        private void SelectChoice(string id, string[] tags)
        {
            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.Disable();
            }
            
            if (id == string.Empty)
            {
                _canSkip = false;
                _upperSequence = DOTween.Sequence();
                _bottomSequence = DOTween.Sequence();
                _dialogueEnd = true;
                _bottomSequence.Append(_dialogueView.BottomFrame.DOAnchorPosY(-100, 0.5f)).OnComplete(() =>
                {
                    _canSkip = false;
                    _dialogueView.gameObject.SetActive(false);
                    G.Get<RootInput>().UnpauseSystem();
                });

                _upperSequence.Append(_dialogueView.TopFrame.DOAnchorPosY(100, 0.5f));
            }
            
            UseTags(tags);
            
            if (id == string.Empty)
            {
                return;
            }

            StartDialogue(_speaker.GetDialogue(id), _speaker);
        }
        
        private void OnWriteComplete()
        {
            if (_currentChoices.Count > 0)
            {
                for (int i = 0; i < _currentChoices.Count; i++)
                {
                    _dialogueView.ChoiceButtons[i].SetDialogueIdAndTags(_currentChoices[i].TargetDialogueId, _currentChoices[i].Tags.ToArray());
                    
                    _dialogueView.ChoiceButtons[i].SetText(_currentChoices[i].Text);
                    _dialogueView.ChoiceButtons[i].Enable();
                }
            }
            else
            {
                _canContinue = true;
            }
        }

        private void ContinueDialogue()
        {
            if (!_canContinue)
            {
                return;
            }
            
            UseTags(_currentDialogue.Lines[_currentLine].Tags.ToArray());
            _currentLine++;
            DialogueProcess(_currentLine);
        }

        private void SkipDialogue()
        {
            if (!_canSkip)
            {
                return;
            }
            
            StopCoroutine(_writeCoroutine);
            OnWriteComplete();
            _dialogueView.Text.text = _currentDialogue.Lines[_currentLine].Text;
        }
        
        private IEnumerator WriteDialogue(string text, Action callback)
        {
            string writedText = string.Empty;
            _dialogueView.Text.text = writedText;
            foreach (var letter in text)
            {
                yield return new WaitForSeconds(0.02f);
                CreateClickSound();
                writedText += letter;
                _dialogueView.Text.text = writedText;
            }
            
            _canSkip = false;
            callback?.Invoke();
        }

        private void UseTags(string[] tags)
        {
            _tagHandler.HandleTags(tags);
        }

        private void CreateClickSound()
        {
            float pitch = Random.Range(0.8f, 1.2f);
            var assset = Resources.Load<AudioSource>("Prefabs/WriteSound");
            AudioSource soundObj = Instantiate(assset, new Vector3(0, 0, 0), Quaternion.identity);
            soundObj.pitch = pitch;
            soundObj.Play();
            
            Destroy(soundObj.gameObject, 0.5f);
        }

        private void OnDestroy()
        {
            foreach (var button in _dialogueView.ChoiceButtons)
            {
                button.OnClick -= SelectChoice;
            }
        }

       
    }
}