using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using System.Linq;

namespace Game
{
    public class PlayerController
    {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;
        private readonly InputManager _inputManager;

        public PlayerController(PlayerModel model, Upgrades.Upgrade[] upgrades, PlayerView view, InputManager inputManager)
        {
            _model = model;
            _view = view;
            _inputManager = inputManager;

            _view.Init(model, upgrades);

            _view.OnClicked += HandleClicked;
            _view.OnCubeDeath += HandleCubeDeath;
            _view.OnLevelPassed += HandleLevelPassed;
            _view.OnTeamWeaponBtnClick += HandleTeamWeaponBtnClick;
            _view.OnClickGunBtnClick += HandleClickGunBtnClick;
            _view.OnResearchBtnClick += HandleResearchBtnClick;
            _view.OnResearchCenterToggleBtnClick += HandleResearchCenterBtnClick;
            _inputManager.OnKeyPress += HandleResearchKeyboardKeyPress;

            _model.PropertyChanged += HandlePropertyChanged;
            _model.OnPlayerStatsChanged += HandlePlayerStatsChanged;
            _model.PlayerStats.TeamSkills.StatChanged += TeamSkills_StatChanged; ;
        }

        private void TeamSkills_StatChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO: more useful info can be retrieved here, rather than: e.PropertyName == "Value"
            // Debug.Log("PlayerController: TeamSkills_StatChanged: " + e.PropertyName);
            _view.TeamPanel.UpdateView(_model.TeamWeapons);
            ResourceLoader.SavePlayerStats(_model.PlayerStats);
        }

        private void HandleResearchCenterBtnClick(object sender, EventArgs e)
        {
            _view.ResearchPanel.IsHidden = !_view.ResearchPanel.IsHidden;
            //_view.ResearchPanel.Hide();
            //Debug.Log("PlayerController: HandleResearchCenterBtnClick");
        }

        private void HandleLevelPassed(object sender, EventArgs e)
        {
            _model.PlayerStats.Level++;
        }

        private void HandleAbilityBtnClick(object sender, UpgradeBtnClickEventArgs e)
        {
            
        }

        private void HandleResearchBtnClick(object sender, UpgradeBtnClickEventArgs e)
        {
            string skillIndexer = e.Upgrade.Skill;

            StatsContainer statsContainer = (StatsContainer)_model.PlayerStats[e.Upgrade.SkillContainer];
            PlayerStat skill = statsContainer.Stats.Find(sk => sk.Name == skillIndexer);
            float cachedFloat = skill.Value;
            skill.Value = cachedFloat + 1;

            e.Upgrade.IsActive = false;
        }

        private void HandlePlayerStatsChanged(object sender, GenericEventArgs<string> args)
        {
            ResourceLoader.SavePlayerStats(_model.PlayerStats);

            //if (args.Val == "TeamSkills")
            //{
            //    _view.TeamPanel.UpdateView(_model.TeamWeapons);
            //}

            if (args.Val == "Gold")
            {
                //_view.Ui.PlayerGoldTxt.text = _model.PlayerStats.Gold.ToString();
                _view.Ui.PlayerGoldTxt.text = _model.PlayerStats.Gold.SciFormat();
                // Don't need to redraw panels if only gold changed
                return;
            }

            if (args.Val == "Level")
            {
                _view.Ui.PlayerLevelTxt.text = _model.PlayerStats.Level.ToString();
                return;
            }
            _view.TeamPanel.UpdateView(_model.TeamWeapons);
            _view.ClickGunPanel.UpdateView(_model.DPS, _model.DMG);
        }

        // One important notice: 
        // 1. Abilities have nothing to do with WeaponStat.Price
        // 2. Abilities only work with WeaponStat.Value
        // 3. 

        // HandleAbilityClicked(object sender, string weapon /*Ability ability*/) 
        // {
        //     string weapon; // string weapon = ability.TargetObjName; 
        //     Weapon weaponToPerformAbilityOn = model.FindWeapon(weapon);
        //     Command command = new Command(new AbilityIncreaseDPSBy1%(weaponToPerformAbilityOn, ability));
        //     ability.Execute();
        // }

        // Command AbilityIncreaseDPSBy1%
        // {
        //     private Weapon weapon;
        //     private abilityProps;
        //      
        //     void Execute() 
        //     {
        //         WeaponStat prevWeaponStat = weapon.DPS; <-- here should be deep copy of an object
        //         weapon.DPS = new WeaponStat(data.dpsLevel, SomeSpecialEmptyAlgorithm.DPS);
        //
        //         //!! MAYBE WE DON'T NEED ABOVE LINES AT ALL !!//
        //         float increaseAmount = 1%amount of weapon.DPS.Value;
        //         weapon.DPS.Value += increaseAmount;
        //         // ... HERE GOES WAITING FOR %ABILITY_DURATION% ... //
        //         weapon.DPS.Value -= increaseAmount;
        //     }
        // }

