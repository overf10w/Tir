using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MessageHandler
{
    public ResourceLoader ResourceLoader;
    private PlayerData playerData;
    private Wave wave;

    private int lvlInd;

    private int cubesSpawned;
    private int cubesDestroyed;

    // Use this for initialization
    IEnumerator Start()
    {
        playerData = ResourceLoader.playerData;
        lvlInd = playerData._level;   
        yield return null;  // we need this so the InGameCanvas receives event on spawned wave (through MessageBus)
        SpawnWave();
        long elapsedTicks = DateTime.Now.Ticks - playerData._timeLastPlayed;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameStarted, DoubleValue = elapsedSpan.TotalSeconds });
    }

    void SpawnWave()
    {
        var waves = playerData.playerWaves.waves;
        var wavePrefab = playerData.playerWaves.waves[UnityEngine.Random.Range(0, waves.Length)];
        wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
        cubesSpawned = wave.cubesNumber;
        cubesDestroyed = 0;
        MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WaveChanged, objectValue = wave });
    }

    private AssetBundle myLoadedAssetBundle;

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
            ResourceLoader.playerData._level = message.IntValue;
        }
    }

    private void ChangeLevel(int level)
    {
        lvlInd = level;
        Debug.Log("Level was changed to: " + lvlInd);
        ChangeSceneEnvironment();
    }
}