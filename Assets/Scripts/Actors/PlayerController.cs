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

            view.OnClicked += HandleClicked;
            view.OnCubeDeath += HandleCubeDeath;
            view.OnUpdateWeaponBtnClick += HandleWeaponBtnClick;

            model.PropertyChanged += HandlePropertyChanged;
        }

        // 

        private void HandleClicked(object sender, EventArgs e)
        {
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

        public void HandleWeaponBtnClick(object sender, GenericEventArgs<string> e)
        {
            string buttonName = e.val;
            switch (buttonName)
            {
                case WeaponBtns.StandardPistolDmgBtn:
                    // string weaponName = "StandardPistolDmgBtn";
                    // if (model.weapons.lookUpForDictionaryKeyValuePair(weaponName).GetValue.DMG.Cost <= model.Gold) 
                    // {
                    //     BUY AN UPGRADE
                    // }
                    Debug.Log("PlayerController: WeaponBtns.StandardPistolDmgBtn pressed");
                    break;
                case WeaponBtns.StandardPistolDpsBtn:
                    string weaponName = "StandardPistol";
                    Weapon wpn;
                    if (model.teamWeapons.TryGetValue(weaponName, out wpn))
                    {
                        //Debug.Log("PlayerController: wpn == null? : " + (wpn == null));
                        Debug.Log("PlayerController: wpn.DPS.CurrCost: " + wpn.WeaponModel.DPS.CurrCost);
                        if (wpn.WeaponModel.DPS.CurrCost <= model.Gold)
                        {
                            Debug.Log("PlayerController: Updating weapon...");
                        }
                    }
                    //Debug.Log("PlayerController: WeaponBtns.StandardPistolDpsBtn pressed");
                    break;
                default:
                    break;
            }
        }

        public void BuyAnItem()
        {
            
        }

    }
}