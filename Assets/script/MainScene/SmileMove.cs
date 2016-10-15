using UnityEngine;
using System.Collections;

public class SmileMove : MonoBehaviour {
    private GameObject m_Smile;
    // Use this for initialization
    void Start () {
        m_Smile = GameObject.Find("smile");

    }
	
	// Update is called once per frame
	void Update () {
        //TODO: debug for mouse right button
        if (Input.GetMouseButton(1))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = m_Smile.transform.position.z;
            DebugLog.Log("mouse right button pos:" + pos.ToString());
            DropSmile(pos);
            return;
        }
    }

    private void DropSmile(Vector3 pos)
    {
        DebugLog.Log("mouse right button pos:" + pos.ToString());
        m_Smile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_Smile.transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        DebugLog.Log("Collision");
    }
}
