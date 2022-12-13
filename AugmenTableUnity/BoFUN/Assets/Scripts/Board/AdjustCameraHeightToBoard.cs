using UnityEngine;

public class AdjustCameraHeightToBoard : MonoBehaviour
{
    public Camera boardCamera;
    public Transform targetPlane;

    float fieldOfView=0;

    public void Update()
    {
        if(fieldOfView != boardCamera.fieldOfView)
        {
            RecalculateCameraPosition();
            fieldOfView = boardCamera.fieldOfView;
        }
    }

    private void RecalculateCameraPosition()
    {
        targetPlane.transform.position = Vector3.zero;

        // Calculate camera height
        float height = (targetPlane.transform.localScale.z * 10f / 2f) / Mathf.Tan((boardCamera.fieldOfView / 2.0f) * Mathf.Deg2Rad);

        boardCamera.transform.position = new Vector3(0f, height, 0f);
    }
}