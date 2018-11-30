using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkills.Asset", menuName = "Character/PlayerSkills")]
public class PlayerSkills : ScriptableObject
{
    [SerializeField]
    private Skill[] _damageLvls;
    [SerializeField]
    private Skill[] _autoFireLvls;

    public Skill[] DamageLvls { get { return _damageLvls; } set { } }
    public Skill[] AutoFireLvls { get { return _autoFireLvls; } set { } }
}