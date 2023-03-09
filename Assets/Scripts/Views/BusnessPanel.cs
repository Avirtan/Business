using Components;
using Data;
using Leopotam.EcsLite;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Views
{
    public class BusnessPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _nameTMP;
        [SerializeField] Slider _progress;
        [SerializeField] TextMeshProUGUI _lvlTMP;
        [SerializeField] TextMeshProUGUI _incomeTMP;
        [SerializeField] Button _lvlUpButton;
        [SerializeField] TextMeshProUGUI _lvlUpTMP;
        [SerializeField] TextMeshProUGUI _upg1TMP;
        [SerializeField] Button _upg1Button;
        [SerializeField] TextMeshProUGUI _upg2TMP;
        [SerializeField] Button _upg2Button;

        [SerializeField] private string _key;
        private BusinessService _businessService;
        private int _lvl;
        private float _baseIncome;
        private EcsWorld _world;

        public void Init(BusinessService businessService, string key, EcsWorld world)
        {
            _world = world;
            _businessService = businessService;
            _key = key;
            BaseInfoUpdate();
            _progress.value = _businessService.GetSaveProgress(key);
            _nameTMP.text = _businessService.GetName(key);
            var upgrade1Data = _businessService.GetUpgradeData(key, 0);
            var upgrade2Data = _businessService.GetUpgradeData(key, 1);
            UpdateUpgradeInfo(upgrade1Data, _upg1TMP, 0, _upg1Button);
            UpdateUpgradeInfo(upgrade2Data, _upg2TMP, 1, _upg2Button);
            _lvlUpButton.onClick.AddListener(LvlUp);
            if (_lvl == 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void UpdateUpgradeInfo(UpgradeData upgradeData, TextMeshProUGUI upgTMP, int idUpg, Button upgButton)
        {
            var text = "";
            if(_businessService.CheckOpenUpgrade(_key, idUpg))
            {
                text = string.Format(StringConst.UpgradeBuy, upgradeData.name, upgradeData.IncomeMultiplier);
                upgButton.interactable = false;
            }
            else
            {
                text = string.Format(StringConst.UpgradeNotBuy, upgradeData.name, upgradeData.IncomeMultiplier, upgradeData.Price);
                upgButton.onClick.AddListener(delegate { BuyUpgrade(upgradeData, upgTMP, idUpg, upgButton); });
            }
            upgTMP.text = text;
        }

        private void LvlUp()
        {
            var priceLvlUp = _businessService.GatPriceLvlUp(_lvl, _baseIncome);
            if (priceLvlUp > _businessService.GetMoney()) return;
            CreateEventBuy(priceLvlUp);
            _businessService.LevelUp(_key);
            BaseInfoUpdate();
            if(_lvl == 1)
            {
                var openNextBusinessPanelEventPool = _world.GetPool<OpenNextBusinessPanelEvent>();
                var entityOpenEvent = _world.NewEntity();
                openNextBusinessPanelEventPool.Add(entityOpenEvent);

                var enableProgressEventPool = _world.GetPool<EnableProgressEvent>();
                var entityEnableProgressEvent = _world.NewEntity();
                ref var enableProgressEvent = ref enableProgressEventPool.Add(entityEnableProgressEvent);
                enableProgressEvent.Key = _key;
            }
        }

        public void SetProgress(float value)
        {
            _progress.value = value;
        }

        private void BaseInfoUpdate()
        {
            _lvl = _businessService.GetLevel(_key);
            _baseIncome = _businessService.GetBaseIncome(_key);
            _lvlTMP.text = string.Format(StringConst.Lvl, _lvl);
            _incomeTMP.text = string.Format(StringConst.Income, _businessService.GetIncome(_lvl, _baseIncome, _key));
            _lvlUpTMP.text = string.Format(StringConst.LvlUpButton, _businessService.GatPriceLvlUp(_lvl, _baseIncome));
        }

        private void BuyUpgrade(UpgradeData upgradeData, TextMeshProUGUI upgTMP, int idUpg, Button upgButton)
        {
            if (upgradeData.Price > _businessService.GetMoney()) return;
            _businessService.BuyUpgrade(_key, idUpg);
            CreateEventBuy(_businessService.GetIncomeByKey(_key));
            UpdateUpgradeInfo(upgradeData, upgTMP, idUpg, upgButton);
            BaseInfoUpdate();
        }

        private void CreateEventBuy(float value)
        {
            var updateBalanceEventPool = _world.GetPool<UpdateBalanceEvent>();
            var updateBalanceEventEntity = _world.NewEntity();
            ref var updateBalanceEvent = ref updateBalanceEventPool.Add(updateBalanceEventEntity);
            updateBalanceEvent.Value = value;
            updateBalanceEvent.Type = TypeUpdateBalance.Sub;
        }
    }
}