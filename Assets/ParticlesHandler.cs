using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour {

    ParticleSystem particle;

    public static ParticlesHandler Instance;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }

        particle = GetComponent<ParticleSystem>();
    }

    public void PlayEffect(GameObject objectToPlace) {
        if(particle == null || objectToPlace == null) return;

        // Obtener el tamaño del objeto
        Vector3 objectSize = objectToPlace.GetComponentInChildren<Renderer>().bounds.size;
        float maxSize = Mathf.Max(objectSize.x, objectSize.y, objectSize.z);

        // Ajustar el tamaño del sistema de partículas
        var shapeModule = particle.shape;
        shapeModule.radius = maxSize * 0.5f;

        particle.transform.position = objectToPlace.transform.position;
        particle.Play();
    }
}
