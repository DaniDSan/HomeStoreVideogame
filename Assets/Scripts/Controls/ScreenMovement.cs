using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour {
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float zoomSpeed;

    [SerializeField] private float verticalSpeed;

    [SerializeField] float minFOV = 20f;
    [SerializeField] float maxFOV = 60f;

    [Header("CamReferences")]
    //Centro que corresponde al apartamento que se este visualizando.
    [SerializeField] private Transform center;

    //Rotador de la camara.
    [SerializeField] private Transform rotator;

    private float camHorizontalDir;

    private float camZoomDir;

    [SerializeField] private float camVerticalDir;

    //Referencia a la camara.
    private Camera cam;

    private void Awake() {
        cam = Camera.main;

        //Initialise.
        //rotator.position = new Vector3(center.position.x, cam.position.y, center.position.z);

        cam.transform.SetParent(rotator);
    }

    private void Update() {
        ReadInputs();

        RotateCamera();

        ZoomCamera();

        MoveCamera();
    }

    private void ReadInputs() {
        //Movimiento horizontal de la cámara alrededor del centro de rotación.
        //Forma más explicada.
        //float camHorizontalDirPrimary = ControlsManager.instance.c.CustomizeApartment.CamRotationDirPrimary.ReadValue<float>();
        //float camHorizontalDirSecondary = ControlsManager.instance.c.CustomizeApartment.CamRotationDirSecondary.ReadValue<float>();
        //camHorizontalDir = Mathf.Clamp(camHorizontalDirPrimary + camHorizontalDirSecondary, -1f, 1f);
        camHorizontalDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamRotationDirPrimary.ReadValue<float>() + ControlsManager.instance.c.CustomizeApartment.CamRotationDirSecondary.ReadValue<float>(), -1f, 1f);

        camVerticalDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamVerticalMovementPrimary.ReadValue<float>() + ControlsManager.instance.c.CustomizeApartment.CamVerticalMovementSecondary.ReadValue<float>(), -1f, 1f);

        camZoomDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamZoomDir.ReadValue<float>(), -1f, 1f);
    }

    private void RotateCamera() {
        cam.transform.LookAt(center);

        rotator.Rotate(Vector3.down * camHorizontalDir * rotationSpeed * Time.deltaTime);
    }

    private void ZoomCamera() {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - camZoomDir * zoomSpeed * Time.deltaTime, minFOV, maxFOV);
    }

    private void MoveCamera() {
        Vector3 newPosition = cam.transform.position + Vector3.up * camVerticalDir * verticalSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, -40f, -20f);
        cam.transform.position = newPosition;
    }
}