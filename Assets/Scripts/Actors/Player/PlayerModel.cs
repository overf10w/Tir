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

        public event EventHandler<GenericEventArgs<string>> OnPlayerStatsChanged;

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
            PlayerStats = ResourceLoader.LoadPlayerStats();
            PlayerStats.PropertyChanged += HandlePlayerStatChanged;
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

            DPS.PropertyChanged += HandleClickGunChanged;
            DMG.PropertyChanged += HandleClickGunChanged;
        }

        private void HandlePlayerStatChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPlayerStatsChanged?.Invoke(sender, new GenericEventArgs<string>(args.PropertyName));
        }

        private void HandleClickGunChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
            ResourceLoader.SaveClickGun(DPS, DMG, GunData);
        }
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
            //Debug.Log("PlayerStats: SetField<T>(): Invoked");
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        [SerializeField] private string _name;
        public string Name { get => _name; set { SetField(ref _name, value); } }

        [SerializeField] private float _value;
        public float Value { get => _value; set { SetField(ref _value, value); } }
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
            //Debug.Log("PlayerStats: SetField<T>(): Invoked");
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
        // TODO: seems we don't need a setter here, since _teamSkills can only be set through editor
        //public List<PlayerStat> TeamSkills { get => _teamSkills; set { SetField(ref _teamSkills, value); } }
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

        // >>> ALPHA VERSION CHECKLIST: <<<
        // Upgrades.cs:
        // Upgrade class: add an indexer pool field: string: will be used for referencing either _clickGunStats, _artifacts or _teamSkills
        // Upgrade class: add an indexer weapon field: string: will be used for referencing weapons
        // GameDes element:
        // Upgrades.cs:
        // New upgrade: Synthetize a click gun skill (i.e. click gun goldmultiplier: cube kill money earned (using the click gun) * 1.2) <-- the skill is actually always present, but we reveal it 
        // New upgrade: synthetize a click gun active ability (basically, it's just a skill, but that lasts 30-40 secs and has a cooldown)

        // On GameCanvas: 
        //      TeamWeaponsPanel: Add Skills panel (where _teamSkills purchased in Research Center will be revealed, (actually all the skills >= 1))
        //      ClickGunPanel:    Add Skills panel (where _clickGunSkills purchased in Research Center will be revealed, (actually all the skills >= 1))

        // Player:
        // Add an ability to move/look around our beautiful location (Lul wut?)
        // >>> ENDOF ALPHA VERSION CHECKLIST <<<


        //public SkillsContainer SkillsContainer => _skillsContainer != null ? _skillsContainer : new SkillsContainer(Skills);

        // Indexer (will be used by Upgrade system a lot)
        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }
    }

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
                stat.PropertyChanged += Stat_PropertyChanged;
            }
        }

        private void Stat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            StatChanged?.Invoke(sender, e);
        }
    }
}