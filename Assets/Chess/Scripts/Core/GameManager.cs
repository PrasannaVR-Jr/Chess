using Chess.Scripts.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    None,
    Player=1,
    Enemy=2
}

public class GameManager : MonoBehaviour
{
    internal PlayerType[,] PiecesArray = new PlayerType[8, 8];

    public static GameManager Instance;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var ChessPiecesGOArr = GameObject.FindGameObjectsWithTag("Player");

        foreach (var piece in ChessPiecesGOArr)
        {
            if (piece != null)
            {
                var chesspiece = piece.AddComponent<ChessPiece>();
                chesspiece.AssignPieceType(chesspiece.name);
                chesspiece.AssignPositionArray();
            }
        }

        DisplayTypeArray();
    }

    [ContextMenu("Display Array")]
    public void DisplayTypeArray()
    {
        string typeArrayToString="";
        for (int i = 0; i < PiecesArray.GetLength(0); i++)
        {
            for (int j = 0; j < PiecesArray.GetLength(1); j++)
            {
                typeArrayToString += $"{PiecesArray[i, j]} ";
            }
            typeArrayToString += "\n";
        }
        Debug.Log(typeArrayToString);
    }


}
