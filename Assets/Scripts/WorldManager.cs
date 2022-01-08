using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public GameObject generatingText;
    public Inventory inventory;
    public string assetsDataPath;
    public string directory;

    public void Awake()
    {
        generatingText.SetActive(false);
        assetsDataPath = Application.dataPath;
        directory = assetsDataPath + "/saves/world1/inventory.txt";
    }
    
    public void CreateWorld1()
    {
        SceneManager.LoadScene("World1");
        generatingText.SetActive(true);
        
        //GameObject.Find("TerrainGenerator1").GetComponent<TerrainGeneration>().Generate();
    }
    
    public void CreateWorld2()
    {
        SceneManager.LoadScene("World2");
        generatingText.SetActive(true);
        //GameObject.Find("TerrainGenerator1").GetComponent<TerrainGeneration>().Generate();
    }
    
    public void CreateWorld3()
    {
        SceneManager.LoadScene("World3");
        generatingText.SetActive(true);
        //GameObject.Find("TerrainGenerator1").GetComponent<TerrainGeneration>().Generate();
    }
}
