using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public Transform levelContainer;
    public RectTransform menuContainer;
    public float transitionTime = 1f;
    private int screenWidth;
    

    private void Start()
    {
        InitLevelButtons();
        screenWidth = Screen.width;
    }

    private void InitLevelButtons()
    {
        int i = 0;
        foreach(Transform t in levelContainer)
        {
            int currentIdx = i;
            Button button = t.GetComponent<Button>();
            button.onClick.AddListener(() => OnLevelSelect(currentIdx));
            i++;
        }
    }

    private void ChangeMenu(MenuType menuType)
    {
        Vector3 newPos;
        if(menuType == MenuType.Map1Menu)
        {
            newPos = new Vector3(-screenWidth, 0f, 0f);
        }
        //defalut
        else
        {
            newPos = Vector3.zero;
        }

        StopAllCoroutines();
        StartCoroutine(ChageMenuAnimation(newPos));
    }

    private IEnumerator ChageMenuAnimation(Vector3 newPos)
    {
        float elapsed = 0f;
        Vector3 oldPos = menuContainer.anchoredPosition3D;

        while(elapsed <= transitionTime)
        {
            elapsed += Time.deltaTime;
            Vector3 currentPos = Vector3.Lerp(oldPos, newPos, elapsed / transitionTime);
            menuContainer.anchoredPosition3D = currentPos;
            yield return null;
        }
    }

    private void OnLevelSelect(int idx)
    {
        Debug.Log("We Press the level button " + idx);
    }
   
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play Button Clicked");
        ChangeMenu(MenuType.Map1Menu);
    }

    public void OnMainMenuButtonClicked()
    {
        Debug.Log("Clicked main button");
        ChangeMenu(MenuType.MainMenu);
    }

    public void OnNextMapButtonClicked()
    {
        Debug.Log("Next map clicked");
    }

    private enum MenuType
    {
        MainMenu,
        Map1Menu
    }

}
