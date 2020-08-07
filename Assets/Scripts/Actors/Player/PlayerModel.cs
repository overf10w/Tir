using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using System.Linq;


namespace Game
{
    // TODO: move PlayerStat(s), PlayerStat(s)Data to PlayerStats.cs file
    [System.Serializable]
    public class PlayerModel
    {
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        public event EventHandler<EventArgs<string>> OnPlayerStatsChanged;

        public PlayerStats PlayerStats { get; private set; }

        public Dictionary<string, Weapon> TeamWeapons { get; private set; }

        // Click Gun Data
        public WeaponData GunData { get; private set; }
        public WeaponStat DPS { get; private set; }
        public WeaponStat DMG { get; private set; }
        // endof Click Gun Data

        public PlayerModel()
        {
            InitPlayerStats();
            InitTeamWeapons(PlayerStats);
            InitClickGun();
        }

        private void InitPlayerStats()
        {
            string _playerStatsDataPath = Path.Combine(Application.persistentDataPath, "playerStatsData.dat");

            PlayerStats = ResourceLoader.LoadPlayerStats();
            PlayerStats.SetPlayerStats(ResourceLoader.Load<PlayerStatsData>(_playerStatsDataPath));
            PlayerStats.PropertyChanged += PlayerStatChangedHandler;
        }

        // TODO (LP): instantiating of TeamWeapons should be moved to PlayerView
        private void InitTeamWeapons(PlayerStats playerStats)
        {
            TeamWeapons = new Dictionary<string, Weapon>();

            WeaponData[] weaponDataArray = ResourceLoader.LoadTeamWeapons();

            foreach (var weaponData in weaponDataArray)
            {
                GameObject obj = new GameObject(weaponData.WeaponName);
                Weapon weaponScript = obj.AddComponent<Weapon>();
                // TODO:
                // 1. Subscribe to weaponScript.OnPropertyChanged
                // 2. Raise the event when notified OnPropertyChanged
                // 3. PlayerController subscribes to this event and changes view accordingly (it just updates the views with the ref to teamWeapons dictionary;
                weaponScript.Init(weaponData, playerStats);
                TeamWeapons.Add(weaponData.WeaponName, weaponScript);
            }
        }

        private void InitClickGun()
        {
            GunData = ResourceLoader.LoadClickGun();

            DPS = new WeaponStat(GunData.DPS, PlayerStats, GunData.algorithms.DPS);
            DMG = new WeaponStat(GunData.DMG, PlayerStats, GunData.algorithms.DMG);

            DPS.PropertyChanged += ClickGunChangedHandler;
            DMG.PropertyChanged += ClickGunChangedHandler;
        }

        private void PlayerStatChangedHandler(object sender, PropertyChangedEventArgs args)
        {
            OnPlayerStatsChanged?.Invoke(sender, new EventArgs<string>(args.PropertyName));
        }

