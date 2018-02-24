using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameConfiguration : MonoBehaviour {
    
    public static GameConfiguration instance = null;

    public Configuration config;
    public VirtualController p1controller;
    public VirtualController p2controller;
    public Material p1material;
    public Material p2material;
    public bool p2isAI = true;
    public bool p1isAI = false;
    public float AIReflex = 0.1f;

	// Use this for initialization
	void Awake () {
        p1controller = Instantiate<VirtualController>(p1controller);
        p2controller = Instantiate<VirtualController>(p2controller);
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
        Load();
        Save();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Save()
    {
        config.p1controls.Setup(p1controller);
        config.p2controls.Setup(p2controller);
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
    public VController p1controls = null;
    public VController p2controls = null;
    public int matchTime = 99;
}
