using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{

    //public void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}
    
    public void CreateWorld()
    {
        SceneManager.LoadScene("World1");
        //GameObject.Find("TerrainGenerator1").GetComponent<TerrainGeneration>().Generate();
    }
}
