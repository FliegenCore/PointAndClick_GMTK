using System.Collections;
using System.Linq;
using _Game.Scripts.Fader;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using DG.Tweening;
using Game.ServiceLocator;
using UnityEngine;

namespace _Game.Scripts.DaySystem
{
    public class DayController : IService
    {
        private DayHandler _dayHandler;
        private TimesDayView _timesDayView;
        private int _currentDay;
        private int _currentDayTime;
        private SkyBlack _skyBlack;
        
        public void Initialize()
        {
            _timesDayView = Object.FindObjectOfType<TimesDayView>();
            _skyBlack = Object.FindObjectOfType<SkyBlack>();
            _dayHandler = new DayHandler();
            RefreshDayUI();
        }

        private void RefreshDayUI()
        {
            int dayTime = _currentDayTime + 1;
    
            float targetAlpha = Mathf.Lerp(0.2f, 0.8f, (dayTime - 1) / 3f);
            _skyBlack.Image.DOFade(targetAlpha, 0.5f); 
    
            _timesDayView.DayImage.sprite = SpriteLoader.LoadDaySprite(dayTime);
            if(dayTime > 1)
                _timesDayView.BupAudio.Play();
        }
        
        public void NextTimeDay()
        {
            _currentDayTime++;

            if (_currentDayTime >= 3)
            {
                ChangeDay();
            }
            
            RefreshDayUI();
        }

        private void ChangeDay()
        {
            G.Get<RootInput>().PauseSystem();
            
            G.Get<FadeController>().FadeIn(callback: () =>
            {
                _currentDay++;
                _currentDayTime = 0;
                _dayHandler.OnDayChanged(_currentDay);
                OnDayChanged();
                RefreshDayUI();
                G.Get<RoutineStarter>().StartCoroutine(WaitFadeOut());
            });
        }

        private IEnumerator WaitFadeOut()
        {
            yield return new WaitForSeconds(1f);
            G.Get<FadeController>().FadeOut(callback: () =>
            {
                G.Get<RootInput>().UnpauseSystem();
            });
        }

        private void OnDayChanged()
        {
            IReactToTheDay[] ractToDay = Object.FindObjectsOfType<MonoBehaviour>().OfType<IReactToTheDay>().ToArray();

            foreach (var react in ractToDay)
            {
                react.ReactToTheDay();
            }
        }
    }
}