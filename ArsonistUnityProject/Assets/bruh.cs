using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bruh : MonoBehaviour
{
    Vector3[] segmentList;
    [SerializeField]
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        segmentList = new Vector3[250];
        CalculateSemiCircle(5f, 0.66f);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = segmentList.Length-1;
        lineRenderer.SetPositions(segmentList);
    }

    private void CalculateSemiCircle(float radius, float percentage) 
    {
        float range = percentage * radius * 2f;
        float incrementX = range / (float)250;

        float x = percentage * radius * -1f;

        //assuming a percentage, we want the center to be where x = 0
        for (int i = 0; i < segmentList.Length; i++) 
        {
            //y = sqrt(r^2 - x^2)
            float y = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2));

            //write positions into array.
            segmentList[i] = new Vector3(x, y, 0f);
            Debug.Log($"{i}: {segmentList[i]}");
            x += incrementX;
        }
    }
}
