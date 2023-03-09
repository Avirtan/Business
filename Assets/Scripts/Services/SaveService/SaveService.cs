using System.IO;
using UnityEngine;

namespace Services
{
    public class SaveService
    {
#if UNITY_EDITOR
        private string _savePath = Application.dataPath  + "/save.json";
#elif PLATFORM_ANDROID
        private string _savePath = Application.persistentDataPath + "/save.json";
#endif
        private SaveData _saveData;

        public SaveData SaveData => _saveData;
        public bool IsFirstOpen => _saveData == null;

        public SaveService()
        {
            _saveData = GetSaveJson();
        }

        private SaveData GetSaveJson()
        {
            if(File.Exists(_savePath))
            {
                var stringSave = File.ReadAllText(_savePath);
                return JsonUtility.FromJson<SaveData>(stringSave);
            }
            else
            {
                return null;
            }
        }

        public void SaveJson(SaveData saveData)
        {
            _saveData = saveData;
            var saveString = JsonUtility.ToJson(saveData);
            File.WriteAllText(_savePath, saveString);
        }

        public void Save()
        {
            if (_saveData == null) return;
            var saveString = JsonUtility.ToJson(_saveData);
            File.WriteAllText(_savePath, saveString);
        }
    }
}