using Data;
using Services;
using UnityEngine;
using Views;
using Worlds;

namespace Components
{
    sealed class GameManager : MonoBehaviour
    {
        [SerializeField] UI _ui;
        [SerializeField] BusinessesData _businessesData;

        private GameWorld _gameWorld;
        private SaveService _saveService;
        private BusinessService _businessService;

        private void Start()
        {
            _saveService = new SaveService();
            _businessService = new BusinessService(_businessesData, _saveService);
            _gameWorld = new GameWorld();
            _ui.Init(_gameWorld.World, _businessService);
            _gameWorld.Inject(new object[]{ _businessesData, _saveService, _ui, _businessService});
            _gameWorld.Init(); 
        }

        private void Update()
        {
            _gameWorld?.UpdateSystem();
        }

        private void OnApplicationPause(bool pause)
        {
            if(pause)
            {
                _saveService.Save();
            }
        }

        private void OnApplicationQuit()
        {
            _saveService.Save();
        }

        void OnDestroy()
        {
            _gameWorld.Dispose();
        }
    }
}