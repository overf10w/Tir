using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int gold;
    public static int currentWave;

    public static void Nullify()
    {
        gold = 0;
        currentWave = 0;
    }
}
