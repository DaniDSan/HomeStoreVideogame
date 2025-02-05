using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public Controls c;

    public static ControlsManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        c = new Controls();
        c.Enable();
    }

    public void ChangeControls()
    {
        Debug.Log("Implementar cambio de controles");
    }
}