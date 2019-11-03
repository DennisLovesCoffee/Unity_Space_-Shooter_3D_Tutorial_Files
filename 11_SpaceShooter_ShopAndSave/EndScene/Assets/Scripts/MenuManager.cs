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

            //check if we own the saceship
            if (SaveManager.Instance.IsSpaceshipowned(currentIdx))
            {
                //disable the text element
                btn.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                //change the text element
                Text buttonText = btn.GetComponentInChildren<Text>();
                buttonText.text = GameManager.Instance.spaceshipPrices[currentIdx].ToString();

                //change image to gray
                button.image.color = Color.gray;
            }

            i++;
        }
    }

    private void OnShopButtonClicked(int idx)
    {
        if (SaveManager.Instance.IsSpaceshipowned(idx))
        {
            //own the spaceship
            GameManager.Instance.ChangeCurrentSpaceship(idx);
            UpdateSpaceshipPreview();
        }
        else
        {
            //check if we have enaught gold
            int constOfSpaceship = GameManager.Instance.spaceshipPrices[idx];
            int currentGold = SaveManager.Instance.GetGold();
            if(currentGold >= constOfSpaceship)
            {
                //buy it
                SaveManager.Instance.RemoveGold(constOfSpaceship);
                SaveManager.Instance.PurchaseSpaceship(idx);

                //update the button
                Transform clickedBtn = shopButtonsParent.GetChild(idx);
                clickedBtn.GetChild(0).gameObject.SetActive(false); //disabling the text
                Button buttonComponent = clickedBtn.GetComponent<Button>();
                buttonComponent.image.color = Color.white;

                //selcet the spaceship
                GameManager.Instance.ChangeCurrentSpaceship(idx);
                UpdateSpaceshipPreview();

                //update the gold text
                UpdateGoldText();
            }


        }
        
    }

    private void InitLevelButtons()
    {
        int lastLevelCompleted = SaveManager.Instance.GetLevelsCompleted();

        int i = 0;
        foreach(Transform t in levelContainer)
        {
            int currentIdx = i;
            Button button = t.GetComponent<Button>();
            if(currentIdx <= lastLevelCompleted)
            {
                //completed level
                button.onClick.AddListener(() => OnLevelSelect(currentIdx));
                button.image.color = Color.white;
            }
            else if(currentIdx == lastLevelCompleted + 1)
            {
                //the current level to be completed
                button.onClick.AddListener(() => OnLevelSelect(currentIdx));
                button.image.color = Color.green;
            }
            else
            {
                //not completed
                button.interactable = false;
                button.image.color = Color.gray;
            }
            
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
        GameManager.Instance.currentLevelIdx = idx;

        int levelIdx = idx + 1;
        string sceneName = "Level" + levelIdx.ToString();
        SceneManager.LoadScene(sceneName);
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
