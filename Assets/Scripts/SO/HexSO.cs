using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/Hex", fileName = "Hex")]
public class HexSO : ScriptableObject
{
    public List<HexColor> hexColors;

    public Material GetMaterial(EHexColor eHexColor)
    {
        return hexColors.Find(x => x.eHexColor == eHexColor).material;
    }
}

[Serializable]
public class HexColor
{
    public EHexColor eHexColor;
    public Material material;
}
