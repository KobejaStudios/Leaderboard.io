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
        }

        public void UpdateView()
        {
            foreach (var user in _leaderboardService.GetSortedList((x, y) => x.Score.CompareTo(y.Score)))
            {
                Instantiate(_userView, _scrollContent)
                    .SetText(user.Placement, user.PlayerName, user.Score);
            }
        }
    }
}
