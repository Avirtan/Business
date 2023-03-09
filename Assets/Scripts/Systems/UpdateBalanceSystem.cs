using Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;
using Utils;
using Views;

namespace Systems
{
    //обновление баланса
    sealed class UpdateBalanceSystem : IEcsRunSystem
    {
        readonly EcsCustomInject<BusinessService> _businessService = default;
        readonly EcsCustomInject<UI> _ui = default;

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<UpdateBalanceEvent>().End();
            var updateBalanceEventPool = world.GetPool<UpdateBalanceEvent>();
            foreach (var entity in filter)
            {
                ref var updateBalanceEvent = ref updateBalanceEventPool.Get(entity);
                switch (updateBalanceEvent.Type)
                {
                    case TypeUpdateBalance.Add : 
                        _businessService.Value.AddMoney(updateBalanceEvent.Value);
                        break;
                    case TypeUpdateBalance.Sub:
                        _businessService.Value.RemoveMoney(updateBalanceEvent.Value);
                        break;
            }
            _ui.Value.UpdateBalance();
            world.DelEntity(entity);
        }
    }
}
}