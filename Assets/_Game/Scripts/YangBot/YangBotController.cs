using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.DaySystem;
using _Game.Scripts.Dialogues;
using _Game.Scripts.Fader;
using _Game.Scripts.Interactives.Variants;
using _Game.Scripts.Inventory;
using _Game.Scripts.PitSystem;
using _Game.Scripts.Player;
using _Game.Scripts.PlayerInput;
using _Game.Scripts.Utils;
using _Game.Scripts.YangBot;
using Game.ServiceLocator;
using UnityEngine;
using Object = UnityEngine.Object;

public class YangBotController :  IService
{
    private Box[] _boxes;
    private YangBotView _yangBotView;
    private Pit _pit;
    private PlayerSpawnPoint _playerSpawnPoint;
    
    private Berries _berries;
    private Skelet _skelet;

    public YangBotView YangBotView => _yangBotView; 
    
    public void Initialize()
    {
        _boxes = Object.FindObjectsOfType<Box>();
        _berries = Object.FindObjectOfType<Berries>();
        _pit = Object.FindObjectOfType<Pit>();
    }
    
    public void Spawn()
    {
        _playerSpawnPoint = Object.FindObjectOfType<PlayerSpawnPoint>();
        
        var asset = Resources.Load<YangBotView>("Prefabs/YangBot");
        _yangBotView = Object.Instantiate(asset,_playerSpawnPoint.transform.position,Quaternion.identity);
    }

    public void BringBerries()
    {
        _yangBotView.Busy = true;
        _yangBotView.Move(_berries.transform, () =>
        {
            _berries.Deativate();   
            _yangBotView.Move(G.Get<PlayerController>().PlayerView.transform, () =>
            {
                _yangBotView.Busy = false;
                G.Get<InventoryController>().AddItem("berries");
            });
        });
    }

    public void ThrowPlayerSkeleton()
    {
        _yangBotView.Busy = true;
        _skelet = Object.FindObjectOfType<Skelet>();

        _yangBotView.Move(_skelet.transform, () =>
        {
            G.Get<RoutineStarter>().StartCoroutine(WaitTake());
            
        });
    }

    private IEnumerator WaitTake()
    {
        yield return new WaitForSeconds(1f);
        
        _yangBotView.EnableSkelet();
        _skelet.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _yangBotView.Move(_pit.transform, () =>
        {
            _yangBotView.DisableSkelet();
            G.Get<RoutineStarter>().StartCoroutine(WaitReload());
        });
    }

    private IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(2f);
        G.Get<ReloadScene>().GameEnded = true;
        G.Get<ReloadScene>().Reload();
    }

    public void MoveTheBoxes()
    {
        _yangBotView.Busy = true;
        G.Get<DayController>().NextTimeDay();
        if (_boxes[0].HaveBox)
        {
            _yangBotView.Move(_boxes[0].transform, () =>
            {
                _boxes[0].DisableBox();
                _yangBotView.Move(_boxes[1].transform, () =>
                {
                    _boxes[1].EnableBox();
                    _yangBotView.Move(_playerSpawnPoint.transform, () =>
                    {
                        _yangBotView.Busy = false;
                    });
                });
            });
        }
        else if (_boxes[1].HaveBox)
        {
            _yangBotView.Move(_boxes[1].transform, () =>
            {
                _boxes[1].DisableBox();
                _yangBotView.Move(_boxes[0].transform, () =>
                {
                    _boxes[0].EnableBox();
                    _yangBotView.Move(_playerSpawnPoint.transform, () =>
                    {
                        _yangBotView.Busy = false;
                    });
                });
            });
        }
    }

    public void KillPlayer()
    {
        _yangBotView.Busy = true;
        G.Get<RootInput>().PauseSystem();
        _yangBotView.ShowKnife();
        
        PlayerView playerView = Object.FindObjectOfType<PlayerView>();
        
        _yangBotView.Move(playerView.transform, () =>
        {
            playerView.Dead();
            _yangBotView.HideKnife();
        });
    }
}
