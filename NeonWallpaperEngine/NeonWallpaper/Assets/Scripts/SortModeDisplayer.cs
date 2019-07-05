using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortModeDisplayer : MonoBehaviour
{
    public Text TextBox;
    public MusicManager MusicManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextBox.text = GetSortModeString(MusicManager.SortMode);
    }
    public string GetSortModeString(int id)
    {
        string txt = "";
        switch (id)
        {
            case 0:
                txt = "all";
                break;
            case 1:
                txt = "random";
                break;
            case 2:
                txt = "repeat";
                break;
        }
        return txt;
    }
}
