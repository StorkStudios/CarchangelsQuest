using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanSpawnPoints", menuName = "Scriptable Objects/HumanSpawnPoints")]
public class HumanSpawnPoints : ScriptableObject
{
    public List<Vector2> SpawnPoints;
}
