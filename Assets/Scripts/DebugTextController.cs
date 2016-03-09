using UnityEngine;
using UnityEngine.UI;

public class DebugTextController : MonoBehaviour {

    private Text text;
    private float deltaTime = 0;
    private float fps;
    private string fpsText;

	void Awake ()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (!Debug.isDebugBuild) { gameObject.SetActive(false); }
#endif
    }

}