        // TODO: Instead of updating view manually here, react to teamWeapons updates
        private void HandleResearchKeyboardKeyPress(object sender, InputEventArgs e)
        {
            switch(e.KeyCode)
            {
                case InputEventArgs.INPUT_KEY_CODE.NUM_KEY_1:
                    if (_model.TeamWeapons.ContainsKey("StandardPistol"))
                    {
                        Weapon wpn;

                        if (_model.TeamWeapons.TryGetValue("StandardPistol", out wpn))
                        {
                            wpn.DPS.Upgrade();
                            ResourceLoader.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model.TeamWeapons);
                            Debug.Log("StandardPistol was upgraded");
                            // TODO: (LP):
                            // ideally, 
                            // (1) player controller updates the model, 
                            // (2) player controller reacts to the model update by updating the view
                            //wpn.DPS.Level++;
                            //model.SaveTeamWeapons(model.teamWeapons);
                            //view.TeamPanel.UpdateTeamPanel(model.teamWeapons);
                        }
                    }
                    break;

                case InputEventArgs.INPUT_KEY_CODE.DPS_MULTIPLIER:
                    //float cached = _model.PlayerStats.DPSMultiplier;
                    //_model.PlayerStats["DPSMultiplier"] = cached + 1;
                    break;
                case InputEventArgs.INPUT_KEY_CODE.NUM_KEY_3:
                    Debug.Log("PlayerController: KeyCode_3");
                    break;

                default:
                    break;
            }
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            // TODO (LP): view.Gun.Shoot(model.teamWeapons['PlayerPistol'].model.DPS);
            // For that matter, model.teamWeapons['PlayerPistol'] should be cached in PlayreController on a startup
            // And also, for that matter, playerGun.DPS shouldn't be upgradeable at all, its dps.Multiplier should be 0.
            _view.Gun.Shoot(_model.DMG.Value);
        }

        private void HandleCubeDeath(object sender, CustomArgs e)
        {
            // TODO: null value handling
            // TODO: Add TeamWeaponsSkill: GoldGainedMultiplier
            PlayerStat goldGainedMultiplier = _model.PlayerStats.ClickGunSkills.Stats.Find(stat => stat.Name == "GoldGainedMultiplier");
            float goldGained = e.Val * goldGainedMultiplier.Value;
            _model.PlayerStats.Gold += goldGained;
            // TODO: save 'em every 15 seconds instead.
            // This drastically decreases performance!!!
            ResourceLoader.SavePlayerStats(_model.PlayerStats);
        }

        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is WeaponStat weaponStat)
            {
                Debug.Log("PlayerController: HandlePropertyChanged: model.Changed: weaponStatObject: " + weaponStat.Level);
                _view.ClickGunPanel.UpdateView(_model.DPS, _model.DMG);
            }
        }

        private void HandleClickGunBtnClick(object sender, GenericEventArgs<WeaponStatBtnClickArgs> e)
        {
            string weaponName = e.Val.WeaponName;
            string buttonName = e.Val.ButtonName;

            if (weaponName == "ClickGun")
            {
                switch (buttonName)
                {
                    case "DPS":
                        Debug.Log("PlayerController: HandleClickGunBtnClick: Update DPS");
                        _model.DPS.Level++;
                        break;
                    case "DMG":
                        Debug.Log("PlayerController: HandleClickGunBtnClick: Update DMG");
                        _model.DMG.Level++;
                        break;
                    default:
                        break;
                }
            }
        }

        private void HandleTeamWeaponBtnClick(object sender, GenericEventArgs<WeaponStatBtnClickArgs> e)
        {
            string weaponName = e.Val.WeaponName;
            string buttonName = e.Val.ButtonName;

            if (_model.TeamWeapons.ContainsKey(weaponName))
            {
                Weapon wpn;

                if (_model.TeamWeapons.TryGetValue(weaponName, out wpn))
                {
                    // TODO: (LP): 
                    // ideally, 
                    // (1) player controller updates the model, 
                    // (2) player controller reacts to the model update by updating the view
                    switch (buttonName)
                    {
                        case "DPS":
                            wpn.DPS.Level++;
                            ResourceLoader.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model.TeamWeapons);
                            break;
                        case "DMG":
                            wpn.DMG.Level++;
                            ResourceLoader.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model.TeamWeapons);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}