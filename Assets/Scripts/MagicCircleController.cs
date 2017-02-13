using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleController : MonoBehaviour {

    [SerializeField]
    private GameObject Target;

    void Start () {
        		
	}

	void Update () {
        transform.LookAt(Target.transform);
        float Angle = Time.deltaTime * 20;
        transform.Rotate(0.0f,0.0f, Angle);
    }
}