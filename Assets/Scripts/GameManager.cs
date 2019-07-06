using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

  public int scoreToWin;
  public int curScore;
  public bool gamePaused;

  //instance
  public static GameManager instance;

  private void Awake()
  {
    //set instance
    instance = this;
  }

  private void Start()
  {
    Time.timeScale = 1.0f;
  }

  private void Update()
  {
    if (Input.GetButtonDown("Cancel"))
    {

      TogglePauseGame();

    }
  }

  public void TogglePauseGame()
  {

    gamePaused = !gamePaused;
    Time.timeScale = gamePaused == true ? 0.0f : 1.0f;

    Cursor.lockState = gamePaused == true ? CursorLockMode.None : CursorLockMode.Locked;
    Cursor.visible = gamePaused == true ? true : false;

    //toggle pause menu
    GameUI.instance.TogglePauseMenu(gamePaused);

  }

  public void AddScore(int score)
  {

    curScore += score;

    //update score text
    GameUI.instance.UpdateScoreText(curScore);

    //have reached score to win
    if (curScore >= scoreToWin)
    {

      WinGame();

    }

  }

  private void WinGame()
  {

    //set endgame screen
    GameUI.instance.SetEndGameScreen(true, curScore);
    Time.timeScale = 0.0f;
    gamePaused = true;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

  }

  public void LoseGame()
  {

    //set endgame screen
    GameUI.instance.SetEndGameScreen(false, curScore);
    Time.timeScale = 0.0f;
    gamePaused = true;
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

  }

}
