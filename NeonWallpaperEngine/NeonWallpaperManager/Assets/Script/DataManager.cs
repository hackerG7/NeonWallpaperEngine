using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Windows.Forms;
using SFB;
using UnityEngine.UI;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Image = UnityEngine.UI.Image;
using System.Drawing.Imaging;

public class GameData
{
    public List<string> BackgroundPathList = new List<string>();
    public List<string> MusicPathList = new List<string>();
    public List<string> NameList = new List<string>();
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
public class DataManager : MonoBehaviour
{
    public GameObject ColumnObject;
    public List<GameObject> ColumnObjectList;
    public float VerticalGap = 10;
    public float Offset = 50f;
    public string path;
    public float TopY;
    public RectTransform rt;
    public GameData data = new GameData();
    // Start is called before the first frame update
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        Debug.Log(rt);
        TopY = -rt.rect.height;
        if (path == "")
        {
            path = UnityEngine.Application.dataPath.Replace("/NeonWallpaperManager_Data","")+"/BackgroundProgram/NeonWallpaper_Data/GameData.json";
        }
    }
    void Start()
    {
        ClearAllColumns();
        data = LoadData();
        Debug.Log(data);
        UpdateColumns();

        //GetBackgroundImageByBrowser(0);
    }
    public void GetBackgroundImageByBrowser(int id)//always use
    {
        //GetImageDataByBrowser();var extensions = new [] {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };
        string[] r = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        data.BackgroundPathList[id] = r[0];
        UpdateColumns();
    }

    public void GetMusicByBrowser(int id)//always use
    {
        //GetMusicDataByBrowser();
        var extensions = new[] {
            new ExtensionFilter("Music Files", "mp3","wav","ogg")
        };
        string[] r = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        
        data.MusicPathList[id] = r[0];
        UpdateColumns();
    }
    public void ClearAllColumns()
    {
        foreach (Transform c in transform)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
    public void AddColumn(string bp, string mp, string n)
    {
        data.Add(bp, mp, n);
        UpdateColumns();
    }
    public byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        int t1 = Environment.TickCount;
        var o = System.Drawing.GraphicsUnit.Pixel;
        RectangleF r1 = imageIn.GetBounds(ref o);
        Rectangle r2 = new Rectangle((int)r1.X, (int)r1.Y, (int)r1.Width, (int)r1.Height);
        System.Drawing.Imaging.BitmapData omg = ((Bitmap)imageIn).LockBits(r2, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        byte[] rgbValues = new byte[r2.Width * r2.Height * 4];
        Marshal.Copy((IntPtr)omg.Scan0, rgbValues, 0, rgbValues.Length);
        ((Bitmap)imageIn).UnlockBits(omg);
        Debug.Log("i2ba time: " + (Environment.TickCount - t1));
        //flip
        byte[] nbytes = new byte[rgbValues.Length];//new byte
        var RGBwidth = r2.Width * 4;
        var RGBheight = r2.Height;
        for(var y = 0; y < RGBheight; y++)
        {
            for(var x = 0; x < RGBwidth; x++)
            {
                nbytes[x +RGBwidth* y] = rgbValues[x +RGBwidth* (RGBheight - 1 - y)];
            }
        }
        return nbytes;
    }
    public void AddEmptyColumn()
    {
        data.Add("", "", "");
        UpdateColumns();
    }
    public void UpdateColumns()
    {
        ClearAllColumns();
        for (int i = 0; i < data.GetLength(); i++)
        {
            var bp = data.BackgroundPathList[i];
            var mp = data.MusicPathList[i];
            var n = data.NameList[i];
            var o = CreateColumnObject(i, bp, mp, n);
            var ImageChild = o.transform.GetChild(0);
            var r = ImageChild.GetComponent<RectTransform>().rect;
            o.GetComponent<Column>().id = i;
            try
            {
                ColumnObjectList.Add(o);
                System.Drawing.Image myImg = System.Drawing.Image.FromFile(bp);
                var w = myImg.Size.Width;
                var h = myImg.Size.Height;
                var size = new Vector2(w,h);
                Debug.Log("format: " + myImg.PixelFormat);
                Texture2D tex = new Texture2D(w,h, TextureFormat.BGRA32, false);
                tex.LoadRawTextureData(imageToByteArray(myImg));
                tex.Apply();
                var im = ImageChild.gameObject.GetComponent<Image>();
                var rr = new Rect(new Vector2(0, 0), size);
                Debug.Log("r.size: "+r.size);
                r.size = size;
                Debug.Log(r);
                im.sprite = Sprite.Create(tex, rr, size);
            }catch(FileNotFoundException e)
            {
                Debug.Log(e);
            }
        }
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, (data.GetLength()+1) * VerticalGap/2);
    }
    public GameObject CreateColumnObject(int id, string bp, string mp, string n)
    {
        var o = Instantiate(ColumnObject, gameObject.transform);
        var c = o.GetComponent<Column>();
        RectTransform rt = o.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector3(0,VerticalGap * (1-id)+Offset,0);

        c.BackgroundPathText.text = bp;
        c.MusicPathText.text = mp;
        c.NameText.text = n;
        return o;
    }
    public void UpdateColumnObjectList()
    {
        ColumnObjectList = new List<GameObject>();
        foreach(Transform child in transform)
        {
            ColumnObjectList.Add(child.gameObject);
        }
    }
    public void UpdateData()
    {
        UpdateColumnObjectList();
        data.BackgroundPathList = new List<string>();
        data.MusicPathList = new List<string>();
        data.NameList = new List<string>();

        foreach(GameObject obj in ColumnObjectList)
        {
            data.BackgroundPathList.Add(obj.GetComponent<Column>().BackgroundPathText.text);
            data.MusicPathList.Add(obj.GetComponent<Column>().MusicPathText.text);
            data.NameList.Add(obj.GetComponent<Column>().NameText.text);
        }
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
        var D = JsonUtility.FromJson<GameData>(s);
        if(D == null)
        {
            D = new GameData();
        }
        return D;
    }
    public void SaveData()
    {
        bool e = File.Exists(path);
        UpdateData();
        Debug.Log("exists: " + e);
        if (!e)
        {
            InitializeData();
        }
        string s = JsonUtility.ToJson(data);
        Debug.Log("save: "+s);
        File.WriteAllText(path,s);
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
