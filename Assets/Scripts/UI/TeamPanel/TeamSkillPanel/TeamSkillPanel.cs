using System.Collections;
using System.Collections.Generic;
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
                if (skill.Value > 1.0f)
                {
                    GameObject skillEntry = Instantiate(_skillEntryPrefab, _content);
                    skillEntry.name = skill.Name;
                    var sr = skillEntry.GetComponent<Image>();
                    if (skill.Icon != null)
                    {
                        sr.sprite = skill.Icon;
                    }
                    // TODO: skillEntry.GetComponent<TMProUGUI>().text = (skill.Value * 100).ToString();
                    _unlockedEntries.Add(skill, skillEntry);
                }
            }
        }

        public void Render(PlayerStat skill)
        {
            if (_unlockedEntries.ContainsKey(skill))
            {
                GameObject obj;
                if (_unlockedEntries.TryGetValue(skill, out obj)) 
                {
                    Debug.Log("TeamSkillPanel: Updating the skill entry");
                    return;
                }
            }
            else
            {
                if (skill.Value > 1.0f)
                {
                    GameObject skillEntry = Instantiate(_skillEntryPrefab, _content);
                    skillEntry.name = skill.Name;
                    var sr = skillEntry.GetComponent<Image>();
                    if (skill.Icon != null)
                    {
                        sr.sprite = skill.Icon;
                    }
                    // TODO: skillEntry.GetComponent<TMProUGUI>().text = (skill.Value * 100).ToString();
                    _unlockedEntries.Add(skill, skillEntry);
                }
            }
        }
    }
}