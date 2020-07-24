using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponStatBtnClickArgs
    {
        public string WeaponName { get; }
        public string ButtonName { get; }

        public WeaponStatBtnClickArgs(string weaponName, string buttonName)
        {
            WeaponName = weaponName;
            ButtonName = buttonName;
        }
    }

    public class WeaponBtnClick
    {
        public PlayerView PlayerView;

        public void Dispatch(WeaponStatBtnClickArgs weaponClickInfo)
        {
            if (PlayerView != null)
            {
                PlayerView.HandleWeaponBtnClick(weaponClickInfo);
            }
        }
    }

    public class WeaponButton
    {
        public Transform transform;

        public Weapon weapon;

        public Button DpsBtn;
        public Button DmgBtn;
        public TextMeshProUGUI DpsTxt;
        public TextMeshProUGUI DmgTxt;
    }

    public class WeaponBtns
    {
        public const string StandardPistolDpsBtn = "StandardPistol/DPSBtn";
        public const string StandardPistolDmgBtn = "StandardPistol/DMGBtn";
    }

    public class TeamPanel : MonoBehaviour
    {
        public WeaponBtnClick WeaponBtnClick { get; private set; }

        private Transform _content;
        private GameObject _weaponUiEntryPrefab;
        private List<GameObject> _weaponUiEntries;

        public void UpdateView(Dictionary<string, Weapon> weapons)
        {
            if (weapons != null)
            {
                foreach(var weapon in weapons)
                {
                    foreach (var entry in _weaponUiEntries)
                    {
                        if (entry.name == weapon.Key)
                        {
                            var script = entry.GetComponent<WeaponPanelEntry>();
                            script.UpdateSelf(weapon.Value.DPS, weapon.Value.DMG);
                            break;
                        }
                    }
                }
            }
        }

        private TeamSkillPanel _teamSkillPanel;
        private StatsContainer _skills;


        public void Init(PlayerStats playerStats, Dictionary<string, Weapon> weapons)
        {
            _skills = playerStats.TeamSkills;
            _skills.StatChanged += HandleSkillChanged;

            _teamSkillPanel = GetComponentInChildren<TeamSkillPanel>();
            _teamSkillPanel.Init(_skills.Stats);

            WeaponBtnClick = new WeaponBtnClick();
            _content = transform.Find("Scroll View/Viewport/Content").GetComponent<Transform>();
            _weaponUiEntryPrefab = Resources.Load<GameObject>("Prefabs/UI/TeamPanel/WeaponPanelEntry");
            _weaponUiEntries = new List<GameObject>();

            if (weapons != null)
            {
                foreach (var weapon in weapons)
                {
                    GameObject entryGameObject = Instantiate(_weaponUiEntryPrefab, _content);
                    _weaponUiEntries.Add(entryGameObject);

                    entryGameObject.name = weapon.Key;
                    WeaponPanelEntry script = entryGameObject.GetComponent<WeaponPanelEntry>();

                    script.Init(weapon.Key, weapon.Value.DPS, weapon.Value.DMG);

                    script.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DPS")); });
                    script.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DMG")); });
                }
            }
        }

        private void HandleSkillChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlayerStat stat = (PlayerStat)sender;

            Debug.Log("ClickGunPanel: HandleSkillChanged: Skill.Name: " + stat.Name + ", Skill.Value: " + stat.Value);
            _teamSkillPanel.UpdateSelf(stat);
        }
    }
}