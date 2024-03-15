using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexManager : MonoBehaviour
{
    public static HexManager Instance;

    public HexSO hexSO;

    public GameObject hexPrefab;
    public GameObject hexStackPrefab;
    public GameObject hexSlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public Material GetMaterial(EHexColor eHexColor)
    {
        return hexSO.GetMaterial(eHexColor);
    }
}
