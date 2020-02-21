using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Accelorometerable : MonoBehaviour
{
    public float smooth = 0.4f;

    public float newRotation;
    public float sensitivity = 6;
    private Vector3 currentAcceleration, initialAcceleration;
    private Vector3 originalPos;


    void Start()
    {
        initialAcceleration = Input.acceleration;
        currentAcceleration = Vector3.zero;

        originalPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        transform.Translate(Input.acceleration.x * (1f), 0, -Input.acceleration.z * (1f));

        int tapCount = Input.touchCount;
        if (tapCount ==3)
        {
          
            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];
            Touch touch3 = Input.touches[2];

            transform.position = originalPos;

            if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                SceneManager.LoadScene(0);
            }
        }
       


       


    }



}