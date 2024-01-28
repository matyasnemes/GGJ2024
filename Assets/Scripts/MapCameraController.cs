using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    private float minCameraSize = 15f;
    private float maxCameraSize = 44f;
    private float camereScrollSensitivity = 5f;

    private Camera mapCamera;
    private bool isZoomingOut = false;
    private void Start()
    {
        mapCamera = GetComponent<Camera>();
    }

    public void ZoomOut()
    {
        isZoomingOut = true;
    }
    public void ZoomOut(float zoom)
    {
        var size = mapCamera.orthographicSize;
        size += zoom * camereScrollSensitivity;
        if (size > maxCameraSize)
        {
            size = maxCameraSize;
            isZoomingOut = false;
        }

        mapCamera.orthographicSize = size;
    }

    public void ZoomTo()
    {

    }

    void Update()
    {
        if (isZoomingOut)
        {
            ZoomOut(Time.deltaTime);
        }
    }
}
