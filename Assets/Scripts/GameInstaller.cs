using System.Collections;
using System.Collections.Generic;
using Gybe.Game;
using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject playerDataServicePrefab;
        public GameObject productManagerPrefab;
        public GameObject groundPrefab;
        public OrderUI orderUIPrefab;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerData>()
                .FromComponentInNewPrefab(playerDataServicePrefab)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<IProductManager>()
                .FromComponentInNewPrefab(productManagerPrefab)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<IGroundController>()
                .FromComponentInNewPrefab(groundPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}