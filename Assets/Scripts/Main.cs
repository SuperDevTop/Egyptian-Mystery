using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private Button[] levelBtns;

    void Start()
    {
        if (PlayerPrefs.GetInt("LEVEL_PASSED") == 0)
        {
            PlayerPrefs.SetInt("LEVEL_PASSED", 1);
        }

        for (int i = 0; i < PlayerPrefs.GetInt("LEVEL_PASSED"); i++)
        {
            levelBtns[i].interactable = true;
        }
    }

    void Update()
    {

    }

    public void LevelSelect(string levelStr)
    {
        SceneManager.LoadScene("Level" + levelStr);
    }
}
