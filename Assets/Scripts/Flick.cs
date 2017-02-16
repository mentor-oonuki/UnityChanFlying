using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Flick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter 
{
    [SerializeField]
    public FlickEvent PointerDown;
    [SerializeField]
    public FlickEvent Drag;
    [SerializeField]
    public FlickEvent PointerUp;

    private bool IsDragging = false;

    public PointerEventData Data;


    public void OnPointerDown(PointerEventData data)
    {
        PointerDown.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        IsDragging = true;
    }

    public void OnDrag(PointerEventData data)
    {
        Data = data;
        Drag.Invoke(this);
    }

    public void OnEndDrag(PointerEventData data)
    {
        IsDragging = false;
    }

    public void OnPointerUp(PointerEventData data)
    {
        PointerUp.Invoke(this);
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return !IsDragging;
    }
}
