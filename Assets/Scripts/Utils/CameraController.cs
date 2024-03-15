using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera cam;

    public float defaultFov = 20;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    public float zoomSensitive = 0.01f;
    public float mouseZoomSensitive = 2f;

    public float currZoomOutMin;

    public bool canZoom = false;

    public static Action<bool> OnSetCanZoom;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        //currZoomOutMin = zoomOutMin;

        OnSetCanZoom += SetCanZoom;

    }

    // Update is called once per frame
    void Update()
    {
        //if (!GamePlayManager.instance.IsPlaying || !GamePlayManager.instance.IsDoneLoadLevel) return;


        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * zoomSensitive);
        }

#if UNITY_EDITOR
        zoom(Input.GetAxis("Mouse ScrollWheel") * mouseZoomSensitive);
#endif
    }

    void SetCanZoom(bool state)
    {
        canZoom = state;
    }

    void zoom(float increment)
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - increment, currZoomOutMin, zoomOutMax);

    }
}
