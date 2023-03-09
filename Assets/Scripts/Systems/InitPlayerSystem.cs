using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;

namespace Systems
{
    sealed class InitPlayerSystem : IEcsInitSystem
    {
        readonly EcsCustomInject<SaveService> _saveService = default;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerComponentPool = world.GetPool<PlayerComponent>();
            var entity = world.NewEntity();
            ref var playerComponent = ref playerComponentPool.Add(entity);
            playerComponent.Money = _saveService.Value.SaveData.Money;
        }
    }
}