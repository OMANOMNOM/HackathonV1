using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement")]
    public float currentX;
    public float currentY;
    public float sensitivity;
    private float YMin = -50f;
    private float YMax = 50f;
    public Quaternion rotation;

    [Header("Object To Look At")]
    public float distance;
    public Transform lookat;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);

        rotation = Quaternion.Euler(-currentY, currentX, 0);

        transform.position = lookat.position + rotation * Direction;
        transform.LookAt(lookat.position);
    }

    public void SensitivitySlider(float value)
    {
        sensitivity = value;
    }
}
