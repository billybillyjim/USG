using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;
    public GameObject trail;
    public Plane plane;
    public Vector3 startPos;
    public bool secondHalf = true;
    private Vector3 lastPos;
    public float threshold = 0.01f;
    public CircleGenerator circleGen;
    public float minPointDistance = 1;
    public float closeCircleDistance = 4;

    // Start is called before the first frame update
    void Start()
    {
        plane = new Plane(Camera.main.transform.forward * -1, this.transform.position);    

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {        
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if(plane.Raycast(ray, out distance))
            {
                startPos = ray.GetPoint(distance);
            }

            lastPos = startPos;
            circleGen.GenerateCircle(startPos);


        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                var newPos = ray.GetPoint(distance);
                if(Vector3.Distance(newPos, lastPos) > minPointDistance) {
                    circleGen.GenerateLineSegment(lastPos, newPos);
                    lastPos = newPos;
                }
                
            }
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                var newPos = ray.GetPoint(distance);
                if (Vector3.Distance(newPos, lastPos) > minPointDistance)
                {
                    circleGen.GenerateCircle(newPos);
                    lastPos = newPos;
                }
                if(Vector3.Distance(startPos, newPos) < closeCircleDistance)
                {
                    circleGen.GenerateLineSegment(newPos, startPos);
                }
                

            }
        }

    }
}
