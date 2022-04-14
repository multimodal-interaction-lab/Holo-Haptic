using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelCameraControls : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    GameObject workSpace;
    [SerializeField]
    KeyCode dragKey;
    [SerializeField]
    float rotationSpeed;


    bool isDragging;
    Vector3 rotXAxis;
    Vector3 rotYAxis;


    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseDrag();
        
    }


    void HandleMouseDrag()
    {
        if (Input.GetKeyUp(dragKey))
        {
            isDragging = false;
            return;
        }


        if (!isDragging)
        {
            if(mouseInWorkArea() && Input.GetKeyDown(dragKey))
            {
                isDragging = true;
                rotXAxis = Vector3.up;
                rotYAxis = Vector3.right;
            } else
            {
                return;
            }
            
        }
        if (Input.GetKey(dragKey))
        {
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            Debug.Log(workSpace.transform.up);
            workSpace.transform.Rotate(rotXAxis, XaxisRotation);
            workSpace.transform.Rotate(rotYAxis, YaxisRotation, Space.World);
        }

    }

    bool mouseInWorkArea()
    {
        var viewPos = cam.ScreenToViewportPoint(Input.mousePosition);
        if(viewPos.x <= 1f && viewPos.x >= 0f && viewPos.y <= 1f && viewPos.y >= 0f)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
