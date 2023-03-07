using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGuy : MonoBehaviour
{
    public Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(parent == null)
        {
            parent = collision.transform;
            transform.SetParent(collision.transform.Find("CardImage"));
            Debug.Log("Collision");
        }
        
    }
}
