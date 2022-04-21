using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;

public class SendInfo : MonoBehaviour
{

    SerialPort sp;
    float next_time;
    int ii = 0;
    float x, y, z;
    public int baudrate = 115200;
    public bool readFromFile;
    private bool noPortsFound;


    // Start is called before the first frame update
    void Start()
    {
        string the_com = "";
        next_time = Time.time;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        Debug.Log("Hello");

        if (readFromFile)
        {
            the_com = File.ReadAllText("port.ini");

            if (!the_com.StartsWith("COM"))
            {
                noPortsFound = true;
                Debug.Log("No port found!");
            }
            else
            {
                try
                {
                    OpenPort(the_com, baudrate);
                    noPortsFound = false;
                }
                catch (System.Exception e)
                {
                    noPortsFound = true;
                    Debug.Log("Error opening port: " + e.Message);
                }
            }
        }
        else
        {
            string[] ports = SerialPort.GetPortNames();
            Debug.Log(ports);
            if (ports.Length < 1)
            {
                noPortsFound = true;
                Debug.Log("No port found!");
            }
            else
            {
                noPortsFound = false;
                the_com = ports[0];
                OpenPort(the_com, baudrate);
            }
        }
        Debug.Log(the_com);
    }

    public void OpenPort(string portname, int baudrate)
    {
        sp = new SerialPort(portname, baudrate);
        sp.Open();
        sp.ReadTimeout = 100;
        sp.Handshake = Handshake.None;
        if (sp.IsOpen) 
        { 
            print("Open");
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
                sp.Write("X=" + x.ToString() + "Y=" + z.ToString() + "Z=" + y.ToString());
            }
            next_time = Time.time + 5;
        }
    }
}
