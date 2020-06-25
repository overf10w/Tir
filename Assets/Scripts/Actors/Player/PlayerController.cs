using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace Game
{
    public class PlayerController
    {
        public PlayerModel model;
        public PlayerView view;

        public PlayerController(PlayerModel model, PlayerView view)
        {
            this.model = model;
            this.view = view;

            view.Init(model);

            view.OnClicked += HandleClicked;
            view.OnCubeDeath += HandleCubeDeath;

            view.OnTeamWeaponBtnClick += HandleTeamWeaponBtnClick;
            view.OnClickGunBtnClick += HandleClickGunBtnClick;

            // TODO (24-JUN): 
            // (1) view.OnClickGunBtnClick += HandleClickGunBtnClick { // update model.DPS, model.DMG, ... } - [done]
            // (2) model.OnPropertyChanged() += HandleModelPropertyChanged { // case "DMG"/"DPS" { // update view.ClickGunPanel.DMG/DPS }}

            model.PropertyChanged += HandlePropertyChanged;
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            // TODO (LP): view.Gun.Shoot(model.teamWeapons['PlayerPistol'].model.DPS);
            // For that matter, model.teamWeapons['PlayerPistol'] should be cached in PlayreController on a startup
            // And also, for that matter, playerGun.DPS shouldn't be upgradeable at all, its dps.Multiplier should be 0.
            view.Gun.Shoot(model.DMG.Value);
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
                view.ClickGunPanel.UpdateClickGunPanel(model.DPS, model.DMG);
            }

            //UnityEngine.Debug.Log("PlayerController: " + e.PropertyName);
            //view.Ui.PlayerGoldTxt.text = e.PropertyName.ToString();
            if (e.PropertyName == "Gold")
            {
                view.Ui.PlayerGoldTxt.text = model.Gold.ToString();
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
                            view.TeamPanel.UpdateTeamPanel(model.teamWeapons);
                            break;
                        case "DMG":
                            wpn.DMG.Level++;
                            model.SaveTeamWeapons(model.teamWeapons);
                            view.TeamPanel.UpdateTeamPanel(model.teamWeapons);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}