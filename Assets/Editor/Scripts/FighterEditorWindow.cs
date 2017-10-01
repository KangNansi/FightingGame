using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FighterEditorWindow : SceneView {

    private GameObject target;
    private Camera cam = null;
    private Rect sceneRect;

    [MenuItem("Window/Fighter Editor")]
    static void init()
    {
        init(null);
        //window.linemat = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Line.mat");
    }

    static void init(GameObject t)
    {
        FighterEditorWindow window = (FighterEditorWindow)EditorWindow.GetWindow(typeof(FighterEditorWindow));
        window.Show();
    }



}
