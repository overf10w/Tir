using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;

public class PlayerController
{
    public PlayerModel model;
    public PlayerView view;

    public PlayerController(PlayerModel model, PlayerView view)
    {
        this.model = model;
        this.view = view;

        // subscribe to view
        view.OnClicked += HandleClicked;
        view.OnCubeDeath += HandleCubeDeath;

        // subscribe to model
        model.PropertyChanged += HandlePropertyChanged;

        // subscribe to ui events
        // ui.PropChanged += HandleUIUpdated;
    }

    private void HandleClicked(object sender, EventArgs e)
    {
        view.gun.Shoot(model.currentDamage);
    }

    private void HandleCubeDeath(object sender, CustomArgs e)
    {
        model.Gold += e.val;
    }

    // TODO: remove this thing
    public IEnumerator FireWeapons()
    {
        while (true)
        {
            foreach (var weapon in view.weapons)
            {
                weapon.Fire();
                yield return null;
            }
            yield return null;
        }
    }

    public void HandleGoldChanged(float value)
    {
        // view.ui.playerGoldLbl.text = value.ToString();
    }

    public void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        UnityEngine.Debug.Log("PlayerController: " + e.PropertyName);
        view.ui.PlayerGoldTxt.text = e.PropertyName.ToString();
        if (e.PropertyName == "Gold")
        {
            view.ui.PlayerGoldTxt.text = model.Gold.ToString();
        }
        else
        {
            // 
        }
    }
}
