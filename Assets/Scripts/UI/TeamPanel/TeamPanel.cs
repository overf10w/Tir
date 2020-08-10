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
                PlayerView.WeaponBtnClickHandler(weaponClickInfo);
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
        [SerializeField] private TextMeshProUGUI _teamDpsTxt;

        public WeaponBtnClick WeaponBtnClick { get; private set; }

        private Transform _content;
        private GameObject _weaponUiEntryPrefab;
        private List<GameObject> _weaponUiEntries;

        private TeamSkillPanel _teamSkillPanel;
        private StatsList _skills;

        private float _prevDps = 0;

        public void Init(PlayerModel model)
        {
            _skills = model.PlayerStats.TeamSkillsList;
            _skills.StatChanged += SkillChangedHandler;

            _teamSkillPanel = GetComponentInChildren<TeamSkillPanel>();
            _teamSkillPanel.Init(_skills.List);

            WeaponBtnClick = new WeaponBtnClick();
            _content = transform.Find("Scroll View/Viewport/Content").GetComponent<Transform>();
            _weaponUiEntryPrefab = Resources.Load<GameObject>("Prefabs/UI/TeamPanel/WeaponPanelEntry");
            _weaponUiEntries = new List<GameObject>();

            Dictionary<string, Weapon> weapons = model.TeamWeapons;

            float teamDps = 0;
            if (weapons != null)
            {
                foreach (var weapon in weapons)
                {
                    GameObject entryGameObject = Instantiate(_weaponUiEntryPrefab, _content);
                    _weaponUiEntries.Add(entryGameObject);

                    entryGameObject.name = weapon.Key;
                    WeaponPanelEntry script = entryGameObject.GetComponent<WeaponPanelEntry>();

                    script.Render(model, weapon.Key, weapon.Value.DPS, weapon.Value.DMG);

                    script.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DPS")); });
                    script.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DMG")); });

                    teamDps += weapon.Value.DPS.Value;
                }
            }

            _teamDpsTxt.text = "Team DPS: " + teamDps.SciFormat().ToString();
        }

        public void UpdateView(PlayerModel model)
        {
            float teamDps = 0;

            Dictionary<string, Weapon> weapons = model.TeamWeapons;
            if (weapons != null)
            {
                foreach (var weapon in weapons)
                {
                    foreach (var entry in _weaponUiEntries)
                    {
                        if (entry.name == weapon.Key)
                        {
                            var script = entry.GetComponent<WeaponPanelEntry>();
                            script.Render(model, weapon.Key, weapon.Value.DPS, weapon.Value.DMG);
                            teamDps += weapon.Value.DPS.Value;
                            break;
                        }
                    }
                }
            }


            LeanTween.value(_prevDps, teamDps, 0.45f).setOnUpdate((float val) =>
            {
                _teamDpsTxt.text = "Team DPS: " + val.SciFormat().ToString();
            });
            //_teamDpsTxt.text = "Team DPS: " + teamDps.SciFormat().ToString();
            _prevDps = teamDps;
        }

        private void SkillChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PlayerStat stat = (PlayerStat)sender;
            _teamSkillPanel.Render(stat);
        }
    }
}