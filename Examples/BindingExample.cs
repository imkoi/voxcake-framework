using System;
using UnityEngine;
using VoxCake.IoC;
using Object = UnityEngine.Object;

namespace VoxCake.Framework
{
    internal class BindingExample : Context
    {
        [SerializeField] private PlayerView _playerPrefab;
        
        protected override void Install()
        {
            BasicBinder.Bind<IPlayerFactory>().To<PlayerFactory>().WithInstance(_playerPrefab).AsSingle();
            BasicBinder.Bind<PlayerController>();

            CommandBinder.Bind<PlayerSpawnObserver>().To<PlayerSpawnCommand>();

            var playerSpawnObserver = GetInstance<PlayerSpawnObserver>();
            playerSpawnObserver.Dispatch(); // spawn player
        }
    }

    internal interface IPlayerFactory
    {
        PlayerView CreatePlayer();
    }

    internal class PlayerFactory : IPlayerFactory
    {
        private readonly PlayerView _playerPrefab;

        public PlayerFactory(PlayerView playerPrefab)
        {
            _playerPrefab = playerPrefab;
        }
        
        public PlayerView CreatePlayer()
        {
            return Object.Instantiate(_playerPrefab);
        }
    }

    internal class PlayerView : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }

    internal class PlayerController : IInitializable, IDisposable
    {
        private readonly IPlayerFactory _playerFactory;
        private PlayerView _view;

        public PlayerController(IPlayerFactory playerFactory)
        {
            _playerFactory = playerFactory;
        }

        //Invokes when we get instance of PlayerController
        public void Initialize()
        {
            _view = _playerFactory.CreatePlayer();
            _view.gameObject.SetActive(false);
        }

        public void Spawn()
        {
            _view.gameObject.SetActive(true);
        }

        //Invokes when container destroying
        public void Dispose()
        {
            _view.Destroy();
        }
    }

    internal class PlayerSpawnObserver : Observer
    {
    }

    internal class PlayerSpawnCommand : ICommand
    {
        private readonly PlayerController _playerController;

        public PlayerSpawnCommand(PlayerController playerController)
        {
            _playerController = playerController;
        }

        public void Execute()
        {
            _playerController.Spawn();
        }
    }
}