using System;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard.io
{
    public class EditPlayerPopup : MonoBehaviour
    {
        [SerializeField] private InputField _nameInput;
        [SerializeField] private InputField _scoreInput;
        [SerializeField] private Toggle _isLocalUserToggle;
        private ILeaderboardService _leaderboardService;
        private PlayerData _selectedPlayer;
        public static string SelectedPlayerKey = "SelectedPlayerKey";

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
        }

        public void Init()
        {
            string playerId = PlayerPrefs.GetString(SelectedPlayerKey);
            _selectedPlayer = _leaderboardService.GetPlayer(playerId);
            _nameInput.text = _selectedPlayer.PlayerName;
            _scoreInput.text = _selectedPlayer.Score.ToString();
            _isLocalUserToggle.isOn = _selectedPlayer.IsLocalPlayer;
            transform.localPosition = Vector3.zero;
        }

        public void SubmitPlayer()
        {
            _leaderboardService.UpdatePlayer(_selectedPlayer.Id, playerData =>
            {
                playerData.PlayerName = _nameInput.text;
                playerData.Score = int.Parse(_scoreInput.text);
                playerData.IsLocalPlayer = _isLocalUserToggle;
            });
        }

        public void Dismiss()
        {
            transform.localPosition = new Vector3(0, -1000, 0);
        }
    }
}
