using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Leaderboard.io
{
    public interface ILeaderboardService
    {
        PlayerData GetLocalPlayer();
        PlayerData GetPlayer(string id);
        List<PlayerData> GetLeaderboard(Comparison<PlayerData> comparison);
        void InitializeLocalPlayer();
        void UpdatePlayer(string id, Action<PlayerData> updateAction);
        void DeleteLeaderboard();
        void AddPlayer(PlayerData user);
        void SaveLeaderboard();
        void LoadLeaderboard();
        void CreateLeaderboard();
        void UpdateLeaderboard(Comparison<PlayerData> comparison);
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

        public PlayerData GetLocalPlayer()
        {
            return _players.Find(x => x.IsLocalPlayer);
        }

        public PlayerData GetPlayer(string id)
        {
            return _players.Find(x => x.Id == id);
        }

        public List<PlayerData> GetLeaderboard(Comparison<PlayerData> comparison)
        {
            List<PlayerData> copy = new List<PlayerData>(_players);
            copy.Sort(comparison);
            for (int i = 0; i < copy.Count; i++)
            {
                copy[i].Placement = i + 1;
            }
            return copy;
        }

        public void InitializeLocalPlayer()
        {
            if (_players.Find(x => x.IsLocalPlayer) != null) return;
            PlayerData localPlayer = new PlayerData
            {
                Placement = 0,
                Score = 0,
                PlayerName = SystemInfo.deviceUniqueIdentifier,
                Id = SystemInfo.deviceUniqueIdentifier,
                IsLocalPlayer = true
            };
            AddPlayer(localPlayer);
        }

        public void UpdatePlayer(string id, Action<PlayerData> updateAction)
        {
            PlayerData playerToUpdate = _players.Find(x => x.Id == id);
            if (playerToUpdate != null)
            {
                updateAction(playerToUpdate);
                SaveLeaderboard();
                return;
            }
            Debug.LogWarning($"Player with id: ({id}) cannot be found");
        }

        public void DeleteLeaderboard()
        {
            _players.Clear();
            SaveLeaderboard();
        }

        public void AddPlayer(PlayerData player)
        {
            _players.Add(player);
            SaveLeaderboard();
        }

        public void SaveLeaderboard()
        {
            BinaryFormatter binForm = new BinaryFormatter();
            try
            {
                using FileStream file = File.Create(SaveFilePath);
                binForm.Serialize(file, _players);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving leaderboard data: {ex.Message}");
                throw;
            }
        }
        
        public void LoadLeaderboard()
        {
            BinaryFormatter binForm = new BinaryFormatter();
            try
            {
                using FileStream file = File.Open(SaveFilePath, FileMode.Open);
                _players = (List<PlayerData>)binForm.Deserialize(file);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error loading leaderboard data: {ex.Message}");
                throw;
            }
        }

        public void CreateLeaderboard()
        {
            throw new NotImplementedException();
        }

        public void UpdateLeaderboard(Comparison<PlayerData> comparison)
        {
            _players.Sort(comparison);
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Placement = i + 1;
            }
            SaveLeaderboard();
        }
    }
}
