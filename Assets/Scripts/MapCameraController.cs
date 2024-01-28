using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public float minCameraSize = 15f;
    public float maxCameraSize = 30f;
    public float camereScrollSensitivity = 5f;

    private Camera mapCamera;
    private bool isZoomingOut = false;
    private float lastTime;
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
        size = zoom * camereScrollSensitivity;
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
            var currTime = Time.deltaTime;
            ZoomOut(currTime - lastTime);
            lastTime = currTime;
        }
    }
}
