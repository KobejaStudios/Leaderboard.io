using System;
using System.Collections;
using System.Collections.Generic;
using Leaderboard.io;
using UnityEngine;

public class Test : MonoBehaviour
{
    private IRandomIdGenerator _randomIdGenerator;
    private void Start()
    {
        _randomIdGenerator = ServiceLocator.GetService<IRandomIdGenerator>();
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
}
