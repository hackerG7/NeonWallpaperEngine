using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class OnWallpaperClick : MonoBehaviour
{
    public RectTransform rect;
    public EventTrigger.TriggerEvent callback;
    private BaseEventData eventData;
    // Start is called before the first frame update
    void Start()
    {
        var ES = EventSystem.current;
        //ES.sendNavigationEvents = false;
        eventData = new BaseEventData(ES);
        eventData.selectedObject = this.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        CheckClick(callback);
    }
    
    public void CheckClick(EventTrigger.TriggerEvent callback)
    {
        var x = WallpaperEngine.mouseX;
        var y = WallpaperEngine.mouseY;
        bool inrect = PositionInRect(x, y, rect.position, rect.rect.size);
        //var eventData = new 
        if (inrect && WallpaperEngine.mouseIsClicked){
            callback.Invoke(eventData);
        }
        
        //Debug.Log($"x: {x} y: {y} rx: {rect.position.x} ry: {rect.position.y} rwidth: {rect.rect.width} r:height{rect.rect.size.y} inrect: {inrect}");
    }
    public bool PositionInRect(float x, float y, Vector2 pos, Vector2 size)
    {
        bool result = false;
        var minX = pos.x - size.x / 2;//top left x
        var minY = pos.y - size.y / 2;//top left y
        var maxX = minX + size.x;
        var maxY = minY + size.y;
        if(x>minX && x < maxX && y<maxY && y > minY)
        {
            result = true;
        }
        return result;
    }
}
