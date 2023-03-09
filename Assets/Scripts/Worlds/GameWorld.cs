using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System;
using Systems;

namespace Worlds
{
    public class GameWorld : IDisposable
    {
        private EcsWorld _world;
        private IEcsSystems _systems;

        public EcsWorld World => _world;

        public GameWorld() 
        {
            _world = new EcsWorld();
            _systems = new EcsSystems(_world);
            _systems
                .Add(new InitBusinessSystem())
                .Add(new InitPlayerSystem())
                .Add(new UpdateProgressSystem())
                .Add(new UpdateBalanceSystem())
                .Add(new ShowBusinessPanelSystem())
                .Add(new EnableProgressSystem());
#if UNITY_EDITOR
                _systems.Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        }

        public void Inject(object[] obj)
        {
            _systems.Inject(obj);
        }

        public void Init()
        {
            _systems?.Init();
        }

        public void UpdateSystem()
        {
            _systems?.Run();
        }

        public void Dispose()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
