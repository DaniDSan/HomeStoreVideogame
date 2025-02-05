using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenMovement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float zoomSpeed;

    public Vector3 zoomDir;

    [Header("CamReferences")]
    //Centro que corresponde al apartamento que se este visualizando.
    [SerializeField] private Transform center;

    //Rotador de la camara.
    [SerializeField] private Transform rotator;

    //Dirección a la que tiene que rotar la cámara.
    //1->Derecha.
    //0->Quieta.
    //-1->Izquierda.
    private float camDir;

    //Referencia a la camara.
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;

        zoomDir = center.position - cam.position;

        zoomDir.Normalize();

        //Initialise.
        rotator.position = new Vector3(center.position.x, cam.position.y, center.position.z);
    }

    private void Update()
    {
        camDir = ControlsManager.instance.c.CustomizeApartment.CamRotationDir.ReadValue<float>();

        cam.SetParent(rotator);
        
        cam.LookAt(center);

        RotateCamera();

        if (Input.GetKey(KeyCode.W))
        {
            cam.Translate(cam.forward * zoomSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey(KeyCode.S))
        {
            cam.Translate(-cam.forward * zoomSpeed * Time.deltaTime, Space.World);
        }
    }

    private void RotateCamera()
    {
        rotator.Rotate(Vector3.down * camDir * rotationSpeed * Time.deltaTime);
    }
}