using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightMenuManager : MonoBehaviour
{
    GameObject canvasObj;
    void Start()
    {
        canvasObj = GetComponentInChildren<Canvas>().gameObject;
    }

    public void HideMenus()
    {
        foreach(Transform t in canvasObj.transform) 
        {
            t.gameObject.SetActive(false);
        }
    }
}
