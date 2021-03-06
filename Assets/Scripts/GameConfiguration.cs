﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using InputManager;

public class GameConfiguration : MonoBehaviour {
    
    public static GameConfiguration instance = null;

    public Configuration config;
    public LZFightPlayer p1controller;
    public LZFightPlayer p2controller;
    public Material p1material;
    public Material p2material;
    public bool p2isAI = true;
    public bool p1isAI = false;
    public float AIReflex = 0.1f;

	// Use this for initialization
	void Awake () {
        p1controller = Instantiate(p1controller);
        p2controller = Instantiate(p2controller);
        DontDestroyOnLoad(this);
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
        Debug.Log("Config file in " + Application.persistentDataPath + "/config.dat");
        //Load();
        Save();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Save()
    {
        config.p1controls = p1controller.pairs;
        config.p2controls = p2controller.pairs;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.persistentDataPath + "/config.dat", FileMode.OpenOrCreate);
        bf.Serialize(fs, config);
        fs.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/config.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/config.dat", FileMode.Open);
            config = (Configuration)bf.Deserialize(fs);
            fs.Close();
            p1controller.Setup(config.p1controls);
            p2controller.Setup(config.p2controls);
        }
    }
}

[Serializable]
public class Configuration
{
    public List<LZFightPlayer.EventInputPair> p1controls = null;
    public List<LZFightPlayer.EventInputPair> p2controls = null;
    public int matchTime = 99;
}
