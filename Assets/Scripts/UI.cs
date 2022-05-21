using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField]
    Text LeftUp;

    [SerializeField]
    Text Center;

    [SerializeField]
    Button PlayButton;

    [SerializeField]
    Button ExitButton;

    public Button HomeButton1;
    public Button HomeButton2;



    void Start()
    {
        MenuUIBeforeGameStart();
    }




    public void ChangeUIWord(string NewWord, string TextPosition, Color color , int Size)
    {
        if (!Center && !LeftUp)
            return;

        
        if (nameof(Center) == TextPosition)
        {

            Center.text = NewWord;
            Center.color = color;
            Center.fontSize = Size;
        }


        if (TextPosition == nameof(LeftUp))
        {
            LeftUp.text = NewWord;
            LeftUp.color = color;
            LeftUp.fontSize = Size;
        }

    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public void StartGame()
    {
        Time.timeScale = 1f;
        PlayButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
        LeftUp.gameObject.SetActive(true);
        HomeButton2.gameObject.SetActive(true);
        Center.gameObject.SetActive(true);


    }

    public void RestartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }


    void MenuUIBeforeGameStart()
    {

        LeftUp.color = Color.green;

        HomeButton1.gameObject.SetActive(false);
        HomeButton2.gameObject.SetActive(false);

        Time.timeScale = 0f;
        LeftUp.gameObject.SetActive(false);
        Center.gameObject.SetActive(false);
    }

}

