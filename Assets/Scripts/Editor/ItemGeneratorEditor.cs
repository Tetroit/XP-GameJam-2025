using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemGenerator), true)]
public class ItemGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemGenerator generator = (ItemGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            generator.GenerateItems(generator.SpawnTestAmount);
        }
        base.OnInspectorGUI();
    }
}
