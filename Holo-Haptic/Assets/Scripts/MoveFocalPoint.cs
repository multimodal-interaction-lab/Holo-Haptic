using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveFocalPoint : MonoBehaviour
{
    // Start is called before the first frame update

    public InputField x;
    public InputField y;
    public InputField z;
    public GameObject hapticboard;

    void Start()
    {
        //Debug.Log(transform.position.x.ToString());

        x.text = System.Math.Round(transform.position.x, 2).ToString();
        y.text = System.Math.Round(transform.position.y, 2).ToString();
        z.text = System.Math.Round(transform.position.z, 2).ToString();
    }

    // Update is called once per frame

    public void incrementVal(InputField g)
    {
        g.text = (float.Parse(g.text) + 0.01f).ToString();
    }

    public void decrementVal(InputField g)
    {
        g.text = (float.Parse(g.text) - 0.01f).ToString();
    }

    void Update()
    {
        float x_pos = float.Parse(x.text);
        float y_pos = float.Parse(y.text);
        float z_pos = float.Parse(z.text);

        transform.Translate(x_pos - transform.position.x, y_pos - transform.position.y, z_pos - transform.position.z);

    }
}
