using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 16-JUN-20:
// 0. Do TODOs in WeaponModel.cs, Weapon.cs, PlayerController.cs - [done]
// 1. Refactor MessageBus (as in Trumpage) - [done]
// 2. Reorganize project (hierarchy, etc.) - [done]
// 3. Figure out if we really need GameData and GameStats as separate classes - [done]
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
// 0. Refactor PlayerController, Weapon, PlayerModel a bit, rename some of the model/view classes - [done]
// 1. Prettify the TeamPanel entries: for each weapon entry show: icon, name of weapon, curr/next DPS/DMG, price for DPS/DMG - [done]
// 2. Rewrite Cube.cs: TakeDamage() - so that it takes damage in a queue - [done]

// 22-JUN-20:
// 0. Cube.cs should be configured through its CubeStats script obj; this SO should have a float: TakeDamageEffectDuration - [done]
// 1. Cube.cs to have OnDamageTakenEvent(float duration); <-- duration is configured with CubeStats SO - [done]
// 2. The Cube should have View monobehaviour: On Cube.OnDamageTaken(duration) changes cube's color outline from green to red, 
// plays sound effect (Like in Trumpage sound machine ya know - такой терпкий плотный звук скрежета кирпичей смешанный с лопанием воздушного шарика)
// 3. Make the team weapons actually shoot at the cubes - [done]

// 23-JUN-20:
// 0. Refactor - [done]
// 1. Create GunDataFile (just as WeaponDataFiles), which stores serialized info on disk about this gun - so that on startup we see curr. player's gun dmg - [done]
// 2. Create PlayerDataFiles (just as WeaponDataFiles), with gold, lastTimePlayed, currLevel; so that on startup we see curr. player's gold - [done]
// 3. Reconfigure how the environment is changed according to current level: introduce environment manager which gets notified through message bus when level changed
// 4. When level changes to more advanced one, the cube stats and config (textures, sounds, particles, colors, hp) should also change

// 24-JUN-20:
// 0. Import all of the Sci-Fi fonts that are opened in Chrome
// 1. Do the TODO in PlayerController.cs - [done]
// 2. Do all previous TODOs 

// 25-JUN-20:
// 0. Refactor - [done]
// 1. Do all previous TODOs

// 26-JUN-20:
// 0. Refactor
// 1. Duplicate THIS project and import and play and experiment with downloaded assets, also import Ultimate VFX assets (just for air floating particles) - [done]
// 2. Do all the previous TODOs

// 27-JUN-20:
// 0. Import Ultimate VFX, Hit & Slashes, Unique Projectiles, sky skyboxes (just 3 skyboxes), some 3d models of spaceships etc.
// 1. Duplicate the main scene and play around with these assets (and models) just to see the possibilities the assets offer
// 2. Integrate them into project, make a backup
// 3. Import MK_Glow and play around with it, too: try setting simple 3d-meshes to glow (i.e. cube, sphere, cylinder), try setting halo, flare lens effects, consider performance
// 4. Try playing with that beautify post processing asset

// The concept of game is this
// 0. The Waves aren't really changed with levels. What changes is just Cube.cs configuration - its HP, bonusPoints and appearence (through SO config file)
// 1. We can only have one real visible weapon - player Orange Gun - Gun.cs - [done]
// 2. All Fire() commands of that visible weapon should be queued : but why ? - actually we don't need to queue this, remember KISS principle
// 3. All TakeDamage() methods of Cube.cs should be queued - [done]
// NOTE the command patterns don't really need an undo functionality : so why we need the command pattern at all?

namespace Game
{
    public class GameManager : MessageHandler
    {
        public ResourceLoader ResourceLoader;
        
        // TODO: remove, change to PlayerData

        private Wave wave;

        public PlayerWaves playerWaves;

        private int cubesSpawned;
        private int cubesDestroyed;

        private AssetBundle myLoadedAssetBundle;

        private IEnumerator Start()
        {
            InitMessageHandler();

            PlayerView view = Instantiate(Resources.Load<PlayerView>("Prefabs/Player"));
            PlayerModel model = new PlayerModel();
            PlayerController pc = new PlayerController(model, view);

            yield return null;  // we need this so the InGameCanvas receives event on spawned wave (through MessageBus)
            SpawnWave();

            //long elapsedTicks = DateTime.Now.Ticks - gameData._timeLastPlayed;
            //TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
            //// TODO (?): this can be set solely by PlayerController.cs
            //MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameStarted, DoubleValue = elapsedSpan.TotalSeconds });
        }

        private void SpawnWave()
        {
            var waves = playerWaves.waves;
            var wavePrefab = waves[UnityEngine.Random.Range(0, waves.Length)];
            wave = Instantiate(wavePrefab, wavePrefab.transform.position, Quaternion.identity) as Wave;
            cubesSpawned = wave.cubesNumber;
            cubesDestroyed = 0;
            MessageBus.Instance.SendMessage(new Message() { Type = MessageType.WaveChanged, objectValue = wave });
        }

        // TODO: this be moved to SceneEnvironmentManager
        //       (1) SceneEnvironmentManager listens to MessageBus.ChangeLevel event 
        //       (2) MessageBus.ChangeLevel event to be spawned by the PlayerController through PlayerView
        //           (2.1) PlayerController through PlayerView knows and counts how many cubes were killed, waves passed, and points earned
        //           (2.2) PlayerController updates PlayerModel.Level/PlayerModel.MaxLevel after certain threshold of aforementioned stats was reached
        //           (2.3) PlayerController listens to PlayerModel.Level update and spawns (through PlayerView) MessageBus.ChangeLevel event
        //private void ChangeSceneEnvironment()
        //{
        //    Texture2D texture2D;
        //    GameObject go1;

        //    if (myLoadedAssetBundle == null)
        //    {
        //        myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "levelbackgrounds"));
        //        return;
        //    }
        //    if (lvlInd <= 2)
        //    {
        //        texture2D = myLoadedAssetBundle.LoadAsset<Texture2D>("McLaren");
        //        go1 = new GameObject("BackGround ksta");
        //        go1.transform.position = new Vector3(0, 0, 0);
        //    }
        //    else
        //    {
        //        texture2D = myLoadedAssetBundle.LoadAsset<Texture2D>("Porsche");
        //        go1 = new GameObject("BackGround ksta");
        //        go1.transform.position = new Vector3(0, 0, -0.1f);
        //    }
        //    Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        //    SpriteRenderer renderer = go1.AddComponent<SpriteRenderer>();
        //    renderer.sprite = sprite;
        //}

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
                    //if (lvlInd >= 3)
                    //{
                    //    MessageBus.Instance.SendMessage(new Message() { Type = MessageType.GameOver });
                    //    return;
                    //}
                    SpawnWave();
                }
            }
            // TODO: this be checked by PlayerView/Controller
            //else if (message.Type == MessageType.LevelChanged)
            //{
            //    ChangeLevel(message.IntValue);
            //    gameData._level = message.IntValue;
            //}
        }

        //private void ChangeLevel(int level)
        //{
        //    lvlInd = level;
        //    Debug.Log("Level was changed to: " + lvlInd);
        //    ChangeSceneEnvironment();
        //}

        //public void OnDisable()
        //{
        //    ResourceLoader.Instance.WriteGameStats(gameData.GetData());
        //}
    }
}