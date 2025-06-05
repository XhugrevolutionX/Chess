using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> legalMoves = new List<Vector2Int>();
        int dir = (color == Color.White) ? 1 : -1;
        Vector2Int forward = BoardPosition + new Vector2Int(0, dir);

        // Forward 1 step
        if (Board.IsInsideBoard(forward) && Board.IsEmpty(forward))
            legalMoves.Add(forward);

        // Forward 2 steps from initial rank
        Vector2Int forward2 = BoardPosition + new Vector2Int(0, 2 * dir);
        bool isStartRow = (color == Color.White && BoardPosition.y == 1) ||
                          (color == Color.Black && BoardPosition.y == 6);
        if (isStartRow && Board.IsInsideBoard(forward2) && Board.IsEmpty(forward) && Board.IsEmpty(forward2))
            legalMoves.Add(forward2);

        // Diagonal captures
        foreach (int dx in new int[] { -1, 1 })
        {
            Vector2Int diag = BoardPosition + new Vector2Int(dx, dir);
            if (Board.IsInsideBoard(diag) && Board.IsEnemy(diag, color))
                legalMoves.Add(diag);
        }

        // TODO: Promotion and en passant

        return legalMoves;
    }
}