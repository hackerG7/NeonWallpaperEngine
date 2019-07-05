using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProcessController : MonoBehaviour
{

    public string ProgramPath;
    [DllImport("kernal32.dll", CharSet = CharSet.Auto)]
    
    public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);
    void Awake()
    {
        ProgramPath = Application.dataPath.Replace("/NeonWallpaperManager_Data","")+"/BackgroundProgram/NeonWallpaper.exe";
    }
    public void RestartWallpaperProgram()
    {
        KillWallpaperProgram();
        RunWallpaperProgram();
    }
    public void KillWallpaperProgram()
    {
        KillProcess("NeonWallpaper");
    }
    public void RunWallpaperProgram()
    {
        Process.Start(ProgramPath);
    }
    public void KillProcess(string name)
    {
        var processes = from p in Process.GetProcessesByName(name)
                        select p;

        /*
        var pp = Process.GetProcesses();
        foreach(var p in pp)
        {
            UnityEngine.Debug.Log(p.ProcessName);
        }*/
        foreach (var process in processes)
        {
            UnityEngine.Debug.Log(process);
            process.Kill();
        }
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
