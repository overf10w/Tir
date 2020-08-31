using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class TeamSkillPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _skillEntryPrefab;

        private Transform _content;
        private Dictionary<PlayerStat, GameObject> _unlockedEntries;

        public void Init(List<PlayerStat> skills)
        {
            _content = transform.Find("Viewport/Content").GetComponent<Transform>();
            _unlockedEntries = new Dictionary<PlayerStat, GameObject>();

            foreach (var skill in skills)
            {
                RenderNewIcon(skill);
            }
        }

        public void Render(PlayerStat skill)
        {
            if (_unlockedEntries.ContainsKey(skill))
            {
                GameObject iconGO;
                if (_unlockedEntries.TryGetValue(skill, out iconGO)) 
                {
                    RenderIcon(iconGO, skill);
                    return;
                }
            }
            else
            {
                RenderNewIcon(skill);
            }
        }

        private void RenderNewIcon(PlayerStat skill)
        {
            if (skill.Value > 1.0f)
            {
                GameObject iconGO = Instantiate(_skillEntryPrefab, _content);
                iconGO.name = skill.Name;
                RenderIcon(iconGO, skill);
                _unlockedEntries.Add(skill, iconGO);
            }
        }

        private void RenderIcon(GameObject iconGO, PlayerStat skill)
        {
            SkillEntryIcon icon = iconGO.GetComponent<SkillEntryIcon>();
            if (skill.Icon != null)
            {
                icon.Icon.sprite = skill.Icon;
            }
            icon.ValueTxt.text = (skill.Value * 100.0f).ToString();
        }
    }
}