using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTimeSpot : MonoBehaviour
{
    public MusicManager MusicManager;
    public float width;
    public float height;
    public float XOffset;
    public float CurrentValue;
    public float MaxValue;
    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicManager.CurrentMusic != null)
        {
            CurrentValue = MusicManager.CurrentMusic.time;
            MaxValue = MusicManager.CurrentMusic.clip.length;
        }
        float percent;
        if (MaxValue == 0)
        {
            percent = 0;
        }
        else
        {
            percent = CurrentValue / MaxValue;
        }
        width = Screen.width+XOffset;
        height = Screen.height;
        transform.position = new Vector3(-width/2+width*percent, transform.position.y,transform.position.z);
    }
}
