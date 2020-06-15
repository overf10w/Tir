using System;
using System.Collections;
using UnityEngine;

public class CustomArgs : EventArgs
{
    public float val;

    public CustomArgs(float val)
    {
        this.val = val;
    }
}

public class PlayerView : MessageHandler
{
    public event EventHandler<EventArgs> OnClicked = (sender, e) => {};
    public event EventHandler<CustomArgs> OnCubeDeath = (sender, e) => {};

    public UserStatsCanvas ui;

    public Weapon[] weapons;


    private void Start()
    {
        ui = FindObjectOfType<UserStatsCanvas>();
        gun = GetComponentInChildren<Gun>();
        weapons = GetComponentsInChildren<Weapon>();
        StartCoroutine(FireWeapons());
    }

    private void Update()
    {
        gun.UpdateGunRotation();
        if (Input.GetMouseButton(0))
        {
            OnClicked(this, EventArgs.Empty);
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            Cube cube = (Cube)message.objectValue;
            OnCubeDeath(this, new CustomArgs(cube.Gold));
        }
    }

    public void OnAutoShoot(float _autoShootDuration)
    {
        StartCoroutine(AutoShoot(_autoShootDuration));
    }

    public IEnumerator AutoShoot(float _autoShootDuration)
    {
        float timer = 0.0f;
        float timeBetweenShots = 0.2f;
        while (timer <= _autoShootDuration)
        {
            timer += Time.deltaTime;
            timeBetweenShots++;
            if (timeBetweenShots >= 0.2f)
            {
                // gun.Shoot(model.currentDamage);
                timeBetweenShots = 0.0f;
            }
            yield return null;
        }
    }

    //public void OnDisable()
    //{
    //    //model.OnAutoFireUpdated -= OnAutoShoot;
    //    //ResourceLoader.instance.Write(model.GetStats());
    //}

    //
    public Gun gun;



    // TODO: remove, this method should only be called from PlayerController.cs
    public IEnumerator FireWeapons()
    {
        while (true)
        {
            foreach (var weapon in weapons)
            {
                weapon.Fire();
                yield return null;
            }
            yield return null;
        }
    }
}