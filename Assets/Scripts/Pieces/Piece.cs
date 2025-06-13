using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public enum Type { Pawn, Rook, Knight, Bishop, Queen, King }
    public enum Color { White, Black }

    public Type type;
    public Color color;

    public Vector2Int BoardPosition { get; protected set; }
    public Board Board { get; set; }
    
    public bool HasMoved { get; protected set; } = false;

    
    protected virtual void Start()
    {
        Board = FindFirstObjectByType<Board>();
        SyncWithTransform();
    }

    // Finds closest tile to current transform and updates logical position
    public void SyncWithTransform()
    {
        float minDist = float.MaxValue;
        Vector2Int closest = Vector2Int.zero;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                float dist = Vector3.Distance(transform.position, Board.positions[x, y].position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = new Vector2Int(x, y);
                }
            }
        }

        BoardPosition = closest;
        transform.position = Board.positions[BoardPosition.x, BoardPosition.y].position;
    }
    

    // Override in subclasses
    public virtual List<Vector2Int> GetLegalMoves()
    {
        List<Vector2Int> rawMoves = GetRawMoves(); 
        List<Vector2Int> legalMoves = new();

        foreach (var move in rawMoves)
        {
            if (!Board.WouldKingBeInCheckAfterMove(this, move))
                legalMoves.Add(move);
        }

        return legalMoves;
    }
    public virtual List<Vector2Int> GetRawMoves() { return new List<Vector2Int>(); }


    // Moves piece to a new position and updates transform
    public virtual void Move(Vector2Int newPos)
    {
        if (!Board.IsInsideBoard(newPos)) return;

        BoardPosition = newPos;
        transform.position = Board.GetTileAt(newPos).position;
        HasMoved = true;
    }

}