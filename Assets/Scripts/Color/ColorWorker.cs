﻿using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Default,
    Camera,
    Text
}

public class ColorWorker : MonoBehaviour
{
    public ColorManager colorManager;
    public Type type;

    private SpriteRenderer sR;
    // if it's regular piece
    private Camera cam;
    // if it's a camera
    private Text text;
    // if it's gui text;
    private Color currColor;

    void Awake()
    {
        if (colorManager == null)
        {
            colorManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ColorManager>();
        }
        sR = GetComponent<SpriteRenderer>();
        cam = GetComponent<Camera>();
        text = GetComponent<Text>();
    }

    public void Paint()
    {
        switch (type)
        {
            case Type.Default:
                currColor = sR.color;
                currColor = colorManager.foregroundColor;
                currColor.a = sR.color.a;
                sR.color = currColor;
                break;
            case Type.Camera:
                cam.backgroundColor = colorManager.backgroundColor;
                break;
            case Type.Text:
                text.color = colorManager.foregroundColor;
                break;
        }
    }
}
