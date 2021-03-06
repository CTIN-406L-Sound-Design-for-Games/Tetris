using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameMaster : MonoBehaviour
{
    [Header("Tetrimino")] public GameObject[] TetriminoPrefabs;
    public static int height = 20;
    public static int width = 10;
    public float speed = 1.0f;
    public float coolOffTime;
    [HideInInspector] public GameObject currentTetriminoFalling = null;
    [HideInInspector] public GameObject nextTetriminoFalling = null;
    public static GameObject[,] grid = new GameObject[width, height];

    [Header("UI Data Management")] public static int rows = 0;
    public static int score = 0;
    public static int level = 1;
    public int stage = 1;
    public int rowsStage1 = 10;
    public int rowsStage2 = 10;
    public int rowsStage3 = 10;
    private int rowsUpdate = 0;

    private int maxHeight = 0;
    private int stageRows = 0;

    [Header("UI Texts")] public Text hud_score;
    public Text hud_level;
    public Text hud_rows;
    public Text hud_stageRows;
    public Text hud_stage;

    [Header("Particle Effects")] public GameObject stage1Effect;
    public GameObject stage2Effect;
    public GameObject stage3Effect;


    [HideInInspector] public GameObject NextTetrimino;
    [HideInInspector] public GameObject PreviewTetrimino;
    [HideInInspector] public bool gameStarted = false;

    private float spaceTimer = 0.0f;
    private bool spacePressed = false;
    private bool stage2Played = false;
    private bool stage3Played = false;
    private void Awake()
    {
        GameInfoUpdate();
    }

    // Start is called before the first frame update
    void Start()
    {
        coolOffTime = 0.0f;
        AkSoundEngine.SetRTPCValue("score", GameMaster.score, GameObject.Find("WwiseGlobal"));
        Debug.Log("RTPC Value Score ");
        SoundManager.PlayStage1();
        //currentTetriminoFalling = SpawnNextTetrimino();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceTimer = Time.time;
            spacePressed = true;
            // Debug.Log("Space = " + spaceTimer.ToString());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceTimer = Time.time - spaceTimer;
            // Debug.Log("Space = " + spaceTimer.ToString());
        }
        if (coolOffTime < Time.time)
        {
            
            // instantiate a new tetrimino and move it down ...
            if (currentTetriminoFalling == null || currentTetriminoFalling.GetComponent<Tetrimino>().isActive == false)
            {
                if ((spacePressed && spaceTimer<=2.0f && spaceTimer>0.1f) || Input.GetKey(KeyCode.Space))
                {
                    SoundManager.PlayDrop();
                    spacePressed = false;
                }
                currentTetriminoFalling = SpawnNextTetrimino();
                currentTetriminoFalling.GetComponent<Tetrimino>().isActive = true;
                
            }
        }

        if (coolOffTime < Time.time)
        {
            if ((spacePressed && spaceTimer<=2.0f && spaceTimer>0.1f) || Input.GetKey(KeyCode.Space))
            {
                SoundManager.PlayDrop();
                spacePressed = false;
            }
            
            if (currentTetriminoFalling.GetComponent<Tetrimino>().isActive)
            {
                currentTetriminoFalling.transform.Translate(Vector3.down);
                
                

                if (!CheckIsValidPosition())
                {
                    currentTetriminoFalling.transform.Translate(Vector3.up);
                    currentTetriminoFalling.GetComponent<Tetrimino>().isActive = false;
                    if (CheckIsAboveGrid(currentTetriminoFalling))
                    {
                        GameOver();
                    }

                    GetComponent<Tetrimino>().UpdateGrid();
                    GetComponent<Tetrimino>().DeleteRow();
                    GameInfoUpdate();
                }
                else
                {
                }
            }

            coolOffTime = Time.time + speed;
            Speed();
            CheckMaximumHeightRemaining();
        }
        
    }

    GameObject SpawnNextTetrimino()
    {
        GameObject NextTetrimino = GameObject.Instantiate(
            TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
            new Vector3(5, 20, 0),
            Quaternion.identity) as GameObject;

        //Debug.Log(NextTetrimino.GetComponent<ITetrimino>().GetType());

        return NextTetrimino;
    }


    public bool CheckIsValidPosition()
    {
        foreach (GameObject cube in FindObjectOfType<Tetrimino>().cubes)
        {
            Vector2 v = Tetrimino.RoundVector(cube.transform.position);

            if (!Tetrimino.IsInsideBorder(v))
            {
                return false;
            }

            if (grid[(int) v.x, (int) v.y] != null && grid[(int) v.x, (int) v.y].transform.parent.parent !=
                currentTetriminoFalling.transform)
                return false;
        }

        return true;
    }

    public void CheckMaximumHeightRemaining()
    {
        int tempHeight = 0;
        foreach (GameObject tetrimino in GameObject.FindGameObjectsWithTag("Tetrimino"))
        {
            //Debug.Log("tetrimino.name: " + tetrimino.name);
            if (tetrimino.GetComponent<Tetrimino>().isActive == false)
            {
                Vector2 v = Tetrimino.RoundVector(tetrimino.transform.position);

                //Debug.Log("height: " + (int)v.y);
                if ((int) v.y > tempHeight)
                {
                    tempHeight = (int) v.y;
                }

                if (maxHeight != tempHeight)
                {
                    maxHeight = tempHeight;
                    AkSoundEngine.SetRTPCValue("hieght", maxHeight, GameObject.Find("WwiseGlobal"));
                    Debug.Log("RTPC Value height");
                    Debug.Log("maxHeight: " + maxHeight);
                }
            }
        }
    }

    public bool CheckIsAboveGrid(GameObject tetrimino)
    {
        for (int x = 0; x < width; x++)
        {
            foreach (GameObject cube in FindObjectOfType<Tetrimino>().cubes)
            {
                Vector2 v = Tetrimino.RoundVector(cube.transform.position);
                if ((int) v.y > height - 1)
                    return true;
            }
        }

        return false;
    }

    public void GameInfoUpdate()
    {
        //Debug.Log("Score = " + score);
        hud_score.text = score.ToString();
        level = (rows / 10) + 1;
        //Debug.Log("Level = " + level);
        hud_level.text = level.ToString();

        if (rows >= rowsStage1 && !stage2Played)
        {
            stage1Effect.SetActive(true);
            stage = 2;
            SoundManager.PlayStage2();
            stage2Played = true;
        }

        if (rows >= rowsStage1 + rowsStage2 && !stage3Played)
        {
            stage2Effect.SetActive(true);
            stage = 3;
            SoundManager.PlayStage3();
            stage3Played = true;
        }

        if (rows >= rowsStage1 + rowsStage2 + rowsStage3)
        {
            stage3Effect.SetActive(true);
            //You Win
            StartCoroutine(GameWin());
        }

        if (stage == 1)
        {
            rowsUpdate = rows;
            stageRows = rowsStage1;
        }

        if (stage == 2)
        {
            rowsUpdate = rows;
            rowsUpdate -= rowsStage1;
            stageRows = rowsStage2;
        }

        if (stage == 3)
        {
            rowsUpdate = rows;
            rowsUpdate -= (rowsStage1 + rowsStage2);
            stageRows = rowsStage3;
        }

        hud_stage.text = stage.ToString();
        //Debug.Log("Rows = " + rows);
        hud_rows.text = rowsUpdate.ToString();
        hud_stageRows.text = stageRows.ToString();
    }

    public void OutOfBounds(GameObject tetrimino)
    {
        for (int x = 0; x < width; ++x)
        {
            foreach (GameObject cube in FindObjectOfType<Tetrimino>().cubes)
            {
                Vector2 v = Tetrimino.RoundVector(cube.transform.position);
                if (v.y > height - 1)
                    // SoundManager.PlayGameOver();
                    GameOver();
            }
        }
    }

    public void Speed()
    {
        if (level == 1)
            speed = 1.0f;
        if (level == 2)
            speed = 0.8f;
        if (level == 3)
            speed = 0.6f;
        if (level == 4)
            speed = 0.5f;
        // if (level == 5)
        //     speed = 0.4f;
        // if (level == 6)
        //     speed = 0.3f;
        // if (level == 7)
        //     speed = 0.2f;
        // if (level == 8)
        //     speed = 0.1f;
        // if (level > 8)
        //     speed = 0.1f;
    }

    public void GameOver()
    {
        AkSoundEngine.PostEvent("game_lose", GameObject.Find("WwiseGlobal"));
        Debug.Log("Playing game_lose event ");

        SceneManager.LoadScene("GameOver");
    }

    IEnumerator GameWin()
    {
        AkSoundEngine.PostEvent("game_win", GameObject.Find("WwiseGlobal"));
        Debug.Log("Playing game_win event ");
        yield return new WaitForSeconds(1.5f);
        //SceneManager.UnloadSceneAsync("GamePlay");
        SceneManager.LoadScene("GameOver_WIN");
    }
}

