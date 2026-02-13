using UnityEngine;
using UnityEngine.UI;

public class ShipControls : MonoBehaviour
{
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Text _speedText;
    private float _vertical;
    private float _horizontal;

    void Start()
    {
        _currentSpeed = 1;
        _speedText.text = "Speed: " + _currentSpeed.ToString("0.0");
    }

    void Update()
    {
        ShipMovement();
    }

    private void ShipMovement()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.E)) // Hold to increase speed
        {
            _currentSpeed += 2.5f * Time.deltaTime;
            if (_currentSpeed > _maxSpeed)
            {
                _currentSpeed = _maxSpeed;
            }
            _speedText.text = "Speed: " + _currentSpeed.ToString("0.0");
        }

        if (Input.GetKey(KeyCode.Q)) // Hold to decrease speed
        {
            _currentSpeed -= 2f * Time.deltaTime;
            if (_currentSpeed < 1)
            {
                _currentSpeed = 1;
            }
            _speedText.text = "Speed: " + _currentSpeed.ToString("0.0");
        }

        // Apply rotation
        Vector3 rotateH = new Vector3(0, _horizontal, 0);
        transform.Rotate(rotateH * _rotSpeed * Time.deltaTime);

        Vector3 rotateV = new Vector3(_vertical, 0, 0);
        transform.Rotate(rotateV * _rotSpeed * Time.deltaTime);

        transform.Rotate(new Vector3(0, 0, -_horizontal * 0.2f), Space.Self);

        // Apply movement
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }
}
