using Components;
using Data;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Services;
using Views;

namespace Systems
{
    sealed class InitBusinessSystem : IEcsInitSystem
    {
        readonly EcsCustomInject<BusinessesData> _businessesData = default;
        readonly EcsCustomInject<SaveService> _saveService = default;
        readonly EcsCustomInject<BusinessService> _businessService = default;
        readonly EcsCustomInject<UI> _ui = default;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var businessComponentPool = world.GetPool<BusinessComponent>();
            var openNextBusinessPanelEventPool = world.GetPool<OpenNextBusinessPanelEvent>();
            var updateProgressPool = world.GetPool<UpdateProgress>();
            //первый заход в игру, создание данных для сохранения
            if (_saveService.Value.IsFirstOpen)
            {
                var saveData = new SaveData();
                foreach (var business in _businessesData.Value.Businesseses)
                {
                    var lvl = 0;
                    if (business.OpenByDefault)
                    {
                        lvl = 1;
                    }
                    BusinessSaveData busnissSaveData = new BusinessSaveData(business.Key, lvl);
                    saveData.AddBusness(busnissSaveData);
                }
                _saveService.Value.SaveJson(saveData);
            }
            _ui.Value.UpdateBalance();
            //создание панелей с инфо о бизнесах 
            foreach(var business in _saveService.Value.SaveData.DataBusinesses)
            {
                var businessPanel = _ui.Value.SpawnBusnessPanel(business.Key);
                var entity = world.NewEntity();
                ref var businessComponent = ref businessComponentPool.Add(entity);
                businessComponent.BusnessPanel = businessPanel;
                businessComponent.Key = business.Key;
                businessComponent.BusnessSaveData = business;
                if (business.Lvl > 0)
                {
                    ref var progress = ref updateProgressPool.Add(entity);
                    progress.Value = business.Progress;
                    progress.Delay = _businessService.Value.GetDelay(business.Key);
                }
            }
            var entityOpenEvent = world.NewEntity();
            openNextBusinessPanelEventPool.Add(entityOpenEvent);
        }
    }
}