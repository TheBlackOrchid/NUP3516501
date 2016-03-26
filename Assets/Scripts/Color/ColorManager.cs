using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public Color foregroundColor;
    public Color backgroundColor;
    [Range(0, 359)]
    public int hue; 
    [Range(0, 359)]
    public int hueStep;
    public bool setRandomHueOnStart = true;

    private ColorWorker[] colorWorkers;
    private float currColorH;
    private float currColorS;
    private float currColorV;
    private int currForegroundHue;
    private int currBackgroundHue;
    private int defaultForegroundHue;
    private int defaultBackgroundHue;

    void Start()
    {
        Color.RGBToHSV(foregroundColor, out currColorH, out currColorS, out currColorV);
        defaultForegroundHue = (int)(currColorH * 360);
        Color.RGBToHSV(backgroundColor, out currColorH, out currColorS, out currColorV);
        defaultBackgroundHue = (int)(currColorH * 360);
        colorWorkers = FindObjectsOfType<ColorWorker>();
        if (setRandomHueOnStart) SetRandomHue();
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
        backgroundColor = Color.HSVToRGB(currBackgroundHue / 360f, currColorS, currColorV);
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
        UpdateColors();
        Paint();
    }

    static public int HueOverflow(int value)
    {
        if (value < 360) return value;
        else return value - 360;
    }
}
