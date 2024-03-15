using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static GameUtils Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ClearChildren(Transform trans)
    {
        for (int i = trans.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(trans.GetChild(i).gameObject);
        }
    }
}
