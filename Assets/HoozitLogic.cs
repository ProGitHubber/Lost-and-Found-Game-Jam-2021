using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoozitLogic : MonoBehaviour
{
    public SpriteRenderer colour;
    public Color targetColor;
    public float timeUntilNewColor = 1;
    float newColorTimer;
    public float transitionSpeed = 10;
    playerController pc;
    GameObject[] spawnPoints;
    public GameObject[] cosmetics;
    AudioSource audioSource;
    public AudioSource feedbackSource;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pc = FindObjectOfType<playerController>();
        spawnPoints = GameObject.FindGameObjectsWithTag("HidingPlace");
        Spawn();
    }
    public void Spawn()
    {
        //play a sound
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        feedbackSource.Play();
        //choose a random spawnpoint
        GameObject spawnToCheck = spawnPoints[Random.Range(0, spawnPoints.Length)];
        //cosmetics
        foreach (GameObject cos in cosmetics)
        {
            cos.SetActive(false);
        }
        //number of cosmetics to add

        for (int cosNum = Random.Range(0, cosmetics.Length); cosNum > 0; cosNum--)
        {
            cosmetics[Random.Range(0, cosmetics.Length)].SetActive(true);
        }
        //spawn
        transform.position = spawnToCheck.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(pc.transform.position);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        //pick a colour after x time;
        if (newColorTimer <= 0)
        {
            targetColor = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
            newColorTimer = timeUntilNewColor;
        }
        else
        {
            newColorTimer -= Time.deltaTime;
        }
        //lerp to that colour;
        colour.color = Color.Lerp (colour.color, targetColor, transitionSpeed * Time.deltaTime);


    }

    public void setVolume(float newVolume)
    {
        audioSource.volume = newVolume;
        feedbackSource.volume = newVolume;
        audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        feedbackSource.Play();
    }
}
