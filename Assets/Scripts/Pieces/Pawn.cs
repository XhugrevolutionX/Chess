using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> legalMoves = new();
        int dir = (color == Color.White) ? 1 : -1;

        Vector2Int oneStep = BoardPosition + new Vector2Int(0, dir);
        if (Board.IsInsideBoard(oneStep) && Board.GetPieceAt(oneStep) == null)
        {
            legalMoves.Add(oneStep);

            Vector2Int twoStep = BoardPosition + new Vector2Int(0, 2 * dir);
            bool isOnStartingRow = (color == Color.White && BoardPosition.y == 1) || (color == Color.Black && BoardPosition.y == 6);
            if (isOnStartingRow && Board.GetPieceAt(twoStep) == null)
            {
                legalMoves.Add(twoStep);
            }
        }

        // Diagonal captures and en passant
        Vector2Int[] diagonals = {
            BoardPosition + new Vector2Int(-1, dir),
            BoardPosition + new Vector2Int(1, dir)
        };

        foreach (var pos in diagonals)
        {
            if (!Board.IsInsideBoard(pos)) continue;

            Piece target = Board.GetPieceAt(pos);
            if (target != null && target.color != color)
            {
                legalMoves.Add(pos);
            }
            else if (Board.enPassantTargetSquare.HasValue && pos == Board.enPassantTargetSquare.Value)
            {
                legalMoves.Add(pos);
            }
        }
        
        return legalMoves;
    }
    
    public override void Move(Vector2Int newPos)
    {
        int dir = (color == Color.White) ? 1 : -1;
        Vector2Int from = BoardPosition;

        // Check en passant capture
        if (Board.enPassantTargetSquare.HasValue && newPos == Board.enPassantTargetSquare.Value)
        {
            Vector2Int capturedPos = new Vector2Int(newPos.x, newPos.y - dir);
            Piece captured = Board.GetPieceAt(capturedPos);
            if (captured != null && captured is Pawn)
            {
                Board.RemovePiece(captured);
            }
        }

        // Update en passant square (only if moving 2 steps forward)
        if (Mathf.Abs(newPos.y - from.y) == 2)
        {
            Board.enPassantTargetSquare = new Vector2Int(from.x, from.y + dir);
        }
        else
        {
            Board.enPassantTargetSquare = null;
        }

        // Move piece
        BoardPosition = newPos;
        transform.position = Board.positions[newPos.x, newPos.y].position;

        // Promotion check
        if ((color == Color.White && newPos.y == 7) || (color == Color.Black && newPos.y == 0))
        {
            Promote();
        }
    }
    
    void Promote()
    {
        GameObject prefab = (color == Color.White) ? Board.whiteQueenPrefab : Board.blackQueenPrefab;
        GameObject newQueen = Instantiate(prefab, transform.position, Quaternion.identity, (color == Color.White) ? Board.whiteParent : Board.blackParent);
        
        Piece newPiece = newQueen.GetComponent<Piece>();
        newPiece.Board = Board;
        newPiece.color = color;
        newPiece.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);


        // Move the new queen to the promotion square
        newPiece.Move(BoardPosition);

        if (color == Color.White)
        {
            Board.whitePieces.Add(newPiece);
            Board.whitePieces.Remove(this);
        }
        else
        {
            Board.blackPieces.Add(newPiece);
            Board.blackPieces.Remove(this);
        }

        Destroy(gameObject);
    }
}