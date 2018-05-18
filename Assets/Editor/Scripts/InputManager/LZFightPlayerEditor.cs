using UnityEngine;
using UnityEditor;
using InputManager;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(LZFightPlayer))]
public class LZFightPlayerEditor : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        LZFightPlayer player = target as LZFightPlayer;
        EditorGUILayout.BeginVertical();
        foreach (LZFight.LZFIGHTERINPUTEVENT key in Enum.GetValues(typeof(LZFight.LZFIGHTERINPUTEVENT))) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key.ToString(), EditorStyles.miniBoldLabel, GUILayout.MaxWidth(100));
            
            if(!player.pairs.Exists((p) => p.ev == key)) {
                LZFightPlayer.EventInputPair p = new LZFightPlayer.EventInputPair();
                p.ev = key;
                player.pairs.Add(p);
            }
            GUI.backgroundColor = Color.grey;

            EditorGUILayout.BeginVertical(GUITools.GreyBlock);
            GUI.backgroundColor = Color.white;
            LZFightPlayer.EventInputPair pair = player.pairs.Find((p) => p.ev == key);
            GUI.color = GUITools.first;
            pair.input.type = (InputObject.TYPE)EditorGUILayout.EnumPopup(pair.input.type);
            GUI.color = Color.white;
            if (pair.input.type == InputObject.TYPE.AXIS) {
                pair.input.axisName = EditorGUILayout.TextField(pair.input.axisName);
                pair.input.axisThreshold = EditorGUILayout.FloatField(pair.input.axisThreshold);
            }
            else {
                EditorGUILayout.BeginHorizontal(EditorStyles.inspectorDefaultMargins);
                KeyCode[] codes = (KeyCode[]) Enum.GetValues(typeof(KeyCode));
                List<string> keys = new List<string>();
                List<string> joystick1 = new List<string>();
                int selected = 0;
                foreach(var code in codes) {
                    
                    if(code.ToString().Length < 2) {
                        keys.Add(code.ToString());
                        if (pair.input.keyType == InputObject.KeyType.KEY && code == pair.input.code) {
                            selected = keys.Count - 1;
                        }
                    }
                    if (code.ToString().Contains("Joystick1")) {
                        joystick1.Add(code.ToString());
                        if (pair.input.keyType == InputObject.KeyType.JOYSTICK && code == pair.input.code) {
                            selected = joystick1.Count - 1;
                        }
                    }
                }
                GUI.color = GUITools.second;
                pair.input.keyType = (InputObject.KeyType)EditorGUILayout.EnumPopup(pair.input.keyType);
                GUI.color = GUITools.third;
                if(pair.input.keyType == InputObject.KeyType.KEY) {
                    int newIndex = EditorGUILayout.Popup(selected, keys.ToArray());
                    pair.input.code = (KeyCode)Enum.Parse(typeof(KeyCode), keys[newIndex]);
                }
                else if(pair.input.keyType == InputObject.KeyType.JOYSTICK) {
                    int newIndex = EditorGUILayout.Popup(selected, joystick1.ToArray());
                    pair.input.code = (KeyCode)Enum.Parse(typeof(KeyCode), joystick1[newIndex]);
                }
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            //pair.input = InputObjectEditor.InputObjectField(pair.input);
            //pair.input = (InputObject)EditorGUILayout.ObjectField(pair.input, typeof(InputObject), false);
            
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();


        EditorUtility.SetDirty(target);
    }
}