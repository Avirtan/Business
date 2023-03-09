using Leopotam.EcsLite;
using Services;
using System.Text;
using TMPro;
using UnityEngine;
using Utils;

namespace Views
{
    public class UI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI balanceText;
        [SerializeField] BusnessPanel _busnessPanel;
        [SerializeField] RectTransform _rootSpawn;

        private EcsWorld _world;
        private BusinessService _businessService;
        private StringBuilder _textBalance;

        public void Init(EcsWorld world, BusinessService businessService)
        {
            _world = world;
            _businessService = businessService;
            _textBalance = new StringBuilder(20);
        }

        public void UpdateBalance()
        {
            _textBalance.Clear();
            _textBalance.Append(string.Format(StringConst.Balance, _businessService.GetMoney()));
            balanceText.text = _textBalance.ToString();
        }

        public BusnessPanel SpawnBusnessPanel(string key)
        {
            var busnessPanel = Instantiate(_busnessPanel, _rootSpawn);
            busnessPanel.Init(_businessService, key, _world);
            return busnessPanel;
        }
    }
}
