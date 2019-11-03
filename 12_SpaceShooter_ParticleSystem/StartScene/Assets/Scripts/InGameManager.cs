using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public Image healthBarFill;
    public float healthBarChangeTime = 0.5f;

    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject levelCompleteMenu;

    public PlayerManager playerManager;

    public Text asteroidKillText;

    public void ChangeHealthbar(int maxHeath, int currentHealth)
    {
        if (currentHealth < 0)
            return;

        if(currentHealth == 0)
        {
            Invoke("OpenDeathMenu", healthBarChangeTime);
        }

        float healthPct = currentHealth / (float)maxHeath;
        StartCoroutine(SmootheHealthbarChange(healthPct));
    }

    private IEnumerator SmootheHealthbarChange(float newFillAmt)
    {
        float elapsed = 0f;
        float oldFillAmt = healthBarFill.fillAmount;
        while(elapsed <= healthBarChangeTime)
        {
            elapsed += Time.deltaTime;
            float currentFillAmt = Mathf.Lerp(oldFillAmt, newFillAmt, elapsed / healthBarChangeTime);
            healthBarFill.fillAmount = currentFillAmt;
            yield return null;
        }
    }


    public void OnFireButtonClicked()
    {
        playerManager.FireRockets();
    }

    public void OnMenuBtnClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit App");
        //save game

        Application.Quit();
    }

    public void OnPauseBtnClicked()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void OnContinueBtnClicked()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void OnRestartBtnClicked()
    {
        //save game

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OpenDeathMenu()
    {
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
    }

    public void ChangeAsteroidKillCount(int toKill)
    {
        asteroidKillText.text = toKill.ToString();
    }

    public void OpenLevelCompleteMenu()
    {
        Time.timeScale = 0f;
        levelCompleteMenu.SetActive(true);
    }



}
