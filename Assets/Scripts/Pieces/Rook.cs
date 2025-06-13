using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> GetRawMoves()
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down,
            Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in directions)
        {
            Vector2Int current = BoardPosition + dir;
            while (Board.IsInsideBoard(current))
            {
                if (Board.IsEmpty(current))
                    legalMoves.Add(current);
                else
                {
                    if (Board.IsEnemy(current, color))
                        legalMoves.Add(current);
                    break;
                }
                current += dir;
            }
        }
        
        return legalMoves;
    }

}