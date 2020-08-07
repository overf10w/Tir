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

        public PlayerController(PlayerModel model, UpgradesSO upgrades, PlayerView view, InputManager inputManager)
        {
            _model = model;
            _view = view;
            _inputManager = inputManager;

            _view.Init(model, upgrades);

            _view.Clicked += ClickedHandler;
            _view.CubeDeath += CubeDeathHandler;
            _view.LevelPassed += LevelPassedHandler;
            _view.TeamWeaponBtnClicked += TeamWeaponBtnClickHandler;
            _view.ClickGunBtnClicked += ClickGunBtnClickHandler;
            _inputManager.KeyPressed += ResearchKeyboardKeyPressHandler;

            _model.PropertyChanged += ModelChangedHandler;
            _model.OnPlayerStatsChanged += PlayerStatsChangedHandler;
            _model.PlayerStats.TeamSkills.StatChanged += TeamSkillsChangedHandler;
        }

        private void TeamSkillsChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            // TODO: more useful info can be retrieved here, rather than: e.PropertyName == "Value"
            _view.TeamPanel.UpdateView(_model);
            ResourceLoader.SavePlayerStatsData(_model.PlayerStats);
        }

        private void LevelPassedHandler(object sender, EventArgs e)
        {
            _model.PlayerStats.Level++;
        }

        private void HandleAbilityBtnClick(object sender, UpgradeBtnClickEventArgs e)
        {
            
        }

        private void PlayerStatsChangedHandler(object sender, EventArgs<string> args)
        {
            ResourceLoader.SavePlayerStatsData(_model.PlayerStats);

            //if (args.Val == "TeamSkills")
            //{
            //    _view.TeamPanel.UpdateView(_model.TeamWeapons);
            //}

            if (args.Val == "Gold")
            {
                //_view.Ui.PlayerGoldTxt.text = _model.PlayerStats.Gold.ToString();
                //_view.Ui.PlayerGoldTxt.text = _model.PlayerStats.Gold.SciFormat();
                _view.Ui.UpdatePlayerGold(_model.PlayerStats.Gold);

                _view.TeamPanel.UpdateView(_model);
                _view.ClickGunPanel.UpdateView(_model, _model.DPS, _model.DMG);
                // Don't need to redraw panels if only gold changed
                return;
            }

            if (args.Val == "Level")
            {
                _view.Ui.PlayerLevelTxt.text = _model.PlayerStats.Level.ToString();
                return;
            }
            _view.TeamPanel.UpdateView(_model);
            _view.ClickGunPanel.UpdateView(_model, _model.DPS, _model.DMG);
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

        // TODO: Remove 
        private void ResearchKeyboardKeyPressHandler(object sender, InputEventArgs e)
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
                            _view.TeamPanel.UpdateView(_model);
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

        private void ClickedHandler(object sender, EventArgs e)
        {
            // TODO (LP): view.Gun.Shoot(model.teamWeapons['PlayerPistol'].model.DPS);
            // For that matter, model.teamWeapons['PlayerPistol'] should be cached in PlayreController on a startup
            // And also, for that matter, playerGun.DPS shouldn't be upgradeable at all, its dps.Multiplier should be 0.
            _view.Gun.Shoot(_model.DMG.Value);
        }

        private void CubeDeathHandler(object sender, EventArgs<float> e)
        {
            // TODO: null value handling
            // TODO: Add TeamWeaponsSkill: GoldGainedMultiplier
            PlayerStat goldGainedMultiplier = _model.PlayerStats.ClickGunSkills.Stats.Find(stat => stat.Name == "GoldGainedMultiplier");
            float goldGained = e.Val * goldGainedMultiplier.Value;
            _model.PlayerStats.Gold += goldGained;
            // TODO: save 'em every 15 seconds instead.
            // This drastically decreases performance!!!
            ResourceLoader.SavePlayerStatsData(_model.PlayerStats);
        }

        private void ModelChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender is WeaponStat weaponStat)
            {
                _view.ClickGunPanel.UpdateView(_model, _model.DPS, _model.DMG);
            }
        }

        private void ClickGunBtnClickHandler(object sender, EventArgs<WeaponStatBtnClickArgs> e)
        {
            string weaponName = e.Val.WeaponName;
            string buttonName = e.Val.ButtonName;

            if (weaponName == "ClickGun")
            {
                switch (buttonName)
                {
                    case "DPS":
                        if (_model.PlayerStats.Gold >= _model.DPS.Price)
                        {
                            _model.PlayerStats.Gold -= _model.DPS.Price;
                            _model.DPS.Level++;
                        }
                        break;
                    case "DMG":
                        if (_model.PlayerStats.Gold >= _model.DMG.Price)
                        {
                            _model.PlayerStats.Gold -= _model.DMG.Price;
                            _model.DMG.Level++;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void TeamWeaponBtnClickHandler(object sender, EventArgs<WeaponStatBtnClickArgs> e)
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
                            if (_model.PlayerStats.Gold >= wpn.DPS.Price)
                            {
                                _model.PlayerStats.Gold -= wpn.DPS.Price;
                                wpn.DPS.Level++;
                            }
                            ResourceLoader.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model);
                            break;
                        case "DMG":
                            if (_model.PlayerStats.Gold >= wpn.DMG.Price)
                            {
                                _model.PlayerStats.Gold -= wpn.DMG.Price;
                                wpn.DMG.Level++;
                            }
                            ResourceLoader.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}