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

    private void OnTriggerExit(Collider other){
        Debug.Log(other.name);
        if(other.name == "bone3"){
            if(toggle.isOn){
            toggle.isOn = false;
            }
            else{
            toggle.isOn = true;
            }
        }
        //toggle.isOn = !toggle.isOn;
        /*LefthandMenu.SetActive(false);
        RighthandMenu.SetActive(true);
        AnimationOptions.SetActive(true);*/
    }
}