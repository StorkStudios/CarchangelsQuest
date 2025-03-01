using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HumanSpawner))]
public class HumanSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		if (GUILayout.Button("Generate spawn points"))
        {
            ((HumanSpawner)target).GenerateSpawnPoints();
        }
    }
}
