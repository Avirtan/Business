using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;

namespace Systems
{
    //Включение прогресс бара при получении 1 уровня
    sealed class EnableProgressSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<BusinessService> _businessService = default;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filterEvent = world.Filter<EnableProgressEvent>().End();
            var filter = world.Filter<BusinessComponent>().Exc<UpdateBalanceEvent>().End();
            var updateProgressPool = world.GetPool<UpdateProgress>();
            var businessComponentPool = world.GetPool<BusinessComponent>();
            var enableProgressEventPool = world.GetPool<EnableProgressEvent>();
            foreach (var entityEvent in filterEvent)
            {
                var enableProgressEvent = enableProgressEventPool.Get(entityEvent);
                foreach (var entity in filter)
                {
                    ref var businessComponent = ref businessComponentPool.Get(entity);
                    if(businessComponent.Key == enableProgressEvent.Key)
                    {
                        ref var progress = ref updateProgressPool.Add(entity);
                        progress.Value = 0;
                        progress.Delay = _businessService.Value.GetDelay(businessComponent.Key);
                    }
                }
                world.DelEntity(entityEvent);
            }
        }
    }
}