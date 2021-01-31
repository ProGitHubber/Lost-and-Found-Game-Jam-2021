using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public CharacterController controller;

    public float moveSpeed = 6;

    bool sprinting;

    public float mouseSensitivity = 100f;

    float xRotation = 0;

    public GameObject clickUI;

    GameManager gm;

    HoozitLogic lastHoozit;
    float resetTimer;

    public Animator anim;

    bool paused;
    public GameObject pauseMenu;
    public float pauseTimeScale = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        Cursor.lockState = CursorLockMode.Locked;
        anim = GetComponent<Animator>();
    }

    void Pause()
    {
        paused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = pauseTimeScale;
    }

    void UnPause()
    {
        paused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
                Pause();
            else
                UnPause();
        }
        if (paused)
        {
            return;
        }
        sprinting = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprinting = true;
        }
        //Mouse Looking
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * ((sprinting) ? moveSpeed * 2: moveSpeed) * Time.deltaTime);
        bool clickUIEnabled = false;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out RaycastHit hit))
        {
            HoozitLogic hl = hit.collider.GetComponentInParent<HoozitLogic>();
            if (hl && gm.gameStarted && gm.timerStarted)
            {
                //enable the press overlay
                lastHoozit = hl;
                resetTimer = 0.2f;   
            }

        }
        if (lastHoozit)
        {
            clickUIEnabled = true;
            clickUI.transform.position = Camera.main.WorldToScreenPoint(lastHoozit.transform.position);
            if (Input.GetButtonDown("Fire1") && gm.gameStarted && gm.timerStarted)
            {
                
                lastHoozit.Spawn();
                lastHoozit = null;
                gm.AddScore();
            }
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
            {
                lastHoozit = null;
            }
        }

        anim.SetFloat("speed", ((sprinting) ? ((x>z) ? x: z)*2 : ((x > z) ? x : z)));

        clickUI.SetActive(clickUIEnabled);
        
        
    }

    public void SetMouseSensitivity(float newSens)
    {
        mouseSensitivity = newSens;
    }
}
