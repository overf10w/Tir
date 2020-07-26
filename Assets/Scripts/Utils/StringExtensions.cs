using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class StringExtensions 
    {
        public static string SciFormat(this double num)
        {
            return string.Format((num < 1000) ? "{0}" : "{0:0.00e0}", num);
        }

        public static string SciFormat(this float num)
        {
            return string.Format((num < 1000) ? "{0}" : "{0:0.00e0}", num);
        }
    }
}