using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PictureSpawner : MonoBehaviour
{

    [SerializeField]
    float SpawnMinWaitTime;

    [SerializeField]
    float SpawnMaxWaitTime;

    [SerializeField]
    float MinGravity;

    [SerializeField]
    float MaxGravity;

    [SerializeField]
    int NumberOfPicturesThatSpawnEachTime;

    [HideInInspector]
    public bool bAllPicturesWentDown;

    [HideInInspector]
    public bool bStopSpawn;

    public List<SpriteRenderer> spriteRenderer;

    GameMode gameMode;

    List<SpriteRenderer> PicturesThatSpawnEachLevel;

    int PictureNumber;



    void Awake()
    {
        PicturesThatSpawnEachLevel = new List<SpriteRenderer>();

        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();

        bAllPicturesWentDown = false;

        PictureNumber = 0;

        bStopSpawn = false;
    }



    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(Random.Range(SpawnMinWaitTime, SpawnMaxWaitTime));

        Spawn();
    }





    public void Spawn(bool bNewLevelSpawn = false)
    {
        if (bNewLevelSpawn)
        {
            PictureNumber = 0;
            bAllPicturesWentDown = false;
            ChoosePicturesToSpawnEachLevel();
        }

        PictureNumber++;

        if (bStopSpawn)
        {
            return;
        }

        if (PictureNumber > NumberOfPicturesThatSpawnEachTime)
        {
            StartCoroutine(WaitForLastPictureGoesDown());
            PictureNumber = 0;
            return;
        }


        Instantiate(PicturesThatSpawnEachLevel[PictureNumber - 1], new Vector3(Random.Range(-1.29f, 1.29f), 8f, -1f), Quaternion.identity);

        PicturesThatSpawnEachLevel[PictureNumber - 1].GetComponent<Rigidbody2D>().gravityScale = Random.Range(MinGravity, MaxGravity);

        StartCoroutine(SpawnWait());

    }


    void ChoosePicturesToSpawnEachLevel()
    {

        PicturesThatSpawnEachLevel.Clear();

        int KeyIndex = Random.Range(0, NumberOfPicturesThatSpawnEachTime); // KeyIndex In PicturesThatSpawnEachLevel


        for (int i = 0; i < NumberOfPicturesThatSpawnEachTime; i++)
        {
            int RandomNumberWithoutReplacement = Random.Range(0, spriteRenderer.Count);

            while (PicturesThatSpawnEachLevel.Contains(spriteRenderer[RandomNumberWithoutReplacement]) || RandomNumberWithoutReplacement == KeyIndex)
                RandomNumberWithoutReplacement = Random.Range(0, spriteRenderer.Count);

            if (i == KeyIndex)
                PicturesThatSpawnEachLevel.Add(FindSpriteKey());

            else
                PicturesThatSpawnEachLevel.Add(spriteRenderer[RandomNumberWithoutReplacement]);
        }

    }



    SpriteRenderer FindSpriteKey()
    {
        int SpriteKeyIndex = 0;

        for (int i = 0; i < spriteRenderer.Count; i++)     
            if (gameMode.GuessKey[gameMode.LevelNumber - 1] == spriteRenderer[i].tag)
            {
                SpriteKeyIndex = i ;
                break;
            }

        return spriteRenderer[SpriteKeyIndex];
    }


    IEnumerator WaitForLastPictureGoesDown()
    {
        yield return new WaitForSeconds(1f);
        bAllPicturesWentDown = true;
    }

}
