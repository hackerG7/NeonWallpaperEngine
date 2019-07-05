using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class BackgroundManager : MonoBehaviour
{
    public Canvas canvas;
    public Image[] BackgroundList;
    public int CurrentBackgroundID;
    public Image CurrentBackground;
    public Sprite DefaultBackgroundSprite;
    private GameObject EmptyObject;
    public bool d = true;//debug mode

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
        for (var y = 0; y < RGBheight; y++)
        {
            for (var x = 0; x < RGBwidth; x++)
            {
                nbytes[x + RGBwidth * y] = rgbValues[x + RGBwidth * (RGBheight - 1 - y)];
            }
        }
        return nbytes;
    }
    public void LoadBackground(string path, int id)
    {
        try { 
            System.Drawing.Image myImg = System.Drawing.Image.FromFile(path);
            var w = myImg.Size.Width;
            var h = myImg.Size.Height;
            var size = new Vector2(w, h);
            Debug.Log("format: " + myImg.PixelFormat);
            Texture2D tex = new Texture2D(w, h, TextureFormat.BGRA32, false);
            tex.LoadRawTextureData(imageToByteArray(myImg));
            tex.Apply();

            var rr = new Rect(new Vector2(0, 0), size);
            Sprite sp = Sprite.Create(tex, rr, size);
            AddBackground(sp,Path.GetFileName(path), id);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log(e);
            AddBackground(DefaultBackgroundSprite, "default", id);
        }
    }
    public void AddBackground(Sprite sprite, string name, int id)
    {
        var o = Instantiate(EmptyObject, transform);
        o.name = name;
        var img = o.gameObject.AddComponent<UnityEngine.UI.Image>();
        Background bk = o.gameObject.AddComponent<Background>();
        bk.id = id;
        img.sprite = sprite;
        img.rectTransform.sizeDelta = new Vector2(sprite.rect.width,sprite.rect.height);
        img.rectTransform.localScale = new Vector3(0.025f, 0.025f, 0.025f);
        UpdateBackgroundList();
    }
    void Awake()
    {
        EmptyObject =  new GameObject(); 
        UpdateBackgroundList();
        DisableAllBackground();
        int i = 0;
        foreach(var bk in BackgroundList)
        {
            bk.name=i.ToString();
            i++;
        }

    }
    public void UpdateBackgroundList()
    {
        //BackgroundList = GetComponentsInChildren<Image>(true);
        BackgroundList = new Image[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            BackgroundList[i] = child.GetComponent<Image>();
            child.name = i.ToString();
            child.gameObject.SetActive(false);
            i++;
            //child is your child transform
        }
    }
    void DisableAllBackground()
    {
        foreach(var bk in BackgroundList)
        {
            bk.enabled = false;
        }
    }
    public Image GetBackgroundByID(int id)
    {

        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Background>().id == id)
            {
                return child.GetComponent<Image>();
            }
            //child is your child transform
        }/*
        foreach (var im in BackgroundList)
        {
            if (im.gameObject.GetComponent<Background>().id == id)
            {
                return im;
            }
        }*/
        return null;
    }
    public void SetBackground(int id)
    {
        GetComponent<Image>().enabled = true;
        if(id < BackgroundList.Length)
        {
            if (d)
                Debug.Log("set background to: " + id);
            CurrentBackgroundID = id;
            CurrentBackground = GetBackgroundByID(id);//BackgroundList[id];
            var sr = GetComponent<Image>();
            sr.sprite = CurrentBackground.sprite;
            var rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(sr.sprite.rect.width, sr.sprite.rect.height);
        }
        else
        {
            if (d)
                Debug.Log("out of range: " + id);
        }
    }
    void Start()
    {
        SetBackground(2);
    }
    void Update()
    {

    }
}
