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

    public void RotateAround(Vector3 axis, float duration)
    {
        StartCoroutine(Rotate(axis, duration));
    }

    IEnumerator Rotate(Vector3 axis, float duration)
    {
        float lerp = (180f / duration) * Time.deltaTime;
        float angle = 0;
        while (angle < 180f)
        {
            float rot = Mathf.Min(lerp, 180f - angle);

            Debug.Log(angle);

            transform.localRotation = Quaternion.AngleAxis(angle, axis);
            yield return null;
            angle += rot;
        }
        //transform.localPosition = target;
        //transform.localRotation = Quaternion.Euler(Vector3.zero);

        //transform.localRotation = Quaternion.AngleAxis(180f, axis);

    }


}
