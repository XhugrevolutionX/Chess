using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    [SerializeField] private Transform[] flatPositions = new Transform[64];
    
    public Transform[,] positions = new Transform[8, 8];
    
    public List<Piece> whitePieces = new List<Piece>();
    public List<Piece> blackPieces = new List<Piece>();

    private void Awake()
    {
        if (flatPositions.Length != 64)
        {
            Debug.LogError("Board must have exactly 64 tile positions assigned.");
            return;
        }

        // Fill the 2D board grid from the flat array (row-major order)
        for (int i = 0; i < 64; i++)
        {
            int x = i % 8;
            int y = i / 8;
            positions[x, y] = flatPositions[i];
        }
    }

    // Check if a board coordinate is valid
    public bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    // Get the world transform at a board position
    public Transform GetTileAt(Vector2Int pos)
    {
        if (!IsInsideBoard(pos)) return null;
        return positions[pos.x, pos.y];
    }

    // Get the piece (if any) located at a given board position
    public Piece GetPieceAt(Vector2Int pos)
    {
        return whitePieces.Concat(blackPieces)
            .FirstOrDefault(piece => piece.BoardPosition == pos);
    }

    // Is this square empty?
    public bool IsEmpty(Vector2Int pos)
    {
        return GetPieceAt(pos) == null;
    }

    // Is the square occupied by an enemy of the given color?
    public bool IsEnemy(Vector2Int pos, Piece.Color myColor)
    {
        Piece piece = GetPieceAt(pos);
        return piece != null && piece.color != myColor;
    }

    // Optionally: remove captured pieces
    public void RemovePiece(Piece piece)
    {
        if (piece.color == Piece.Color.White)
            whitePieces.Remove(piece);
        else
            blackPieces.Remove(piece);

        Destroy(piece.gameObject);
    }
}