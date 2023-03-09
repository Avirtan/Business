using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    [Serializable]
    public class SaveData
    {
        public List<BusinessSaveData> DataBusinesses;
        public float Money;
        public SaveData()
        {
            DataBusinesses = new List<BusinessSaveData>();
        }

        public BusinessSaveData GetBusnessByName(string name)
        {
            return DataBusinesses.FirstOrDefault(b => b.Equals(name));
        }

        public void AddBusness(BusinessSaveData busnissSaveData)
        {
            DataBusinesses.Add(busnissSaveData);
        }
    }

    [Serializable]
    public class BusinessSaveData
    {
        public string Key;
        public int Lvl = 0;
        public float Progress = 0f;
        public List<int> BuyIdUpgrade;
        public BusinessSaveData(string key, int lvl)
        {
            Key = key;
            Lvl = lvl;
            BuyIdUpgrade = new List<int>();
        }

        public void SetIdUpgrade(int id)
        {
            BuyIdUpgrade.Add(id);
        }

        public void IncLevel()
        {
            Lvl++;
        }
    }
}