using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Refactor such things:
// Cosmetic:
// 1. Public/private getters/setters: private fields to start with "_"
//     1.1 Expression Body Properties where possible
//     1.2 In Properties: not to have setter by default at all
//     1.3 In Properties: if setters are needed, have private setters
//     1.4 In Properties: if setters are needed outside of class, it should be very rare
//     1.5 In Properties: setters should ONLY perform validation logic, nothing more
// 2. Regarding [SerializeField]:
//     2.1 [SerializeField] on same line with field, other attributes above
// 3. Rearrange class members/fields/properties in this order:
//     - Inner classes/INotifyPropertyChanged, encapsulated in regions
//     - Constants
//     - Static
//     - Events/public fields (though there shouldn't be such thing as public field)
//     - Serialized fields
//     - Public properties
//     - Private properties
//     - Private fields
//     - Awake/Start/Constructor
// 4. Mark fields const/readonly where appropriate
// 5. Do todos

// 16-JUN-20:
// 0. Do TODOs in WeaponModel.cs, Weapon.cs, PlayerController.cs - [done]
// 1. Refactor MessageBus (as in Trumpage) - [done]
// 2. Reorganize project (hierarchy, etc.) - [done]
// 3. Figure out if we really need GameData and GameStats as separate classes - [done]
// 4. Refactor PlayerModel.cs class - make it like in Trumpage - [done]

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
// 4. When level changes to more advanced one, the cube stats and config (textures, sounds, particles, colors, hp) should also change - [done]

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
// 1. Git commit - [done]

// 06-JUL-20:
// 0. Introduce Ability / Skill / Upgrade concept (Ability is a skill applied for a limited amount of time): - [done]
//    https://forum.unity.com/threads/ability-slots-system.618382/
//    https://www.reddit.com/r/Unity3D/comments/b5dcz1/best_way_to_create_an_ability_system/
//    https://forum.unity.com/threads/how-did-you-design-your-ability-system.355467/

// 07-JUL-20:
// 0. Introduce fortnite-style combos for gaining extra money (encourages active gameplay)

// 08-JUL-20:
// 0. After Level 100, introduce concept of a time warp: the user loses their progress to play the game anew:
//      1. The TeamWeapons levels are all set to 0
//      2. The Gun level is set to 0
//      3. The Player Level is set to 0
//      4. Some of their skills and abilities preserved: (done via the skill tree):
//                                                            - global DPS multiplier
//                                                            - ability cooldown decrease multiplier
//                                                            - bank coefficient (money earned multiplier (gives +3% to earned money))
//                                                            - increased chance of weapons not missing the targets
//                                                            - decrease cookie anti-matter (the cookie anti-matter is a coeff, like dpsMultiplier ())
//                                                            - increase cookie-UFO encounter (cookie UFO is just like container with trillion cubes)
//                                                            - increase cookie wisdom cube encounter (contains very big amount of gold)
//                                                            - convert cookie anti-matter into cookie pro-matter (for each 1% of anti-matter reduced, pro-matter increases by 1%)
//    so the user can get lvl(0-200) quicker, so the user level increase speed is higher than that if they chose not to do a time warp

// 09-JUL-20:
// 0. Add upgrades panel to each team weapon - i.e. increase chance of rage mode by 10%, increase chance of anti-cookie-matter hit by 10%
// 1. Add upgrades panel to player abilities: i.e. increase chance of diamond/legendary cookie appearing
//-1. [LP] Each team weapon has its own margin of error, (some lower, some higher), and with upgrades player can narrow that error down to 0

// 10-JUL-20:
// 0. Introduce skill concept - same as ability concept, but without duration (i.e. lasts forever, some of the skills even after time wrap) - [done]

// 11-JUL-20:
// 0. Add achievements

// 07-AUG-20:
// 0. Add tweening to ClickGun DMG - [done]
// 1. Add icon paths to skills! - [done]
// 2. Assign icons to skills via Drag&Drop in editor - [done]
// 3. Do ToDo in PlayerModel.cs - [done]

