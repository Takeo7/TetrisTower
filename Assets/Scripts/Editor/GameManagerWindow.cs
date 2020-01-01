using UnityEngine;
using UnityEditor;

public class GameManagerWindow : EditorWindow
{
    public static GameManagerWindow GMToolWindow;
    private Scriptable_GameManagementData scriptableGM;

    [MenuItem("Window/Miniclip - GameManagerWindow")]
    public static void Init()
    {
        // initialize window, show it, set the properties
        GMToolWindow = GetWindow<GameManagerWindow>(false, "GameManagerWindow", true);
        GMToolWindow.Show();
        GMToolWindow.Populate();
    }

    // initialization of troubled asset	
    void Populate()
    {
        string[] assetScriptable = AssetDatabase.FindAssets("scriptableGM");
        string path = AssetDatabase.GUIDToAssetPath(assetScriptable[0]);
        scriptableGM =(Scriptable_GameManagementData)AssetDatabase.LoadAssetAtPath(path,typeof(Scriptable_GameManagementData));
        //Object[] selection = Selection.GetFiltered(typeof(Scriptable_GameManagementData), SelectionMode.Assets);
        //if (selection.Length > 0)
        //{
        //    if (selection[0] == null)
        //        return;

        //    scriptableGM = (Scriptable_GameManagementData)selection[0];
        //}
    }

    public void OnGUI()
    {
        if (scriptableGM == null)
        {
            /* certain actions if my asset is null */
            return;
        }
        EditorGUILayout.LabelField("Game Options");
        EditorGUILayout.Space();
        scriptableGM.winHeight = EditorGUILayout.Slider("Win Height:", scriptableGM.winHeight, 2f, 40f);
        scriptableGM.health = (byte)EditorGUILayout.IntField("Health: ", scriptableGM.health);
        scriptableGM.gravity = EditorGUILayout.Slider("Gravity:", scriptableGM.gravity, 0.1f, 2f);
        scriptableGM.FallingVel = EditorGUILayout.Slider("Falling Velocity",scriptableGM.FallingVel, 0.001f, 2f);
        EditorGUILayout.Space();
        if (GUILayout.Button("Reset Values"))
        {
            scriptableGM.winHeight = 5.6f;
            scriptableGM.health = 5;
            scriptableGM.gravity = 1;
            scriptableGM.FallingVel = 0.015f;
        }
        

        // Magic of the data saving
        if (GUI.changed)
        {
            // writing changes of the testScriptable into Undo
            Undo.RecordObject(scriptableGM, "scriptableGM Editor Modify");
            // mark the testScriptable object as "dirty" and save it
            EditorUtility.SetDirty(scriptableGM);
        }
    }

    void OnSelectionChange() { Populate(); Repaint(); }
    void OnEnable() { Populate(); }
    void OnFocus() { Populate(); }
}
