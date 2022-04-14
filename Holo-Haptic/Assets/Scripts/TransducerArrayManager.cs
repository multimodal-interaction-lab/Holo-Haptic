using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransducerArrayManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The number of rows in the transducer array.")]
    int numRows;
    [SerializeField]
    [Tooltip("The number of columns in the transducer array.")]
    int numCols;

    [SerializeField]
    [Tooltip("The prefab object representing the transducer.")]
    GameObject transducerObject;

    const string CONTAINER_NAME = "TransducerArray";
    const float TRANSDUCER_DIAMETER = .01f;
    const float TRANSDUCER_HEIGHT = .008f;

    //The currently created and tracked transducers
    GameObject[,] transducerArray;



    void Start()
    {
        GenerateTransducerArray();   
    }


    void GenerateTransducerArray()
    {
        //Destroy the current transducer array, if it exists
        DestroyTransducerArray();
        GameObject newTransducerArray = Instantiate(new GameObject(CONTAINER_NAME), transform);

        if(numRows <= 0 || numCols <= 0)
        {
            Debug.LogError("The number of rows and columns in the transducer array must be greater than 0!");
            return;
        }

        transducerArray = new GameObject[numRows,numCols];

        //Create the transducers
        for (int i = 0; i < numRows; i++)
        {
            for(int j = 0; j < numCols; j++)
            {
                GameObject newTransducer = Instantiate(transducerObject, transform);
                //Want the array to start at top left and end at bottom right
                float xOffset = (j * TRANSDUCER_DIAMETER) - (((float)numCols - 1) / 2);
                float zOffset = (((float)numRows - 1) / 2) - (i * TRANSDUCER_DIAMETER);

                //Currently there isn't a yOffset
                newTransducer.transform.localPosition = new Vector3(xOffset, 0f, zOffset);

                transducerArray[i,j] = newTransducer;
            }
        }

    }

    void DestroyTransducerArray()
    {
        var currentContainer = transform.Find(CONTAINER_NAME);
        if (currentContainer != null)
        {
            Destroy(currentContainer);
        }
        
    }
}
