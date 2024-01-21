using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard.io
{
    public class LeaderboardUserView : MonoBehaviour
    {
        [SerializeField] private Text _placeText;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _scoreText; 
        private PlayerData _playerData;

        public void Setup(PlayerData playerData)
        {
            _playerData = playerData;
            _placeText.text = playerData.Placement.ToString();
            _nameText.text = playerData.PlayerName;
            _scoreText.text = playerData.Score.ToString();
        }

        public void OnPlayerClicked()
        {
            PlayerPrefs.SetString(EditPlayerPopup.SelectedPlayerKey, _playerData.Id);
            FindObjectOfType<EditPlayerPopup>().Init();
        }
    }
}
