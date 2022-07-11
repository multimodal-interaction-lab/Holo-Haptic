using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderToggle : MonoBehaviour
{
    public Toggle toggle;

    /*void OnCollisionEnter(Collision col){
        print("TRIGGER!!!");
        AnimationOptions.SetActive(true);on
    }*/

    private void OnTriggerEnter(Collider other){
        Debug.Log("hit detected");
        if(toggle.isOn){
            toggle.isOn = false;
        }
        else{
            toggle.isOn = true;
        }
        //toggle.isOn = !toggle.isOn;
        /*LefthandMenu.SetActive(false);
        RighthandMenu.SetActive(true);
        AnimationOptions.SetActive(true);*/
    }
}