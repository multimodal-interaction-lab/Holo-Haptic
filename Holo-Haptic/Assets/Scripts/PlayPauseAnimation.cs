using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayPauseAnimation : MonoBehaviour
{
    [SerializeField]
    Slider animSlider;

    [SerializeField]
    MoveFocalPoint animSrc;



    [SerializeField]
    Sprite playGraphic;
    [SerializeField]
    Sprite pauseGraphic;


    bool IsPlaying
    {
        get { return animSrc.animPlaying; }
        set { animSrc.animPlaying = value; }
    }

    private void Start()
    {
        IsPlaying = false;
    }


    private void FixedUpdate()
    {

    }


    public void TogglePlayPause()
    {
        IsPlaying = !IsPlaying;
        if (IsPlaying)
        {
            GetComponent<Image>().sprite = pauseGraphic;
        }
        else
        {
            GetComponent<Image>().sprite = playGraphic;
        }

       
    }

    public void SetPaused()
    {
        IsPlaying = false;
        GetComponent<Image>().sprite = playGraphic;
    }



}
