using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class MoveFocalPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public InputField x;
    public InputField y;
    public InputField z;
    public GameObject hapticboard;
    public Slider intensitySlider;
    SerialPort sp;

    void Start()
    {
        //Debug.Log(transform.position.x.ToString());

        string the_com = "";

        Vector3 relativePos = transform.localPosition - hapticboard.transform.localPosition;

        x.text = System.Math.Round(relativePos.x, 2).ToString();
        y.text = System.Math.Round(relativePos.y, 2).ToString();
        z.text = System.Math.Round(relativePos.z, 2).ToString();

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

    public void incrementVal(InputField g)
    {
        g.text = System.Math.Round(float.Parse(g.text) + 0.01f, 2).ToString();
        sendData();
    }

    public void decrementVal(InputField g)
    {
        g.text = System.Math.Round(float.Parse(g.text) - 0.01f, 2).ToString();
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


    void Update()
    {
        float x_pos = float.Parse(x.text);
        float y_pos = float.Parse(y.text);
        float z_pos = float.Parse(z.text);

        //transform.Translate(x_pos - transform.position.x, y_pos - transform.position.y, z_pos - transform.position.z);
        transform.localPosition = new Vector3(x_pos + hapticboard.transform.localPosition.x, y_pos + hapticboard.transform.localPosition.y, z_pos + hapticboard.transform.localPosition.z);

    }
}
