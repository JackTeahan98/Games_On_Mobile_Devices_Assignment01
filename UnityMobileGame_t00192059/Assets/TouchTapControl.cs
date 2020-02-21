using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchTapControl : MonoBehaviour
{      
    float timer = 0.2f;
    bool isTapped = false;
    string touchType = "";  
    float distanceToDrag;
    Controllable temp_object;
    Text objSelectedText;
    Text moveTypeText;
    Text numOfFingersText;
    float initialDistance;
    float cameraZoomSpeed = 0.5f;
    Camera my_camera = new Camera();            
    public float speed = 0.01F; 
    float firstAngle, firstDistance;
    Vector3 firstSize;
    Quaternion firstOrientation;

    void Start()
    {
        my_camera = Camera.main;
        objSelectedText = GameObject.Find("Canvas/objSelected").GetComponent<Text>();
        moveTypeText = GameObject.Find("Canvas/moveType").GetComponent<Text>();
        numOfFingersText = GameObject.Find("Canvas/numOfFingers").GetComponent<Text>();
    }



    void Update()
    {
        if (isTapped)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                touchType = "Touch";
            }
        }

        determineTap();

        if (temp_object)
        {
            dragObject();
            pinchObject();
        }

        else if (!temp_object)
        {
            dragCamera();
            pinchCamera();
            rotateCamera();
        }


        uiText();
       
    }

    void uiText()
    {
      

        if (temp_object)
        {
            objSelectedText.text = temp_object.name;
        }

        else if (!temp_object)
        {
            objSelectedText.text = "Camera";
        }


        int tapCount = Input.touchCount;
        numOfFingersText.text = tapCount.ToString();

        if (touchType.Equals(""))
        {
            moveTypeText.text = "";
        }

        else
            moveTypeText.text = touchType;

    }

    void pinchObject()
    {
        if (Input.touchCount == 2)
        {
           
            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                Vector2 difference = touch2.position - touch1.position;
                firstAngle = Mathf.Atan2(difference.y, difference.x);
                firstOrientation = temp_object.transform.rotation;
                
                firstDistance = Vector2.Distance(touch1.position, touch2.position);
                firstSize = temp_object.transform.localScale;              
            }

            if (temp_object)
            {
                Vector2 difference = touch2.position - touch1.position;
                float new_finger_angle = Mathf.Atan2(difference.y, difference.x);
                
                temp_object.transform.rotation = firstOrientation * Quaternion.AngleAxis(Mathf.Rad2Deg * (new_finger_angle - firstAngle), my_camera.transform.forward);
                temp_object.transform.localScale = (Vector2.Distance(touch1.position, touch2.position) / firstDistance) * firstSize;
                touchType = "Pinch";
            }
        }
    }


    void dragCamera()
    {
        // https://answers.unity.com/questions/517529/pan-camera-2d-by-touch.html
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            my_camera.transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);
            touchType = "Move";
        }
    }

   


    void rotateCamera()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.touches[0];
            Touch touch2 = Input.touches[1];

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                Vector2 diff = touch2.position - touch1.position;
                firstAngle = Mathf.Atan2(diff.y, diff.x);
                firstOrientation = my_camera.transform.rotation;
            }

            if (my_camera)
            {
                Vector2 diff = touch2.position - touch1.position;
                float new_finger_angle = Mathf.Atan2(diff.y, diff.x);
                my_camera.transform.rotation = firstOrientation * Quaternion.AngleAxis(Mathf.Rad2Deg * (new_finger_angle - firstAngle), my_camera.transform.up);
                touchType = "Pinch";
            }
        }
    }



    void pinchCamera()
    {
        if (Input.touchCount == 2)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float distanceBetweenTouches;
                Vector2 touch1 = Input.GetTouch(0).position;
                Vector2 touch2 = Input.GetTouch(1).position;

                distanceBetweenTouches = Vector2.Distance(touch1, touch2);

                float pinchAmoutAsNumber = (initialDistance - distanceBetweenTouches) * cameraZoomSpeed * Time.deltaTime;
                my_camera.transform.Translate(0, 0, pinchAmoutAsNumber);

                initialDistance = distanceBetweenTouches;
                touchType = "Pinch";
            }  
        }
    }

    

    





    void determineTap()
        {
            if (Input.touchCount == 1)
            {

                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isTapped = true;
                    timer = 0.2f;
                    touchType = "Tap";
                }

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    isTapped = false;

                    touchType = "Move";
                }

                if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {

                    isTapped = false;
                    timer = 0.2f;
                    touchAndTap();

                }

            }

        }



        void dragObject()
        {
            if (Input.touchCount ==1 )
            {
                if (temp_object)
                {
                    if ((Input.GetTouch(0).phase == TouchPhase.Began))
                    {

                        distanceToDrag = Vector3.Distance(temp_object.transform.position, my_camera.transform.position);

                    }

                    if ((Input.GetTouch(0).phase == TouchPhase.Moved))
                    {

                        touchType = "Move";

                        Touch touch = Input.GetTouch(0);
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        if (temp_object)
                        {
                            temp_object.dragObject(ray.GetPoint(distanceToDrag));
                        }
                    }
                }
           

            
            

        }

        }








        void touchAndTap()
        {
            if (Input.touchCount == 1)
            {


                Touch touch = Input.GetTouch(0);
                Ray my_ray = Camera.main.ScreenPointToRay(touch.position);
                Debug.DrawRay(my_ray.origin, 20 * my_ray.direction);

                RaycastHit info_on_hit;


                if (Physics.Raycast(my_ray, out info_on_hit))
                {
                    Controllable my_obj = info_on_hit.transform.GetComponent<Controllable>();
                     


                    if (my_obj)
                    {
                        if (temp_object)
                        {
                            temp_object.deselect();
                            temp_object = null;
                           
                    }


                        if (touchType.Equals("Tap") || touchType.Equals("Move"))
                        {

                            my_obj.go_blue();
                            temp_object = my_obj;
                           


                        }

                    }

                    else
                    {
                        if (temp_object)
                            temp_object.deselect();
                        temp_object = null;




                    }

                }

                else
                {
                    if (temp_object)
                        temp_object.deselect();
                    temp_object = null;


                }
            }

        
        
           
           

        }

    }

