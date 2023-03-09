using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;
using UnityEngine;
using Utils;
using Views;

namespace Systems
{
    //обновление прогресс бара, и создание эвента на добавление денег, после заполнения прогресс бара 
    sealed class UpdateProgressSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<BusinessService> _businessService = default;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<UpdateProgress>().Inc<BusinessComponent>().End();
            var updateProgressPool = world.GetPool<UpdateProgress>();
            var businessComponentPool = world.GetPool<BusinessComponent>();
            var updateBalanceEventPool = world.GetPool<UpdateBalanceEvent>();
            foreach (var entity in filter)
            {
                ref var progress = ref updateProgressPool.Get(entity);
                ref var businessComponent = ref businessComponentPool.Get(entity);
                progress.Value += Time.deltaTime;
                businessComponent.BusnessSaveData.Progress = progress.Value;
                businessComponent.BusnessPanel.SetProgress(progress.Value / progress.Delay);
                if(progress.Value >= progress.Delay)
                {
                    progress.Value = 0;

                    var updateBalanceEventEntity = world.NewEntity();
                    ref var updateBalanceEvent = ref updateBalanceEventPool.Add(updateBalanceEventEntity);
                    updateBalanceEvent.Value = _businessService.Value.GetIncomeByKey(businessComponent.Key);
                    updateBalanceEvent.Type = TypeUpdateBalance.Add;
                }
            }
        }
    }
}