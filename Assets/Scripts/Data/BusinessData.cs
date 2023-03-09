using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "BusinessData", menuName = "Data/BusinessData", order = 1)]
    public class BusinessData : ScriptableObject
    {
        [Header("Уникальный ключ")]
        [SerializeField] private string _key;
        [Header("Название")]
        [SerializeField] private string _name;
        [Header("Задержка дохода")]
        [SerializeField] private float _incomeDelay;
        [Header("Базовая стоимость")]
        [SerializeField] private float _baseCost;
        [Header("Базовый доход")]
        [SerializeField] private float _basicIncome;
        [Header("Открыт по умолчанию")]
        [SerializeField] private bool _openByDefault;
        [Header("Улучшения")]
        [SerializeField] private List<UpgradeData> _upgradeData;

        public bool OpenByDefault => _openByDefault;
        public string Key => _key;
        public string Name => _name;
        public float BaseCost => _baseCost;
        public float IncomeDelay => _incomeDelay;
        public float BaseIncome => _basicIncome;
        public List<UpgradeData> UpgradeData => _upgradeData;

        private void Awake()
        {
            if (string.IsNullOrEmpty(_key))
            {
                Debug.Log(_key);
                Debug.Log(_name);
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                _key = new string(Enumerable.Repeat(chars, 15).Select(s => s[Random.Range(0, s.Length)]).ToArray());
            }
        }
    }
}
