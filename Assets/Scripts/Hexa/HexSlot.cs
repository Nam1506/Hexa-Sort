using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexSlot : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_renderer;

    public HexStack hexStack;

    public Element element;

    public List<HexSlot> neighbors = new();

    private void ChangeMaterial(EHexColor material)
    {
        m_renderer.sharedMaterial = HexManager.Instance.GetMaterial(material);
    }

    private void OnMouseDown()
    {
        //ChangeMaterial(EHexColor.White);
    }

    public void Select()
    {
        ChangeMaterial(EHexColor.White);
    }

    public void UnSelect()
    {
        ChangeMaterial(EHexColor.Shadow);
    }

    public bool IsAvailable()
    {
        return hexStack == null;
    }

    public void SetHexStack(HexStack stack)
    {
        hexStack = stack;

        hexStack.transform.SetParent(this.transform);

        hexStack.transform.localPosition = Vector3.zero;

        GameplayManager.Instance.CheckMergeHex(this);

        HolderGenerator.Instance.Regenerate();
    }
}
