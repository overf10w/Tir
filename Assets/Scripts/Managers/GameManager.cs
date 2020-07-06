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
// 0. Import Ultimate VFX, Hit & Slashes, Unique Projectiles, sky skyboxes (just 3 skyboxes), some 3d models of spaceships etc. - [done]
// 1. Duplicate the main scene and play around with these assets (and models) just to see the possibilities the assets offer - [done]
// 2. Integrate them into project, make a backup - [done]

// 28-JUN-20:
// 0. Learn how to import the 3D models into Unity - like with textures and stuff - [done]
// 1. Import MK_Glow and play around with it, too: try setting simple 3d-meshes to glow (i.e. cube, sphere, cylinder), 
//    try setting halo, flare lens effects, consider performance - [done]
// 2. Play with lights on the scene, their colors - [done]

// 29-JUN-20:
// 0. Organize scene hierarchy - 10 mins - [done]
// 1. Play with ligths a little bit more - [done]
// 2. Throw in some more stuff (3d models), play around with them on the scene - [done]

// 30-JUN-20:
// 0. Add yet more stuff and light to the scene, play around with the whole thing - [done]
// 1. Organize scene hierarchy - 10 mins - [done]
// 2. Add flying space cookies here and there in the space - [done]
// 3. Add some bright point lights (white and red ones) on the edge of our "spaceship"

// 02-JUL-20:
// 0. Start playing the cookie clicker - spend at least 2 hours on it (btw now I know why Ascensions is used -- to actually RESET those super big numbers) - [3 hrs]
//    Read this: https://steamcommunity.com/app/385770/discussions/0/492378265883294390/?ctp=2
//    Read this: https://www.reddit.com/r/CookieClicker/comments/1tq9nq/mathematical_solution_to_cookie_clicker/
//    Start playing the time clickers - [3 hrs] - [done]
// 1. Sketch out, blueprint the Abilities system - [5 hrs]

// 03-JUL-20:
// 0. Organize the scene hierarchy - 25 mins - [done]
// 1. Prettify the interface - just fonts and images of buttons/panels - 5 hrs - [done]
// 2. Make the cubes glowing and sexy - GameDoc/Cube - 6 hrs - [done]
// 3. Try to play with CookiePlanet camera particles/mist/postprocessing/force field shaders:
//    https://www.youtube.com/watch?v=lekE0Ez_go0 -- https://www.youtube.com/watch?v=NiOGWZXBg4Y -- 
//    The cookie planet thing should be done only if it looks really neat, otherwise, abandon the idea

// 04-JUL-20: Day Off

// 05-JUL-20:
// 0. Make the cubes glowing and sexy - GameDoc/Cube - 6 hrs - [done]
// 1. Git commit

// 06-JUL-20:
// 0. Introduce Ability / Skill concept (Ability is a skill applied for a limited amount of time):
//    https://forum.unity.com/threads/ability-slots-system.618382/
//    https://www.reddit.com/r/Unity3D/comments/b5dcz1/best_way_to_create_an_ability_system/
//    https://forum.unity.com/threads/how-did-you-design-your-ability-system.355467/

// 07-JUL-20:
// 0. Introduce fortnite-style combos for gaining extra money (encourages active gameplay)

// 08-JUL-20:
// 0. After Level 100, introduce concept of a time warp: the user loses their progress to play the game anew, with some of their skills and abilities preserved
//    so the user can get lvl(0-200) quicker, so the user level increase speed is higher than that if their chose not to do a time warp

// 09-JUL-20:
// 0. Add upgrades panel to each team weapon - i.e. increase chance of rage mode by 10%, increase chance of anti-cookie-matter hit by 10%
// 1. Add upgrades panel to player abilities: i.e. increase chance of diamond/legendary cookie appearing
//-1. [LP] Each team weapon has its own margin of error, (some lower, some higher), and with upgrades player can narrow that error down to 0

// 10-JUL-20:
// 0. Introduce skill concept - same as ability concept, but without duration (i.e. lasts forever, some of the skills even after time wrap)

// 11-JUL-20:
// 0. Add achievements

// The concept of game is this
// 0. On each wave, the player can complete objectives: i.e: to shoot 2/3/4 blue/yellow/blinking/anime/wave blocks, etc., 
//    when it's done, the player gets the gold award, thus, there's a good stimulus for player to be active in the game
//-1. LP. Like in fortnite mining system:
//        - there should appear halo around one random cube visible to player
//        - when player kills that cube, the halo should appear on next visible cube for player,
//        - when player kills 3 cubes in such a manner, he is rewarded with gold and the whole wave HP is decreased slightly
// 0. The Waves aren't really changed with levels. What changes is just Cube.cs configuration - its HP, bonusPoints and appearence (through SO config file)
// 1. With each level completed the giant planet cookie on horizon comes a little bit closer to player, and in the end of game, it's like, very close
// 2. There's achievement: "cookie hero", "cookieverse explosion", "cookieverse spacecookie warp", "5 clicks 5 kills 
//    (when player kills 5 in a row 1 click per cube, ofc they need to have strong stats to achieve this)"
// 3. We can only have one real visible weapon - player Orange Gun - Gun.cs - [done]
// 4. All Fire() commands of that visible weapon should be queued : but why ? - actually we don't need to queue this, remember KISS principle
// 5. All TakeDamage() methods of Cube.cs should be queued - [done]
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