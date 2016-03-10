using UnityEngine;
using UnityEngine.UI;

public class DebugEnabler : MonoBehaviour {

	void Awake ()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        if (!Debug.isDebugBuild) { gameObject.SetActive(false); }
#endif
    }

}
