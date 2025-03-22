using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Totalpool), true)]
public class TotalpoolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Totalpool generator = (Totalpool)target;
        if (GUILayout.Button("Sort"))
        {
            generator.SortItems();
        }
        base.OnInspectorGUI();
    }
}
