using System;
using GameInput;
using PlayerDir;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using WorldObjects;
using Zenject;

namespace Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private CellGrid _grid;
        [SerializeField] private Player _player;
        private IInput _input;
        [SerializeField] private DesktopInput _desktopInput;
        [SerializeField] private DragAndDrop _dragAndDrop;
        [SerializeField] private DataManager _dataManager;
        [SerializeField] private BaseDropSpot _baseDropSpot;
        [SerializeField] private UIManager _uiManager;

        private void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            _input = _desktopInput;
            _dataManager.RegisterDataObject(_player);
            _dataManager.RegisterDataObject(_grid);
            
            _input.OnPress += hit =>
            {
                var cell = hit.collider.GetComponentInParent<ICell>();
                if (cell != null)
                {
                    if (!cell.Dropped && _player.UseShovel())
                    {
                        _uiManager.SetShavelText(_player.Shovels);
                        cell.Dig();
                    }
                }
            };

            _baseDropSpot.OnReceiveItem += () =>
            {
                _player.CollectItem();
                _uiManager.SetGoldText(_player.Bag);
                _uiManager.SetShavelText(_player.Shovels);
            };
            
            _dragAndDrop.Initialize(_input);

            if (!_dataManager.LoadAll())
            {
                _player.Initialize(50);
                _grid.Initialize(10, 3);
            }

            _uiManager.OnRestart = RestartGame;
            _uiManager.SetShavelText(_player.Shovels);
            _uiManager.SetGoldText(_player.Bag);
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
            _dataManager.SaveAll();
        }

        public void RestartGame()
        {
            _dataManager.DeleteAll();
            _player.Initialize(50);
            _grid.Initialize(10, 3);
            _uiManager.SetShavelText(_player.Shovels);
            _uiManager.SetGoldText(_player.Bag);
        }
    }
}