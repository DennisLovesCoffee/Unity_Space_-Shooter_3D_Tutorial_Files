using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public GameObject[] spaceshipPrefabs;
    public Texture2D[] spaceshipTextures;

    public static GameManager Instance;

    public GameObject currentSpaceship;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        spaceshipTextures = new Texture2D[spaceshipPrefabs.Length];
        for(int i = 0; i < spaceshipPrefabs.Length; ++i)
        {
            GameObject prefab = spaceshipPrefabs[i];
            Texture2D texture = AssetPreview.GetAssetPreview(prefab);
            spaceshipTextures[i] = texture;
        }

        currentSpaceship = spaceshipPrefabs[0];

    }

    public void ChangeCurrentSpaceship(int idx)
    {
        currentSpaceship = spaceshipPrefabs[idx];
    }
}
