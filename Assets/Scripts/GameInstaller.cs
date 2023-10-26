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
        public OrderUI orderUIPrefab;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerData>()
                .FromComponentInNewPrefab(playerDataServicePrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}