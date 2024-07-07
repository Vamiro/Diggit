using GameInput;
using PlayerDir;
using UnityEngine;
using WorldObjects;
using Zenject;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private CellGrid cellGrid;
        [SerializeField] private Player playerPrefab;

        public override void InstallBindings()
        {
            Container.Bind<DataManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<IInput>().To<DesktopInput>().FromNew().AsSingle().NonLazy();
            Container.Bind<DragAndDrop>().FromNew().AsSingle().NonLazy();
            Container.Bind<Player>().FromComponentInNewPrefab(playerPrefab).AsSingle().NonLazy();
            Container.Bind<CellGrid>().FromComponentInNewPrefab(cellGrid).AsSingle().NonLazy();
            Container.Bind<GameController>().FromNew().AsSingle().NonLazy();
        }
    }
}