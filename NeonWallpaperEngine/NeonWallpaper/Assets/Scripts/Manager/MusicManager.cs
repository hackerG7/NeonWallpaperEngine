using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public DataLoader DataLoader;
    public BackgroundManager BackgroundManager;
    public int SortMode;//0 next, 1 random 2 loop
    public bool Playing = false;
    public float MusicStartTime = 0.5f;
    public AudioSource[] MusicList;
    [HideInInspector]
    public string CurrentMusicName = "";
    public int CurrentMusicID = 0;
    public AudioSource CurrentMusic;
    public Text CurrentMusicNameBox;
    public bool d = true;
    private GameObject EmptyObject;
    public bool Paused = false;
    private bool SwitchingMusic;
    // Start is called before the first frame update
    void Awake()
    {
        EmptyObject = new GameObject(); ;
        UpdateMusicList();
        DisableAllMusic();
    }
    public void UpdateMusicList()
    {
        MusicList = GetComponentsInChildren<AudioSource>();
    }
    public void LoadMusic(string path, int id)
    {
        StartCoroutine(LoadSongCoroutine(path, id));

    }
    public AudioType ExtensionToType(string ext)
    {
        AudioType result = AudioType.UNKNOWN;
        switch (ext)
        {
            case ".wav":
                result = AudioType.WAV;
                break;
            case ".mp3":
                result = AudioType.MPEG;
                break;
            case ".ogg":
                result = AudioType.OGGVORBIS;
                break;
        }
        return result;

    }
    IEnumerator LoadSongCoroutine(string path, int id)
    {
        string url = string.Format("file://{0}", path);
        string extension = Path.GetExtension(path);
        AudioType type = ExtensionToType(extension);
        Debug.Log(extension);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();
            AudioClip myClip = AudioClip.Create("", (int)MusicStartTime * 1000+1, 1, 1000, false);
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                myClip = DownloadHandlerAudioClip.GetContent(www);
                //AudioSource song = new AudioSource();
                Debug.Log(myClip);
            }
            string songName = Path.GetFileName(path);
            //string songName = DL.data.NameList[id];
            Debug.Log("downloaded :" + songName);
            AddMusic(myClip, songName, id);
        }
        //UnityWebRequestMultimedia www = new UnityWebRequestMultimedia(url, AudioType.WAV);

        //WWW www = new WWW("file://" + path);
        //yield return www;
        /*
        var song = new AudioSource();
        song.clip = www.audioClip;//audioClip;//UnityWebRequestMultimedia.GetAudioClip(url,AudioType.WAV).downloadedBytes;
        //song.clip = www.GetAudioClip();
        string songName = song.clip.name;
        Debug.Log("downloaded :" + song.clip.name);
        AddMusic(song, name);*/
    }
    public void PlayRandom()
    {
        var l = MusicList.Length;
        int r = (int)Random.Range(0, l);
        PlayMusic(r);
    }
    public void AddMusic(AudioClip clip, string name, int id)
    {
        AudioSource audio = new AudioSource();
        //GameObject g = new GameObject();
        var o = Instantiate(EmptyObject, transform);
        var dn = DataLoader.data.NameList[id];
        if (dn == "")
        {
            o.name = name;
        }
        else
        {
            o.name = dn;
        }
        Music music = o.AddComponent<Music>();
        music.id = id;
        AudioSource AS = o.AddComponent<AudioSource>();
        AS.clip = clip;
        AS.enabled = false;
        UpdateMusicList();
    }
    void Start()
    {
    }
    public void StopMusic() //pause the music
    {
        if (d)
            Debug.Log("stopped music");
        Paused = true;
        CurrentMusic.Pause();
    }
    public void DisableAllMusic()
    {
        foreach (var m in MusicList)
        {
            m.enabled = false;
        }
    }
    public void NextMusic()
    {
        CurrentMusicID++;
        if (CurrentMusicID >= MusicList.Length)
        {
            CurrentMusicID = 0;
        }
        PlayMusic(CurrentMusicID);
        Debug.Log("next music: " + CurrentMusicID + " playing: " + Playing);
        SwitchingMusic = true;
        if(CurrentMusic.clip.length <= MusicStartTime+1)
        {
            NextMusic();
        }
    }

    public void PreviousMusic()
    {
        CurrentMusicID--;
        if (CurrentMusicID < 0)
        {
            CurrentMusicID = MusicList.Length - 1;
        }
        Debug.Log("previous music: " + CurrentMusicID);
        PlayMusic(CurrentMusicID);
        SwitchingMusic = true;
        if (CurrentMusic.clip.length <= MusicStartTime+1)
        {
            PreviousMusic();
        }
    }
    public void NextSortMode()
    {
        SortMode++;
        if(SortMode >= 3)
        {
            SortMode = 0;
        }
    }
    public AudioSource GetMusicByID(int id)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Music>().id == id)
            {
                return child.GetComponent<AudioSource>();
            }
            //child is your child transform
        }
        return null;
    }
    public void PlayMusic(int id) //play the music by the music index in list
    {
        if (GetMusicByID(id) != null)
        {
            Playing = true;
            CurrentMusicID = id;
            CurrentMusicName = GetMusicByID(id).gameObject.name;
            CurrentMusic = GetMusicByID(id);
            DisableAllMusic();
            GetMusicByID(id).enabled = true;
            CurrentMusic.time = MusicStartTime;
            BackgroundManager.SetBackground(id);
            Paused = false;

            if (d)
                Debug.Log("play music named: " + CurrentMusicName + "  enabled: " + GetMusicByID(id).enabled);
        }
        else
        {
            if (d) { }
                //Debug.Log("out of range: " + id);
        }
    }
    public bool MusicIsDone(AudioSource audioSource)
    {
        //Debug.Log($"time: {audioSource.time}  max: {audioSource.clip.length}");
        return !audioSource.loop
        && audioSource.time < MusicStartTime;//audioSource.time >= audioSource.clip.length;
    }
    public bool PlayMusic(string name) //play the music by the music index in list
    {
        bool found = false;
        for (int i = 0; i < MusicList.Length; i++)
        {
            var audio = GetMusicByID(i);
            Debug.Log(audio.gameObject.name);
            if (audio.gameObject.name == name)
            {
                Playing = true;
                CurrentMusicID = i;
                CurrentMusicName = name;
                DisableAllMusic();
                GetMusicByID(i).enabled = true;

                CurrentMusic = GetMusicByID(i);
                found = true;
                i = MusicList.Length;
                Paused = false;
            }
        }
        if (found)
        {
            if (d)
                Debug.Log("playing music: " + name);
            return true;
        }
        else
        {
            if (d)
                Debug.Log("cannot find the music named: " + name);
            return false;
        }
    }

    public void PlayMusic() //continue playing the paused music
    {
        if (d)
        {
            Debug.Log("continue play music");
        }
        DisableAllMusic();
        MusicList[CurrentMusicID].enabled = true;
        Playing = true;
        Paused = false;
        CurrentMusic.UnPause();
        CurrentMusic.Play();
    }
    public void TogglePlay()
    {
        Debug.Log("toggled");
        if (CurrentMusic.isPlaying)
        {
            StopMusic();
        }
        else
        {
            CurrentMusic.UnPause();
        }
    }
    // Update is called once per frame

    void Update()
    {
        if (CurrentMusic == null)
        {
            PlayRandom();
        }
        else
        {
            if (SwitchingMusic)
            {
                Debug.Log(CurrentMusic);
                if (CurrentMusic.isPlaying)
                {
                    SwitchingMusic = false;
                }
            }
            else
            {
                CurrentMusicNameBox.text = CurrentMusicName;
                if (MusicIsDone(CurrentMusic) || !Playing)
                {
                    Debug.Log($"next:  music is done: {MusicIsDone(CurrentMusic)} not playing: {!Playing} current time{CurrentMusic.time} , length{ MusicStartTime}");
                    CurrentMusic.time = 0;
                    switch (SortMode)
                    {
                        case 0://next
                            NextMusic();
                            break;
                        case 1://random
                            PlayRandom();
                            break;
                        case 2://loop
                            PlayMusic(CurrentMusicID);
                            break;

                    }
                }
            }
        }
    }
}
