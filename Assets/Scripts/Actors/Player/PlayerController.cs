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
            view.OnUpdateWeaponBtnClick += HandleWeaponBtnClick;

            model.PropertyChanged += HandlePropertyChanged;
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            // TODO (LP): view.Gun.Shoot(model.teamWeapons['PlayerPistol'].model.DPS);
            // For that matter, model.teamWeapons['PlayerPistol'] should be cached in PlayreController on a startup
            // And also, for that matter, playerGun.DPS shouldn't be upgradeable at all, its dps.Multiplier should be 0.
            view.Gun.Shoot(model.currentDamage);
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
            UnityEngine.Debug.Log("PlayerController: " + e.PropertyName);
            view.Ui.PlayerGoldTxt.text = e.PropertyName.ToString();
            if (e.PropertyName == "Gold")
            {
                view.Ui.PlayerGoldTxt.text = model.Gold.ToString();
            }
            else
            {
                // 
            }
        }

        public void HandleWeaponBtnClick(object sender, GenericEventArgs<WeaponStatBtnClickArgs> e)
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