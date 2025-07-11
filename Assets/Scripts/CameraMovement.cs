using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float cameraSpeed = 100f;
    [SerializeField] private float moveSmoothinTime = 0.3f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomSmoothinTime = 0.3f;

    private Vector3 targetPosition;
    private Vector3 moveVelocity = Vector3.zero;

    private float currentZoom = 0f;
    private float targetZoom;
    private float zoomVelocity = 0f;

    void Start()
    {
        targetPosition = transform.position;
        targetZoom = currentZoom;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void LateUpdate()
    {
        currentZoom = Mathf.SmoothDamp(currentZoom, targetZoom, ref zoomVelocity, zoomSmoothinTime);

        Vector3 desired = targetPosition + transform.forward * currentZoom;

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref moveVelocity, moveSmoothinTime);
    }

    private void HandleMovement()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 pan = new Vector3(HorizontalInput, 0f, VerticalInput) * cameraSpeed * Time.deltaTime;

        targetPosition += pan;
    }

    private void HandleZoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        if (zoomInput == 0f || MenuController.Instance.isPaused == true) return;

        targetZoom += zoomInput * zoomSpeed;

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
    }
}