using UnityEngine;

public class ShareInfo : MonoBehaviour
{
    public string text;
    public string format;

    void Awake()
    {
        text = text.Replace("\\n", "\n");
        format = format.Replace("\\n", "\n");
    }
}
