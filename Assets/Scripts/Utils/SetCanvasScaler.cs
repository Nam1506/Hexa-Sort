using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasScaler : MonoBehaviour
{
    public CanvasScaler canvasScaler;


    void Start()
    {
        float ratio = (float)(Screen.width / (float)Screen.height);

        if (ratio >= 0.6f)
        {
            canvasScaler.matchWidthOrHeight = 1;

        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }

}
