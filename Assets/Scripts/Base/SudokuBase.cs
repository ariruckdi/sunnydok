using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

namespace Sudoku
{
 
public class SudokuData
{
    public static readonly uint[,] Rows = {{00, 01, 02, 09, 10, 11, 18, 19, 20}, 
                                          {03, 04, 05, 12, 13, 14, 21, 22, 23},
                                          {06, 07, 08, 15, 16, 17, 24, 25, 26},
                                          {27, 28, 29, 36, 37, 38, 45, 46, 47},
                                          {30, 31, 32, 39, 40, 41, 48, 49, 50},
                                          {33, 34, 35, 42, 43, 44, 51, 52, 53},
                                          {54, 55, 56, 63, 64, 65, 72, 73, 74},
                                          {57, 58, 59, 66, 67, 68, 75, 76, 77},
                                          {60, 61, 62, 69, 70, 71, 78, 79, 80}};

    public static readonly uint[,] Cols = {{00, 03, 06, 27, 30, 33, 54, 57, 60},
                                          {01, 04, 07, 28, 31, 34, 55, 58, 61},
                                          {02, 05, 08, 29, 32, 35, 56, 59, 62},
                                          {09, 12, 15, 36, 39, 42, 63, 66, 69},
                                          {10, 13, 16, 37, 40, 43, 64, 67, 70},
                                          {11, 14, 17, 38, 41, 44, 65, 68, 71},
                                          {18, 21, 24, 45, 48, 51, 72, 75, 78},
                                          {19, 22, 25, 46, 49, 52, 73, 76, 79},
                                          {20, 23, 26, 47, 50, 53, 74, 77, 80}};

    public static readonly uint[,] Blocks = {{00, 01, 02, 03, 04, 05, 06, 07, 08},
                                            {09, 10, 11, 12, 13, 14, 15, 16, 17},
                                            {18, 19, 20, 21, 22, 23, 24, 25, 26},
                                            {27, 28, 29, 30, 31, 32, 33, 34, 35},
                                            {36, 37, 38, 39, 40, 41, 42, 43, 44},
                                            {45, 46, 47, 48, 49, 50, 51, 52, 53},
                                            {54, 55, 56, 57, 58, 59, 60, 61, 62},
                                            {63, 64, 65, 66, 67, 68, 69, 70, 71},
                                            {72, 73, 74, 75, 76, 77, 78, 79, 80}};

    public static readonly uint[][,] Groupings = {Rows, Cols, Blocks};
    public static uint ROW = 0;
    public static uint COL = 1;
    public static uint BLOCK = 2;
    public static uint[,] RCB_Ids = {{0, 0, 0}, {0, 1, 0}, {0, 2, 0}, 
                                    {1, 0, 0}, {1, 1, 0}, {1, 2, 0}, 
                                    {2, 0, 0}, {2, 1, 0}, {2, 2, 0}, 
                                    {0, 3, 1}, {0, 4, 1}, {0, 5, 1}, 
                                    {1, 3, 1}, {1, 4, 1}, {1, 5, 1}, 
                                    {2, 3, 1}, {2, 4, 1}, {2, 5, 1}, 
                                    {0, 6, 2}, {0, 7, 2}, {0, 8, 2}, 
                                    {1, 6, 2}, {1, 7, 2}, {1, 8, 2}, 
                                    {2, 6, 2}, {2, 7, 2}, {2, 8, 2}, 
                                    {3, 0, 3}, {3, 1, 3}, {3, 2, 3}, 
                                    {4, 0, 3}, {4, 1, 3}, {4, 2, 3}, 
                                    {5, 0, 3}, {5, 1, 3}, {5, 2, 3}, 
                                    {3, 3, 4}, {3, 4, 4}, {3, 5, 4}, 
                                    {4, 3, 4}, {4, 4, 4}, {4, 5, 4}, 
                                    {5, 3, 4}, {5, 4, 4}, {5, 5, 4}, 
                                    {3, 6, 5}, {3, 7, 5}, {3, 8, 5}, 
                                    {4, 6, 5}, {4, 7, 5}, {4, 8, 5}, 
                                    {5, 6, 5}, {5, 7, 5}, {5, 8, 5}, 
                                    {6, 0, 6}, {6, 1, 6}, {6, 2, 6}, 
                                    {7, 0, 6}, {7, 1, 6}, {7, 2, 6}, 
                                    {8, 0, 6}, {8, 1, 6}, {8, 2, 6}, 
                                    {6, 3, 7}, {6, 4, 7}, {6, 5, 7}, 
                                    {7, 3, 7}, {7, 4, 7}, {7, 5, 7}, 
                                    {8, 3, 7}, {8, 4, 7}, {8, 5, 7}, 
                                    {6, 6, 8}, {6, 7, 8}, {6, 8, 8}, 
                                    {7, 6, 8}, {7, 7, 8}, {7, 8, 8}, 
                                    {8, 6, 8}, {8, 7, 8}, {8, 8, 8}};
}

class SudokuMask
{
    readonly bool[] mask;

