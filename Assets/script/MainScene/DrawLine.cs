using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {
    public enum DrawPointState { First, Second, Finished};
    public GameObject m_PointSpritePrefab;

    private Vector3 m_FisrtPointPos;
    private Vector3 m_SecondPointPos;

    private DrawPointState m_DrawPointState;

    private float m_PointRadius;

    private GameObject m_LineGroup;
    // Use this for initialization
    void Start () {
        m_DrawPointState = DrawPointState.First;

        m_LineGroup = GameObject.Find("lineGroup");


        m_PointRadius = m_PointSpritePrefab.GetComponent<CircleCollider2D>().radius;
	}
	
	// Update is called once per frame
	void Update () {
        int inputType = 0;

        //platform specific
#if UNITY_EDITOR        
        if (Input.GetMouseButtonDown(0)) inputType |= 1;
#elif UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) inputType |= 1;
#elif UNITY_IPHONE
        if (1 == Input.touchCount) inputType |= 1;
#elif UNITY_ANDROID
        if (1 == Input.touchCount) inputType |= 1;
#endif
        Vector3 tmpPoint = new Vector3();

        switch (inputType) {
            case 0:
                return;
            case 1:
                //platform specific
#if UNITY_EDITOR
                tmpPoint = Input.mousePosition;
                break;
#elif UNITY_STANDALONE
                tmpPoint = Input.mousePosition;
                break;
#elif UNITY_IPHONE
                tmpPoint = Input.touches[0].position;
                break;
#elif UNITY_ANDROID
                tmpPoint = Input.touches[0].position;
                break;
#endif          
            default:
                break;
        }
        tmpPoint = Camera.main.ScreenToWorldPoint(tmpPoint);
        tmpPoint.z = m_LineGroup.transform.position.z;

        switch (m_DrawPointState)
        {
            case DrawPointState.First:
                {
                    m_FisrtPointPos = tmpPoint;
                    m_DrawPointState = DrawPointState.Second;
                    break;
                }
            case DrawPointState.Second:
                {
                    m_SecondPointPos = tmpPoint;
                    m_DrawPointState = DrawPointState.Finished;
                    DrawPoints();
                    break;
                }
                
            case DrawPointState.Finished:
                {
                    this.ClearPreviousPoints();
                    m_FisrtPointPos = tmpPoint;
                    m_DrawPointState = DrawPointState.Second;
                    break;
                }
            default:
                break;


        }

    }

    private void DrawPoints()
    {
        if (DrawPointState.Finished == m_DrawPointState)
        {
#if UNITY_EDITOR
            DebugLog.Log("first point position:" + m_FisrtPointPos.ToString());
            DebugLog.Log("second point position:" + m_SecondPointPos.ToString());
                
#endif
            Vector3 lineVec3 = m_SecondPointPos - m_FisrtPointPos;
            float lineLength = lineVec3.magnitude;
            float pointOffset = m_PointRadius * 2.0f;
            Vector3 normalizedLineVector = lineVec3.normalized * pointOffset;
            int pointNumber = Mathf.CeilToInt(lineLength / pointOffset);
            int pointsToBeAdded = pointNumber - m_LineGroup.transform.childCount;

            int index = 0;
            if (pointsToBeAdded <= 0)
            {
                
                foreach(Transform childTs in m_LineGroup.transform)
                {
                    if (index > pointNumber) break;
                    childTs.position = m_FisrtPointPos + index * normalizedLineVector;
                    RestorePoint(childTs.gameObject);
                    ++index;
                }
            }
            else
            {
                foreach(Transform childTs in m_LineGroup.transform)
                {
                    childTs.position = m_FisrtPointPos + index * normalizedLineVector;
                    RestorePoint(childTs.gameObject);
                    ++index;
                }

                for (int i = 0; i < pointsToBeAdded; ++i)
                {
                    Instantiate(m_PointSpritePrefab, m_FisrtPointPos + index * normalizedLineVector, Quaternion.identity, m_LineGroup.transform);          
                    ++index;
                }
            }
        }
    }

    private void ClearPreviousPoints()
    {
        foreach(Transform childTs in m_LineGroup.transform)
        {
            childTs.position = new Vector3(-10, -10, 0);
        }
    }

    private float GetPointRadius(Object pointGameObject)
    {
        Sprite pointSprite = (Sprite)pointGameObject;
        return pointSprite.rect.width / pointSprite.pixelsPerUnit;
    }

    private void RestorePoint(GameObject point)
    {
        point.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