// 08-AUG-20:
// 0. Refactor - [done]

// 09-AUG-20:
// 0. Show skills' icons in the game - [done]
// 1. Add the button from space gui I've got on mah home computer - [done]

// 10-AUG-20:
// 0. Add a background sound to a game - [done]

// 11-AUG-20
// 0. Experiment with the cubes materials/sprites/particles/sounds/etc. a little bit - maybe in another project - [1/2]
// 1. Add a sound when click gun shoots - [done]
// 2. Add Cube destroy particles (gotta try a lot of particle systems) - [done]
// 3. Add cube take damage particles (maybe UniqueProjectiles particles will be enough) - [done]
// 4. Do ToDo in WeaponPanelEntry.cs - [done]
// 5. Add projectile particles when click gun shoots - [done]
// 6. Add camera shake - https://github.com/andersonaddo/EZ-Camera-Shake-Unity

// 12-AUG-20:
// 0. Change how we configure and reconfigure player waves in editor mode - [done]
//      0.1 Make the scale of each wave prefab (and thus its cubes) configurable through script - [done]
//      0.2 Add WaveSpawnPoint (will be the center of the spawned wave) - [done]
//      0.2 Find the Center of spawned wave - [done]
//      0.3 Find the offset of Center os spawned wave with the WaveSpawnPoint - [done]
//      0.4 Apply the counter-offset - [done]
// 1. Add some cube structures that resemble buildings, cars, animals, smiles, etc - [done]
// 2. Play with cube outline (i.e.)
// 2. Animate some of the waves (with unity animator)
//      2.1 When cubes are spawned, make each a child of a spawnpoint

// 15-AUG-20:
// 0. Experiment with particle systems
// 1. Do ToDo in CubeRenderer.cs (Very important) - [done]

// 17-AUG-20:
// 0. Do ToDos in CubeRenderer.cs - SUPER IMPORTANT - CAUSES SEVERE BUGS - [done]
// 1. Animate waves (through the animator component) - [(?)done - at least the hardest part - in the Wave.cs: SpawnCubes()] - [in process]

// 20-AUG-20:
// 0. Make some cubes in some waves half-transparent
// 1. Make an option to choose the color of a spawned cube (in the editor)

// 21-AUG-20:
// 0. Add some more content: waves, upgrades, artifacts

// 25-AUG-20:
// 0. Add outline shader indicator that indicates the cube is taking damage - [done]

// 26-AUG-20:
// 1. Refactor ResearchPanel/ResearchPanelEntry/UpgradesController - [doing...]

// 27-AUG-20:
// 0. Change the take damage outline shader color of cube to: - [done]
//    - Purple, when the damage is taken from enemies - [done]
//    - Blue, when the damage is taken from player - [done]

// 28-AUG-20:
// 0. Animate all of the existing waves - [done]
// 1. Add some more updates - [2 hrs]
// 3. Profile the game a little bit - [1 hrs]

// 30-AUG-20:
// 0. Do ToDo in TeamSkillPanel - [done]
//      - same in ClickGunSkillPanel - [done]
// 1. Assign appropriate icons to TeamSkills, ClickGunSkills - [done]
// 2. Make spawned waves non-recurring - [done]
// 3. Fix some bugs in CubeRenderer, when the cube HP is 0 - [done]

