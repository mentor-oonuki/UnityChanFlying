using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleController : MonoBehaviour {

    private float Angle;


    void Start () {
        Angle = 0.0f;		
	}

	void Update () {
        Angle += Time.deltaTime * 20;
        transform.rotation = Quaternion.Euler(0.0f,0.0f, Angle);
    }
}