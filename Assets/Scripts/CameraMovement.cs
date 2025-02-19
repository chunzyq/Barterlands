using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float cameraSpeed = 100f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;

    private float smoothingSpeed = 0.4f;

    Vector3 velocity = Vector3.zero;
    private float targetDistance;
    private float currentZoom = 10f;

    void Start()
    {
        // targetDistance = Vector3.Distance(Camera.main.transform.position, Vector3.zero);
        targetDistance = Mathf.Abs(Camera.main.transform.localPosition.z);
    }
    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 moveCamera = new Vector3(HorizontalInput, 0, VerticalInput) * cameraSpeed * Time.deltaTime;

        Vector3 targetPosition = transform.position + moveCamera;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity ,smoothingSpeed);
    }

    private void HandleZoom()
    {
        // float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // currentZoom -= zoomSpeed * zoomInput;
        // currentZoom = Math.Clamp(currentZoom, minZoom, maxZoom);
        // Vector3 zoomDirection = transform.forward;
        // transform.position += zoomDirection * zoomSpeed * zoomInput;

        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        if (zoomInput != 0)
        {
            // targetDistance -= zoomInput * zoomSpeed;
            // targetDistance = Mathf.Clamp(targetDistance, minZoom, maxZoom);

            // Vector3 direction = Camera.main.transform.forward;
            // Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0, 0, -targetDistance), Time.deltaTime * zoomSpeed);

            currentZoom -= zoomSpeed * zoomInput;
            currentZoom = Math.Clamp(currentZoom, minZoom, maxZoom);
            Vector3 zoomDirection = transform.forward;
            transform.position += zoomDirection * zoomSpeed * zoomInput;
        }
    }
}
