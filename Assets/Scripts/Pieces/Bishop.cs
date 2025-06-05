using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        Vector2Int[] directions = {
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
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