using UnityEngine;

namespace Leaderboard.io
{
    public class LaunchInit : MonoBehaviour
    {
        private void Awake()
        {
            ILeaderboardService leaderboardService = new LeaderboardService();
            IRandomIdGenerator randomIdGenerator = new RandomIdGenerator();
            IRandomNameGenerator randomNameGenerator = new RandomNameGenerator();
            
            ServiceLocator.RegisterService(randomNameGenerator);
            ServiceLocator.RegisterService(leaderboardService);
            ServiceLocator.RegisterService(randomIdGenerator);
        }
    }
}