/*
    public GameObject SpawnNextTetrimino()
    {

        if (!gameStarted)
        {
            gameStarted = true;

            NextTetrimino = GameObject.Instantiate(
                            TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                            new Vector3(5, 20, 0),
                            Quaternion.identity) as GameObject;

             PreviewTetrimino = GameObject.Instantiate(
                            TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                            new Vector3(15, 5, 0),
                            Quaternion.identity) as GameObject;
            PreviewTetrimino.GetComponent<Tetrimino>().enabled = false;
            Debug.Log(NextTetrimino.GetComponent<ITetrimino>().GetType());
            return NextTetrimino;

        }

        else
        {
            PreviewTetrimino.GetComponent<Tetrimino>().enabled = true;
            NextTetrimino = PreviewTetrimino;
            NextTetrimino.transform.localPosition = new Vector3(5, 20, 0);
            //PreviewTetrimino = null;
            PreviewTetrimino = GameObject.Instantiate(
                            TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                            new Vector3(15, 5, 0),
                            Quaternion.identity) as GameObject;
            PreviewTetrimino.GetComponent<Tetrimino>().enabled = false;
            Debug.Log(NextTetrimino.GetComponent<ITetrimino>().GetType());

            return NextTetrimino;


        }
    }

    /*
    public void SpawnNextTetrimino()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            currentTetriminoFalling = GameObject.Instantiate(
                        TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                        new Vector3(5, 20, 0),
                        Quaternion.identity) as GameObject;

            previewTetrimino =  GameObject.Instantiate(
                        TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                        previewTetriminoPosition,
                        Quaternion.identity) as GameObject;
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
            //return nextTetrimino;
        }

        else
        {
            previewTetrimino.transform.localPosition = new Vector3(5, 20, 0);
            currentTetriminoFalling = previewTetrimino;
            currentTetriminoFalling.GetComponent<Tetrimino>().enabled = true;
            previewTetrimino = GameObject.Instantiate(
                        TetriminoPrefabs[Random.Range(0, TetriminoPrefabs.Length)],
                        previewTetriminoPosition,
                        Quaternion.identity) as GameObject;
            previewTetrimino.GetComponent<Tetrimino>().enabled = false;
            //return nextTetrimino;

        }
       
        Debug.Log(nextTetrimino.GetComponent<ITetrimino>().GetType());


    }
     */