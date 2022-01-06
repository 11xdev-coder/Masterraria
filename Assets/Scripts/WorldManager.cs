using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public GameObject generatingText;

    public void Awake()
    {
        generatingText.SetActive(false);
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
