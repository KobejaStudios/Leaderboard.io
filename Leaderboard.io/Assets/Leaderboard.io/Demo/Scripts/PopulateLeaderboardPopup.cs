using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Leaderboard.io
{
    public class PopulateLeaderboardPopup : MonoBehaviour
    {
        [SerializeField] private DemoButtons _demoButtonsController;
        [SerializeField] private InputField _numberOfPlayersInput;
        [SerializeField] private Text _descText;

        private void Start()
        {
            UpdateDescriptionText();
        }

        [ContextMenu("pop board")]
        public void SubmitLeaderboardPopulation()
        {
            int numberOfPlayers = int.Parse(_numberOfPlayersInput.text);

            for (int i = 0; i < numberOfPlayers; i++)
            {
                _demoButtonsController.AddRandomPlayer();
            }
        }

        public void UpdateDescriptionText()
        {
            int.TryParse(_numberOfPlayersInput.text, out int numberOfPlayers);
            _descText.text = $"Will create {numberOfPlayers} random players with random scores";
        }
    }
}
