using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] CameraController cameraController;
    PlayerControls playerControls;
    bool paused;

    void Awake()
    {
        playerControls = new PlayerControls();
        pausePanel.SetActive(false);
    }

    void OnEnable()
    {
        playerControls.UI.Enable();
        playerControls.UI.Pause.performed += OnPressPause;
    }

    void OnPressPause(InputAction.CallbackContext context)
    {
        if (paused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        cameraController.sensitivity = 0f;
        pausePanel.SetActive(true);
    }

    void Continue()
    {
        paused = false;
        Time.timeScale = 1f;
        Cursor.visible = false;
        cameraController.sensitivity = 0.1f;
        pausePanel.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
