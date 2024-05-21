using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Unity.VisualScripting;

[CustomEditor(typeof(SpriteBoard))]
public class SpriteLayoutEditor : Editor
{
    uint idToHighlight;
    uint groupToHighlight;
    string boardString;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Layout Sprites")) target.GetComponent<SpriteBoard>().LayoutSprites();

        idToHighlight = (uint)EditorGUILayout.IntField("Space ID: ", (int)idToHighlight); 
        groupToHighlight = (uint)EditorGUILayout.IntField("Group ID: ", (int)groupToHighlight);

        if(GUILayout.Button("Highlight Space")) {
            target.GetComponent<SpriteBoard>().UnhighlightAll();
            target.GetComponent<SpriteBoard>().HighlightSpace(idToHighlight);
        } 
        if(GUILayout.Button("Highlight Row")) { 
            target.GetComponent<SpriteBoard>().UnhighlightAll();
            target.GetComponent<SpriteBoard>().HighlightGrouping(Sudoku.SudokuData.ROW, groupToHighlight);
        }
        if(GUILayout.Button("Highlight Column")) { 
            target.GetComponent<SpriteBoard>().UnhighlightAll();
            target.GetComponent<SpriteBoard>().HighlightGrouping(Sudoku.SudokuData.COL, groupToHighlight);
        }
        if(GUILayout.Button("Highlight Block")) { 
            target.GetComponent<SpriteBoard>().UnhighlightAll();
            target.GetComponent<SpriteBoard>().HighlightGrouping(Sudoku.SudokuData.BLOCK, groupToHighlight);
        }
        if(GUILayout.Button("Highlight RCB")) { 
            target.GetComponent<SpriteBoard>().UnhighlightAll();
            target.GetComponent<SpriteBoard>().HighlightRCB(idToHighlight);
        }

        boardString = EditorGUILayout.TextField("Board String: ", boardString);
        if (GUILayout.Button("Show Board")) {
            target.GetComponent<SpriteBoard>().DisplayBoard(boardString);
        }
    }
}
