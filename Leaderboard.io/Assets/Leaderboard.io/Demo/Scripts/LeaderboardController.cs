using UnityEngine;

namespace Leaderboard.io
{
    public class LeaderboardController : MonoBehaviour
    {
        [SerializeField] private LeaderboardUserView _userView;
        [SerializeField] private RectTransform _scrollContent;
        private ILeaderboardService _leaderboardService;

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
            UpdateView();
        }

        public void UpdateView()
        {
            foreach (Transform child in _scrollContent)
            {
                Destroy(child.gameObject);
            }
            foreach (var user in _leaderboardService.GetSortedList((x, y) => y.Score.CompareTo(x.Score)))
            {
                Instantiate(_userView, _scrollContent)
                    .SetText(user.Placement, user.PlayerName, user.Score);
            }
        }
    }
}
