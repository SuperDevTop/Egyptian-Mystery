using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameEngine : MonoBehaviour
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject failedUI;
    [SerializeField] private GameObject[] heart;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image[] panelImages;
    [SerializeField] private Image[] originalImages;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] levelSprites;
    [SerializeField] private Text alertText;
    [SerializeField] private int currentLevelIndex;
    
    // Indices for game engine
    public int gameStatus;
    private int originIndex;
    private int firstOriginIndex;
    private int panelIndex;
    private int matchedCount;
    private int failedCount;
    List<int> spritesToSort = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    void Start()
    {                
        StartCoroutine(DelayToStart());

        // Initialize parameters
        matchedCount = 0;
        failedCount = 0;
        originIndex = 100;
    }

    void Update()
    {
        if (gameStatus == 1)
        {     
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;                

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "origin")
                    {
                        originIndex = int.Parse(hit.transform.gameObject.name.Split(" ")[1]);
                        firstOriginIndex = int.Parse(hit.transform.gameObject.name.Split(" ")[0]);              
                    }
                }
            }
 
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "panel")
                    {
                        panelIndex = int.Parse(hit.transform.gameObject.name.Split(" ")[1]);                        

                        if(originIndex != 100)
                        {
                            if (originIndex == panelIndex)
                            {
                                panelImages[panelIndex - 1].sprite = levelSprites[originIndex - 1];
                                matchedCount++;

                                originalImages[firstOriginIndex].GetComponent<Collider>().enabled = false;
                                Color originColor = Color.white;
                                originColor.a = 0.5f;
                                originalImages[firstOriginIndex].GetComponent<Image>().color = originColor;

                                // Pass level
                                if (matchedCount == 9)
                                {
                                    gameStatus = 0;
                                    winUI.SetActive(true);
                                }
                            }
                            else
                            {     
                                // If users lose 3 lives, they will fail
                                if(failedCount == 3)
                                {
                                    failedUI.SetActive(true);
                                    gameStatus = 0;
                                }
                                else
                                {
                                    StartCoroutine(DelayToShowAlertText("Wrong!"));
                                    heart[failedCount].SetActive(false);
                                    failedCount++;
                                }                                
                            }
                        }                                                                        
                    }

                    originIndex = 100;
                }
            }
        }
    }

    public void RetryBtnClick()
    {
        SceneManager.LoadScene("Level" + currentLevelIndex);
    }

    public void NextBtnClick()
    {
        SceneManager.LoadScene("Level" + (currentLevelIndex + 1));
    }

    IEnumerator DelayToStart()
    {
        gameStatus = 0;

        yield return new WaitForSeconds(5f);

        gameStatus = 1;

        for(int i = 0; i < panelImages.Length; i++)
        {
            panelImages[i].sprite = defaultSprite;
        }

        SortSpritesRandomly(spritesToSort);
    }

    IEnumerator DelayToShowAlertText(string str)
    {
        alertText.text = str;
        alertText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        alertText.gameObject.SetActive(false);
    }

    void SortSpritesRandomly(List<int> sprites)
    {
        System.Random random = new System.Random();
        List<int> sortedList = sprites.OrderBy(x => random.Next()).ToList();

        for(int i = 0; i < sortedList.Count; i++)
        {
            originalImages[i].sprite = levelSprites[sortedList[i]];
            originalImages[i].name = i + " " + (sortedList[i] + 1);
        }
    }
}
