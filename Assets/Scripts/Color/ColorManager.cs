using UnityEngine;

public class ColorManager : MonoBehaviour
{
    // public variables
    public Color foregroundColor;
    public Color backgroundColor;

    [Range(0, 359)]
    public int hue;
    [Range(0, 359)]
    public int hueStep;

    [Range(0, 359)]
    public int vCorrStartHue;
    [Range(0, 359)]
    public int vCorrEndHue;
    [Range(0, 1)]
    public float vCorrMostHue;
    [Range(0, 255)]
    public int minValue;

    public bool setRandomHueOnStart = true;
    public bool valueCorrection = true;

    // private variables
    private ColorWorker[] colorWorkers;

    private float currColorH;
    private float currColorS;
    private float currColorV;

    // hue stuff
    private float mostHueAbsolute; // most hue in absolute scale from 0 to 359
    private int currForegroundHue;
    private int currBackgroundHue;
    private int defaultForegroundHue;
    private int defaultBackgroundHue;

    // value  stuff
    private float valueStepBefore; // step before most hue
    private float valueStepAfter; // step after most hue
    private int hueSpan; // hue end - hue start
    private int valueSpan; // min - max
    private float currBackgroundValue;
    private int defaultBackgroundValue;

    void Start()
    {
        Color.RGBToHSV(foregroundColor, out currColorH, out currColorS, out currColorV);
        defaultForegroundHue = (int)(currColorH * 360);
        Color.RGBToHSV(backgroundColor, out currColorH, out currColorS, out currColorV);
        defaultBackgroundHue = (int)(currColorH * 360);
        defaultBackgroundValue = (int)(currColorV * 256);
        colorWorkers = FindObjectsOfType<ColorWorker>();
        hueSpan = vCorrEndHue - vCorrStartHue;
        valueSpan = minValue - defaultBackgroundValue;
        mostHueAbsolute = vCorrStartHue + (vCorrMostHue * hueSpan);
        currBackgroundValue = defaultBackgroundValue;

        if (setRandomHueOnStart) SetRandomHue();
        if (valueCorrection)
        {
            valueStepBefore = ValueCorrection(1, vCorrMostHue);
            valueStepAfter = ValueCorrection(-1, 1 - vCorrMostHue);
            CheckCorrection();
        }
        UpdateColors();
        Paint();
    }

    [ContextMenu("UpdateColors")]
    void UpdateColors()
    {
        currForegroundHue = defaultForegroundHue + hue;
        currForegroundHue = HueOverflow(currForegroundHue);
        currBackgroundHue = defaultBackgroundHue + hue;
        currBackgroundHue = HueOverflow(currBackgroundHue);

        Color.RGBToHSV(foregroundColor, out currColorH, out currColorS, out currColorV);
        foregroundColor = Color.HSVToRGB(currForegroundHue / 360f, currColorS, currColorV);
        Color.RGBToHSV(backgroundColor, out currColorH, out currColorS, out currColorV);
        backgroundColor = Color.HSVToRGB(currBackgroundHue / 360f, currColorS, currBackgroundValue / 256f);
    }

    float ValueCorrection(int dir, float mostHue)
    {
        return (hueStep * valueSpan * dir) / (hueSpan * mostHue);
    }

    void CheckCorrection()
    {
        if (hue > vCorrStartHue && hue < vCorrEndHue)
        {
            if (hue < mostHueAbsolute)
            {
                currBackgroundValue = defaultBackgroundValue
                + ((hue - vCorrStartHue) / hueStep) * valueStepBefore;
            }
            else
            {
                currBackgroundValue = defaultBackgroundValue
                + ((mostHueAbsolute - vCorrStartHue) / hueStep) * valueStepBefore
                + ((hue - mostHueAbsolute) / hueStep) * valueStepAfter;
            }
        }      
        Debug.Log(currBackgroundValue);
    }

    void Paint()
    {
        foreach (ColorWorker w in colorWorkers)
        {
            w.Paint();
        }
    }

    void SetRandomHue()
    {
        hue = Random.Range(0, 359);
    }

    public void ChangeColor()
    {
        hue += hueStep;
        hue = HueOverflow(hue);
        if (valueCorrection)
        {
            if (currBackgroundHue > vCorrStartHue && currBackgroundHue < vCorrEndHue)
            {
                if (currBackgroundHue < mostHueAbsolute) currBackgroundValue += valueStepBefore;
                else currBackgroundValue += valueStepAfter;
            }
            else if (currBackgroundValue != defaultBackgroundValue) currBackgroundValue = defaultBackgroundValue;
        }
        else if (currBackgroundValue != defaultBackgroundValue) currBackgroundValue = defaultBackgroundValue;

        UpdateColors();
        Paint();
    }

    static public int HueOverflow(int value)
    {
        if (value < 360) return value;
        else return value - 360;
    }
}
