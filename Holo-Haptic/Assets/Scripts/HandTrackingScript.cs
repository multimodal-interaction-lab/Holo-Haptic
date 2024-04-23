using UnityEngine;
using Oculus;
using TMPro;


public class HandTrackingScript : MonoBehaviour
{
    public OVRHand hand;
    public Transform handRootTransform;
    public TextMeshPro hand_x;
    public TextMeshPro hand_y;
    public TextMeshPro hand_z;
    public GameObject spherePrefab; // Reference to the sphere prefab

    private GameObject sphereInstance; // Reference to the instantiated sphere

    public float y_delta = 0.00f;
    public float z_delta = -0.00f;
    void FixedUpdate()
    {
        if (hand.IsTracked)
        {
            Vector3 palmPosition = hand.transform.position;
            Quaternion palmOrientation = hand.transform.rotation;

            // Use the palm position and orientation as needed
            handRootTransform.position = palmPosition;
            handRootTransform.rotation = palmOrientation;

            

            // Extract x, y, z components from palmPosition
            float palmPosX = Mathf.Round(palmPosition.x * 1000f) / 1000f;
            float palmPosY = Mathf.Round(palmPosition.y * 1000f) / 1000f;
            float palmPosZ = Mathf.Round(palmPosition.z * 1000f) / 1000f;
            //Debug.Log(palmPosition);
            hand_x.text = palmPosX.ToString();
            hand_y.text = palmPosY.ToString();
            hand_z.text = palmPosZ.ToString();
           
            // Instantiate sphere below palm if not already instantiated
            if (sphereInstance == null)
            {
                Vector3 spherePosition = new Vector3(palmPosition.x, palmPosition.y - y_delta, palmPosition.z - z_delta); // Adjust Y position as needed
                sphereInstance = Instantiate(spherePrefab, spherePosition, Quaternion.identity);
            }
            else
            {
                // Update position of existing sphere
                sphereInstance.transform.position = new Vector3(palmPosition.x, palmPosition.y - y_delta, palmPosition.z - z_delta); // Adjust Y position as needed
            }
        }
        else
        {
            hand_x.text = "0";
            hand_y.text = "0";
            hand_z.text = "0.2";

            if (sphereInstance != null)
            {
                Destroy(sphereInstance);
                sphereInstance = null;
            }
        }
    }
}
