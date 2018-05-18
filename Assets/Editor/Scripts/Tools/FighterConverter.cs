using LZFight;
using FightingGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class FighterConverter {

    public static void Convert(FighterObject obj) {
        string objPath = AssetDatabase.GetAssetPath(obj);
        string directory = Path.GetDirectoryName(objPath);
        if (!AssetDatabase.IsValidFolder(directory + "/" + obj.name)) {
            AssetDatabase.CreateFolder(directory, obj.name);
        }
        string folderPath = directory + "/" + obj.name + "/";
        foreach (Move m in obj.moves) {
            string name = m.name;
            LZFighterStateData data = ScriptableObject.CreateInstance<LZFighterStateData>();
            AssetDatabase.CreateAsset(data, folderPath + name + ".asset");
            data.name = m.name;
            data.velocity = m.velocity;
            foreach(FighterState frame in m.frames) {
                LZFighterFrame f = new LZFighterFrame();

                f.frameType = (LZFighterFrame.Type) frame.frameType;
                f.velocity = frame.velocity;
                f.sprite = frame.sprite;
                f.time = frame.time;
                foreach(HitBox hb in frame.hitboxes) {
                    f.hitboxes.Add(new HitBox(hb));
                }
                data.frames.Add(f);
            }
        }
    }
}
