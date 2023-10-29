using UnityEngine;
using Zenject;

namespace Gybe.Game
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerDataServicePrefab;
        [SerializeField] private GameObject productManagerPrefab;
        [SerializeField] private GameObject groundPrefab;
        [SerializeField] private OrderUI orderUIPrefab;
        
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