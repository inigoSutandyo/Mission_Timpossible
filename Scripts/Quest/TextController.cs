using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(playerPosition);
        Vector3 targetAngle = gameObject.transform.eulerAngles + 180f * Vector3.up;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngle, 1);
    }
}
