using System;
using System.Globalization;
using UnityEngine;

namespace Leaderboard.io
{
    public class LeaderboardUpdateController : MonoBehaviour
    {
        [Tooltip("Set a cycle for automated leaderboard updates")]
        [SerializeField] private LeaderboardUpdateCycle _updateCycle;
        private DateTime _newUpdateDate;
        private ILeaderboardService _leaderboardService;
        public const string NewUpdateDateKey = "NewUpdateDate";

        private void Start()
        {
            _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
            string newUpdateDateString = PlayerPrefs.GetString(NewUpdateDateKey);
            
            if (!string.IsNullOrEmpty(newUpdateDateString))
            {
                _newUpdateDate = DateTime.Parse(newUpdateDateString);
            }

            CheckForUpdates();
        }

        private void CheckForUpdates()
        {
            if (_newUpdateDate >= DateTime.Now) return;
            SetNewUpdateCycle();
            ExecuteLeaderboardUpdate();
        }

        private void SetNewUpdateCycle()
        {
            Debug.Log($"update date has passed: {_newUpdateDate}, current date: {DateTime.Now}");

            int dayDelta = Math.Abs(DateTime.Now.Day - _newUpdateDate.Day);
            int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int newUpdateDayFormula = 7 - dayDelta % 7;
            int newUpdateMonthFormula = daysInCurrentMonth - dayDelta % daysInCurrentMonth;

            switch (_updateCycle)
            {
                case LeaderboardUpdateCycle.Daily:
                    _newUpdateDate = DateTime.Today.AddDays(1);
                    break;
                case LeaderboardUpdateCycle.Weekly:
                    _newUpdateDate = DateTime.Today.AddDays(newUpdateDayFormula);
                    break;
                case LeaderboardUpdateCycle.Monthly:
                    _newUpdateDate = DateTime.Today.AddDays(newUpdateMonthFormula);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PlayerPrefs.SetString(NewUpdateDateKey, _newUpdateDate.ToString(CultureInfo.InvariantCulture));
        }
        
        private void ExecuteLeaderboardUpdate()
        {
            _leaderboardService.UpdatePlayers(5, 2, 4, 1.15f, 1.37f);
            Debug.Log($"Leaderboard has been updated. Setting new update date to: {_newUpdateDate}");
        }
    }
}
