using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Leaderboard.io
{
    public interface ILeaderboardService
    {
        void UpdatePlayer(PlayerData player);
        List<PlayerData> GetLeaderboard();
        List<PlayerData> GetSortedList(Comparison<PlayerData> comparison);
        void DeleteLeaderboard();
        void AddUser(PlayerData user);
        void SaveLeaderboard();
        void LoadLeaderboard();
        void CreateLeaderboard();
        void UpdateLeaderboard();
    }
    // Add this `LeaderboardService` to your service locator, inject it with DI, construct one, or use any 
    // other pattern that fits your project
    public class LeaderboardService : ILeaderboardService
    {
        private List<PlayerData> _players = new();
        private string SaveFilePath => Application.persistentDataPath + "/leaderboardData.dat";
        public LeaderboardService()
        {
            LoadLeaderboard();
        }
        
        public void UpdatePlayer(PlayerData player)
        {
            
        }

        public List<PlayerData> GetLeaderboard()
        {
            return new List<PlayerData>(_players);
        }

        public List<PlayerData> GetSortedList(Comparison<PlayerData> comparison)
        {
            List<PlayerData> sortedList = new List<PlayerData>(_players);
            sortedList.Sort(comparison);
            int placement = 0;
            foreach (var user in sortedList)
            {
                placement++;
                user.Placement = placement;
            }
            return sortedList;
        }

        public void DeleteLeaderboard()
        {
            _players.Clear();
            SaveLeaderboard();
        }

        public void AddUser(PlayerData user)
        {
            _players.Add(user);
            SaveLeaderboard();
        }

        public void SaveLeaderboard()
        {
            BinaryFormatter binForm = new BinaryFormatter();
            FileStream file = File.Create(SaveFilePath);
            binForm.Serialize(file, _players);
            file.Close();
        }

        public void LoadLeaderboard()
        {
            if (!File.Exists(SaveFilePath))
            {
                Debug.LogError($"There is no saved file at: {SaveFilePath}");
                return;
            }
        
            BinaryFormatter binForm = new BinaryFormatter();
            using FileStream file = File.Open(SaveFilePath, FileMode.Open);
            _players = (List<PlayerData>)binForm.Deserialize(file);
            file.Close();
        }

        public void CreateLeaderboard()
        {
            throw new NotImplementedException();
        }

        public void UpdateLeaderboard()
        {
            throw new NotImplementedException();
        }
    }
}
