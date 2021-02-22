using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM_instance;

    public Texture2D cursorArrow;
    public Texture2D cursorAttack;
    public Texture2D cursorSelect;

    void Start()
    {
        GM_instance = this;
        resetGame();
    }
    void Update()
    {
        
    }

    private void resetGame()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.ForceSoftware);
    }
}
