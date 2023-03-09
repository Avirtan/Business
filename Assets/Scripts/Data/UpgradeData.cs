using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "Data/UpgradeDatas", order = 1)]
    public class UpgradeData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _price;
        [SerializeField] private float _incomeMultiplier;

        public string Name => _name;
        public float Price => _price;
        public float IncomeMultiplier => _incomeMultiplier;
    }
}
