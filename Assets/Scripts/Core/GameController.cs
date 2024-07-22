using System;
using System.Timers;
using Configurations;
using GameInput;
using ModestTree;
using PlayerDir;
using Settings;
using UI;
using UnityEngine;
using UnityEngine.Audio;
using WorldObjects;


namespace Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CellGrid _grid;
        [SerializeField] private string _saveFileDir = "SaveDefault";
        [SerializeField] private Player _player;
        private IInput _input;
        [SerializeField] private DesktopInput _desktopInput;
        [SerializeField] private DragAndDrop _dragAndDrop;
        [SerializeField] private BaseDropSpot _baseDropSpot;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private AudioMixer _audioMixer;

        private void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            _input = _desktopInput;
            _dragAndDrop.Initialize(_input);
            
            _input.OnPress += hit =>
            {
                var cell = hit.collider.GetComponentInParent<ICell>();
                if (cell != null)
                {
                    if (!cell.CellInfo.dropped && _player.Instruments > 0)
                    {
                        cell.Dig();
                        _player.UseShovel();
                        UpdateUI();
                    }
                }
            };

            _player.OnInstrumentsEnded += () =>
            {
                if (!_grid.CheckDrop())
                {
                    BaseWindow.Get<CancelGameWindow>().Show(_player.Bag, _player.Record);
                    _player.UpdateRecord();
                }
            };

            _baseDropSpot.OnReceiveItem += () =>
            {
                _player.CollectItem();
                UpdateUI();
            };

            SettingsManager.Instance.AudioMixer = _audioMixer;
            
            if (Data.DataManager.Instance.GetSaveList().IsEmpty())
            {
                _player.Initialize(_gameConfig.ShovelsAmount);
                _grid.Initialize(_gameConfig.GridSize, _gameConfig.GridDepth, _gameConfig.DropChance);
            }
            else
            {
                Data.DataManager.Instance.LoadGame(_saveFileDir);
            }

            BaseWindow.Get<CancelGameWindow>().OnTryAgain = RestartGame;
            _uiManager.OnRestart = RestartGame;
            _uiManager.OnRotateLeft = () => _grid.RotateGrid(90);
            _uiManager.OnRotateRight = () => _grid.RotateGrid(-90);
            _uiManager.OnMute = () =>
            {
                SettingsManager.Instance.IsMuted = !SettingsManager.Instance.IsMuted;
                _uiManager.SwitchMuteImage(SettingsManager.Instance.IsMuted);
            };
            _uiManager.SwitchMuteImage(SettingsManager.Instance.IsMuted);
            UpdateUI();
        }

        private void Update()
        {
            _input.UpdateInput();

            if (Input.GetMouseButtonDown(2))
            {
                RestartGame();
            }
        }

        private void OnApplicationQuit()
        {
            Data.DataManager.Instance.SaveGame(_saveFileDir);
        }

        public void RestartGame()
        {
            _player.Initialize(_gameConfig.ShovelsAmount);
            _grid.Initialize(_gameConfig.GridSize, _gameConfig.GridDepth, _gameConfig.DropChance);
            UpdateUI();
        }

        private void UpdateUI()
        {
            _uiManager.SetInstrumentsText(_player.Instruments);
            _uiManager.SetBagText(_player.Bag);
        }
    }
}