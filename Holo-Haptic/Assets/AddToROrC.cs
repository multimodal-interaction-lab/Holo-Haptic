using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class AddToROrC : MonoBehaviour
{
    public TMP_Text box;

    private string value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        value = box.text;
    }

    public void plus1()
    {
        int valueToInt = int.Parse(value);
        valueToInt = valueToInt+1;
        box.text = valueToInt.ToString();
    }

    public void minus1()
    {
        int valueToInt = int.Parse(value);
        valueToInt = valueToInt-1;
        box.text = valueToInt.ToString();
    }

    public void plus5()
    {
        int valueToInt = int.Parse(value);
        valueToInt = valueToInt+5;
        box.text = valueToInt.ToString();
    }
    public void minus5()
    {
        int valueToInt = int.Parse(value);
        valueToInt = valueToInt-5;
        box.text = valueToInt.ToString();
    }

}
