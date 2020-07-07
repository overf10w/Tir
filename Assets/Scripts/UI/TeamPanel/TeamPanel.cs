using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WeaponStatBtnClickArgs
    {
        public string weaponName;
        public string buttonName;

        public WeaponStatBtnClickArgs(string weaponName, string buttonName)
        {
            this.weaponName = weaponName;
            this.buttonName = buttonName;
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
        public WeaponBtnClick WeaponBtnClick { get; set; }

        private Transform content;
        private GameObject weaponUiEntryPrefab;
        private List<GameObject> weaponUiEntries;

        public void UpdateTeamPanel(Dictionary<string, Weapon> weapons)
        {
            foreach (var weapon in weapons)
            {
                Debug.Log("UpdateTeamPanel: weapon.Key: " + weapon.Key + ", weapon.Value.DPS.Price " + weapon.Value.DPS.Price);
            }
            if (weapons != null)
            {
                foreach(var weapon in weapons)
                {
                    foreach (var entry in weaponUiEntries)
                    {
                        if (entry.name == weapon.Key)
                        {
                            var script = entry.GetComponent<WeaponPanelEntry>();

                            script.UpdateSelf(weapon.Value.DPS, weapon.Value.DMG);

                            Debug.Log("TeamPanel.cs: hey there!");
                            break;
                        }
                    }
                }
            }
        }


        public void Init(Dictionary<string, Weapon> weapons)
        {
            WeaponBtnClick = new WeaponBtnClick();

            content = transform.Find("Scroll View/Viewport/Content").GetComponent<Transform>();

            weaponUiEntryPrefab = Resources.Load<GameObject>("Prefabs/WeaponPanelEntry");

            weaponUiEntries = new List<GameObject>();
            Debug.Log("TeamPanel.cs: weaponUiEntry == null: " + (weaponUiEntryPrefab == null).ToString());


            if (weapons != null)
            {
                foreach (var weapon in weapons)
                {
                    GameObject entryGameObject = Instantiate(weaponUiEntryPrefab, content);
                    weaponUiEntries.Add(entryGameObject);

                    entryGameObject.name = weapon.Key;
                    WeaponPanelEntry script = entryGameObject.GetComponent<WeaponPanelEntry>();

                    script.Init(weapon.Key, weapon.Value.DPS, weapon.Value.DMG);

                    script.DPSButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DPS")); });
                    script.DMGButton.onClick.AddListener(() => { WeaponBtnClick.Dispatch(new WeaponStatBtnClickArgs(weapon.Key, "DMG")); });
                }
            }
        }
    }
}