using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material[] _skyboxes;
    [SerializeField] private Cubemap[] _reflectionCubemaps;
    [SerializeField] private GameObject[] _lights;
    [SerializeField] private GameObject _instructions;
    [SerializeField] private Text _timescaleText;

    private int _skyboxIndex = 0;

    private void Awake()
    {
        UpdateUI();
        SetCursorVisible(false);
        if (PlayerPrefs.HasKey("Skybox") == false)
            PlayerPrefs.SetInt("Skybox", 0);
        LoadSkybox(PlayerPrefs.GetInt("Skybox"));
    }

    private void Update()
    {
        HandleSkybox();
        HandleTimescale();

        if (Input.GetKeyDown(KeyCode.H))
        {
            _instructions.SetActive(!_instructions.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ReloadScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.anyKey)
        {
            UpdateUI();
        }
    }

    private void HandleSkybox()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _skyboxIndex = (_skyboxIndex + 1) % _skyboxes.Length;
            PlayerPrefs.SetInt("Skybox", _skyboxIndex);
            LoadSkybox(_skyboxIndex);
        }
    }

    private void HandleTimescale()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateUI();
            Time.timeScale = 1f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateUI();
            Time.timeScale = 1.5f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateUI();
            Time.timeScale = 2f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UpdateUI();
            Time.timeScale = 3f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UpdateUI();
            Time.timeScale = 5f;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UpdateUI();
            Time.timeScale = 0.5f;
        }
    }

    private void UpdateUI()
    {
        // update time scale text
        if (Time.timeScale != 1f)
            _timescaleText.text = "Time scale: " + Time.timeScale.ToString() + "x";
        else _timescaleText.text = "";
    }

    private void LoadSkybox(int skyboxIndex)
    {
        _skyboxIndex = skyboxIndex;
        RenderSettings.skybox = _skyboxes[_skyboxIndex];
        RenderSettings.customReflectionTexture = _reflectionCubemaps[_skyboxIndex];
        foreach (var light in _lights)
            light.SetActive(false);
        _lights[_skyboxIndex].SetActive(true);
        DynamicGI.UpdateEnvironment();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetCursorVisible(bool visible)
    {
        if (visible == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
