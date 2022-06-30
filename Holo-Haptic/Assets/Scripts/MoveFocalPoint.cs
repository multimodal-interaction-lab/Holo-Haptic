using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
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
    public GameObject pivotPoint;
    private Vector3 axis = Vector3.right;
    public Toggle line;
    public Toggle circleAnim;
    public Slider speedSlider;
    public Slider RadiusSlider;

    private float ElevationOffset = 0;
    private float angle;
    private Vector3 positionOffset;
    float minIntensity = 0f;
    float maxIntensity = 1f;
    public int baudrate = 115200;
    public bool readFromFile;
    private bool noPortsFound;
    public bool animationOn = false;
    private bool resetPos = true;
   

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

/*        foreach (string mysps in SerialPort.GetPortNames())
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
        }*/


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
                    sendData();
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
                sendData();
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
            sp.Write("X=" + x.text + "Y=" + z.text + "Z=" + y.text + " I=" + System.Math.Round(intensitySlider.value, 2).ToString());
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

    void CircleAnimation()
    {
        RadiusSlider.maxValue = ((hapticboard.GetComponent<TransducerArrayManager>().getCol())/2) * 0.1f;
        if(resetPos)
        {
            transform.localPosition = new Vector3(0.0f, 0.15f, 0.0f);
            resetPos = false;
        }
        positionOffset.Set(
         Mathf.Cos( angle ) * RadiusSlider.value*.1f,
         ElevationOffset,
         Mathf.Sin( angle ) * RadiusSlider.value *.1f
        );
        transform.Translate(positionOffset,Space.Self);
        angle += Time.deltaTime * speedSlider.value;
    }

    void LineAnimation()
    {
        int col = hapticboard.GetComponent<TransducerArrayManager>().getCol();
        float lineStart = (-1 * (col /2) * 0.01f);
        transform.localPosition = new Vector3(lineStart + Mathf.PingPong(speedSlider.value * Time.time, col* 0.01f), transform.localPosition.y,transform.localPosition.x);
    }

    void Update()
    {

        if (!animationOn)
        {
            UpdatePosition();
        }
        if(Input.GetKey(KeyCode.A))
        {
            animationOn = true;
        }

        if (Input.GetKey(KeyCode.U))
        {
            animationOn = false;
        }

        if (line.isOn)
        {
           LineAnimation();
        }
        if(circleAnim.isOn)
        {
            CircleAnimation();
            //RadiusSlider.setActive(true);
        }
        else{
            //RadiusSlider.setActive(true);
        }
        
        

    }
}
