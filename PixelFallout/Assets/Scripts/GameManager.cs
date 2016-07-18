using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public BoardManager boardScript;
    private int level = 1;


    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    // Summon Board Manager
    void InitGame()
    {
       boardScript.SetupScene(level);
    }
}
