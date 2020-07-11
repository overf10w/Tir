using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    // TODO: 
    // 1. Create an Upgrades Research Center View ((with canvas and all), which listens to when playerModel.playerStats/playerModel.teamWeapons has changed)
    // 2. Create an Upgrades Research Center Controller (which listens to the View and upgrades Upgrades.playerStats accordingly)
    // 3. Don't forget to create such playerStats: SilverCookieArtifact, GoldCookieArtifact, BronzeCookieArtifact, HolyCookieGraal, PhilosophyCookie, MeteorCookie
    // 4. Dont' forget to create such playerStats: CookieAntiMatterMult
    [CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/Ugprades", order = 6)]
    public class Upgrades : ScriptableObject
    {
        [System.Serializable]
        public class Criteria
        {
            private PlayerStats playerStats;
            public string indexer;
            public float threshold;
            public bool IsSatisfied { get => (float)playerStats[indexer] >= threshold; }
        }

        [System.Serializable]
        public class Upgrade
        {
            public string name;
            public string description;
            public float price;
            public float amount;

            public Criteria[] criterias;

            public bool isActive;
            public bool IsActive()
            {
                foreach (var criteria in criterias)
                {
                    if (criteria.IsSatisfied)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private PlayerStats playerStats;
        // private Dictionary<string, Weapon> teamWeapons; // to keep an eye on weapons
        public Upgrade[] upgrades;
    }
}