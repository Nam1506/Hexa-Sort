using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridGenerator grid = (GridGenerator)target;

        if (GUILayout.Button("Generate"))
        {
            grid.GenerateHexGrid();
        }
    }
}
