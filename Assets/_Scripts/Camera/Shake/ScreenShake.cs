using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    private float _startingIntensity;
    private float _shakeTime;
    private float _shakeTimeTotal;



    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    public void ShakeCamera(float intensity, float time)
    {

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        _startingIntensity = intensity;
        _shakeTime = time;
        _shakeTimeTotal = time;
    }

    private void StopShake()
    {

        if (_shakeTime > 0f)
        {
            _shakeTime -= Time.deltaTime;

            // _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTime / _shakeTimeTotal));

        }

    }


    private IEnumerator ScreenShaker(float intensity, float time) 
    {

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        _startingIntensity = intensity;
        _shakeTime = time;
        _shakeTimeTotal = time;

        while (_shakeTime > 0f)
        {
            _shakeTime -= Time.deltaTime;

            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - (_shakeTime / _shakeTimeTotal));

            yield return new WaitForFixedUpdate();

        }


    }

}
