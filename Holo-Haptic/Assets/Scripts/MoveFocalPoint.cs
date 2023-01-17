using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.IO.Ports;
using TMPro;
using System;

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
    public Toggle squiggle;
    public Toggle blinkAnim;
    public Toggle randomAnim;
    public Slider speedSlider;
    public Slider RadiusSlider;
    public GameObject focalPoint;
    public GameObject transducerPrefab;

    public Slider animFrameSlider;
    float timeToNextFrame;
    public bool animPlaying;
    int m_startTime;

    private float x_pos, y_pos, z_pos;
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
    private bool sendingData = false;
    public float updateRate;
    private float updateTimer = 0f;
    private MeshRenderer meshRend;
   

    SerialPort sp;

    void Start()
    {
        //Debug.Log(transform.position.x.ToString());
        meshRend = GetComponent<MeshRenderer>();
        string the_com = "";

        Vector3 relativePos = transform.localPosition - hapticboard.transform.localPosition;

        x.SetTextWithoutNotify(System.Math.Round(relativePos.x, 2).ToString());
        y.SetTextWithoutNotify(System.Math.Round(relativePos.y, 2).ToString());
        z.SetTextWithoutNotify(System.Math.Round(relativePos.z, 2).ToString());
        intensityInput.SetTextWithoutNotify(System.Math.Round(intensitySlider.value, 2).ToString());
        focalTextUpdated();


        intensitySlider.minValue = minIntensity;
        intensitySlider.maxValue = maxIntensity;
        speedSlider.minValue = .2f;
        speedSlider.maxValue = 2f;
        m_startTime = (int)DateTime.Now.Ticks;
        
        timeToNextFrame = 0f;
        animFrameSlider.value = 0;
        animPlaying = true;




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

    public void focalTextUpdated()
    {
        UpdateTextPosition();
        sendData();
    }

    public void sendData()
    {
        if (!sendingData)
        {
           StartCoroutine(asyncSendData());
        }
    }

    public IEnumerator asyncSendData()
    {
        sendingData = true;
        string xPacket = "#x" + ((short)(transform.localPosition.x * 1000 / .0034)).ToString("X5") + "$";
        string yPacket = "#y" + ((short)(transform.localPosition.y * 1000 / .0034)).ToString("X5") + "$";
        string zPacket = "#z" + ((short)(transform.localPosition.z * 1000 / .0034)).ToString("X5") + "$";
        string iPacket = "#i" + ((short)0).ToString("X5") + "$";
        string jPacket = "#j" + ((short)0).ToString("X5") + "$";
        string aPacket = "#a" + ((short)(meshRend.enabled ? System.Math.Round(intensitySlider.value, 2) * 1023 : 0)).ToString("X5") + "$";

        Debug.LogFormat("xPack : " + xPacket + "\nyPack : " + yPacket + "\nzPack : " + zPacket + "\niPack : " + iPacket + "\njPack : " + jPacket + "\naPack : " + aPacket);
        try
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                print("opened sp");
            }
            if (sp.IsOpen)
            {
                print("Writing ");
                //sp.Write("test");

                sp.Write(xPacket);
                sp.Write(yPacket);
                sp.Write(zPacket);
                sp.Write(iPacket);
                sp.Write(jPacket);
                sp.Write(aPacket);


            }
        } catch(System.Exception e)
        {
            print("Error attempting to send data to port: " + e.Message);
        }
       
        sendingData = false;
        yield return null;
    }

    void UpdateTextPosition()
    {
        Debug.Log("started");

        bool xParse = float.TryParse(x.text, out x_pos);
        bool yParse = float.TryParse(y.text, out y_pos);
        bool zParse = float.TryParse(z.text, out z_pos);

        Debug.Log("x: " + x_pos + ",y: " + y_pos + ",z: " + z_pos);

    }

    void StaticPosition()
    {
        transform.localPosition = new Vector3(x_pos + hapticboard.transform.localPosition.x, y_pos + hapticboard.transform.localPosition.y, z_pos + hapticboard.transform.localPosition.z);
    }

    void CircleAnimation()
    {
        RadiusSlider.maxValue = ((hapticboard.GetComponent<TransducerArrayManager>().getCol())/2) * 0.1f;

        angle = animFrameSlider.value * ((Mathf.PI * 2) / animFrameSlider.maxValue);

        transform.localPosition = new Vector3(x_pos, y_pos, z_pos);
        positionOffset.Set(Mathf.Cos( angle ) * RadiusSlider.value*.1f,ElevationOffset, Mathf.Sin( angle ) * RadiusSlider.value *.1f);
        transform.Translate(positionOffset,Space.Self);
        
    }


    //Made it so that the line animation starts on the far left of the board and moves all the way to the other side; the length of the distance traveled changes with the board size
    void LineAnimation()
    {
        int col = hapticboard.GetComponent<TransducerArrayManager>().getCol();
        float lineStart = (-1 * (col /2) * 0.01f);
        var length = col * 0.01f;
        transform.localPosition = new Vector3(lineStart + (animFrameSlider.value * (2f / animFrameSlider.maxValue) * length - Mathf.Max(0, animFrameSlider.value - animFrameSlider.maxValue / 2f) * (2f / animFrameSlider.maxValue) * length), y_pos, z_pos);
    }

    void SquiggleAnimation(){
        int col = hapticboard.GetComponent<TransducerArrayManager>().getCol();
        float lineStart = (-1 * (col /2) * 0.01f);
        int a = 1;
        transform.localPosition = new Vector3(lineStart + Mathf.PingPong((animFrameSlider.value/animFrameSlider.maxValue) * 0.5f, col* 0.01f), y_pos, lineStart + Mathf.PingPong((animFrameSlider.value / animFrameSlider.maxValue) * 3, col* 0.01f));
    }
    void BlinkAnimation(){
        int chunk = (int)( 4 * animFrameSlider.value / animFrameSlider.maxValue);
        meshRend.enabled = chunk % 2 == 1;

        
    }
    
    public void SpeedValueChanged()
    {

    }

    void RandomAnimation()
    {
        int col = hapticboard.GetComponent<TransducerArrayManager>().getCol();
        int row = hapticboard.GetComponent<TransducerArrayManager>().getRow();
        float xvalue = (-1 * (col / 2) * 0.01f);
        float zvalue = (-1 * (row / 2) * 0.01f);

        UnityEngine.Random.InitState(m_startTime + (int)(10 * animFrameSlider.value / animFrameSlider.maxValue));
        var position = new Vector3(UnityEngine.Random.Range(xvalue, Mathf.Abs(xvalue)), .15f, UnityEngine.Random.Range(zvalue, Mathf.Abs(zvalue)));
        transform.localPosition = position;
    }

    void Update()
    {

        updateTimer += Time.deltaTime;
        if(updateTimer > updateRate)
        {
            updateTimer = 0f;
            sendData();
        }
        if (animPlaying)
        {
            timeToNextFrame += Time.deltaTime * speedSlider.value;
            if (timeToNextFrame > 1f / animFrameSlider.maxValue)
            {
                animFrameSlider.value = (animFrameSlider.value + 1) % animFrameSlider.maxValue;
                timeToNextFrame -= 1f / animFrameSlider.maxValue;
               
            }
        }
       

        speedSlider.minValue= 0f;
        if (!animationOn & !randomAnim.isOn)
        {
            StaticPosition();
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
        }
        if(squiggle.isOn){
            SquiggleAnimation();
        }
        if (blinkAnim.isOn)
        {
            BlinkAnimation();
        }
        else
        {
            meshRend.enabled = true;
        }
        if (randomAnim.isOn)
        {
            RandomAnimation();
        }
    }
}
