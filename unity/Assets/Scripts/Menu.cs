using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject resumeButton;
    public GameObject playButton;
    public GameObject restartButton;

    public GameObject overlay;
    public GameObject RoundOverOverlay;
    bool hasStarted, paused, gameOver;

    public float roundTime;
    private float currentTimeLeft;
    public TextMeshPro timeLeftInGame;

    public float timeoutThreshold;
    private float timeoutCounter;
    bool fadingTimeout;
    public CanvasGroup canvas;

    public RectTransform phone, endGameTarget;

    private void Start()
    {
        hasStarted = false;
        paused = true;
        gameOver = false;
        currentTimeLeft = roundTime;
    }
    void Update()
    {
        if (!paused)
        {
            if (currentTimeLeft > 0)
            {
                currentTimeLeft -= Time.deltaTime;
                timeLeftInGame.text = "" + Mathf.Floor(currentTimeLeft) + "s";
            }
            else
            {
                currentTimeLeft = 0;
                timeLeftInGame.text = "0s";
                gameOver = true;
                RoundOverOverlay.SetActive(true);
                phone.position = endGameTarget.position;
            }
        }
        if (Input.GetButtonDown("Escape"))
        {
            overlay.SetActive(!overlay.activeSelf);
            paused = overlay.activeSelf;
        }
        if (!hasStarted || paused || gameOver)
        {
            timeoutCounter += Time.deltaTime;
            if(timeoutCounter > timeoutThreshold && !fadingTimeout)
            {
                fadingTimeout = true;
                StartCoroutine(FadeOverlay(0));
            }
            if(Input.anyKeyDown)
            {
                StopAllCoroutines();
                fadingTimeout = false;
                timeoutCounter = 0;
                canvas.alpha = 1;
            }
        }
    }
    IEnumerator FadeOverlay(float target)
    {
        while(canvas.alpha != target)
        {
            canvas.alpha = Mathf.MoveTowards(canvas.alpha, target, 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }
    public void PlayGame()
    {
        //change play button text to 'restart'
        //change boolean to true
        //if true already then restart the scene
        if (hasStarted) { SceneManager.LoadScene(0);  }
        hasStarted = true;
        playButton.SetActive(false);
        resumeButton.SetActive(true);
        restartButton.SetActive(true);
        //hide menu       
        overlay.SetActive(false);
        paused = false;
    }

    public void Resume()
    {
        overlay.SetActive(false);
        paused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
