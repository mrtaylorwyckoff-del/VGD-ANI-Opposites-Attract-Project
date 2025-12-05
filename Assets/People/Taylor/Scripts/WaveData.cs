using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;

public class WaveData : ScriptableObject
{
    public string waveName;
    public List<GameObject> enemyPrefabsInWave;
    public int totalEnemyCount;
}
