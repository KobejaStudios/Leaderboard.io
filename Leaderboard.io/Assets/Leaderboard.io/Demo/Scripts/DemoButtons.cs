using UnityEngine;
using Random = System.Random;

namespace Leaderboard.io
{
    public class DemoButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _createUserPopup;
        [SerializeField] private GameObject _populateLeaderboarPopup;
        private ILeaderboardService _leaderboardService;
        private IRandomIdGenerator _randomIdGeneratorService;
        private IRandomNameGenerator _randomNameGeneratorService;
        private Random _random = new();

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
            _randomIdGeneratorService = ServiceLocator.GetService<IRandomIdGenerator>();
            _randomNameGeneratorService = ServiceLocator.GetService<IRandomNameGenerator>();
            _leaderboardService.InitializeLocalPlayer();
        }

        public void AddPlayer()
        {
            _createUserPopup.SetActive(true);
        }

        public void AddRandomPlayer()
        {
            var id = _random.Next(100,99999);
            var player = new PlayerData
            {
                Score = UnityEngine.Random.Range(100,5000),
                PlayerName = $"player{id}",
                Placement = 0,
                IsLocalPlayer = false,
                Id = id.ToString()
            };
            _leaderboardService.AddPlayer(player);
        }

        public void PopulateLeaderboard()
        {
            _populateLeaderboarPopup.SetActive(true);
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
            _randomNameGeneratorService.WipeData();
            _randomIdGeneratorService.WipeData();
            PlayerPrefs.DeleteAll();
        }
    }
}
