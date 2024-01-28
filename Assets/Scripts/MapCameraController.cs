using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public Transform finalPlace;

    private float minSize = 20f;
    private float maxSize = 44f;
    private float starterSize;
    private float dragSpeed = 10f;
    private float zoomOutSpeed;
    private float zoomInSpeed;

    private Camera mapCamera;
    private bool isZoomingOut = false;
    private bool isZoomingIn = false;
    private bool isMoving = false;

    private void Start()
    {
        mapCamera = GetComponent<Camera>();
        var fin = finalPlace.position;
        fin.z = transform.position.z;
        finalPlace.position = fin;
    }

    public void ZoomOut()
    {
        isZoomingOut = true;
        isMoving = true;
        starterSize = mapCamera.orthographicSize;

        var allDistance = Vector3.Distance(transform.position, finalPlace.position);
        var allTime = allDistance / dragSpeed;
        zoomOutSpeed = (maxSize - starterSize) / (allTime / 2.0f);
        zoomInSpeed = (maxSize - starterSize) / (allTime / 2.0f);
    }
    void ZoomOut(float zoom)
    {
        var size = mapCamera.orthographicSize;
        size += zoom * zoomOutSpeed;
        if (size > maxSize)
        {
            size = maxSize;
            isZoomingOut = false;
            isZoomingIn = true;
        }

        mapCamera.orthographicSize = size;
    }

    void ZoomIn(float zoom)
    {
        var size = mapCamera.orthographicSize;
        size -= zoom * zoomInSpeed;
        if (size < minSize)
        {
            size = minSize;
            isZoomingIn = false;
        }

        mapCamera.orthographicSize = size;
    }

    void MoveTo()
    {
        Vector3 newpos = Vector3.MoveTowards(transform.position, finalPlace.position, dragSpeed * Time.deltaTime);
        transform.position = newpos;
    }

    void Update()
    {
        if (isZoomingOut)
        {
            ZoomOut(Time.deltaTime);
        }
        else if (isZoomingIn)
        {
            ZoomIn(Time.deltaTime);
        }
        if (isMoving)
        {
            MoveTo();
        }
    }
}
