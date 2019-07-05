using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;
using Screen = UnityEngine.Screen;

public class WallpaperEngine : MonoBehaviour
{
    public static int ScreenHeight;
    public static int ScreenWidth;
    public static RectTransform CanvasRect;
    public static int mouseX = 0;
    public static int mouseY = 0;
    private static bool previousMouseIsDown = false;
    public static bool mouseIsDown = false;
    public static bool mouseIsClicked = false;
    public static bool db = false; //debug;
    public static void ListenForMouseEvents()
    {
        var GE = Hook.GlobalEvents();
        Debug.Log("Listening to mouse clicks.");
        GE.MouseDown += mouseDown;
        GE.MouseDoubleClick += mouseDoubleClick;
        GE.MouseMove += mouseMove;
        GE.MouseDragStarted += mouseDragStarted;
        GE.MouseDragFinished += mouseDragFinished;
        GE.MouseUp += mouseUp;
        
    }
    public static void StopListenForMouseEvents()
    {
        var GE = Hook.GlobalEvents();
        Debug.Log("Listening to mouse clicks.");
        GE.MouseDown -= mouseDown;
        GE.MouseDoubleClick -= mouseDoubleClick;
        GE.MouseMove -= mouseMove;
        GE.MouseDragStarted -= mouseDragStarted;
        GE.MouseDragFinished -= mouseDragFinished;
        GE.MouseUp -= mouseUp;
    }
    public static void mouseDragStarted(object sender, MouseEventArgs e)
    {
        mouseIsDown = true;
    }
    public static void mouseDragFinished(object sender, MouseEventArgs e)
    {
        mouseIsDown = false;
    }
    public static void mouseUp(object sender, MouseEventArgs e)
    {
        mouseIsDown = false;
    }
    public static void mouseDown(object sender, MouseEventArgs e)
    {
        if(db) Debug.Log($"Mouse {e.Button} Down");
        mouseIsDown = true;


    }
    public static void mouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (db) Debug.Log($"Mouse {e.Button} Down");


    }
    public static void mouseMove(object sender, MouseEventArgs e)
    {
        mouseX = MouseXinCanvas(CanvasRect, e.X);
        mouseY = MouseYinCanvas(CanvasRect, ScreenHeight-e.Y-25);
    }
    public static int MouseXinCanvas(RectTransform canvas, int mX)
    {
        float proportion = canvas.rect.width / ScreenWidth;
        Debug.Log("x " + Mathf.RoundToInt(mX * proportion));
        return Mathf.RoundToInt(mX * proportion);
    }
    public static int MouseYinCanvas(RectTransform canvas, int mY)
    {
        float proportion = canvas.rect.height / ScreenHeight;
        Debug.Log("y "+ mY);
        return Mathf.RoundToInt(mY * proportion);
    }
    public GameObject c;
    void Awake()
    {
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        CanvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        WallpaperProgram.Main(new string[1]);
    }
    void Start()
    {
        
        ListenForMouseEvents();
    }
    void Update()
    {
        Debug.Log(ScreenWidth+"  "+ScreenHeight);
        //Debug.Log($"width: {CanvasRect.rect.width}   height:{CanvasRect.rect.height}");
        //Debug.Log($"screen width: {Screen.width}   screen height:{Screen.height}");
        /*
        var v = new Vector3(mouseX, Screen.height-mouseY, 0);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(v);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform != null)
            {

                c.transform.position = hit.point;
            }
        }*/
        if (!previousMouseIsDown)
        {
            if (mouseIsDown)
            {
                mouseIsClicked = true;
            }
            else
            {
                mouseIsClicked = false;
            }
        }
        else
        {
            mouseIsClicked = false;
        }
        previousMouseIsDown = mouseIsDown;
        var v = new Vector3(mouseX, mouseY);
        c.transform.position = v;
    }
    void OnDisable()
    {
        Debug.Log("Uninstall hook");
        StopListenForMouseEvents();
    }
}