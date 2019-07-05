using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransform_TopLeft : MonoBehaviour
{
    public int x;
    public int y;
    public RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.position = new Vector2(-Screen.width/2+x, -Screen.height / 2+y);
    }
}
