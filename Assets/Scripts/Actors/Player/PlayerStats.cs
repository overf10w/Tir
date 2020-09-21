using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PlayerStatData // Won't be shown directly anywhere in the editor
    {
        [SerializeField] private string _name;
        public string Name { get => _name; set { _name = value; } }

        [SerializeField] private float _value;
        public float Value { get => _value; set { _value = value; } }
    }

    [System.Serializable]
    public class PlayerStatsData // stored on disk, will not be shown directly in the editor, but rather will initialize a playerData ScriptableObject
    {
        public PlayerStatsData(PlayerStats playerStats)
        {
            _gold = playerStats.Gold;
            _level = playerStats.Level;
            _lastPlayTimestamp = playerStats.LastPlayTimestamp;

            _teamSkills = new List<PlayerStatData>();
            _teamSkills = playerStats.GetStatsDataList(playerStats.TeamSkills.List);

            _clickGunSkills = new List<PlayerStatData>();
            _clickGunSkills = playerStats.GetStatsDataList(playerStats.ClickGunSkills.List);

            _artifacts = new List<PlayerStatData>();
            _artifacts = playerStats.GetStatsDataList(playerStats.Artifacts.List);

            _weaponsLevels = new List<PlayerStatData>();
            _weaponsLevels = playerStats.GetStatsDataList(playerStats.WeaponsLevels.List);

            _weaponsMultipliers = new List<PlayerStatData>();
            _weaponsMultipliers = playerStats.GetStatsDataList(playerStats.WeaponsMultipliers.List);
        }

        [SerializeField] private float _gold;
        public float Gold { get => _gold; set { _gold = value; } }

        [SerializeField] private int _level;
        public int Level { get => _level; set { _level = value; } }

        [SerializeField] private long _lastPlayTimestamp;
        public long LastPlayTimestamp { get => _lastPlayTimestamp; set { _lastPlayTimestamp = value; } }

        public long IdleTimeSpan => _lastPlayTimestamp == 0 ? 0 : DateTime.Now.Ticks - _lastPlayTimestamp;

        [SerializeField] private List<PlayerStatData> _teamSkills;
        public List<PlayerStatData> TeamSkills => _teamSkills;

        [SerializeField] private List<PlayerStatData> _clickGunSkills;
        public List<PlayerStatData> ClickGunSkills => _clickGunSkills;

        [SerializeField] private List<PlayerStatData> _artifacts;
        public List<PlayerStatData> Artifacts => _artifacts;

        [SerializeField] private List<PlayerStatData> _weaponsLevels;
        public List<PlayerStatData> WeaponsLevels => _weaponsLevels;

        [SerializeField] private List<PlayerStatData> _weaponsMultipliers;
        public List<PlayerStatData> WeaponsMultipliers => _weaponsMultipliers;
    }

    [System.Serializable]
    public class PlayerStats : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        [Header("Player")]
        [SerializeField] private float _gold;
        public float Gold { get => _gold; set { SetField(ref _gold, value); } }

        [SerializeField] private int _level;
        public int Level { get => _level; set { SetField(ref _level, value); } }

        [SerializeField] private long _lastPlayTimestamp;
        public long LastPlayTimestamp { get => _lastPlayTimestamp; set { SetField(ref _lastPlayTimestamp, value); } }

        public long IdleTimeSpan => _lastPlayTimestamp == 0 ? 0 : DateTime.Now.Ticks - _lastPlayTimestamp;

        public float IdleEarnings { get; set; }

        [SerializeField] private List<PlayerStat> _teamSkills;
        [NonSerialized]
        private StatsList _teamSkillsList;
        public StatsList TeamSkills
        {
            get
            {
                if (_teamSkillsList == null)
                {
                    _teamSkillsList = new StatsList(_teamSkills);
                }
                return _teamSkillsList;
            }
        }

        [SerializeField] private List<PlayerStat> _clickGunSkills;
        [NonSerialized]
        private StatsList _clickGunSkillsList;
        public StatsList ClickGunSkills
        {
            get
            {
                if (_clickGunSkillsList == null)
                {
                    _clickGunSkillsList = new StatsList(_clickGunSkills);
                }
                return _clickGunSkillsList;
            }
        }

        [SerializeField] private List<PlayerStat> _artifacts;
        [NonSerialized]
        private StatsList _artifactsList;
        public StatsList Artifacts
        {
            get
            {
                if (_artifactsList == null)
                {
                    _artifactsList = new StatsList(_artifacts);
                }
                return _artifactsList;
            }
        }

        [SerializeField] private List<PlayerStat> _weaponsLevels;
        [NonSerialized]
        private StatsList _weaponsLevelsList;
        public StatsList WeaponsLevels
        {
            get
            {
                if (_weaponsLevelsList == null)
                {
                    _weaponsLevelsList = new StatsList(_weaponsLevels);
                }
                return _weaponsLevelsList;
            }
        }

        [SerializeField] private List<PlayerStat> _weaponsMultipliers;
        [NonSerialized]
        private StatsList _weaponsMultipliersList;
        public StatsList WeaponsMultipliers
        {
            get
            {
                if (_weaponsMultipliersList == null)
                {
                    _weaponsMultipliersList = new StatsList(_weaponsMultipliers);
                }
                return _weaponsMultipliersList;
            }
        }

        public void SetPlayerStats(PlayerStatsData playerStatsData)
        {
            _gold = playerStatsData.Gold;
            _level = playerStatsData.Level;
            _lastPlayTimestamp = playerStatsData.LastPlayTimestamp;

            SetStatsList(_teamSkills, playerStatsData.TeamSkills);
            SetStatsList(_clickGunSkills, playerStatsData.ClickGunSkills);
            SetStatsList(_artifacts, playerStatsData.Artifacts);
            SetStatsList(_weaponsLevels, playerStatsData.WeaponsLevels);
            SetStatsList(_weaponsMultipliers, playerStatsData.WeaponsMultipliers);
        }

        // Indexer
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        // Used by binary serializer (disk data serialization)
        // Used to serialize TeamSkillsData to disk
        #region BinarySerializerMethods
        public void SetStatsList(List<PlayerStat> stats, List<PlayerStatData> statsDatas)
        {
            Debug.Log("stats == null: " + (stats == null).ToString() + "statsDatas == null: " + (statsDatas == null).ToString());
            if (statsDatas.Count != stats.Count)
            {
                string warning =
                    "PlayerModel.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
            }

            foreach (var kek in statsDatas)
            {
                bool alreadyContains = false;
                for (int i = 0; i < stats.Count; i++)
                {
                    if (stats[i].Name == kek.Name)
                    {
                        alreadyContains = true;
                        stats[i].Value = kek.Value;
                        break;
                    }
                }

                if (!alreadyContains)
                {
                    PlayerStat teamSkill = new PlayerStat();
                    teamSkill.Name = kek.Name;
                    teamSkill.Value = kek.Value;
                    stats.Add(teamSkill);
                }
            }
        }

        public List<PlayerStatData> GetStatsDataList(List<PlayerStat> stats)
        {
            List<PlayerStatData> statsData = new List<PlayerStatData>(stats.Count);
            for (int i = 0; i < stats.Count; i++)
            {
                var data = new PlayerStatData();
                data.Name = stats[i].Name;
                data.Value = stats[i].Value;
                statsData.Add(data);
            }
            return statsData;
        }
        #endregion

        public PlayerStatsData GetPlayerStatsData()
        {
            PlayerStatsData playerStatsData = new PlayerStatsData(this);
            return playerStatsData;
        }
    }

    // TODO: rename to StatsList
    public class StatsList
    {
        [SerializeField] private List<PlayerStat> _list;
        public List<PlayerStat> List { get; private set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatChanged;

        public StatsList(List<PlayerStat> statsList)
        {
            List = statsList;

            foreach (var stat in List)
            {
                stat.PropertyChanged += PropertyChangedHandler;
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            StatChanged?.Invoke(sender, e);
        }
    }
}