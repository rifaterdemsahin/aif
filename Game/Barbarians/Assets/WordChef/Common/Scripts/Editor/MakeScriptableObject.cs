using UnityEngine;
using UnityEditor;

public class MakeScriptableObject
{
    [MenuItem("Tools/MyTool/Create My Scriptable Object")]
    static void DoIt()
    {
        GameLevel asset = ScriptableObject.CreateInstance<GameLevel>();
        AssetDatabase.CreateAsset(asset, "Assets/MyScriptableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}