using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateArrayMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Text rowInput;
    [SerializeField]
    TMP_Text colInput;
    [SerializeField]
    TransducerArrayManager transCntrl;
   
    public void GenerateArrayPressed()
    {
        if(rowInput.text == "" || colInput.text == "")
        {
            Debug.Log("Missing Input Values!");
            return;
        }

        int rows = int.Parse(rowInput.text);
        int cols = int.Parse(colInput.text);

        if(rows <= 0 || cols <= 0)
        {
            Debug.Log("Rows/Cols must be greater than 0!");
            return;
        }

        transCntrl.GenerateTransducerArray(rows, cols);
    }
}
