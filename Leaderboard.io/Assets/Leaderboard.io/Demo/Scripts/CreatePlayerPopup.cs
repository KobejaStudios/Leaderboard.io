using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard.io
{
    public class CreatePlayerPopup : MonoBehaviour
    {
        [SerializeField] private InputField _nameInput;
        [SerializeField] private InputField _scoreInput;
        [SerializeField] private Toggle _isLocalUserToggle;
        private ILeaderboardService _leaderboardService;

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
        }

        public void SubmitPlayer()
        {
            PlayerData newPlayer = new PlayerData
            {
                PlayerName = _nameInput.text,
                Score = int.Parse(_scoreInput.text),
                IsLocalPlayer = _isLocalUserToggle
            };
            _leaderboardService.AddPlayer(newPlayer);
        }
    }
}
