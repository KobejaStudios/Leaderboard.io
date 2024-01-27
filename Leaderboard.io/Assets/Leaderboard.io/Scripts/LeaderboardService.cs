using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = System.Random;

namespace Leaderboard.io
{
    public interface ILeaderboardService
    {
        PlayerData GetLocalPlayer();
        PlayerData GetPlayer(string id);
        PlayerData GetPlayer(int placement);
        List<PlayerData> GetLeaderboard(Comparison<PlayerData> comparison);
        void InitializeLocalPlayer();
        void UpdatePlayer(string id, Action<PlayerData> updateAction);
        void UpdateLocalPlayer(int value);
        void DeleteLeaderboard();
        void AddPlayer(PlayerData user);
        void SaveLeaderboard();
        void LoadLeaderboard();
        void CreateLeaderboard();
        void UpdateLeaderboard(Comparison<PlayerData> comparison);
        void UpdatePlayers(int segment, int minPlayerRange, int maxPlayerRange, float minScoreFactor, float maxScoreFactor);
        void UpdatePlayers(int segment, int playerRange, float scoreFactor);
    }
    
    public class LeaderboardService : ILeaderboardService
    {
        private List<PlayerData> _players = new();
        private string SaveFilePath => Application.persistentDataPath + "/leaderboardData.dat";
        public LeaderboardService()
        {
            LoadLeaderboard();
        }

        /// <summary>
        /// Returns the local player found in the internal player collection.
        /// If no player is found it will return null.
        /// </summary>
        /// <returns></returns>
        public PlayerData GetLocalPlayer()
        {
            return _players.Find(x => x.IsLocalPlayer);
        }

        /// <summary>
        /// Returns a player by id found in the internal player collection.
        /// If no player is found it will return null.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlayerData GetPlayer(string id)
        {
            return _players.Find(x => x.Id == id);
        }

        /// <summary>
        /// Returns a player by placement found in the internal player collection.
        /// If no player is found it will return null.
        /// </summary>
        /// <param name="placement"></param>
        /// <returns></returns>
        public PlayerData GetPlayer(int placement)
        {
            return _players.Find(x => x.Placement == placement);
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

        /// <summary>
        /// Initializes the local player by checking if it already exists in the internal player collection.
        /// If not found, a new local player is created with default properties such as a unique identifier,
        /// initial placement, score of 0, and marked as the local player. The local player is then added to the
        /// internal player collection.
        /// This can be called on app init to ensure there is always a local player defined.
        /// </summary>
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

        /// <summary>
        /// Updates a player in the internal player collection based on the specified player ID.
        /// The update is performed using the provided action.
        /// </summary>
        /// <param name="id">Unique identifier of the player to update.</param>
        /// <param name="updateAction">Action defining the update to be applied to the player.</param>
        public void UpdatePlayer(string id, Action<PlayerData> updateAction)
        {
            PlayerData playerToUpdate = _players.Find(x => x.Id == id);
            if (playerToUpdate != null)
            {
                updateAction(playerToUpdate);
                return;
            }
            Debug.LogWarning($"Player with id: ({id}) cannot be found");
        }

        public void UpdateLocalPlayer(int value)
        {
            PlayerData localPlayerData = GetLocalPlayer();
            localPlayerData.Score = value;
            SaveLeaderboard();
        }

        /// <summary>
        /// Warning! This will delete all of the data from the internal players collection.
        /// Deletes the data in the internal players collection.
        /// Then saves the empty data to disk.
        /// </summary>
        public void DeleteLeaderboard()
        {
            _players.Clear();
            SaveLeaderboard();
        }

        /// <summary>
        /// Adds a player to the internal players collection and then saves the changes.
        /// </summary>
        /// <param name="player"> Definition of PlayerData for new player </param>
        /// <param name="isAutoSave"> Control over if the service will auto-save the changes or not. Defaulted
        ///  to true </param>
        public void AddPlayer(PlayerData player)
        {
            _players.Add(player);
            SaveLeaderboard();
        }

        /// <summary>
        /// Saves the current state of the leaderboard by serializing the internal player collection
        /// and storing it in a binary file at the specified file path.
        /// </summary>
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
        
        /// <summary>
        /// Loads the leaderboard data by deserializing the content of a binary file
        /// located at the specified file path. The deserialized player collection is
        /// assigned to the internal player collection.
        /// </summary>
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

        /// <summary>
        /// Updates the leaderboard by sorting the internal player collection based on the specified comparison,
        /// assigning placements to players according to their positions after sorting,
        /// and saving the updated leaderboard.
        /// </summary>
        /// <param name="comparison">Comparison function defining the sorting order for players.</param>
        public void UpdateLeaderboard(Comparison<PlayerData> comparison)
        {
            _players.Sort(comparison);
            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Placement = i + 1;
            }
            SaveLeaderboard();
        }

