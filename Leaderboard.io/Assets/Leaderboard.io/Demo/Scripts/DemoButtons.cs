using UnityEngine;
using Random = UnityEngine.Random;

namespace Leaderboard.io
{
    public class DemoButtons : MonoBehaviour
    {
        private ILeaderboardService _leaderboardService;

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
        }

        public void AddRandomUser()
        {
            var user = new PlayerData
            {
                Score = Random.Range(100,5000),
                PlayerName = $"user{Random.Range(1,9999)}",
                Placement = 0,
                IsLocalPlayer = false
            };
            _leaderboardService.AddUser(user);
        }

        public void LogLeaderboard()
        {
            foreach (var playerData in _leaderboardService.GetSortedList((x, y) => y.Score.CompareTo(x.Score)))
            {
                Debug.Log($"name: {playerData.PlayerName}, place: {playerData.Placement}," +
                          $" score: {playerData.Score}, isLocal: {playerData.IsLocalPlayer}");
            }
        }

        public void WipeData()
        {
            _leaderboardService.DeleteLeaderboard();
        }
    }
}
