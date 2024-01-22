using System;
using System.Collections;
using System.Collections.Generic;
using Leaderboard.io;
using UnityEngine;

public class Test : MonoBehaviour
{
    private IRandomIdGenerator _randomIdGenerator;
    private ILeaderboardService _leaderboardService;
    private void Start()
    {
        _randomIdGenerator = ServiceLocator.GetService<IRandomIdGenerator>();
        _leaderboardService = ServiceLocator.GetService<ILeaderboardService>();
    }

    [ContextMenu("log ids")]
    private void LogIds()
    {
        foreach (var id in _randomIdGenerator.GetAllIds())
        {
            Debug.Log($"all ids: {id}");
        }

        foreach (var id in _randomIdGenerator.GetIdsInUse())
        {
            Debug.Log($"in use: {id}");
        }
    }

    [ContextMenu("get random id")]
    private void GetRandomId()
    {
        _randomIdGenerator.GenerateRandomIds(20);
        Debug.Log($"id: {_randomIdGenerator.GetRandomId()}");
    }

    [ContextMenu("wipe id data")]
    private void WipeIdData()
    {
        _randomIdGenerator.WipeData();
    }

    [ContextMenu("update players")]
    private void UpdatePlayers()
    {
        _leaderboardService.UpdatePlayers(8, 3, 5, 1.15f, 1.67f);
    }
}
