using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestarGame(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
}
