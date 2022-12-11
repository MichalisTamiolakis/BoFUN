using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class AdjustCameraHeightToBoard : MonoBehaviour
{
    public Camera camera;
    public Transform targetPlane;

    float fieldOfView=0;

    public void Update()
    {
        if(fieldOfView != camera.fieldOfView)
        {
            RecalculateCameraPosition();
            fieldOfView = camera.fieldOfView;
        }
    }

    private void RecalculateCameraPosition()
    {
        targetPlane.transform.position = Vector3.zero;

        // Calculate camera height
        float height = (targetPlane.transform.localScale.z * 10f / 2f) / Mathf.Tan((camera.fieldOfView / 2.0f) * Mathf.Deg2Rad);

        camera.transform.position = new Vector3(0f, height, 0f);
    }
}