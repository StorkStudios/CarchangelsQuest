using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnerBase), true)]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		if (GUILayout.Button("Generate spawn points"))
        {
            ((SpawnerBase)target).GenerateSpawnPoints();
        }
    }
}
