using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLauncher : MonoBehaviour
{
    public Vector3 zOffset = new Vector3(0, 0, 0);
    public DrawManager drawManager;
    public Vector3 offset = new Vector3();
    public GameObject target;
    private Vector3 lastPos;
    public float launchSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (drawManager.plane.Raycast(ray, out distance))
            {
                var touchPos = ray.GetPoint(distance);
                

                RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
                if (hit.collider != null)
                {
                    offset = hit.transform.position - touchPos;
                    hit.transform.position = touchPos + zOffset + offset;
                    target = hit.transform.gameObject;
                    lastPos = target.transform.position;
                    target.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    target.GetComponent<Rigidbody2D>().gravityScale = 0f;
                }
            }

        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (drawManager.plane.Raycast(ray, out distance))
            {
                lastPos = target.transform.position;
                var touchPos = ray.GetPoint(distance);
                target.transform.position = touchPos + zOffset + offset;
                
                //RaycastHit2D hit = Physics2D.Raycast(touchPos, Camera.main.transform.forward);
                //if (hit.collider != null)
                //{
                    
                //    hit.transform.position = touchPos + zOffset + offset;
               // }
            }
            
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            var dist = Vector3.Distance(target.transform.position, lastPos);
            Debug.Log(target.transform.position);
            Debug.Log(lastPos);
            var velocity = Mathf.Abs(dist / Application.targetFrameRate);
            target.GetComponent<Rigidbody2D>().AddForce(Vector2.up * velocity * launchSpeed);
            target.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
            Debug.Log("dist:" + dist + "velocity:" + velocity);
        }


    }
}
