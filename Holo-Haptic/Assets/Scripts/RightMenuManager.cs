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

    public void HideMenus(GameObject currentmenu, GameObject OptionOpened)
    {
        gameObject.SetActive(true);
        currentmenu.SetActive(false);
        OptionOpened.SetActive(true);
    }
}