// 31-AUG-20:
// 0. TeamPanel:TeamSkillPanel - Add background panel image, experiment with position of the panel
// 1. ClickGunPanel:ClickGunSkillPanel - Add background panel image, experiment with position of the panel
// 2. Refactor Upgrade system a bit (make it more useful) - [done]
//      2.1 - Upgrade: Criteria: make criterias more sophisticated - [done]
//      2.2 - WeaponStat: WeaponAlgorithm: Specify Multiplier Lists, so each weaponStat will be multiplied by selected multiplier lists - [done]
//      2.3 - Upgrade, Criteria, WeaponStat, WeaponAlgorithm: StatsList selector shouldn't be a string, but rather an enum - [done]
//      2.4 - Upgrade: Target Stat name selector shouldn't be a string, but rather a dropdown (enum or string array) - [done]
//              - if (StatsList selected) {show corresponding enum of this list's items in form of enum or string arrray} - [done]
//              - helpful links:
//                  - https://stackoverflow.com/questions/60864308/how-to-make-an-enum-like-unity-inspector-drop-down-menu-from-a-string-array-with
//                  - https://forum.unity.com/threads/adding-items-to-an-enum-list-in-editor.310994/
//                  - https://answers.unity.com/questions/1170350/editorscript-generate-enum-from-string.html
//                  - https://answers.unity.com/questions/1454466/need-to-create-enum-out-of-array-of-string.html
//                  - https://answers.unity.com/questions/1085035/how-can-i-create-a-enum-like-as-component-light.html
//      2.5 - Criteria: Target Stat name selector (in Criteria, nested inside of Upgrade) shouldn't be a string, but a dropdown (enum or string array) - [done]
//              - this be done either by:
//                  - 1. UpgradeEditor.cs: accessing "criteria._stat", just like we accessed "_stat" (not preferred)
//                  - 2. Move Criteria to its own file, and implement CustomPropertyDrawer for it - https://docs.unity3d.com/ScriptReference/PropertyDrawer.html (preffered)
//                  - 3. Convert criteria to scriptable object, and do this thing as we've done in UpgradeEditor
//      2.6 - Criteria: There should be optional Upgrade reference (assigned in inspector), so the upgrade becomes unlocked only if the referenced research isn't active - [done]
//      2.7 - CriteriaPropertyDrawer, UpgradesEditor: refactor - [done]
//      2.8 - Criteria: Add weapon indexers - so upgrade becomes unlocked when a selected weapon reaches specified level - [done]
//              - Just add yet another StatsList to PlayerStats: named WeaponsLevels - [done]
//                  - PlayerModel: On PlayerModel():Init() init each gun with value from PlayerStats.WeaponsLevels - [done]
//                  - Weapon: on each weapon DPS change event, upgrade PlayerStats.WeaponLevels['index of this weapon'].Value. - [done]
//                      (All the subscribers of Weapon are still listening to its events and nothing really changes, except we store stat data in PlayerStats.WeaponList container) - [done]
//      2.9 - Upgrade: Add weapon indexers - so the Target Stat in Upgrade would be: WeaponsMultipliers["MachineGun"]
//              - PlayerStats: add another StatsList: named WeaponsMultipliers (just like we did with WeaponsLevels) - [done]
//              - PlayerModel, Weapon: On PlayerModel():Init(): Init() each Weapon and each weapon algorithm with value from PlayerStats.WeaponsMultipliers - [done]
//              - UpgradeEditor: add _weaponsMultipliers indexer - [done]
//              - PlayerController: Handle PlayerStats.WeaponsMultipliers[] change: upgrade each Weapon in _model.TeamWeapons - [done]
//      2.10 - Upgrade, Criteria, WeaponStat, PlayerStats(Especially lists names): Refactor names a bit, etc. - [done]
//      2.11 - Criteria, CriteriaDrawer: - [done]
//              - There should be an optional array of Upgrade references(assigned in inspector), so the upgrade becomes unlocked only if all the referenced upgrades aren't active - [done]

// 3. Assign appropriate icons to TeamSkills, ClickGunSkills - [done]

// 3. PlayerStat: make custom PropertyDrawer - [done]
//              - https://catlikecoding.com/unity/tutorials/editor/custom-data/
//              - https://answers.unity.com/questions/619829/how-to-set-editorguilayoutpropertyfield-label-colu.html
//              - https://forum.unity.com/threads/solved-c-custom-property-drawers-label-text-not-showing.771122/
// 4. WeaponAlgorithm: make custom PropertyDrawer - [done]
// 5. WeaponAlgorithmDrawer: Refactor, do TODOS - [done]

