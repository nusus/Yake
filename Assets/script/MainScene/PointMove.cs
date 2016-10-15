using UnityEngine;
using System.Collections;

public class PointMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        DebugLog.Log("point move collision");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        DebugLog.Log("point move trigger");
    }
}
