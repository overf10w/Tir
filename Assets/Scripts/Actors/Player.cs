using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MessageHandler
{
    public GameManager gameManager;
    private Gun gunContoller;

    void Start()
    {
        gameManager.playerDb.OnAutoFireUpdated += OnAutoShoot;
        gunContoller = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        gunContoller.UpdateGunRotation();
        if (Input.GetMouseButton(0))
        {
            gunContoller.Shoot(gameManager.playerDb.Damage.value);
        }
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            Cube cube = (Cube)message.objectValue;
            gameManager.playerDb.Gold += cube.Gold;
        }
    }

    public void OnAutoShoot(float _autoShootDuration)
    {
        //this.isAutoShoot = isAuthoShoot;
        StartCoroutine(AutoShoot(_autoShootDuration));
    }

    public IEnumerator AutoShoot(float _autoShootDuration)
    {
        Debug.Log("Player: Autoshoot: START");
        float timer = 0.0f;
        float timeBetweenShots = 0.2f;
        while (timer <= _autoShootDuration)
        {
            timer += Time.deltaTime;
            timeBetweenShots++;
            if (timeBetweenShots >= 0.2f)
            {
                gunContoller.Shoot(gameManager.playerDb.Damage.value);
                timeBetweenShots = 0.0f;
            }
            yield return null;
        }
        //yield return new WaitForSeconds(3.0f);
        Debug.Log("Player: Autoshoot: START");
    }

    public void OnDisable()
    {
        gameManager.playerDb.OnAutoFireUpdated -= OnAutoShoot;
    }
}