using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector2Int> GetRawMoves()
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        Vector2Int[] deltas = {
            new Vector2Int(2, 1), new Vector2Int(1, 2), new Vector2Int(-1, 2), new Vector2Int(-2, 1),
            new Vector2Int(-2, -1), new Vector2Int(-1, -2), new Vector2Int(1, -2), new Vector2Int(2, -1)
        };

        foreach (var delta in deltas)
        {
            Vector2Int target = BoardPosition + delta;
            if (!Board.IsInsideBoard(target)) continue;

            if (Board.IsEmpty(target) || Board.IsEnemy(target, color))
                legalMoves.Add(target);
        }
        
        return legalMoves;
    }

}