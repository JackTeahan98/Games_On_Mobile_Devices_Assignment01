using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    Renderer my_renderer;
    Vector3 newPosition;


    // Start is called before the first frame update
    void Start()
    {
        my_renderer = GetComponent<Renderer>();
        newPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
    }

    internal void move_up()
    {
        transform.position += Vector3.up;
 

    }

    internal void deselect()
    {
         my_renderer.material.color = Color.red;
      
    }

 

    internal void go_red()
    {
        my_renderer.material.color = Color.red;
    }

    internal void go_blue()
    {
        my_renderer.material.color = Color.blue;
    }

    

    internal void dragObject(Vector3 dragPosition)
    {
        newPosition = dragPosition;
    }


}
