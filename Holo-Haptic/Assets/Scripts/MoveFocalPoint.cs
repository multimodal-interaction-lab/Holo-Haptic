using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;

public class MoveFocalPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public TMP_InputField x;
    public TMP_InputField y;
    public TMP_InputField z;
    public GameObject hapticboard;
    public Slider intensitySlider;
    public TMP_InputField intensityInput;

    float minIntensity = 0f;
    float maxIntensity = 1f;

    SerialPort sp;

    void Start()
    {
        //Debug.Log(transform.position.x.ToString());

        string the_com = "";

        Vector3 relativePos = transform.localPosition - hapticboard.transform.localPosition;

        x.text = System.Math.Round(relativePos.x, 2).ToString();
        y.text = System.Math.Round(relativePos.y, 2).ToString();
        z.text = System.Math.Round(relativePos.z, 2).ToString();
        intensityInput.SetTextWithoutNotify(System.Math.Round(intensitySlider.value, 2).ToString());

        intensitySlider.minValue = minIntensity;
        intensitySlider.maxValue = maxIntensity;

        foreach (string mysps in SerialPort.GetPortNames())
        {
            print(mysps);
            if (mysps != "COM1") { the_com = mysps; break; }
        }
        sp = new SerialPort("\\\\.\\" + the_com, 115200);
        if (!sp.IsOpen)
        {
            print("Opening " + the_com + ", baud 115200");
            sp.Open();
            sp.ReadTimeout = 500;
            sp.Handshake = Handshake.None;

            if (sp.IsOpen) 
            {
                print("Writing ");
                sp.Write("X=" + x.text + "Y=" + y.text + "Z=" + z.text + " I=" + System.Math.Round(intensitySlider.value,2).ToString());
            }
        }

    }

    public void incrementVal(TMP_InputField g)
    {
        g.text = System.Math.Round(float.Parse(g.text) + 0.01f, 2).ToString();
        sendData();
    }

    public void decrementVal(TMP_InputField g)
    {
        g.text = System.Math.Round(float.Parse(g.text) - 0.01f, 2).ToString();
        sendData();   
    }

    public void intensitySliderUpdate()
    {
        intensityInput.SetTextWithoutNotify(System.Math.Round(intensitySlider.value, 2).ToString());
        sendData();
    }

    public void intensityTextUpdate()
    {
        float val = (float)System.Math.Round(float.Parse(intensityInput.text), 2);
        float clampedVal = Mathf.Clamp(val, minIntensity, maxIntensity);
        intensityInput.SetTextWithoutNotify(clampedVal.ToString());
        intensitySlider.SetValueWithoutNotify(clampedVal);
        sendData();
    }

    public void sendData()
    {
        if (!sp.IsOpen)
        {
            sp.Open();
            print("opened sp");
        }
        if (sp.IsOpen)
        {
            print("Writing ");
            sp.Write("X=" + x.text + "Y=" + y.text + "Z=" + z.text + " I=" + System.Math.Round(intensitySlider.value, 2).ToString());
        }
    }

    void UpdatePosition()
    {
        float x_pos, y_pos, z_pos;

        bool xParse = float.TryParse(x.text, out x_pos);
        bool yParse = float.TryParse(y.text, out y_pos);
        bool zParse = float.TryParse(z.text, out z_pos);

        if (xParse && yParse && zParse)
        {

            transform.localPosition = new Vector3(x_pos + hapticboard.transform.localPosition.x, y_pos + hapticboard.transform.localPosition.y, z_pos + hapticboard.transform.localPosition.z);
        }

    }

    void Update()
    {

        UpdatePosition();

    }
}
