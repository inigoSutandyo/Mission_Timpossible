using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class RecoilWeapon : MonoBehaviour
{
    [HideInInspector] public Cinemachine.CinemachineFreeLook freeLook;
    [HideInInspector] public Cinemachine.CinemachineImpulseSource cameraImpulse;
    public float verticalRecoil;
    public float duration;

    private float time;

    private void Awake()
    {
        cameraImpulse = GetComponent<CinemachineImpulseSource>();
    }

    public void GenerateRecoil()
    {
        time = duration;

        cameraImpulse.GenerateImpulse(Camera.main.transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            freeLook.m_YAxis.Value -= (verticalRecoil * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
