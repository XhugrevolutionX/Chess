using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        Vector2Int[] directions = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
            new Vector2Int(1, 1), new Vector2Int(-1, 1),
            new Vector2Int(1, -1), new Vector2Int(-1, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int target = BoardPosition + dir;
            if (!Board.IsInsideBoard(target)) continue;

            if (Board.IsEmpty(target) || Board.IsEnemy(target, color))
                legalMoves.Add(target);
        }

        // TODO: Castling logic (requires tracking moved status)
        
        return legalMoves;
    }
}