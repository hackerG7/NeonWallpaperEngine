using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Column : MonoBehaviour
{
    public DataManager DM;
    public InputField BackgroundPathText;
    public InputField MusicPathText;
    public InputField NameText;
    public int id;
    public void GetImageByBrowser()
    {
        DM.GetBackgroundImageByBrowser(id);
    }

    public void GetMusicByBrowser()
    {
        DM.GetMusicByBrowser(id);
    }
    void Awake()
    {
        DM = transform.parent.gameObject.GetComponent<DataManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
