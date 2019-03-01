using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public event LevelChanged OnLevelChanged;

    public GameStats stats;

    public long _timeLastPlayed;
    public int _level;

    public GameStats GetData()
    {
        return new GameStats
        {
            _level = this._level,
            _timeLastPlayed = DateTime.Now.Ticks
        };
    }

    public void Reset()
    {
        _timeLastPlayed = 0;
        _level = 0;
    }

    public void Init(GameStats gameStats)
    {
        _timeLastPlayed = gameStats._timeLastPlayed;
    }

    public void InvokeLevelChanged()
    {
        OnLevelChanged?.Invoke(_level);
    }
}

// TODO: Do we even need this?
[System.Serializable]
public class GameStats
{
    public long _timeLastPlayed;
    public int _level;
}