        private void ClickGunChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            ResourceLoader.SaveClickGun(DPS, DMG, GunData);
        }
    }

    [System.Serializable]
    public class PlayerStatData // Won't be shown directly anywhere in the editor
    {
        [SerializeField] private string _name;
        public string Name { get => _name; set { _name = value; } }

        [SerializeField] private float _value;
        public float Value { get => _value; set { _value = value; } }
    }

    [System.Serializable]
    public class PlayerStat : INotifyPropertyChanged
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

        [SerializeField] private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] private string _name;
        public string Name { get => _name; set { SetField(ref _name, value); } }

        [SerializeField] private float _value;
        public float Value { get => _value; set { SetField(ref _value, value); } }
    }

    [System.Serializable]
    public class PlayerStatsData // gets stored on disk, will not be shown directly in the editor, but rather will initialize a playerData ScriptableObject
    { 
        public PlayerStatsData(PlayerStats playerStats)
        {
            _gold = playerStats.Gold;
            _level = playerStats.Level;
            _lastPlayTimestamp = playerStats.LastPlayTimestamp;

            _teamSkills = new List<PlayerStatData>();
            _teamSkills = playerStats.GetTeamSkillsData();

            _clickGunSkills = new List<PlayerStatData>();
            _clickGunSkills = playerStats.GetClickGunSkillsData();

            _artifacts = new List<PlayerStatData>();
            _artifacts = playerStats.GetArtifactsData();
        }

        [SerializeField] private float _gold;
        public float Gold { get => _gold; set { _gold = value; } }

        [SerializeField] private int _level;
        public int Level { get => _level; set { _level = value; } }

        [SerializeField] private long _lastPlayTimestamp;
        public long LastPlayTimestamp { get => _lastPlayTimestamp; set { _lastPlayTimestamp = value; } }

        //pub
        public long IdleTimeSpan => _lastPlayTimestamp == 0 ? 0 : DateTime.Now.Ticks - _lastPlayTimestamp;

        [SerializeField] private List<PlayerStatData> _teamSkills;
        public List<PlayerStatData> TeamSkills => _teamSkills;


        [SerializeField] private List<PlayerStatData> _clickGunSkills;
        public List<PlayerStatData> ClickGunSkills => _clickGunSkills;


        [SerializeField] private List<PlayerStatData> _artifacts;
        public List<PlayerStatData> Artifacts => _artifacts;
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

        [SerializeField] private List<PlayerStat> _teamSkills;
        [NonSerialized]
        private StatsContainer _teamSkillsContainer;
        public StatsContainer TeamSkills
        {
            get
            {
                if (_teamSkillsContainer == null)
                {
                    _teamSkillsContainer = new StatsContainer(_teamSkills);
                }
                return _teamSkillsContainer;
            }
        }

        [SerializeField] private List<PlayerStat> _clickGunSkills;
        [NonSerialized]
        private StatsContainer _clickGunSkillsContainer;
        public StatsContainer ClickGunSkills
        {
            get
            {
                if (_clickGunSkillsContainer == null)
                {
                    _clickGunSkillsContainer = new StatsContainer(_clickGunSkills);
                }
                return _clickGunSkillsContainer;
            }
        }

        [SerializeField] private List<PlayerStat> _artifacts;
        [NonSerialized]
        private StatsContainer _artifactsContainer;
        public StatsContainer Artifacts
        {
            get
            {
                if (_artifactsContainer == null)
                {
                    _artifactsContainer = new StatsContainer(_artifacts);
                }
                return _artifactsContainer;
            }
        }

        public void SetPlayerStats(PlayerStatsData playerStatsData)
        {
            _gold = playerStatsData.Gold;
            _level = playerStatsData.Level;
            _lastPlayTimestamp = playerStatsData.LastPlayTimestamp;

            SetTeamSkillsData(playerStatsData.TeamSkills);
            SetClickGunSkillsData(playerStatsData.ClickGunSkills);
            SetArtifactsData(playerStatsData.Artifacts);
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
        // TEAM_SKILLS
        public List<PlayerStatData> GetTeamSkillsData()
        {
            List<PlayerStatData> teamSkillsData = new List<PlayerStatData>(_teamSkills.Count);

            Debug.Log("_teamSkills.Count: " + _teamSkills.Count + ", teamSkillsData.Count: " + teamSkillsData.Count);

            for (int i = 0; i < _teamSkills.Count; i++)
            {
                var kekData = new PlayerStatData();
                kekData.Name = _teamSkills[i].Name;
                kekData.Value = _teamSkills[i].Value;
                teamSkillsData.Add(kekData);
            }

            return teamSkillsData;
        }
        public void SetTeamSkillsData(List<PlayerStatData> teamSkills)
        {
            if (teamSkills.Count != _teamSkills.Count)
            {
                string warning =
                    "PlayerModel.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
            }

            foreach(var kek in teamSkills)
            {
                bool alreadyContains = false;
                for (int i = 0; i < _teamSkills.Count; i++)
                {
                    if (_teamSkills[i].Name == kek.Name)
                    {
                        alreadyContains = true;
                        _teamSkills[i].Value = kek.Value;
                        break;
                    }
                }

                if (!alreadyContains)
                {
                    PlayerStat teamSkill = new PlayerStat();
                    teamSkill.Name = kek.Name;
                    teamSkill.Value = kek.Value;
                    _teamSkills.Add(teamSkill);
                }
            }
        }

        // CLICK_GUN_SKILLS
        public List<PlayerStatData> GetClickGunSkillsData()
        {
            List<PlayerStatData> clickGunSkillsData = new List<PlayerStatData>(_clickGunSkills.Count);
            for (int i = 0; i < _clickGunSkills.Count; i++)
            {
                var kekData = new PlayerStatData();
                kekData.Name = _clickGunSkills[i].Name;
                kekData.Value = _clickGunSkills[i].Value;
                clickGunSkillsData.Add(kekData);
            }
            return clickGunSkillsData;
        }
        public void SetClickGunSkillsData(List<PlayerStatData> clickGunSkills)
        {
            if (clickGunSkills.Count != _clickGunSkills.Count)
            {
                string warning =
                    "PlayerModel.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
            }

            foreach (var kek in clickGunSkills)
            {
                bool alreadyContains = false;
                for (int i = 0; i < _clickGunSkills.Count; i++)
                {
                    if (_clickGunSkills[i].Name == kek.Name)
                    {
                        alreadyContains = true;
                        _clickGunSkills[i].Value = kek.Value;
                        break;
                    }
                }

                if (!alreadyContains)
                {
                    PlayerStat clickGunSkill = new PlayerStat();
                    clickGunSkill.Name = kek.Name;
                    clickGunSkill.Value = kek.Value;
                    _clickGunSkills.Add(clickGunSkill);
                }
            }
        }

        // ARTIFACTS
        public List<PlayerStatData> GetArtifactsData()
        {
            List<PlayerStatData> artifactsData = new List<PlayerStatData>(_artifacts.Count);
            for (int i = 0; i < _artifacts.Count; i++)
            {
                var kekData = new PlayerStatData();
                kekData.Name = _artifacts[i].Name;
                kekData.Value = _artifacts[i].Value;
                artifactsData.Add(kekData);
            }
            return artifactsData;
        }
        public void SetArtifactsData(List<PlayerStatData> artifacts)
        {
            if (artifacts.Count != _artifacts.Count)
            {
                string warning =
                    "PlayerModel.cs: Upgrades Save File not in sync with Upgrades Scriptable Object\n" +
                    "Setting all Upgrades: IsActive = true";
                Debug.LogWarning(warning);
            }

            foreach (var kek in artifacts)
            {
                bool alreadyContains = false;
                for (int i = 0; i < _artifacts.Count; i++)
                {
                    if (_artifacts[i].Name == kek.Name)
                    {
                        alreadyContains = true;
                        _artifacts[i].Value = kek.Value;
                        break;
                    }
                }

                if (!alreadyContains)
                {
                    PlayerStat artifact = new PlayerStat();
                    artifact.Name = kek.Name;
                    artifact.Value = kek.Value;
                    _artifacts.Add(artifact);
                }
            }
        }
        #endregion
        public PlayerStatsData GetPlayerStatsData()
        {
            PlayerStatsData playerStatsData = new PlayerStatsData(this);
            return playerStatsData;
        }
    }

    // TODO: rename to StatsList
    public class StatsContainer
    {
        public List<PlayerStat> Stats { get; private set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler StatChanged;

        public StatsContainer(List<PlayerStat> stats)
        {
            Stats = stats;

            foreach(var stat in Stats)
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