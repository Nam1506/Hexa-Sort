using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public EHexColor eHexColor;

    [SerializeField] private MeshRenderer m_renderer;

    public void SetHexColor(EHexColor eHex)
    {
        eHexColor = eHex;

        ChangeMaterial(eHexColor);
    }

    public void ChangeMaterial(EHexColor material)
    {
        m_renderer.sharedMaterial = HexManager.Instance.GetMaterial(material);
    }


}
