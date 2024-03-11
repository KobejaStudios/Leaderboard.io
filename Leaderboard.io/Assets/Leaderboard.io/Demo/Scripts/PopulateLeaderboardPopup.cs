using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Leaderboard.io
{
    public class PopulateLeaderboardPopup : MonoBehaviour
    {
        private ILeaderboardService _leaderboardService;
        private IRandomIdGenerator _randomIdGenerator;
        private IRandomNameGenerator _randomNameGenerator;
        
        [SerializeField] private InputField _numberOfPlayersInput;
        [SerializeField] private InputField _scoreRangeMinInput;
        [SerializeField] private InputField _scoreRangeMaxInput;

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
            _randomIdGenerator = ServiceLocator.GetService<IRandomIdGenerator>();
            _randomNameGenerator = ServiceLocator.GetService<IRandomNameGenerator>();
        }

        [ContextMenu("pop board")]
        public void SubmitLeaderboardPopulation()
        {
            int numberOfPlayers = int.Parse(_numberOfPlayersInput.text);
            int scoreRangeMin = int.Parse(_scoreRangeMinInput.text);
            int scoreRangeMax = int.Parse(_scoreRangeMaxInput.text);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (!TryCreatePlayer(scoreRangeMin, scoreRangeMax)) break;
            }
        }

        private bool TryCreatePlayer(int scoreRangeMin, int scoreRangeMax)
        {
            string randomName = _randomNameGenerator.GetRandomName();
            if (string.IsNullOrEmpty(randomName))
            {
                Debug.LogError("Could not fetch a random name, breaking from the player creation loop");
                return false;
            }

            try
            {
                PlayerData player = new PlayerData
                {
                    Score = Random.Range(scoreRangeMin, scoreRangeMax),
                    Id = _randomIdGenerator.GetRandomId(),
                    PlayerName = randomName,
                    IsLocalPlayer = false,
                    Placement = 0
                };
                _leaderboardService.AddPlayer(player, false);
            }
            catch (Exception e)
            {
                Debug.LogError($"Something broke when adding player: {e.Message}");
                throw;
            }
            return true;
        }
    }
}
