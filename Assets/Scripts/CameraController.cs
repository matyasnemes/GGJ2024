using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float minCameraSize = 15f;
    public float maxCameraSize = 30f;
    public float camereScrollSensitivity = 10f;

    private Camera playerCamera;
    private void Start()
    {
        playerCamera = GetComponent<Camera>();
    }
    void Update()
    {
        if (!playerCamera.enabled)
        {
            return;
        }

        var size = playerCamera.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * camereScrollSensitivity;
        size = Mathf.Clamp(size, minCameraSize, maxCameraSize);
        playerCamera.orthographicSize = size;
    }
}
