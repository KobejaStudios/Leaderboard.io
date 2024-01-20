using UnityEngine;

namespace Leaderboard.io
{
    public class LaunchInit : MonoBehaviour
    {
        private void Awake()
        {
            ILeaderboardService leaderboardService = new LeaderboardService();
            IRandomIdGenerator randomIdGenerator = new RandomIdGenerator();
            ServiceLocator.RegisterService(leaderboardService);
            ServiceLocator.RegisterService(randomIdGenerator);
        }
    }
}
