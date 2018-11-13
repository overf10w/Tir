using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkills.Asset", menuName = "Character/PlayerSkills")]
public class PlayerSkills : ScriptableObject
{
    public Skill[] damageLvls;
    public Skill[] autoFireLvls;
}