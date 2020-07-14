using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Game
{
    public class PlayerController
    {
        private PlayerModel model;
        private PlayerView playerView;

        private InputManager inputManager;

        public PlayerController(PlayerModel model, Upgrades.Upgrade[] upgrades, PlayerView view, InputManager inputManager)
        {
            this.model = model;
            this.playerView = view;
            this.inputManager = inputManager;

            view.Init(model, upgrades);

            view.OnClicked += HandleClicked;
            view.OnCubeDeath += HandleCubeDeath;

            view.OnTeamWeaponBtnClick += HandleTeamWeaponBtnClick;
            view.OnClickGunBtnClick += HandleClickGunBtnClick;
            view.OnUpgradeBtnClick += HandleUpgradeBtnClick;

            inputManager.OnKeyPress += HandleResearchKeyPress;

            model.PropertyChanged += HandlePropertyChanged;
            model.OnGlobalStatChanged += HandleGlobalStatChanged;
        }

        private void HandleUpgradeBtnClick(object sender, UpgradeBtnClickEventArgs e)
        {
            //Debug.Log("PlayerController.cs: HandleUpgradeBtnClick");

            string indexer = e.upgrade.indexer;

            float cached = (float)model.playerStats[indexer];

            model.playerStats[indexer] = cached + 1;

            e.upgrade.IsActive = false;
        }

        private void HandleGlobalStatChanged(object sender, GenericEventArgs<string> args)
        {
            Debug.Log("PlayerController: Notified of GlobalDPSMultiplier change");
            model.UpdateTeamWeapons();
            playerView.TeamPanel.UpdateTeamPanel(model.teamWeapons);
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
                    if (model.teamWeapons.ContainsKey("StandardPistol"))
                    {
                        Weapon wpn;

                        if (model.teamWeapons.TryGetValue("StandardPistol", out wpn))
                        {
                            wpn.DPS.Upgrade();
                            model.SaveTeamWeapons(model.teamWeapons);
                            playerView.TeamPanel.UpdateTeamPanel(model.teamWeapons);
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
                    float cached = model.playerStats.DPSMultiplier;

                    model.playerStats["DPSMultiplier"] = cached + 1;
                    // model.DPS
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
            playerView.Gun.Shoot(model.DMG.Value);
        }

        private void HandleCubeDeath(object sender, CustomArgs e)
        {
            model.Gold += e.val;
        }

        public void HandleGoldChanged(float value)
        {
            // view.ui.playerGoldLbl.text = value.ToString();
        }

        public void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is WeaponStat weaponStat)
            {
                Debug.Log("PlayerController: HandlePropertyChanged: model.Changed: weaponStatObject: " + weaponStat.Level);
                playerView.ClickGunPanel.UpdateClickGunPanel(model.DPS, model.DMG);
            }

            //UnityEngine.Debug.Log("PlayerController: " + e.PropertyName);
            //view.Ui.PlayerGoldTxt.text = e.PropertyName.ToString();
            if (e.PropertyName == "Gold")
            {
                playerView.Ui.PlayerGoldTxt.text = model.Gold.ToString();
            }
            else
            {
                // 
            }
        }

        public void HandleClickGunBtnClick(object sender, GenericEventArgs<WeaponStatBtnClickArgs> e)
        {
            string weaponName = e.val.weaponName;
            string buttonName = e.val.buttonName;

            if (weaponName == "ClickGun")
            {
                switch (buttonName)
                {
                    case "DPS":
                        Debug.Log("PlayerController: HandleClickGunBtnClick: Update DPS");
                        model.DPS.Level++;
                        break;
                    case "DMG":
                        Debug.Log("PlayerController: HandleClickGunBtnClick: Update DMG");
                        model.DMG.Level++;
                        break;
                    default:
                        break;
                }
            }
        }

        public void HandleTeamWeaponBtnClick(object sender, GenericEventArgs<WeaponStatBtnClickArgs> e)
        {
            string weaponName = e.val.weaponName;
            string buttonName = e.val.buttonName;

            if (model.teamWeapons.ContainsKey(weaponName))
            {
                Weapon wpn;

                if (model.teamWeapons.TryGetValue(weaponName, out wpn))
                {
                    // TODO: (LP): 
                    // ideally, 
                    // (1) player controller updates the model, 
                    // (2) player controller reacts to the model update by updating the view
                    switch (buttonName)
                    {
                        case "DPS":
                            wpn.DPS.Level++;
                            model.SaveTeamWeapons(model.teamWeapons);
                            playerView.TeamPanel.UpdateTeamPanel(model.teamWeapons);
                            break;
                        case "DMG":
                            wpn.DMG.Level++;
                            model.SaveTeamWeapons(model.teamWeapons);
                            playerView.TeamPanel.UpdateTeamPanel(model.teamWeapons);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}