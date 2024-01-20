using UnityEngine;
using Random = UnityEngine.Random;

namespace Leaderboard.io
{
    public class DemoButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _createUserPopup;
        private ILeaderboardService _leaderboardService;
        private IRandomIdGenerator _randomIdGeneratorService;

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
            _randomIdGeneratorService = ServiceLocator.GetService<IRandomIdGenerator>();
            _leaderboardService.InitializeLocalPlayer();
        }

        public void AddPlayer()
        {
            _createUserPopup.SetActive(true);
        }

        public void AddRandomPlayer()
        {
            var player = new PlayerData
            {
                Score = Random.Range(100,5000),
                PlayerName = $"player{Random.Range(1,9999)}",
                Placement = 0,
                IsLocalPlayer = false,
                Id = _randomIdGeneratorService.GetRandomId()
            };
            _leaderboardService.AddPlayer(player);
        }

        public void LogLeaderboard()
        {
            foreach (var playerData in _leaderboardService.GetLeaderboard((x, y) => y.Score.CompareTo(x.Score)))
            {
                Debug.Log($"id: {playerData.Id} name: {playerData.PlayerName}, place: {playerData.Placement}," +
                          $" score: {playerData.Score}, isLocal: {playerData.IsLocalPlayer}");
            }
        }

        public void WipeData()
        {
            _leaderboardService.DeleteLeaderboard();
        }
    }
}