    public SudokuMask()
    {
        mask = new bool[81];
    }

    public bool GetSpace(uint space_index)
    {
        return mask[space_index];
    }

    public void SetSpace(uint space_index, bool new_value)
    {
        mask[space_index] = new_value;
    }

    public static SudokuMask operator |(SudokuMask left, SudokuMask right)
    {
        var result = new SudokuMask();
        for (uint i = 0; i < 81; i++)
        {
            if (left.GetSpace(i) || right.GetSpace(i)) result.SetSpace(i, true);
        }
        return result;
    }

    public static SudokuMask operator &(SudokuMask left, SudokuMask right)
    {
        var result = new SudokuMask();
        for (uint i = 0; i < 81; i++)
        {
            if (left.GetSpace(i) && right.GetSpace(i)) result.SetSpace(i, true);
        }
        return result;
    }

    public static SudokuMask operator -(SudokuMask left, SudokuMask right)
    {
        var result = new SudokuMask();
        for (uint i = 0; i < 81; i++)
        {
            if (left.GetSpace(i) && !right.GetSpace(i)) result.SetSpace(i, true);
        }
        return result;
    }

    public void SetGrouping(uint groupingType, uint groupingID, bool value = true)
    {
        for (uint i = 0; i < 9; i++)
        {
            SetSpace(SudokuData.Groupings[groupingType][groupingID, i], value);
        }
    }
}

public class SudokuBoard
{
    SudokuMask[] set;
    SudokuMask[] possible;

    public SudokuBoard()
    {
        set = new SudokuMask[9];
        possible = new SudokuMask[9];

        for (int i = 0; i < 9; i++)
        {
            set[i] = new SudokuMask();
            possible[i] = new SudokuMask();
        }
    }

    public uint[] GetSerialized()
    {
        var output = new uint[81];
        for (uint i = 0; i < 81; i++)
        {
            output[i] = GetSetNum(i);
        }
        return output;
    }

    public uint GetSetNum(uint id)
    {
        for (uint i = 0; i < 9; i++)
        {
            if (set[i].GetSpace(id)) return i + 1;
        }
        return 0;
    }

    public void WriteNum(uint id, uint number)
    {
        set[number - 1].SetSpace(id, true);
    }

    public static SudokuBoard FromString(string input)
    {
        var newBoard = new SudokuBoard();
        newBoard.WriteNum(0, 1);
        var blocks = input.Split("|");
        for (uint i = 0; i < 9; i++)
        {
            for (uint j = 0; j < 9; j++)
            {
                uint convertedNumber = uint.Parse(blocks[i][(int)j].ToString());
                if (convertedNumber != 0) newBoard.WriteNum((i*9) + j, convertedNumber);
            }
        }
        return newBoard;
    }
}
}
