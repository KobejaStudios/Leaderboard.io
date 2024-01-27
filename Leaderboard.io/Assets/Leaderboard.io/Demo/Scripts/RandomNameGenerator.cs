using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Leaderboard.io
{
    public interface IRandomNameGenerator
    {
        string GetRandomName();
        List<string> GetAllNames();
        List<string> GetNamesInUse();
        List<string> GetAvailableNames();
        void SaveData();
        void LoadData();
        void WipeData();
    }

    public class RandomNameGenerator : IRandomNameGenerator
    {
        private List<string> _allNames = new();
        private List<string> _namesInUse = new();
        private List<string> _availableNames = new();
        
        private const char DataSeparator = '\n';
        
        public static string GetAllNamesKey = "GetAllNames";
        public static string GetUsedNamesKey = "GetUsedNames";
        public static string GetAvailableNamesKey = "GetAvailableNames";

        public RandomNameGenerator()
        {
            Init();
            if (_allNames.Count > 1) return;
            WipeData();
        }

        public string GetRandomName()
        {
            try
            {
                if (_availableNames.Count == 0)
                {
                    Debug.LogWarning("No available names left");
                    return null;
                }

                string randomName = _availableNames[Random.Range(0, _availableNames.Count)];
                _namesInUse.Add(randomName);
                _availableNames.Remove(randomName);
                //SaveData();
                return randomName.Replace('\r', 'a');
            }
            catch (Exception e)
            {
                Debug.LogError($"something happened here: {e.Message}");
                throw;
            }
            
        }

        public List<string> GetAllNames()
        {
            return new List<string>(_allNames);
        }

        public List<string> GetNamesInUse()
        {
            return new List<string>(_namesInUse);
        }

        public List<string> GetAvailableNames()
        {
            return new List<string>(_availableNames);
        }

        public void SaveData()
        {
            string allNamesString = string.Join(DataSeparator, _allNames);
            string namesInUseString = string.Join(DataSeparator, _namesInUse);
            string availableNamesString = string.Join(DataSeparator, _availableNames);
            PlayerPrefs.SetString(GetAllNamesKey, allNamesString);
            PlayerPrefs.SetString(GetUsedNamesKey, namesInUseString);
            PlayerPrefs.SetString(GetAvailableNamesKey, availableNamesString);
            PlayerPrefs.Save();
        }

        public async void LoadData()
        {
            try
            {
                string allNamesString = PlayerPrefs.GetString(GetAllNamesKey);
                string namesInUseString = PlayerPrefs.GetString(GetUsedNamesKey);

                _allNames = !string.IsNullOrEmpty(allNamesString) 
                    ? new List<string>(allNamesString.Split(DataSeparator)) 
                    : new List<string>();

                _namesInUse = !string.IsNullOrEmpty(namesInUseString)
                    ? new List<string>(namesInUseString.Split(DataSeparator))
                    : new List<string>();

                await Task.Yield();
                _availableNames = _allNames.Where(item => !_namesInUse.Any(x => x.Equals(item))).ToList();
                Debug.Log($"allNamesCount: {_allNames.Count}, inUseCount: {_namesInUse.Count}, availableCount: {_availableNames.Count}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading data: {e.Message}");
                throw;
            }
        }

        public void WipeData()
        {
            _allNames.Clear();
            _namesInUse.Clear();
            _availableNames.Clear();
            SaveData();
        }

        private void Init()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(GetAllNamesKey)))
            {
                TextAsset csv = Resources.Load("Data/Leaderboard_io_Names") as TextAsset;
                PlayerPrefs.SetString(GetAllNamesKey, csv.text);
            }
            LoadData();
        }
    }
}
