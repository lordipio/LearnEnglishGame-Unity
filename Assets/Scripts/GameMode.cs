using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    private class PlayerData
    {
        public string Data1;
        public int Data2;
        public int Data3;
        public PlayerData(string Data1, int Data2, int Data3)
        {
            this.Data1 = Data1;
            this.Data2 = Data2;
            this.Data3 = Data3;
        }
    }



    [SerializeField]
    float DelayBetweenLevels;

    [SerializeField]
    int MaxNumberOfLevels;

    [SerializeField]
    float EndGameWaitTime;

    [HideInInspector]
    public int LevelNumber;

    ClickOnPicture ClickedPicture;

    UI uI;

    PictureSpawner pictureSpawner;

    public string[] GuessKey;

    bool ExecuteLoopOnce;

    int PlayerRightChoice;



    private void Awake()
    {
        pictureSpawner = GameObject.FindGameObjectWithTag("PictureSpawner").GetComponent<PictureSpawner>();
        ClickedPicture = GetComponent<ClickOnPicture>();
        uI = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();

        LevelNumber = 1;
        PlayerRightChoice = 0;

        ExecuteLoopOnce = true;
 
    }



    void FixedUpdate()
    {
        PlayerChoosePicture();
    }



    void Start()
    {
        LoadFirstLevel();

    }



    void PlayerChoosePicture()
    {

        if (!ClickedPicture && !uI)
            return;

        if (LevelNumber > MaxNumberOfLevels)
            return;


        if (ClickedPicture.ClickedPictureTag() == GuessKey[LevelNumber - 1] && LevelNumber <= MaxNumberOfLevels && ExecuteLoopOnce) // If Player Choose Right Pic
        {
            uI.ChangeUIWord("Right!", "Center", Color.green, 100);
            PlayerRightChoice++;
            LoadNextLevel();
            return;

        }

        if (ClickedPicture.ClickedPictureTag() != GuessKey[LevelNumber - 1] && ClickedPicture.ClickedPictureTag() != "NotFound" && LevelNumber <= MaxNumberOfLevels && ExecuteLoopOnce) // If Player Choose Wrong Pic
        {
            print("Wrong" + ClickedPicture.ClickedPictureTag());
            uI.ChangeUIWord("Wrong!", "Center", Color.red, 100);
            LoadNextLevel();
            return;

        }

        if (pictureSpawner.bAllPicturesWentDown && LevelNumber <= MaxNumberOfLevels && ExecuteLoopOnce) // If Player Choose Nothing
        {
            uI.ChangeUIWord("Too Late!", "Center", Color.red, 100);
            LoadNextLevel();
            return;

        }

    }



    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(EndGameWaitTime);

        pictureSpawner.bStopSpawn = true;
        ExecuteLoopOnce = false;
        Time.timeScale = 0f;

        if (PlayerRightChoice >= 4)
            uI.ChangeUIWord("Right Answer: " + PlayerRightChoice + "\nAwsome", "Center", Color.green, 70);
        
        if (PlayerRightChoice == 3)
            uI.ChangeUIWord("Right Answer: " + PlayerRightChoice + "\nGood Job", "Center", Color.yellow, 70);

        if (PlayerRightChoice <= 2)
            uI.ChangeUIWord("Right Answer: " + PlayerRightChoice + "\nTerrible", "Center", Color.red, 70);

        uI.HomeButton1.gameObject.SetActive(true);
        uI.HomeButton2.gameObject.SetActive(false);
        uI.ChangeUIWord("", "LeftUp", Color.green, 60);
        StoreData();
    }




    void LoadNextLevel()
    {
        pictureSpawner.bStopSpawn = true;
        LevelNumber++;

        if (LevelNumber > MaxNumberOfLevels)
        {
            StartCoroutine(EndGame());
            return;
        }

        ExecuteLoopOnce = false;

        StartCoroutine(ShowNewLevelWord());
        StartCoroutine(WaitBetweenLevels());
    }



    IEnumerator WaitBetweenLevels()
    {
        yield return new WaitForSeconds(DelayBetweenLevels);

        MakeReadyForNextLevel();

    }



    IEnumerator ShowNewLevelWord()
    {
        yield return new WaitForSeconds(DelayBetweenLevels / 2);

        uI.ChangeUIWord("Level " + LevelNumber, "LeftUp", Color.green, 60);
        uI.ChangeUIWord(GuessKey[LevelNumber - 1], "Center", Color.white, 130);

    }



    void StoreData()
    {
        PlayerData playerData = new PlayerData(PlayerRightChoice.ToString(), PlayerRightChoice, PlayerRightChoice);
        string json = JsonUtility.ToJson(playerData);

    }



    void LoadFirstLevel()
    {
        uI.ChangeUIWord("Level " + LevelNumber, "LeftUp", Color.green, 60);
        uI.ChangeUIWord(GuessKey[LevelNumber - 1], "Center", Color.white, 130);
        StartCoroutine(WaitBetweenLevels());

    }



    IEnumerator WaitForFirstLevel()
    {
        yield return new WaitForSeconds(DelayBetweenLevels / 2);

        MakeReadyForNextLevel();
    }



    void MakeReadyForNextLevel()
    {
        uI.ChangeUIWord("", "Center", Color.green, 120);
        ExecuteLoopOnce = true;
        pictureSpawner.bStopSpawn = false;
        pictureSpawner.Spawn(true);

    }

}