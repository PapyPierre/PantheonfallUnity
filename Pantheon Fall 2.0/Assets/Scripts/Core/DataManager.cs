using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core
{
    public static class DataManager
    {
        private static readonly Dictionary<System.Type, IDictionary<string, object>> LoadedData = new();

        public static T GetData<T>(string dataName) where T : class
        {
            var type = typeof(T);

            if (!LoadedData.TryGetValue(type, out IDictionary<string, object> dataDict))
            {
                dataDict = new Dictionary<string, object>();
                LoadedData[type] = dataDict;
            }

            if (dataDict.TryGetValue(dataName, out object data))
            {
                return data as T;
            }

            var loaded = LoadEntityData<T>(dataName);
            
            if (loaded != null) dataDict[dataName] = loaded;
            
            return loaded;
        }

        private static T LoadEntityData<T>(string dataName) where T : class
        {
            string address = $"{dataName}_Data";
            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            Debug.LogWarning($"Failed to load data for {dataName} of type {typeof(T)}");
            return null;
        }

        public static void UnloadEntityData<T>(string dataName) where T : class
        {
            var type = typeof(T);

            if (LoadedData.TryGetValue(type, out IDictionary<string, object> dataDict) && dataDict.TryGetValue(dataName, out object data))
            {
                Addressables.Release(data);
                dataDict.Remove(dataName);
            }
        }

        public static void UnloadAllEntityData<T>() where T : class
        {
            var type = typeof(T);

            if (LoadedData.TryGetValue(type, out IDictionary<string, object> dataDict))
            {
                foreach (object data in dataDict.Values)
                {
                    Addressables.Release(data);
                }

                dataDict.Clear();
            }
        }

        public static void UnloadAllData()
        {
            foreach (var kvp in LoadedData)
            {
                foreach (var item in kvp.Value.Values)
                {
                    Addressables.Release(item);
                }
            }

            LoadedData.Clear();
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