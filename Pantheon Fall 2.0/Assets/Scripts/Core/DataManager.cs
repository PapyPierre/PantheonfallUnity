using System.Collections.Generic;
using System.IO;
using Core.Entity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core
{
    public static class DataManager
    {
        private static readonly Dictionary<string, EnemyData> LoadedEnemyData = new();
        
        public static EnemyData GetEntityData(string enemyName)
        {
            return LoadedEnemyData.TryGetValue(enemyName, out EnemyData data) ? data : LoadEntityData(enemyName);
        }

        private static EnemyData LoadEntityData(string enemyName)
        {
            AsyncOperationHandle<EnemyData> handle = Addressables.LoadAssetAsync<EnemyData>($"{enemyName}_Data");

            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                EnemyData data = handle.Result;
                LoadedEnemyData.Add(enemyName, data);
                return data;
            }

            return null;
        }

        public static void UnloadEnemyData(string enemyName)
        {
            Addressables.Release(LoadedEnemyData[enemyName]);
            LoadedEnemyData.Remove(enemyName);
        }

        public static void UnloadAllEnemyData()
        {
            foreach (var kvp in LoadedEnemyData)
            {
                Addressables.Release(kvp.Value);
            }

            LoadedEnemyData.Clear();
        }
    }

    public static class SaveManager
    {
        public static void SaveDataInFolder<T>(T data, string folderName, string dataName)
        {
            var path = Path.Combine(Application.persistentDataPath, folderName);

            if (!File.Exists(path)) Directory.CreateDirectory(path);

            SaveData(data, path, dataName);
        }

        private static void SaveData<T>(T data, string path, string dataName)
        {
            string dataString = JsonUtility.ToJson(data, true);
            var filePath = Path.Combine(path, dataName + ".json");
            File.WriteAllText(filePath, dataString);
            Debug.Log($"{dataName} Saved at {filePath}");
        }

        public static T LoadDataFromFolder<T>(string folderName, string dataName)
        {
            var path = Path.Combine(Application.persistentDataPath, folderName);
            return LoadData<T>(path, dataName, true);
        }

        public static List<T> LoadAllDataFromFolder<T>(string folderName)
        {
            var path = Path.Combine(Application.persistentDataPath, folderName);

            if (!File.Exists(path)) Directory.CreateDirectory(path);

            List<T> datas = new List<T>();

            foreach (string fileName in Directory.GetFiles(path))
            {
                datas.Add(LoadData<T>(path, fileName, false));
            }

            return datas;
        }

        private static T LoadData<T>(string path, string dataName, bool addExtension)
        {
            var filePath = addExtension ? Path.Combine(path, dataName + ".json") : Path.Combine(path, dataName);

            string loadPlayerData = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(loadPlayerData);

            Debug.Log($"{dataName} Loaded!");
            return data;
        }

        public static void DeleteData(string folderName)
        {
            string path = Path.Combine(Application.persistentDataPath, folderName);
            string[] saves = Directory.GetFiles(path);

            for (var index = saves.Length - 1; index >= 0; index--)
            {
                var save = saves[index];
                File.Delete(save);
            }
        }

        public static void OpenDataInExplorer(string folderName)
        {
            string path = Path.Combine(Application.persistentDataPath, folderName);
            Application.OpenURL(path);
        }
    }
}