// 6. ResearchPanelEntry: show required criterias for an upgrade as icons (or slots) - [done]
// 7. ResearchPanelEntry: show tooltip (with name, list, target stat) when hovered over criteria icon - [done]
//      - https://www.youtube.com/watch?v=pg4-7aSf_Co
//      - https://medium.com/@yonem9/create-an-unity-inventory-part-6-generate-tooltip-c50dedcf7457
//      - https://answers.unity.com/questions/1253570/creating-a-tooltip-when-hovering-over-a-ui-button.html
//      - https://www.youtube.com/watch?v=uPmorHLPwnk
//      - https://www.youtube.com/watch?v=d_qk7egZ8_c
// 8. ResearchPanel: Improve ResearchPanel:
//      - Add vertical scroll bar - [done]
//      - Add a sound when new Research is available/unlocked
//      - (?) Each ResearchPanel element takes less (vertical) space
// 9. Optimize ResearchPanel scroll view when it's got ~20 elements - [done]
//      - Just put ResearchPanel in its own parent canvas in hierarchy (so RP isn't that nested)
//      - https://forum.unity.com/threads/scroll-rect-is-tooo-slow.396366/
//      - https://stackoverflow.com/questions/53005040/what-i-have-learned-about-unity-scrollrect-scrollview-optimization-performan
//      - https://learn.unity.com/tutorial/optimizing-unity-ui
//      - https://learn.unity.com/tutorial/working-with-static-and-dynamic-canvases
// 9. Prettify TeamSkill(Panels/Items) - [done]
// 10. Prettify ResearchPanelEntry - [done]
// 11. Optimize TeamPanel - [done]
//      - Just put ResearchPanel in its own canvas in hierarchy (so TP is less nested)
// 12. TeamSkillPanel, ClickGunSkillPanel: Add tooltipls to icons when hovered over - [done]
// 13. ResearchPanelEntry: change an icon of empty slots to something less fancy & more default - [done]
// 10. WeaponStat: remove the thing called _upgradeLevel/UpgradeLevel: we don't need it anymore(?) - [doing...]

// 19-SEP-20:
// 0. ResearchPanel: add tab switching between active upgrades and completed ones - [done]
//      - https://www.youtube.com/watch?v=211t6r12XPQ - [done]

// 21/22-SEP-20:
// 0. Do ToDos in TabGroup.cs - [done]
// 1. Change the way the levels are spawned: - [done]
//      - Before boss wave comes, there should be 4 'easy' waves - [done]
// 2. Add "While you were away" panel - [done]
//      - Add IdleEarnings to Player(Model/Stats) - [done]
//      - On game init: Calculate IdleEarnings using other Player stats - [done]
//      - On game init: PlayerView.cs: Init IdleEarnings canvas with PlayerStats.IdleEarnings - [done]

// 23-SEP-20:
// 0. Refactor; - [done]
// 1. LevelSelectButton.onClick: Restarts a level - [done]

// 24-SEP-20:
// 0. Upgrade: Add IterationMultiplier: - [done]
//      - "For every Nth(1st/2nd/3rd, etc.) %PLAYER_STAT% level, increase %SOME_OTHER_PLAYERSTAT% by X percents" - [done]
// 1. Upgrade: Add IterationMultiplier PropertyDrawer - [done]

// 25-SEP-20:
// 0. Add 5 bosses (not animated):
//      - https://www.rbxleaks.com/leak/2793
//      - https://www.google.com/search?q=8+bit+horse&sxsrf=ALeKk00mO7NqbHQw3tj0ddJtw4GpzvfXhw:1600424566771&source=lnms&tbm=isch&sa=X&ved=2ahUKEwjT2KqOvvLrAhVw-SoKHUgLD2cQ_AUoAXoECAsQAw&biw=1920&bih=1099#imgrc=omu2Iey4rARV9M
// 1. Add 10 upgrades that make sense:
//      - For every 10th level of MachineGun, StandardPistol, increase their DPS by 100% 
// 2. Tweak the game balance to make it 100 level reachable
// 3. Add 5 bosses
// 4. Add a Debug Cheat Console: https://youtu.be/VzOEM-4A2OM

