using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColliderDetector : MonoBehaviour
{
    public GameObject AnimationOptions;
    public GameObject LefthandMenu;
    public GameObject RighthandMenu;
    public Button button;

    /*void OnCollisionEnter(Collision col){
        print("TRIGGER!!!");
        AnimationOptions.SetActive(true);on
    }*/

    private void OnTriggerEnter(Collider other){
        Debug.Log("hit detected");
        button.onClick.Invoke();
        /*LefthandMenu.SetActive(false);
        RighthandMenu.SetActive(true);
        AnimationOptions.SetActive(true);*/
    }
}
