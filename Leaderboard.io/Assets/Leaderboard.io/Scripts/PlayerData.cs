namespace Leaderboard.io
{
    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName { get; set; }
        public int Placement { get; set; }
        public int Score { get; set; }
        public bool IsLocalPlayer { get; set; }
        public string Id { get; set; }
    }
}
