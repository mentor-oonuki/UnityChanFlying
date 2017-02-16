using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalCreater : MonoBehaviour {

    [SerializeField]
    public GameObject GoalPrefab;


	// Use this for initialization
	void Start () {
        int x = Random.Range(-930, -200);
        int z = Random.Range(-660, -100);

        GameObject.Instantiate(GoalPrefab, new Vector3(x, 200.0f, z), Quaternion.identity);
	}
	
}
