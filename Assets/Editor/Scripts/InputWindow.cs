using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class InputWindow : EditorWindow
{

    VirtualController controller;

    [OnOpenAssetAttribute(1)]
    public static bool OpenAsset(int instanceID, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is VirtualController)
        {
            InputWindow window = EditorWindow.GetWindow<InputWindow>();
            window.controller = (VirtualController)obj;
            return true;
        }
        return false; // we did not handle the open
    }

    [MenuItem("Assets/Create/ControlConfiguration")]
    public static void CreateMyAsset()
    {
        VirtualController asset = ScriptableObject.CreateInstance<VirtualController>();

        // AssetDatabase.CreateAsset(asset, "Assets/Fighter.asset");
        //AssetDatabase.SaveAssets();
        ProjectWindowUtil.CreateAsset(asset, "Assets/InputConfiguration.asset");

        //EditorUtility.FocusProjectWindow();

        //Selection.activeObject = asset;
    }

    [MenuItem("Window/Input Editor %#e")]
    static void Init()
    {
        InputWindow window = EditorWindow.GetWindow<InputWindow>();
    }

    void OnGUI()
    {
        controller = (VirtualController)EditorGUILayout.ObjectField("Controller:", controller, typeof(VirtualController), false);
        if(controller)
        {
            controller.P = (KeyCode)EditorGUILayout.EnumPopup("Attack", controller.P);
            controller.Taunt = (KeyCode)EditorGUILayout.EnumPopup("Taunt", controller.Taunt);
            controller.Dash = (KeyCode)EditorGUILayout.EnumPopup("Dash", controller.Dash);
            controller.Block = (KeyCode)EditorGUILayout.EnumPopup("Block", controller.Block);
            controller.Teabag = (KeyCode)EditorGUILayout.EnumPopup("Teabag", controller.Teabag);
            controller.hor = EditorGUILayout.TextField("Horizontal", controller.hor);
            controller.ver = EditorGUILayout.TextField("Vertical", controller.ver);
            controller.dpadhor = EditorGUILayout.TextField("DPad Horizontal", controller.dpadhor);
        }
    }

}