// 26-SEP-20:
// (?) UpgradesSO/UpgradesEditor: show Upgrades as Reordable list:
//  - https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
//  - https://github.com/SubjectNerd-Unity/ReorderableInspector
//  - https://forum.unity.com/threads/what-is-the-best-way-to-do-a-reorderable-list.511070/

// (Important, not urgent):
// 0. Cube: refactor TakeDamage, HealthChanged thingy (a little bit)
// 0. Replace current Cube Outline implementation with something better - [12 hrs]
// 0. Add 'Save' button - [3 hrs]
// 0. Refactor the whole Cube/CubeController/CubeRenderer thing - [12 hrs]
// 0. SkillEntryIcon: tween ValueTxt.text - [2 hrs]
// 0. Add ascending system

// The concept of game is this
// 0. On each wave, the player can complete objectives: i.e: to shoot 2/3/4 blue/yellow/blinking/anime/wave blocks, etc., 
//    when it's done, the player gets the gold award, thus, there's a good stimulus for player to be active in the game
// - The drones that we have on scene can be used to go to outer space and obtain some cookies/money/artifacts etc. 
//      - Each drone fly to outer space, gets some money/cookies/etc., gets some damage and returns back to our base when we call it back
//      - Only when drone returns can we use the resources that drone found
//      - There's a chance of drone getting hit by aliens and not returning back at all
//      - Each drone can be upgraded through Research Center (like its skills and luck: probability and frequency of getting resources)
//-1. LP. Like in fortnite mining system:
//        - there should appear halo around one random cube visible to player
//        - when player kills that cube, the halo should appear on next visible cube for player,
//        - when player kills 3 cubes in such a manner, he is rewarded with gold and the whole wave HP is decreased slightly
// 0. The Waves aren't really changed with levels. What changes is just Cube.cs configuration - its HP, bonusPoints and appearence (through SO config file)
// 1. With each level completed the giant planet cookie on horizon comes a little bit closer to player, and in the end of game, it's like, very close
// 2. There's achievement: "son of a cookie", "cookie leutenant", "cookie hero", "cookieverse explosion", "cookieverse spacecookie warp", "5 clicks 5 kills"
//    (when player kills 5 in a row 1 click per cube, ofc they need to have strong stats to achieve this)"
// 3. We can only have one real visible weapon - player Orange Gun - Gun.cs - [done]
// 4. All Fire() commands of that visible weapon should be queued : but why ? - actually we don't need to queue this, remember KISS principle
// 5. All TakeDamage() methods of Cube.cs should be queued - [done]
// NOTE the command patterns don't really need an undo functionality : so why we need the command pattern at all?

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ResourceLoader _resourceLoader;
        [SerializeField] private WaveSpawner _waveSpawner;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private ResearchPanel _researchPanel;

        private AssetBundle _assetBundle;
        private UpgradesSO _upgrades;

        private string _upgradesPath;

        private void Start()
        {
            _upgradesPath = Path.Combine(Application.persistentDataPath, "upgradesSave.dat");

            string _playerStatsDataPath = Path.Combine(Application.persistentDataPath, "playerStatsData.dat");

            PlayerModel model = new PlayerModel(_playerStatsDataPath);


            _upgrades = Resources.Load<UpgradesSO>("SO/Researches/Upgrades");
            _upgrades.PlayerStats = model.PlayerStats;
            _upgrades.SetUpgrades(ResourceLoader.Load<UpgradeData[]>(_upgradesPath));

            PlayerView view = Instantiate(Resources.Load<PlayerView>("Prefabs/Player"));
            new PlayerController(model, _upgrades, view, _inputManager);
            new PlayerSoundController(view);
            new UpgradesController(model, _upgrades, _researchPanel);

            _waveSpawner.Init(model.PlayerStats);
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
