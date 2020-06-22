using System;
using UnityEngine;

namespace Game
{
    // TODO: rename to PlayerData
    [System.Serializable]
    public class GameData
    {
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

        public void Init(GameStats gameStats)
        {
            _timeLastPlayed = gameStats._timeLastPlayed;
        }
    }

    // TODO: Do we even need this?
    // TODO: Move these stats to PlayerStats
    [System.Serializable]
    public class GameStats
    {
        public long _timeLastPlayed;
        public int _level;
    }
}