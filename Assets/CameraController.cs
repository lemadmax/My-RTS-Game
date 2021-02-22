using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    static public CameraController instance;

    public float movementSpeed;
    public float zoomSpeed;
    public float zoomAmount;

    Transform followTransform;
    Transform cameraTransform;

    Vector3 newPosition;
    Vector3 newZoom;
    int width;
    int height;

    Vector3 zoomVec;
    Vector3 minZoom = new Vector3(0, 10, -10);
    Vector3 maxZoom = new Vector3(0, 80, -80);

    public bool freezeCamera = false;

    void Start()
    {
        instance = this;
        cameraTransform = FindObjectOfType<Camera>().transform;
        newPosition = transform.position;
        newZoom = cameraTransform.localPosition;
        width = Screen.width;
        height = Screen.height;
        zoomVec.Set(0, zoomAmount, -zoomAmount);
    }

    void Update()
    {
        if (freezeCamera) return;
        if (followTransform != null)
        {
            transform.position = followTransform.position;
        }
        else
        {
            HandleMouseInput();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            followTransform = null;
        }
    }

    void HandleMouseInput()
    {
        if(Input.mousePosition.x < 1)
        {
            newPosition -= transform.right * movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x > width - 1)
        {
            newPosition += transform.right * movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y < 1)
        {
            newPosition -= transform.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y > height - 1)
        {
            newPosition += transform.forward * movementSpeed * Time.deltaTime;
        }
        if(Input.mouseScrollDelta.y > 0)
        {
            newZoom -= zoomVec * zoomSpeed * Time.deltaTime;
            if (newZoom.y < minZoom.y) newZoom = minZoom;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            newZoom += zoomVec * zoomSpeed * Time.deltaTime;
            if (newZoom.y > maxZoom.y) newZoom = maxZoom;
        }
        transform.position = newPosition;
        cameraTransform.localPosition = newZoom;
    }
}
