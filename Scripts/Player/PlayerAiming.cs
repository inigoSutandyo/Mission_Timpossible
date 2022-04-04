using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 15f;
    private Camera mainCam;

    

    private PlayerStatus playerStatus;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //weapon = GetComponentInChildren<RaycastWeapon>();
        playerStatus = GetComponent<PlayerStatus>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float yawCamera = mainCam.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        
    }
}
