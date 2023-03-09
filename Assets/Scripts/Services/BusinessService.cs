using Data;
using System.Linq;

namespace Services
{
    public class BusinessService
    {
        private BusinessesData _businessesData;
        private SaveService _saveService;

        public BusinessService(BusinessesData businessesData, SaveService saveService)
        {
            _businessesData = businessesData;
            _saveService = saveService;
        }

        public string GetName(string key)
        {
            return _businessesData.Businesseses.FirstOrDefault(b => b.Key == key).Name;
        }

        public int GetLevel(string key)
        {
            return _saveService.SaveData.DataBusinesses.FirstOrDefault(b => b.Key == key).Lvl;
        }

        public float GetBaseCost(string key)
        {
            return _businessesData.Businesseses.FirstOrDefault(b => b.Key == key).BaseCost;
        }

        public float GetBaseIncome(string key)
        {
            return _businessesData.Businesseses.FirstOrDefault(b => b.Key == key).BaseIncome;
        }

        public float GetDelay(string key)
        {
            return _businessesData.Businesseses.FirstOrDefault(b => b.Key == key).IncomeDelay;
        }

        public float GetSaveProgress(string key)
        {
            return _saveService.SaveData.DataBusinesses.FirstOrDefault(b => b.Key == key).Progress;
        }

        public bool CheckOpenUpgrade(string key, int idUpgrade)
        {
            return _saveService.SaveData.DataBusinesses.FirstOrDefault(b => b.Key == key).BuyIdUpgrade.Any(upg => upg == idUpgrade);
        }

        public UpgradeData GetUpgradeData(string key, int idUpgrade) 
        {
            var upgradeList = _businessesData.Businesseses.FirstOrDefault(b => b.Key == key).UpgradeData;
            if(idUpgrade > upgradeList.Count)
            {
                return null;
            }
            var upgrade = upgradeList[idUpgrade];
            return upgrade;
        }

        public void BuyUpgrade(string key, int idUpgrade)
        {
            var businessSaveData = _saveService.SaveData.DataBusinesses.First(b => b.Key == key);
            businessSaveData.SetIdUpgrade(idUpgrade);
        }

        public float GetUpgradeMultiplier(string key)
        {
            var multiplier = 0f;
            var upgradeData = _businessesData.Businesseses.First(b => b.Key == key).UpgradeData;
            for (int i = 0; i < upgradeData.Count; i++)
            {
                if (CheckOpenUpgrade(key, i))
                {
                    multiplier += upgradeData[i].IncomeMultiplier / 100;
                }
            }
            return multiplier;
        }

        public float GetIncome(int lvl, float baseIncome, string key)
        {
            return lvl * baseIncome * (1 + GetUpgradeMultiplier(key));
        }

        public float GetIncomeByKey(string key)
        {
           return GetIncome(GetLevel(key), GetBaseIncome(key), key);
        }

        public float GatPriceLvlUp(int lvl, float baseIncome)
        {
            return (lvl + 1) * baseIncome;
        }

        public void LevelUp(string key)
        {
            var business = _saveService.SaveData.DataBusinesses.FirstOrDefault(b => b.Key == key);
            business.IncLevel();
        }

        public string GetNextBusinessKey()
        {
            var isFoundCurrentBusinessPanel = false;
            string key = null;
            var dataBusinessesTmp = _saveService.SaveData.DataBusinesses;
            for (int i = 0; i < dataBusinessesTmp.Count - 1; i++)
            {
                var business = dataBusinessesTmp[i];
                if(business.Lvl != 0 && dataBusinessesTmp[i + 1].Lvl == 0)
                {
                    isFoundCurrentBusinessPanel = true;
                }
                if (isFoundCurrentBusinessPanel)
                {
                    key = dataBusinessesTmp[i + 1].Key;
                    break;
                }
            }
            return key;
        }

        public float GetMoney()
        {
            return _saveService.SaveData.Money;
        }

        public void AddMoney(float value)
        {
            _saveService.SaveData.Money += value;
        }

        public void RemoveMoney(float value)
        {
            _saveService.SaveData.Money -= value;
        }
    }
}