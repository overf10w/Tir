using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 16-JUN-20:
// 0. Do TODOs in WeaponModel.cs, Weapon.cs, PlayerController.cs - [done]
// 1. Refactor MessageBus (as in Trumpage) - [done]
// 2. Reorganize project (hierarchy, etc.) - [done]
// 3. Figure out if we really need GameData and GameStats as separate classes
// 4. Refactor PlayerModel.cs class - make it like in Trumpage

// 17-JUN-20:
// 0. Reorganize project structure: files, folders, etc. - [done]
// 1. Prettify all of the game buttons, panels, images, make all of the fonts TMPro - [done]
// 2. Refactor PlayerModel.cs, Weapon.cs, PlayerController.cs, WeaponModel.cs, PlayerView.cs - [done]

// 18-JUN-20:
// 0. Do TODOs in Weapon.cs, PlayerModel.cs - [done]
// 1. Initialize Player Weapons on Startup - [done]
// 2. Read and save Player Weapons' WeaponData - [done]

// 19-JUN-20:
// 0. PlayerController (or PlayerView) - read WeaponStatData and initialize TeamPanel on startup - [done]
// 1. Update weapons when their (DPS, DMG) buttons are clicked! - [done]

// 20-21-JUN-20:
// 0. Refactor PlayerController, Weapon, PlayerModel a bit, rename some of the model/view classes
// 1. Prettify the TeamPanel entries: for each weapon entry show: icon, name of weapon, curr/next DPS/DMG, price for DPS/DMG - [done]
// 2. Rewrite Cube.cs: TakeDamage() - so that it takes damage in a queue - [done]

// 22-JUN-20:
// 0. Cube.cs should be configured through its CubeStats script obj; this SO should have a float: TakeDamageEffectDuration - [done]
// 1. Cube.cs to have OnDamageTakenEvent(float duration); <-- duration is configured with CubeStats SO - [done]
// 2. The Cube should have View monobehaviour: On Cube.OnDamageTaken(duration) changes cube's color outline from green to red, 
// plays sound effect (Like in Trumpage sound machine ya know - такой терпкий плотный звук скрежета кирпичей смешанный с лопанием воздушного шарика)
// 3. Make the team weapons actually shoot at the cubes - [done]

// 23-JUN-20:
// 0. Refactor 
// 1. Create PlayerDataFiles (just as WeaponDataFiles), with gold, lastTimePlayed, currLevel; so that on startup we see curr player gold
// 2. Reconfigure how the environment is changed according to current level: introduce environment manager which gets notified through message bus when level changed
// 3. When level changes to more advanced one, the cube stats and config (textures, sounds, particles, colors, hp) should also change

// The concept of game is this
// 0. The Waves aren't really changed with levels. What changes is just Cube.cs configuration - its HP, bonusPoints and appearence (through SO config file)
// 1. We can only have one real visible weapon - player orange gun - Gun.cs
// 2. All Fire() commands of that visible weapon should be queued : but why ? - actually we don't need to queue this, remember KISS principle
// 3. All TakeDamage() methods of Cube.cs should be queued
// NOTE the command patterns don't really need an undo functionality : so why we need the command pattern in first place?

namespace Game
{
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
            InitMessageHandler();

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

        public override void InitMessageHandler()
        {
            MessageSubscriber msc = new MessageSubscriber();
            msc.Handler = this;
            msc.MessageTypes = new MessageType[] { MessageType.CubeDeath, MessageType.LevelChanged };
            MessageBus.Instance.AddSubscriber(msc);
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
}