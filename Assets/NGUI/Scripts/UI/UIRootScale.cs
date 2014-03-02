using UnityEngine;
 
[ExecuteInEditMode]
public class UIRootScale : MonoBehaviour
{
    public int manualWidth = 1280;
    public int manualHeight = 720;
     
    UIRoot uiRoot_;
     
    public float ratio {
        get {
            if(!uiRoot_){ return 1.0F; }
            return (float)Screen.height / uiRoot_.manualHeight;
        }
    }
     
    void Awake()
    {
        uiRoot_ = GetComponent<UIRoot>();
    }
     
    void Update()
    {
        if(!uiRoot_ || manualWidth <= 0 || manualHeight <= 0){ return; }
         
        int h = manualHeight;
        float r = (float)(Screen.height * manualWidth) / (Screen.width * manualHeight); // (Screen.height / manualHeight) / (Screen.width / manualWidth)
        if(r > 1.0F){ h = (int)(h * r); } // to pretend target height is more high, because screen width is too smaller to show all UI
         
        //if(uiRoot_.automatic){ uiRoot_.automatic = false; } // for before NGUI 2.3.0
        if(uiRoot_.scalingStyle != UIRoot.Scaling.FixedSize){ uiRoot_.scalingStyle = UIRoot.Scaling.FixedSize; } // for NGUI 2.3.0 or later
        if(uiRoot_.manualHeight != h){ uiRoot_.manualHeight = h; }
        if(uiRoot_.minimumHeight > 1){ uiRoot_.minimumHeight = 1; } // only for NGUI 2.2.2 to 2.2.4
        if(uiRoot_.maximumHeight < System.Int32.MaxValue){ uiRoot_.maximumHeight = System.Int32.MaxValue; } // only for NGUI 2.2.2 to 2.2.4
    }
}