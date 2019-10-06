using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketSpawnPoint1, rocketSpawnPoint2;

    public float fireInterval = 2f;
    private bool canFire = true;

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

    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
