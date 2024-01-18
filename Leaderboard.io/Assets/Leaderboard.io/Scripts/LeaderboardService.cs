using System;
using System.Collections.Generic;
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
        void LogLeaderboard();
        void SaveLeaderboard();
        void CreateLeaderboard();
        void UpdateLeaderboard();
    }
    // Add this `LeaderboardService` to your service locator, inject it with DI, construct one, or use any 
    // other pattern that fits your project
    public class LeaderboardService : ILeaderboardService
    {
        private List<PlayerData> _players = new();
        public LeaderboardService()
        {
            var data = PlayerPrefs.GetString(PrefsKeys.LeaderboardKey);
    
            if (string.IsNullOrEmpty(data))
            {
                _players = new List<PlayerData>();
            }
            else
            {
                try
                {
                    _players = JsonUtility.FromJson<List<PlayerData>>(data);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error deserializing JSON: {ex.Message}");
                    _players = new List<PlayerData>();
                }
            }
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
            var sortedList = new List<PlayerData>(_players);
            sortedList.Sort(comparison);
            var placement = 0;
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

        public void LogLeaderboard()
        {
            foreach (var player in _players)
            {
                Debug.Log($"player: {player.PlayerName}, score: {player.Score}, place: {player.Placement}, isLocal: {player.IsLocalPlayer}");
            }
        }

        public void SaveLeaderboard()
        {
            try
            {
                var data = JsonUtility.ToJson(_players);
                PlayerPrefs.SetString(PrefsKeys.LeaderboardKey, data);
                PlayerPrefs.Save();
                Debug.Log("Leaderboard saved successfully");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving leaderboard: {ex.Message}");
            }
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
