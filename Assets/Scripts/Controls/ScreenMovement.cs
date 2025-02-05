using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float zoomSpeed;

    [SerializeField] private float verticalSpeed;

    [Header("CamReferences")]
    //Centro que corresponde al apartamento que se este visualizando.
    [SerializeField] private Transform center;

    //Rotador de la camara.
    [SerializeField] private Transform rotator;

    private float camHorizontalDir;

    private float camZoomDir;

    [SerializeField] private float camVerticalDir;

    //Referencia a la camara.
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;

        //Initialise.
        rotator.position = new Vector3(center.position.x, cam.position.y, center.position.z);

        cam.SetParent(rotator);
    }

    private void Update()
    {
        ReadInputs();

        RotateCamera();

        ZoomCamera();

        MoveCamera();
    }

    private void ReadInputs()
    {
        //Movimiento horizontal de la cámara alrededor del centro de rotación.
        //Forma más explicada.
        //float camHorizontalDirPrimary = ControlsManager.instance.c.CustomizeApartment.CamRotationDirPrimary.ReadValue<float>();
        //float camHorizontalDirSecondary = ControlsManager.instance.c.CustomizeApartment.CamRotationDirSecondary.ReadValue<float>();
        //camHorizontalDir = Mathf.Clamp(camHorizontalDirPrimary + camHorizontalDirSecondary, -1f, 1f);
        camHorizontalDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamRotationDirPrimary.ReadValue<float>() + ControlsManager.instance.c.CustomizeApartment.CamRotationDirSecondary.ReadValue<float>(), -1f, 1f);

        camVerticalDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamVerticalMovementPrimary.ReadValue<float>() + ControlsManager.instance.c.CustomizeApartment.CamVerticalMovementSecondary.ReadValue<float>(), -1f, 1f);

        camZoomDir = Mathf.Clamp(ControlsManager.instance.c.CustomizeApartment.CamZoomDir.ReadValue<float>(),-1f,1f);
    }

    private void RotateCamera()
    {
        cam.LookAt(center);

        rotator.Rotate(Vector3.down * camHorizontalDir * rotationSpeed * Time.deltaTime);
    }

    private void ZoomCamera()
    {
        cam.Translate(cam.forward * camZoomDir * zoomSpeed * Time.deltaTime, Space.World);
    }

    private void MoveCamera()
    {
        cam.Translate(Vector3.up * camVerticalDir * verticalSpeed * Time.deltaTime, Space.World);
    }
}