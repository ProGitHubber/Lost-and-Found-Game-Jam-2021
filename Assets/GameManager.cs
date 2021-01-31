using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform StartingPos;
    public bool gameStarted;
    public Text dynaText;
    public Text scoreTxt, timeTxt;
    public GameObject PlayerUI;
    public GameObject introUI;
    public int score;
    public float roundTime;
    float roundTimer;
    public bool timerStarted;
    public Animator anim;
    public Animator ScoreAnim;
    public GameObject timesUp;

    public AudioSource gameOverSource;
    public AudioSource gameStartAudioSource;
    public AudioSource AmbientMusic;
    public float volumeDuringCutscene;
    public float normalVolume;

    float startDelay = 3f;
    float delayTimer;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        PlayerUI.SetActive(false);
        introUI.SetActive(true);
        //move player into starting position

        //press anything to begin (displayed)
        dynaText.text = "Hoozit" +"\n" +"Hide & Seek";
    }


    void StartGame()
    {
        timesUp.SetActive(false);
        dynaText.gameObject.SetActive(false);
        //initiate cutscene
        gameStartAudioSource.Play();
        AmbientMusic.volume = volumeDuringCutscene;
        anim.SetTrigger("StartCutscene");
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted)
        {
            delayTimer -= Time.deltaTime;
            if (Input.GetButtonDown("Fire1"))
            {
                
                if (delayTimer <= 0)
                {
                    gameStarted = true;
                    StartGame();
                }
            }
        }
        else if (timerStarted)
        {
            roundTimer -= Time.deltaTime;
            if (roundTimer <= 2 && !gameOverSource.isPlaying)
            {
                gameOverSource.Play();
            }
            if (roundTimer < 0)
            {
                TimesUp();
            }
        }

        scoreTxt.text = score.ToString("00");
        timeTxt.text = roundTimer.ToString("00") + "s";
    }

    void StartTimer()
    {
        //give player controls
        introUI.SetActive(false);
        PlayerUI.SetActive(true);
        //timer = maximum time
        timerStarted = true;
        roundTimer = roundTime;
        //score = nothing
        score = 0;
        AmbientMusic.volume = normalVolume;
    }

    public void AddScore()
    {
        if (gameStarted && timerStarted)
        {
            score++;
            ScoreAnim.SetTrigger("ScoreUp");
        }
    }

    public void SetVolume(float newVolume)
    {
        
        normalVolume = Mathf.Lerp(0, 0.75f, newVolume);
        volumeDuringCutscene = Mathf.Lerp(0, 0.5f, newVolume);
        if (gameStarted && !timerStarted)
        {
            AmbientMusic.volume = volumeDuringCutscene;
        }
        else
        {
            AmbientMusic.volume = normalVolume;
        }
    }
    void TimesUp()
    {
        timesUp.SetActive(true);
        timerStarted = false;
        roundTimer = 0;
        gameStarted = false;
        delayTimer = startDelay;
        //move player into starting position

        //you found X hoozits (displayed)
        PlayerUI.SetActive(false);
        introUI.SetActive(true);
        dynaText.gameObject.SetActive(true);
        dynaText.text = "NOT BAD! WE FOUND <b>" + score.ToString() + "</b> HOOZITS!";
        //disable player movement
        
    }
    public void Quit()
    {
        Application.Quit();
    }
}
