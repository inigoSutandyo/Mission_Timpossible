using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairTarget : MonoBehaviour
{
    Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = mainCamera.transform.position;
        ray.direction = mainCamera.transform.forward;
        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        } else
        {
            transform.position = ray.origin + ray.direction * 1000.0f;
        }
    }
}
