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

    private int waveInd;
    private int lvlInd;

    private int cubes;
    private int cubesDestroyed;

    // Use this for initialization
    IEnumerator Start()
    {
        playerData = ResourceLoader.playerData;
        waveInd = playerData._currentWave;
        yield return null;  // we need this so the InGameCanvas receives event on spawned wave (through MessageBus)
        SpawnWave();
        //ChangeSceneEnvironment();
    }

    void SpawnWave()
    {
        waveInd++;
        if (waveInd % 5 == 0)
        {
            lvlInd++;
            ChangeSceneEnvironment();
        }
        var wavePrefab = playerData.playerWaves.waves[waveInd % 5];
        wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
        cubes = wave.cubesNumber;
        cubesDestroyed = 0;
        playerData._currentWave = waveInd;
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
        //Instantiate(go);
    }

    public override void HandleMessage(Message message)
    {
        if (message.Type == MessageType.CubeDeath)
        {
            cubesDestroyed++;
            if (cubes == cubesDestroyed)
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
        // TODO: does this code matter?
        ResourceLoader.playerData._currentWave = waveInd;
    }
}