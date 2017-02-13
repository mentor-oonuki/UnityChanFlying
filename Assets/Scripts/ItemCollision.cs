using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        // ゴール
        if (other.gameObject.tag == "MagicCircle")
        {
            FlyingManager.Instance.GameLoop(FlyingManager.State.GameEnd);
        }
    }
}
