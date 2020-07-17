using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

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

            _view.OnTeamWeaponBtnClick += HandleTeamWeaponBtnClick;
            _view.OnClickGunBtnClick += HandleClickGunBtnClick;
            _view.OnUpgradeBtnClick += HandleUpgradeBtnClick;

            _inputManager.OnKeyPress += HandleResearchKeyPress;

            _model.PropertyChanged += HandlePropertyChanged;
            _model.OnPlayerStatsChanged += HandlePlayerStatsChanged;
        }

        private void HandleUpgradeBtnClick(object sender, UpgradeBtnClickEventArgs e)
        {
            string indexer = e.Upgrade.Indexer;
            float cached = (float)_model.PlayerStats[indexer];
            _model.PlayerStats[indexer] = cached + 1;
            e.Upgrade.IsActive = false;
        }

        private void HandlePlayerStatsChanged(object sender, GenericEventArgs<string> args)
        {

            if (args.Val == "Gold")
            {
                _view.Ui.PlayerGoldTxt.text = _model.PlayerStats.Gold.ToString();
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
        private void HandleResearchKeyPress(object sender, InputEventArgs e)
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
                            _model.SaveTeamWeapons(_model.TeamWeapons);
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
                    float cached = _model.PlayerStats.DPSMultiplier;
                    _model.PlayerStats["DPSMultiplier"] = cached + 1;
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
            _model.PlayerStats.Gold += e.Val;
            // TODO: save 'em every 15 seconds instead.
            // This drastically decreases performance!!!
            _model.SavePlayerStats();
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
                            _model.SaveTeamWeapons(_model.TeamWeapons);
                            _view.TeamPanel.UpdateView(_model.TeamWeapons);
                            break;
                        case "DMG":
                            wpn.DMG.Level++;
                            _model.SaveTeamWeapons(_model.TeamWeapons);
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