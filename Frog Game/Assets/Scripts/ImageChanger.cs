using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChanger : MonoBehaviour
{
    public Sprite NewSprite;
    public Sprite NewSprite2;

    public void Change(){

    var img = GetComponent<Image>();

    if(img.sprite == NewSprite){
        img.sprite = NewSprite2;
    } else {
        img.sprite = NewSprite;
    }
    }
}
