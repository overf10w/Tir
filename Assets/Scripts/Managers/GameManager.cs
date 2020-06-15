using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 0. Do TODOs in WeaponModel.cs, Weapon.cs, PlayerController.cs
// 1. Refactor MessageBus (as in Trumpage)
// 2. Reorganize project (hierarchy, etc.)
// 3. Figure out if we really need GameData and GameStats as separate classes

// The concept of game is this
// 0. 
// 1. We can only have one real visible weapon
// 2. All Fire() commands of that visible weapon should be queued
// 3. All TakeDamage() commands of Cube.cs should be queued
// NOTE the command patterns don't really need an undo functionality

public class GameManager : MessageHandler
{
    public ResourceLoader ResourceLoader;
    private GameData gameData;

    private Wave wave;

    public PlayerWaves playerWaves;

    private int lvlInd;

    private int cubesSpawned;
    private int cubesDestroyed;

    private AssetBundle myLoadedAssetBundle;

    IEnumerator Start()
    {
        gameData = new GameData();
        gameData.Init(ResourceLoader.Instance.ReadGameStats());

        PlayerView view = Instantiate(Resources.Load<PlayerView>("Prefabs/Player"));
        PlayerModel model = new PlayerModel(ResourceLoader.Instance.ReadPlayerStats());
        PlayerController pc = new PlayerController(model, view);

        lvlInd = gameData._level;
        yield return null;  // we need this so the InGameCanvas receives event on spawned wave (through MessageBus)
        SpawnWave();
        long elapsedTicks = DateTime.Now.Ticks - gameData._timeLastPlayed;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        // TODO (?): this can be set solely by PlayerController.cs
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameStarted, DoubleValue = elapsedSpan.TotalSeconds });
    }

    void SpawnWave()
    {
        var waves = playerWaves.waves;
        var wavePrefab = waves[UnityEngine.Random.Range(0, waves.Length)];
        wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
        cubesSpawned = wave.cubesNumber;
        cubesDestroyed = 0;
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WaveChanged, objectValue = wave });
    }

    void ChangeSceneEnvironment()
    {
        Texture2D texture2D;
        GameObject go1;

        if (myLoadedAssetBundle == null)
        {
            myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "levelbackgrounds"));
            return;
        }
        if (lvlInd <= 2)
        {
            texture2D = myLoadedAssetBundle.LoadAsset<Texture2D>("McLaren");
            go1 = new GameObject("BackGround ksta");
            go1.transform.position = new Vector3(0, 0, 0);
        }
        else
        {
            texture2D = myLoadedAssetBundle.LoadAsset<Texture2D>("Porsche");
            go1 = new GameObject("BackGround ksta");
            go1.transform.position = new Vector3(0, 0, -0.1f);
        }
        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        SpriteRenderer renderer = go1.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            cubesDestroyed++;
            if (cubesDestroyed == cubesSpawned)
            {
                if (lvlInd >= 3)
                {
                    MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameOver });
                    return;
                }
                SpawnWave();
            }
        }
        else if (message.Type == MessageType.LevelChanged)
        {
            ChangeLevel(message.IntValue);
            gameData._level = message.IntValue;
        }
    }

    private void ChangeLevel(int level)
    {
        lvlInd = level;
        Debug.Log("Level was changed to: " + lvlInd);
        ChangeSceneEnvironment();
    }

    public void OnDisable()
    {
        ResourceLoader.Instance.WriteGameStats(gameData.GetData());
    }
}