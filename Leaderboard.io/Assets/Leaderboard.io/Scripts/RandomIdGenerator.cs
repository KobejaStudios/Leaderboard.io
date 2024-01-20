using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Leaderboard.io
{
    public interface IRandomIdGenerator
    {
        string GenerateAlphanumericChars();
        string GetRandomId();
        List<string> GetAllIds();
        List<string> GetIdsInUse();
        void GenerateRandomIds(int count);
        void SaveData();
        void LoadData();
        void WipeData();
    }

    public class RandomIdGenerator : IRandomIdGenerator
    {
        private List<string> _allIds = new();
        private List<string> _idsInUse = new();
        private char[] _chars => GenerateAlphanumericChars().ToCharArray();
        public static string GetAllIdsKey;
        public static string GetUsedIdsKey;

        public RandomIdGenerator()
        {
            LoadData();
            if (_allIds.Count > 1) return;
            
            _allIds.Clear();
            _idsInUse.Clear();
            GenerateRandomIds(200);
            SaveData();
        }
        
        public string GenerateAlphanumericChars()
        {
            string alphanumericChars = new string(Enumerable.Range('0', 10)
                .Concat(Enumerable.Range('A', 26))
                .Select(c => (char)c)
                .ToArray());

            return alphanumericChars;
        }

        public string GetRandomId()
        {
            if (_allIds.Count == 0)
            {
                Debug.LogWarning($"There are: {_allIds.Count} ids left, generating a new 100 ids");
                GenerateRandomIds(100);
            }
            
            string id = _allIds[Random.Range(0, _allIds.Count)];
            
            while (_idsInUse.Contains(id))
            {
                id = _allIds[Random.Range(0, _allIds.Count)];
            }
            
            _idsInUse.Add(id);
            SaveData();
            return id;
        }

        public List<string> GetAllIds()
        {
            return new List<string>(_allIds);
        }

        public List<string> GetIdsInUse()
        {
            return new List<string>(_idsInUse);
        }

        public void GenerateRandomIds(int count)
        {
            for (int i = 0; i < count; i++)
            {
                string id = RandomId();
                while (_idsInUse.Contains(id))
                {
                    id = RandomId();
                }
                _allIds.Add(id);
            }
            SaveData();
            return;

            string RandomId()
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < 12; j++)
                {
                    sb.Append(_chars[Random.Range(0, _chars.Length)]);
                }

                return sb.ToString();
            }
        }

        public void SaveData()
        {
            string allIds = string.Join(",", _allIds);
            string idsInUse = string.Join(",", _idsInUse);
            PlayerPrefs.SetString(GetAllIdsKey, allIds);
            PlayerPrefs.SetString(GetUsedIdsKey, idsInUse);
            PlayerPrefs.Save();
        }
        
        public void LoadData()
        {
            string allIds = PlayerPrefs.GetString(GetAllIdsKey);
            string idsInUse = PlayerPrefs.GetString(GetUsedIdsKey);
            _allIds = new List<string>(allIds.Split(','));
            _idsInUse = new List<string>(idsInUse.Split(','));
        }

        public void WipeData()
        {
            _allIds.Clear();
            _idsInUse.Clear();
            SaveData();
        }
    }
}
