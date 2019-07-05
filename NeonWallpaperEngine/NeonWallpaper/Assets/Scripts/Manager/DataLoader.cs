using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class GameData
{
    public List<string> BackgroundPathList = new List<string>();
    public List<string> MusicPathList = new List<string>();
    public List<string> NameList = new List<string>();
    public List<int> ParticleList = new List<int>();
    public GameData()
    {

    }
    public GameData(List<string> bpl, List<string> mpl, List<string> nl)
    {
        BackgroundPathList = bpl;
        MusicPathList = mpl;
        NameList = nl;
    }
    public void Add(string bp, string mp, string n)
    {
        BackgroundPathList.Add(bp);
        MusicPathList.Add(mp);
        NameList.Add(n);
    }
    public int GetLength()
    {
        return NameList.Count;
    }
}
public class DataLoader : MonoBehaviour
{
    public string path;
    public GameData data;
    public Canvas canvas;
    public BackgroundManager BackgroundManager;
    public MusicManager MusicManager;
    // Start is called before the first frame update
    void Awake()
    {
        if (path == "")
        {
            path = Application.dataPath + "/GameData.json";
        }
        data = LoadData();
    }
    void Start()
    {
        //load background
        var MPL = data.MusicPathList;
        var BPL = data.BackgroundPathList;
        for (var i = 0; i < MPL.Count; i++)
        {
            var m = MPL[i];
            var b = BPL[i];
            BackgroundManager.LoadBackground(b, i);
            MusicManager.LoadMusic(m, i);

        }
        MusicManager.PlayRandom();
    }

    public void InitializeData()
    {
        File.WriteAllText(path, "");

    }
    public GameData LoadData()
    {
        bool e = File.Exists(path);
        Debug.Log("exists: " + e);
        if (!e)
        {
            InitializeData();
        }
        string s = File.ReadAllText(path);
        Debug.Log("s: " + s);
        var D = JsonUtility.FromJson<GameData>(s);
        //canvas.transform.GetChild(1).gameObject.GetComponent<Text>().text = "data: " + s;
        return D;
    }
    public GameData GetData()
    {
        return data;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
