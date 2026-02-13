using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject _spaceshipCockpit;
    [SerializeField] private GameObject _spaceshipExt;
    [SerializeField] private GameObject[] _cinematicCameras;
    [SerializeField] private float _initialTime = 5;
    [SerializeField] private GameObject _ui;

    private float _timer;
    private bool _cockpitView = true;
    private bool _cinematicView = false;
    private int _lastSelectedCamera;

    private void Update()
    {
        // Switch between cockpit view and third person view
        if (Input.GetKeyDown(KeyCode.R) && _cinematicView == false)
        {
            if (_cockpitView == true)
            {
                _cockpitView = false;
                _spaceshipCockpit.SetActive(false);
                _spaceshipExt.SetActive(true);
            }
            else
            {
                _cockpitView = true;
                _spaceshipCockpit.SetActive(true);
                _spaceshipExt.SetActive(false);
            }
        }

        // Timer resets and Cinematic view stops when any key pressed or mouse moved except keys [1] to [6] and [Tab]
        bool mouseMovement = Mathf.Abs(Input.GetAxis("Mouse X")) > 0.01f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.01f;
        bool allowedKey = Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha3) ||
            Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Tab);

        if ((Input.anyKey && !allowedKey) || mouseMovement)
        {
            _timer = 0f;
            if (_cinematicView == true)
                StopCinematicView();
        }
        else
        {
            _timer += Time.deltaTime;
        }

        // Starts or changes Cinematic view
        if (_timer >= _initialTime)
        {
            if (_cinematicView == false)
            {
                StartCinematicView();
            }
            else
            {
                ResetCameraPriority();
                SelectRandomCamera();
                _timer = 0f;
            }
        }
    }

    private void StartCinematicView()
    {
        ResetCameraPriority();
        SelectRandomCamera();
        _cinematicView = true;
        _timer = 0f;
        _ui.SetActive(false);
        _spaceshipCockpit.SetActive(false);
        _spaceshipExt.SetActive(true);
    }

    private void StopCinematicView()
    {
        ResetCameraPriority();
        _cinematicView = false;
        _timer = 0f;
        _ui.SetActive(true);
        if (_cockpitView == true)
        {
            _spaceshipCockpit.SetActive(true);
            _spaceshipExt.SetActive(false);
        }
        else
        {
            _spaceshipCockpit.SetActive(false);
            _spaceshipExt.SetActive(true);
        }
    }

    private void SelectRandomCamera() // Selects random cinematic view
    {
        int _randomCamera = Random.Range(0, _cinematicCameras.Length);
        if (_lastSelectedCamera == _randomCamera)
        {
            _randomCamera = (_randomCamera + 1) % _cinematicCameras.Length;
        }
        if (_cinematicCameras[_randomCamera].GetComponent<CinemachineVirtualCamera>())
        {
            _cinematicCameras[_randomCamera].GetComponent<CinemachineVirtualCamera>().m_Priority = 15;
        }
        else if (_cinematicCameras[_randomCamera].GetComponent<CinemachineBlendListCamera>())
        {
            _cinematicCameras[_randomCamera].GetComponent<CinemachineBlendListCamera>().m_Priority = 15;
        }
        else if (_cinematicCameras[_randomCamera].GetComponent<CinemachineDollyCart>())
        {
            _cinematicCameras[_randomCamera].GetComponent<CinemachineDollyCart>().m_Position = 0f;
            _cinematicCameras[_randomCamera].GetComponentInChildren<CinemachineVirtualCamera>().m_Priority = 15;
        }
        _lastSelectedCamera = _randomCamera;
    }

    private void ResetCameraPriority()
    {
        foreach (var cam in _cinematicCameras)
        {
            if (cam.GetComponent<CinemachineVirtualCamera>())
            {
                cam.GetComponent<CinemachineVirtualCamera>().m_Priority = 0;
            }
            else if (cam.GetComponent<CinemachineBlendListCamera>())
            {
                cam.GetComponent<CinemachineBlendListCamera>().m_Priority = 0;
            }
            else if (cam.GetComponent<CinemachineDollyCart>())
            {
                cam.GetComponent<CinemachineDollyCart>().m_Position = 0f;
                cam.GetComponentInChildren<CinemachineVirtualCamera>().m_Priority = 0;
            }
        }
    }
}
