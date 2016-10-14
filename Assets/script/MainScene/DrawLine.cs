using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {
    public enum DrawPointState { First, Second, Finished};
    public GameObject m_PointSpritePrefab;

    private Vector3 m_FisrtPointPos;
    private Vector3 m_SecondPointPos;

    private DrawPointState m_DrawPointState;

    private ArrayList m_PointSpritePool;

    private float m_PointRadius;

    private GameObject m_LineGroup;
	// Use this for initialization
	void Start () {
        m_DrawPointState = DrawPointState.First;

        m_LineGroup = GameObject.Find("lineGroup");

        GameObject point = Instantiate(m_PointSpritePrefab, new Vector3(-100, -100, 0), Quaternion.identity) as GameObject;
        //m_PointRadius = this.GetPointRadius(point);
        m_PointRadius = 0.16f;
        m_PointSpritePool = new ArrayList();
        m_PointSpritePool.Add(point);
	}
	
	// Update is called once per frame
	void Update () {
        int inputType = 0;
        if (Input.GetMouseButtonDown(0)) inputType |= 1;
        if (1 == Input.touchCount) inputType |= 2;
        Vector3 tmpPoint = new Vector3();

        switch (inputType) {
            case 0:
                return;
            case 1:
                tmpPoint = Input.mousePosition;
                break;
            case 2:
                tmpPoint = Input.touches[0].position;
                break;
            default:
                break;
        }

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
            Vector3 lineVec3 = m_SecondPointPos - m_FisrtPointPos;
            float lineLength = lineVec3.magnitude;
            Vector3 normalizedLineVector = lineVec3.normalized * m_PointRadius;
            int pointNumber = Mathf.CeilToInt(lineLength / m_PointRadius);
            int pointsToBeAdded = pointNumber - m_PointSpritePool.Count;
            if(pointsToBeAdded <= 0)
            {
                for (int index = 0; index < pointNumber; ++index)
                {
                    GameObject point = (GameObject)m_PointSpritePool[index];
                    point.transform.position = m_FisrtPointPos + index * normalizedLineVector;
                }
            }
            else
            {
                for(int index = 0; index < m_PointSpritePool.Count; ++ index)
                {
                    GameObject point = (GameObject)m_PointSpritePool[index];
                    point.transform.position = m_FisrtPointPos + index * normalizedLineVector;
                }
                for (int i = 0; i < pointsToBeAdded; ++i)
                {
                    GameObject point = GameObject.Instantiate(m_PointSpritePrefab, m_FisrtPointPos + m_PointSpritePool.Count * normalizedLineVector, Quaternion.identity) as GameObject;
                    m_PointSpritePool.Add(point);
                }
            }
        }
    }

    private void ClearPreviousPoints()
    {

    }

    private float GetPointRadius(Object pointGameObject)
    {
        Sprite pointSprite = (Sprite)pointGameObject;
        return pointSprite.rect.width / pointSprite.pixelsPerUnit;
    }
}
