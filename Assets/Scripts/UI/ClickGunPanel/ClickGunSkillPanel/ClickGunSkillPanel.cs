using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ClickGunSkillPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _skillEntryPrefab;

        private Transform _content;
        private List<GameObject> _unlockedSkillEntries;

        private List<PlayerStat> _skills;

        public void Init(List<PlayerStat> skills)
        {
            _unlockedSkillEntries = new List<GameObject>();

            _skills = skills;

            _content = transform.Find("Viewport/Content").GetComponent<Transform>();
            
            foreach(var skill in _skills)
            {
                if (skill.Value > 1.0f)
                {
                    GameObject skillEntry = Instantiate(_skillEntryPrefab, _content);
                    skillEntry.name = skill.Name;
                    _unlockedSkillEntries.Add(skillEntry);
                }
            }
        }

        public void UpdateSelf(PlayerStat skill)
        {
            bool isUnlocked = false;

            foreach (var sk in _unlockedSkillEntries)
            {
                if (sk.name == skill.Name)
                {
                    isUnlocked = true;
                    break;
                }
            }

            if (!isUnlocked)
            {
                GameObject skillEntry = Instantiate(_skillEntryPrefab, _content);
                skillEntry.name = skill.Name;
                _unlockedSkillEntries.Add(skillEntry);
            }
        }
    }
}