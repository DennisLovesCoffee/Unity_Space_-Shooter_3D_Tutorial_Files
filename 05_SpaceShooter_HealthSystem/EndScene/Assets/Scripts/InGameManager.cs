using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketSpawnPoint1, rocketSpawnPoint2;

    public float fireInterval = 2f;
    private bool canFire = true;

    public Image healthBarFill;
    public float healthBarChangeTime = 0.5f;

    public GameObject pauseMenu;
    public GameObject deathMenu;

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
        if (canFire)
        {
            //fire rocktes
            FireRockets();

            canFire = false;

            StartCoroutine(ReloadDelay());
        }
    }

    private void FireRockets()
    {
        Instantiate(rocketPrefab, rocketSpawnPoint1.position, Quaternion.identity);
        Instantiate(rocketPrefab, rocketSpawnPoint2.position, Quaternion.identity);
    }

    private IEnumerator ReloadDelay()
    {
        //play reload sound 

        yield return new WaitForSeconds(fireInterval);

        canFire = true;
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





}
