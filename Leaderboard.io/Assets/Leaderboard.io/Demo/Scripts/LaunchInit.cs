using UnityEngine;

namespace Leaderboard.io
{
    public class LaunchInit : MonoBehaviour
    {
        private void Awake()
        {
            ILeaderboardService leaderboardService = new LeaderboardService();
            ServiceLocator.RegisterService(leaderboardService);
        }
    }
}
