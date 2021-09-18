using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.IO;
using System;

public class SaveTest : MonoBehaviour
{
    private PlayerState stt = new PlayerState();
    [ShowInInspector]
    private List<GameObject> TestList;
    private Vector3 posvec;
    private float hlth = 0.1f;
    private string save_ext = "save.sav";
    
    //private const string default_path = Application.persistentDataPath;

    [Button]
    private void Testy()
    {
        //Debug.Log("hello pushed");
        this.transform.Translate(new Vector2(1, 2));
        posvec = this.transform.position;
        hlth++;
        Debug.Log(transform.position);
    }
    [Button]
    private void Retesty()
    {
        //Debug.Log("hello pushed");
        stt.Pos = posvec = this.transform.position = Vector3.zero;
        stt.Health = hlth =  1f;
        stt.goNames.Clear();
        
    }
    [Button]
    public void SaveState()
    {
        string filePath  = Application.persistentDataPath + save_ext;
        stt.goNames.Clear();
        foreach (GameObject i in TestList)
        {
            stt.goNames.Add(i.name);

        }
        stt.Health = hlth;
        stt.Pos = posvec;
        stt.gos = this.TestList;
        
        byte[] bytes = SerializationUtility.SerializeValue(this.stt, DataFormat.Binary);
        
        File.WriteAllBytes(filePath, bytes);
    }
    [Button]
    public void LoadState()
    {
        string filePath = Application.persistentDataPath + save_ext;

        PlayerState temp_stt = new PlayerState();
        if (!File.Exists(filePath)) return; // No state to load

        byte[] bytes = File.ReadAllBytes(filePath);
        temp_stt = SerializationUtility.DeserializeValue<PlayerState>(bytes, DataFormat.Binary);
        temp_stt.gos = new List<GameObject>();
        
        
        foreach(string str in temp_stt.goNames)
        {
            GameObject g = GameObject.Find(str);
            if (!g)
                Debug.LogError("GameObj not found:" + str);
            temp_stt.gos.Add(g);
            
        }
        stt = temp_stt;

        this.transform.position = stt.Pos;
        this.TestList.Clear();
        this.TestList = stt.gos;
        this.hlth = stt.Health;

        Debug.Log(TestList[0].name);
    }

    [System.Serializable]
    public class PlayerState
    {
        public Vector3 Pos;
        public float Health;
        public List<String> goNames = new List<string>();
        [System.NonSerialized]
        public List<GameObject> gos = new List<GameObject>();

        public PlayerState()
        {
            gos = new List<GameObject>();
        }
    }

}