        /// <summary>
        /// Updates a subset of players in the internal player collection based on specified parameters.
        /// Players within each segment are shuffled randomly, and their scores are updated according to
        /// random factors within specified ranges. The number of players to update (playerRange)
        /// and the score adjustment factor (scoreFactor) are randomly determined for each segment.
        /// </summary>
        /// <param name="segment">Size of each segment for player processing.</param>
        /// <param name="minPlayerRange">Minimum number of players to update within each segment.</param>
        /// <param name="maxPlayerRange">Maximum number of players to update within each segment.</param>
        /// <param name="minScoreFactor">Minimum factor by which to adjust player scores during the update.</param>
        /// <param name="maxScoreFactor">Maximum factor by which to adjust player scores during the update.</param>
        public void UpdatePlayers(int segment, int minPlayerRange, int maxPlayerRange, float minScoreFactor, float maxScoreFactor)
        {
            List<PlayerData> segmentList = new List<PlayerData>();
            foreach (PlayerData player in _players)
            {
                segmentList.Add(player);
                if (player.Placement % segment != 0) continue;

                int playerRange = UnityEngine.Random.Range(minPlayerRange, maxPlayerRange + 1);
                float scoreFactor = UnityEngine.Random.Range(minScoreFactor, maxScoreFactor);
                PlayerData[] shuffledList = segmentList.ToArray();
                
                Shuffle(shuffledList);
                ExecutePlayerUpdates(playerRange, scoreFactor, shuffledList, segmentList);
            }
        }

        /// <summary>
        /// Updates a subset of players in the internal player collection based on a specified segment size,
        /// player range, and score factor. Players within each segment are shuffled randomly, and their scores
        /// are updated according to the provided factor.
        /// </summary>
        /// <param name="segment">Size of each segment for player processing.</param>
        /// <param name="playerRange">Number of players to process within each segment.</param>
        /// <param name="scoreFactor">Factor by which to adjust player scores during the update.</param>
        public void UpdatePlayers(int segment, int playerRange, float scoreFactor)
        {
            List<PlayerData> segmentList = new List<PlayerData>();
            foreach (PlayerData player in _players)
            {
                segmentList.Add(player);
                if (player.Placement % segment != 0) continue;

                PlayerData[] shuffledList = segmentList.ToArray();
                Shuffle(shuffledList);
                ExecutePlayerUpdates(playerRange, scoreFactor, shuffledList, segmentList);
            }
        }

        private void ExecutePlayerUpdates(int playerRange, float scoreFactor, PlayerData[] shuffledList, List<PlayerData> segmentList)
        {
            for (int i = 0; i < Math.Min(playerRange, shuffledList.Length - 1); i++)
            {
                var oldPlayerScore = shuffledList[i].Score;
                UpdatePlayer(shuffledList[i].Id, playerData => { playerData.Score = (int)(playerData.Score * scoreFactor); });
                Debug.Log($"updated {shuffledList[i].PlayerName} from score: {oldPlayerScore} to: {shuffledList[i].Score}");
            }

            segmentList.Clear();
        }

        /// <summary>
        /// Fisher-Yates Shuffle alg gives O(n) time complexity.
        /// This alg is used to shuffle the segmented lists for random player updates.
        /// </summary>
        /// <param name="array">Collection to shuffle.</param>
        /// <typeparam name="T">Type of collection.</typeparam>
        private void Shuffle<T>(T[] array)
        {
            Random random = new Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
