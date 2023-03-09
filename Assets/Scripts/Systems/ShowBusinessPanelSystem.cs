using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;

namespace Systems
{
    //включние отображения GO панелей
    sealed class ShowBusinessPanelSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<BusinessService> _businessService = default;
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<BusinessComponent>().End();
            var filterEvent = world.Filter<OpenNextBusinessPanelEvent>().End();
            var businessComponentPool = world.GetPool<BusinessComponent>();
            foreach (var eventEntity in filterEvent)
            {
                var keyOpenBusinessPanel = _businessService.Value.GetNextBusinessKey();
                foreach (var entity in filter)
                {
                    if (keyOpenBusinessPanel == null) break;
                    ref var businessComponent = ref businessComponentPool.Get(entity);
                    if(businessComponent.Key == keyOpenBusinessPanel)
                    {
                        businessComponent.BusnessPanel.gameObject.SetActive(true);
                    }
                }
                world.DelEntity(eventEntity);
            }
        }
    }
}