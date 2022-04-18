using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SendInfo : MonoBehaviour
{

    SerialPort sp;
    float next_time;
    int ii = 0;
    float x, y, z;


    // Start is called before the first frame update
    void Start()
    {
        string the_com = "";
        next_time = Time.time;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        Debug.Log("Hello");

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
            sp.ReadTimeout = 100;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen) { print("Open"); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_time)
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                print("opened sp");
            }
            if (sp.IsOpen)
            {
                print("Writing ");
                sp.Write("X=" + x.ToString() + "Y=" + y.ToString() + "Z=" + z.ToString());
            }
            next_time = Time.time + 5;
        }
    }
}
