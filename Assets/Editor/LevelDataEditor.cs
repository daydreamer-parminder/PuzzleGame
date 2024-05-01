using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelData levelData = (LevelData)target;

        int requiredSymbolsCount = (levelData.cols * levelData.rows) / levelData.pairs;

        if (levelData.cardSymbols.Length < requiredSymbolsCount)
        {
            EditorGUILayout.HelpBox($"Number of card symbols ({levelData.cardSymbols.Length}) is less than required ({requiredSymbolsCount}).", MessageType.Error);
        }
    }
}
