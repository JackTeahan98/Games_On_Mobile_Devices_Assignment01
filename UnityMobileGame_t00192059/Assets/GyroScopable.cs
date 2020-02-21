using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroScopable : MonoBehaviour
{
    Gyroscope m_Gyro;
    Quaternion firstOrientation;

    Quaternion initialRotation;
    Quaternion gyroInitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        m_Gyro = Input.gyro;
        m_Gyro.enabled = true;
        initialRotation = transform.rotation;
      
    }

    // Update is called once per frame
    void Update()
    {
    transform.rotation = Input.gyro.attitude; 
    }

  
}