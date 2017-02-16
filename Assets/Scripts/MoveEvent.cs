using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : MonoBehaviour {

    public void OnAccel()
    {
        Debug.Log("public void OnAccel()");
        FlickManager.Instance.Accel = true;
        FlickManager.Instance.Brake = false;
    }

    public void OnDarg(Flick flick)
    {
        float x = flick.Data.delta.x;
        float y = flick.Data.delta.y;
        FlickManager.Instance.DeltaX = x;
        FlickManager.Instance.DeltaY = y;
    }

    public void OnBrake()
    {
        Debug.Log("public void OnBrake()");
        FlickManager.Instance.Accel = false;
        FlickManager.Instance.Brake = true;
    }

}
