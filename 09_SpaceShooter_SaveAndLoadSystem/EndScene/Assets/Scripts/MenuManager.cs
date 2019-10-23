using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Transform levelContainer;
    public RectTransform menuContainer;
    public float transitionTime = 1f;
    private int screenWidth;

    public Transform shopButtonsParent;
    private GameObject currentSpaceshipPreview = null;
    public float rotationSpeed = 10f;

    public Text goldText;

    private void Start()
    {
        InitLevelButtons();
        screenWidth = Screen.width;
        InitShopButtons();
        UpdateSpaceshipPreview();
        UpdateGoldText();
    }

    private void Update()
    {
        if(currentSpaceshipPreview != null)
        {
            currentSpaceshipPreview.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    private void UpdateSpaceshipPreview()
    {
        if(currentSpaceshipPreview != null)
        {
            Destroy(currentSpaceshipPreview);
        }

        GameObject newSpaceshipPrefab = GameManager.Instance.currentSpaceship;
        Vector3 startRotationVector = new Vector3(0f, 180f, 0f);
        currentSpaceshipPreview = Instantiate(newSpaceshipPrefab,Vector3.zero, Quaternion.Euler(startRotationVector));
    }

    private void InitShopButtons()
    {
        int i = 0;
        foreach(Transform btn in shopButtonsParent)
        {
            int currentIdx = i;

            //create sprites
            Texture2D texture = GameManager.Instance.spaceshipTextures[currentIdx];
            Rect newRect = new Rect(0f, 0f, texture.width, texture.height);
            Sprite newSprite = Sprite.Create(texture, newRect, new Vector2(0.5f, 0.5f));
            btn.GetComponent<Image>().sprite = newSprite;

            //onclick event
            Button button = btn.GetComponent<Button>();
            button.onClick.AddListener(() => OnShopButtonClicked(currentIdx));

            i++;
        }
    }

    private void OnShopButtonClicked(int idx)
    {
        GameManager.Instance.ChangeCurrentSpaceship(idx);
        UpdateSpaceshipPreview();
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
        else if(menuType == MenuType.ShopMenu)
        {
            newPos = new Vector3(screenWidth, 0f, 0f);
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
        SceneManager.LoadScene("Level1");
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

    public void OnShopButtonClicked()
    {
        ChangeMenu(MenuType.ShopMenu);
    }

    private void UpdateGoldText()
    {
        goldText.text = SaveManager.Instance.GetGold().ToString();
    }

    private enum MenuType
    {
        MainMenu,
        Map1Menu,
        ShopMenu
    }

}
