using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class NoiseBoxWindow : EditorWindow {

    NoiseBox noise;
    private static Vector2 scrollPosition;

    [OnOpenAssetAttribute(1)]
    public static bool step1(int instanceID, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is NoiseBox)
        {
            NoiseBoxWindow window = EditorWindow.GetWindow<NoiseBoxWindow>();
            window.noise = (NoiseBox)obj;
            return true;
        }
        return false; // we did not handle the open
    }

    [MenuItem("Assets/Create/Noise")]
    public static void CreateMyAsset()
    {
        NoiseBox asset = ScriptableObject.CreateInstance<NoiseBox>();

        // AssetDatabase.CreateAsset(asset, "Assets/Fighter.asset");
        //AssetDatabase.SaveAssets();
        ProjectWindowUtil.CreateAsset(asset, "Assets/Noise.asset");

        //EditorUtility.FocusProjectWindow();

        //Selection.activeObject = asset;
    }

    [MenuItem("Window/Noise Creator %#n")]
    static void Init()
    {
        NoiseBoxWindow window = EditorWindow.GetWindow<NoiseBoxWindow>();
    }

    private void OnGUI()
    {
        noise = (NoiseBox)EditorGUILayout.ObjectField(noise, typeof(NoiseBox), true);
        NoiseBoxEditor(noise);
    }

    public static bool NoiseBoxEditor(NoiseBox nb)
    {
        if(nb == null)
        {
            return false;
        }
        EditorGUI.BeginChangeCheck();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (GUILayout.Button("Add"))
        {
            nb.noises.Add(new NoiseMaterial());
        }
        foreach (NoiseMaterial n in nb.noises)
        {
            NoiseEditor(n);
        }
        EditorUtility.SetDirty(nb);
        EditorGUILayout.EndScrollView();
        return EditorGUI.EndChangeCheck();
    }

    static void NoiseEditor(NoiseMaterial n, bool colored = true)
    {
        EditorGUILayout.Separator();
        n.color = EditorGUILayout.ColorField(n.color);
        n.noise.m_frequency = EditorGUILayout.Slider("Frequency:", n.noise.m_frequency, 0, 100);
        n.strength = EditorGUILayout.Slider("Strength:", n.strength, 0, 100);
        n.pureStrength = EditorGUILayout.Slider("Pure Strength:", n.pureStrength, 0, 100);
        n.exageration = EditorGUILayout.Slider("Exageration:", n.exageration, 0, 100);
        n.invert = EditorGUILayout.Toggle("Invert:", n.invert);
        n.rotation = EditorGUILayout.Slider("Rotation:", n.rotation, 0, 360);
        n.scale = EditorGUILayout.Vector2Field("Scale:", n.scale);
        n.noise.m_noiseType = (FastNoise.NoiseType)EditorGUILayout.EnumPopup("Type:", n.noise.m_noiseType);
        n.noise.m_seed = EditorGUILayout.IntField(n.noise.m_seed);
        if(n.noise.m_noiseType == FastNoise.NoiseType.Linear)
        {
            n.noise.m_linearType = (FastNoise.LinearType)EditorGUILayout.EnumPopup("Linear Type:", n.noise.m_linearType);
        }
        n.noise.m_fractalType = (FastNoise.FractalType)EditorGUILayout.EnumPopup("FType:", n.noise.m_fractalType);
        EditorGUILayout.Separator();
    }
}
