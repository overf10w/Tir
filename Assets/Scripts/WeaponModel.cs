using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    // TODO: remove
    public enum WeaponType
    {
        NONE,
        PISTOL,
        DOUBLE_PISTOL
    }

    [System.Serializable]
    public class WeaponModel
    {
        public int level;

        public float baseDps;
        public float baseCost;

        // TODO: DPS property 
        // usage: WeaponModel.DPS.curr
        //        WeaponModel.DPS.next

        private float _dps, _nextDps;
        private float cost, nextCost;

        public WeaponType weaponType;

        public WeaponModel(float baseCost, float baseDps, int level)
        {
            this.baseCost = baseCost;
            this.baseDps = baseDps;
            this.level = level;

            this.cost = 0;
            this._dps = 0;
        }

        public float Dps
        {
            get { return _dps; }
            set { _dps = value; }
        }

        public float Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public float NextCost
        {
            get { return nextCost; }
            set { nextCost = value; }
        }

        public float NextDps
        {
            get { return _nextDps; }
            set { _nextDps = value; }
        }

        public void Init(int lvl)
        {
            level = lvl;
            if (level == 0)
            {
                nextCost = baseCost;
                _nextDps = baseDps;

                cost = baseCost;
                _dps = 0;

                return;
            }

            cost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, level));
            _dps = baseDps * level;

            var nxtLvl = lvl + 1;

            nextCost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, nxtLvl));
            _nextDps = baseDps * nxtLvl;
        }

        public void UpdateSelf()
        {
            level++;

            cost = nextCost;
            _dps = _nextDps;

            nextCost = (int)Math.Floor(baseCost * (float)Math.Pow(1.10f, level + 1));
            _nextDps = baseDps * (level + 1);
        }

        private Cost _DPS;
        public Cost DPS 
        { 
            get 
            {
                return _DPS;
            }
            set { _DPS = value; }
        }

        public Cost DMG { get; set; }
    }
}