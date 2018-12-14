using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectUtility {
    [MenuItem("Assets/Create/Scriptable Object/CardOld")]
    public static void CreateCard()
    {
        CreateScriptableObject<Card>();
    }

    public static void CreateScriptableObject<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if(string.IsNullOrEmpty(path))
        {
            path = "assets";
        } else if (!string.IsNullOrEmpty(Path.GetExtension(path)))
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T) + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
