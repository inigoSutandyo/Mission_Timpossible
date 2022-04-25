using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 15f;

    private Camera mainCam;
    //[SerializeField] Cinemachine.CinemachineFreeLook freeLook;

    public PauseManager pause;


    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pause != null && pause.isPause)
        {
            return;
        } else
        {
            float yawCamera = mainCam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
        }

        
    }

}
