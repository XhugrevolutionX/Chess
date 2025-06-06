using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> legalMoves = new();

        Vector2Int[] directions = {
            new(1, 0), new(-1, 0), new(0, 1), new(0, -1),
            new(1, 1), new(-1, 1), new(1, -1), new(-1, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int target = BoardPosition + dir;
            if (Board.IsInsideBoard(target))
            {
                Piece other = Board.GetPieceAt(target);
                if ((other == null || other.color != color))
                {
                    legalMoves.Add(target);
                }
            }
        }

        legalMoves.AddRange(GetCastlingMoves());
       
        return legalMoves;
    }
    
    private List<Vector2Int> GetCastlingMoves()
    {
        List<Vector2Int> castles = new();

        if (HasMoved)
            return castles;

        int y = (color == Color.White) ? 0 : 7;

        // Kingside
        if (CanCastle(new Vector2Int(7, y), new[] { new Vector2Int(5, y), new Vector2Int(6, y) }))
            castles.Add(new Vector2Int(6, y));

        // Queenside
        if (CanCastle(new Vector2Int(0, y), new[] { new Vector2Int(1, y), new Vector2Int(2, y), new Vector2Int(3, y) }))
            castles.Add(new Vector2Int(2, y));

        return castles;
    }

    private bool CanCastle(Vector2Int rookPos, Vector2Int[] betweenTiles)
    {
        Piece rook = Board.GetPieceAt(rookPos);
        if (rook == null || rook.type != Type.Rook || rook.color != color || rook.HasMoved)
            return false;

        foreach (var tile in betweenTiles)
        {
            if (Board.GetPieceAt(tile) != null)
                return false;
        }

        return true;
    }
    
    public override void Move(Vector2Int newPos)
    {
        base.Move(newPos);

        // Handle castling rook move
        int y = (color == Color.White) ? 0 : 7;

        if (newPos == new Vector2Int(6, y)) // Kingside
        {
            MoveRookForCastling(new Vector2Int(7, y), new Vector2Int(5, y));
        }
        else if (newPos == new Vector2Int(2, y)) // Queenside
        {
            MoveRookForCastling(new Vector2Int(0, y), new Vector2Int(3, y));
        }
    }

    private void MoveRookForCastling(Vector2Int from, Vector2Int to)
    {
        Piece rook = Board.GetPieceAt(from);
        if (rook != null && rook.type == Type.Rook)
        {
            rook.Move(to);
        }
    }
}