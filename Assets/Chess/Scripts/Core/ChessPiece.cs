using Chess.Scripts.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//whether piece is still in game or not
public enum PieceState
{
    In,
    Out
}

public enum PieceType
{
    Pawn=1,
    Knight=2,
    Bishop=3,
    Rook=4,
    Queen=5,
    King=6
}

public class ChessPiece : MonoBehaviour,PathVisualizer
{
    
    public PieceType pieceType { get; private set; }
    public PlayerType playerType { get; private set; }
    
    int row, col;

    

    public void AssignPositionArray()
    {
        playerType = PlayerType.Player;
        var PlacementHandler = GetComponent<ChessPlayerPlacementHandler>();
        row = PlacementHandler.row;
        col = PlacementHandler.column;
        GameManager.Instance.PiecesArray[row, col] = playerType;
    }


    public void AssignPieceType(string name=null)
    {
        switch(name.ToLower())
        {
            case "pawn": pieceType = PieceType.Pawn;
                break;
            case "knight": pieceType= PieceType.Knight;
                break;
            case "bishop": pieceType= PieceType.Bishop;
                break;
            case "rook": pieceType=PieceType.Rook;
                break;
            case "queen": pieceType=PieceType.Queen;
                break;
            case "king":pieceType=PieceType.King;
                break;
            default:
                Debug.LogError("Please select Piece Type in the Inspector of " + gameObject.name);
                break;
        }
    }

    [ContextMenu("Show Path")]
    public void ShowPath()
    {
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        switch (pieceType)
        {
            case PieceType.Pawn: HighlightPawnPath();
                break;
            case PieceType.Knight: HighlightKnightPath();
                break;
            case PieceType.Bishop: HighlightBishopPath();
                break;
            case PieceType.Rook: HighlightRookPath();
                break;
            case PieceType.Queen: HighlightQueenPath();
                break;
            case PieceType.King: HighlightKingPath();
                break;
            default: Debug.LogError("Check piece type of "+gameObject.name);
                break;

        }
    }

    void HighlightCell(int r,int c)
    {
        ChessBoardPlacementHandler.Instance.Highlight(r,c);
    }

    bool valid(int num)
    {
        return num < 8 && num >= 0;
    }

    [ContextMenu("Highlight Pawn Path")]
    void HighlightPawnPath()
    {
        if (playerType == PlayerType.Player)
        {
            int maxMovement = row == 1 ? 2 : 1;

            for (int i = row+1; i < 8 && i <= row+maxMovement; i++) 
            {
                if (GameManager.Instance.PiecesArray[i, col] == 0)
                {
                    ChessBoardPlacementHandler.Instance.Highlight(i, col);
                }
                else
                    break;
            }
        }
    }

    [ContextMenu("Highlight Knight Path")]
    void HighlightKnightPath()
    {
        int[] movArr = { -2, -1, 1, 2 };

        for (int i = 0; i < movArr.Length; i++)
        {
            for (int j = 0; j < movArr.Length; j++)
            {
                if (i == j || Mathf.Abs(movArr[i]) == Mathf.Abs(movArr[j])) continue;

                if (valid(row + movArr[i]) && valid(col + movArr[j]))
                {
                    if (GameManager.Instance.PiecesArray[row + movArr[i], col + movArr[j]] == 0)
                        ChessBoardPlacementHandler.Instance.Highlight(row + movArr[i], col + movArr[j]);
                }
            }
        }

    }

    [ContextMenu("Highlight Bishop Path")]
    void HighlightBishopPath(int maxDistance=7)
    {
        for(int i=1;i <= maxDistance; i++)
        {
            if (valid(row+i) && valid(col+i) && GameManager.Instance.PiecesArray[row + i, col + i] == 0)
                HighlightCell(row+i,col+ i);
            else
                break;
        }
        for (int i = 1; i <= maxDistance; i++)
        {
            if (valid(row+i) && valid(col-i) && GameManager.Instance.PiecesArray[row + i, col - i] == 0)
                HighlightCell(row + i, col - i);
            else
                break;
        }
        for (int i = 1;i <= maxDistance; i++)
        {
            if (valid(row-i) && valid(col-i) && GameManager.Instance.PiecesArray[row - i, col - i] == 0)
                HighlightCell(row - i, col - i);
            else
                break;
        }
        for (int i = 1; i <= maxDistance; i++)
        {
            if (valid(row - i) && valid(col + i) && GameManager.Instance.PiecesArray[row - i, col + i] == 0)
                HighlightCell(row - i, col + i);
            else
                break;
        }
    }

    [ContextMenu("Highlight Rook Path")]
    void HighlightRookPath(int maxDistance=7)
    {
        for (int i = 1; i <= maxDistance; i++)
        {
            if (valid(row + i) && GameManager.Instance.PiecesArray[row + i, col] == 0)
                HighlightCell(row + i, col);
            else
                break;

        }
        for (int i = 1; i <= maxDistance; i++)
        {
            if (valid(row - i) && GameManager.Instance.PiecesArray[row - i, col] == 0)
                HighlightCell(row - i, col);
            else
                break;
        }
        for(int i = 1;i <= maxDistance; i++)
        {
            if (valid(col + i) && GameManager.Instance.PiecesArray[row, col+i] == 0)
                HighlightCell(row , col+i);
            else
                break;
        }
        for (int i = 1; i <= maxDistance; i++)
        {
            if (valid(col - i) && GameManager.Instance.PiecesArray[row, col - i] == 0)
                HighlightCell(row, col - i);
            else
                break;
        }
    }

    [ContextMenu("Highlight Queen Path")]
    void HighlightQueenPath()
    {
        HighlightBishopPath();
        HighlightRookPath();
    }

    [ContextMenu("Highlight King Path")]
    void HighlightKingPath()
    {
        HighlightBishopPath(1);
        HighlightRookPath(1);
    }
}