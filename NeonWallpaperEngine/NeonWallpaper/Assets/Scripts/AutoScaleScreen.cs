using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaleScreen : MonoBehaviour
{
    RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);
    }
}
