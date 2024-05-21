using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using Sudoku;
using Unity.Burst;
using UnityEditor;
using UnityEngine;

public class SpriteBoard : MonoBehaviour
{
    [SerializeField]
    GameObject[] sprites;

    public GameObject square;

    [Range(0f,1f)]
    public float squareScale;

    public float spacing;

    public float offset;

    bool spritesExist = false;
    // Start is called before the first frame update
    public void Start()
    {
        sprites = new GameObject[81];
        LayoutSprites();
    }

    public void LayoutSprites()
    {
        var fixed_offset = offset-(spacing/2f);
        if (spritesExist)
        {
            foreach (var sprite in sprites)
            {
                DestroyImmediate(sprite);
            }
            sprites = new GameObject[81];
        }
        for (int k = 0; k < 3; k++)
        {
            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var x_pos = (i + (k * 3) - 4 - spacing + k * spacing) * fixed_offset + transform.position.x;
                        var y_pos = (j + (l * 3) - 4 - spacing + l * spacing) * fixed_offset + transform.position.y;
                        var id = i + 3 * j + (k + 3 * l) * 9;
                        var col = i + (k * 3);
                        var row = j + (l * 3); 
                        GameObject new_square = Instantiate(square, new Vector3(x_pos, y_pos), Quaternion.identity);
                        new_square.transform.localScale = new Vector3(squareScale, squareScale, 1f);
                        sprites[id] = new_square;
                        new_square.transform.parent = gameObject.transform;
                        new_square.GetComponent<SudokuSprite>().ID = id;
                        new_square.GetComponent<SudokuSprite>().Col = col;
                        new_square.GetComponent<SudokuSprite>().Row = row;
                    }
                }
            }
        }
        spritesExist = true;
        //LogRCBMap();
    }

    public void LogRCBMap()
    {
        var output = "";
        foreach (var sprite in sprites)
        {
            output += "{"+ sprite.GetComponent<SudokuSprite>().Row.ToString() + ", "
             + sprite.GetComponent<SudokuSprite>().Col.ToString() + ", "
             + (sprite.GetComponent<SudokuSprite>().ID / 9).ToString() + "}, ";
        }
        Debug.Log(output);
    }

    public void HighlightRCB(uint space_id)
    {
        HighlightGrouping(Sudoku.SudokuData.ROW, Sudoku.SudokuData.RCB_Ids[space_id, Sudoku.SudokuData.ROW]);
        HighlightGrouping(Sudoku.SudokuData.COL, Sudoku.SudokuData.RCB_Ids[space_id, Sudoku.SudokuData.COL]);
        HighlightGrouping(Sudoku.SudokuData.BLOCK, Sudoku.SudokuData.RCB_Ids[space_id, Sudoku.SudokuData.BLOCK]);
    }

    public void HighlightGrouping(uint groupingType, uint groupingID)
    {
        for (int i = 0; i < 9; i++)
        {
            HighlightSpace(Sudoku.SudokuData.Groupings[groupingType][groupingID, i]);
        }
    }

    public void HighlightSpace(uint id)
    {
        sprites[id].GetComponent<SudokuSprite>().EnableHighlight();
    }

    public void UnhighlightAll()
    {
        foreach (var sprite in sprites)
        {
            sprite.GetComponent<SudokuSprite>().DisableHighlight();
        }
    }

    public void DisplayBoard(SudokuBoard board)
    {
        var boardArray = board.GetSerialized();
        for (uint i = 0; i < 81; i++)
        {
            if (boardArray[i] != 0) sprites[i].GetComponent<SudokuSprite>().ShowNumber(boardArray[i]);
        }
    }

    public void DisplayBoard(string boardString)
    {
        var board = SudokuBoard.FromString(boardString);
        DisplayBoard(board);
    }
}
