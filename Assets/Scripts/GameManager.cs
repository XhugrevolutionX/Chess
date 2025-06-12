using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Camera mainCamera;

    public float highlightsOffset = 0.05f;
    public GameObject moveHighlightPrefab;
    public GameObject attackHighlightPrefab;
    private List<GameObject> currentHighlights = new List<GameObject>();

    private Piece selectedPiece;
    private Board board;
    private List<Vector2Int> legalMoves = new();

    public LayerMask tileLayerMask;
    public LayerMask pieceLayerMask;
    
    public TextMeshProUGUI turnDisplay;
    
    public enum PlayerTurn { White, Black }
    private PlayerTurn currentTurn = PlayerTurn.White;

    private void Start()
    {
        board = FindFirstObjectByType<Board>();
        if (mainCamera == null)
            mainCamera = Camera.main;
        UpdateTurnUI();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // First check if we hit a piece
        if (Physics.Raycast(ray, out hit, 100f, pieceLayerMask))
        {
            Piece clickedPiece = hit.collider.GetComponent<Piece>();

            if (clickedPiece != null)
            {
                // If no piece is selected yet, allow selection if it's that player's turn
                if (selectedPiece == null)
                {
                    if ((currentTurn == PlayerTurn.White && clickedPiece.color == Piece.Color.White) || (currentTurn == PlayerTurn.Black && clickedPiece.color == Piece.Color.Black))
                    {
                        SelectPiece(clickedPiece);
                    }
                }
                else
                {
                    // If we already selected a piece and clicked an enemy, try capturing
                    Vector2Int targetPos = clickedPiece.BoardPosition;
                    if (legalMoves.Contains(targetPos) && clickedPiece.color != selectedPiece.color)
                    {
                        MoveSelectedPiece(targetPos);
                    }
                    else if (clickedPiece.color == selectedPiece.color)
                    {
                        // Clicking another piece of the same color switches selection
                        SelectPiece(clickedPiece);
                    }
                }

                return;
            }
        }

        // If we hit a tile
        if (Physics.Raycast(ray, out hit, 100f, tileLayerMask))
        {
            Transform tile = hit.transform;
            Vector2Int targetPos = GetBoardPositionFromTile(tile);

            if (selectedPiece != null && legalMoves.Contains(targetPos))
            {
                MoveSelectedPiece(targetPos);
            }
        }
    }

    void SelectPiece(Piece piece)
    {
        ClearHighlights();
        selectedPiece = piece;
        legalMoves = piece.GetLegalMoves();
        ShowHighlights(legalMoves);
    }


    void MoveSelectedPiece(Vector2Int newPos)
    {
        // Check for capturing
        Piece target = board.GetPieceAt(newPos);
        if (target != null && target.color != selectedPiece.color)
        {
            board.RemovePiece(target);
        }

        selectedPiece.Move(newPos);
        
        legalMoves.Clear();
        ClearHighlights();
        
        SwitchTurn();
        
        selectedPiece = null;
    }

    Vector2Int GetBoardPositionFromTile(Transform tile)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (board.positions[x, y] == tile)
                    return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1); // Not found
    }
    
    void ShowHighlights(List<Vector2Int> moves)
    {
        foreach (var move in moves)
        {
            Transform tileTransform = board.positions[move.x, move.y];
            GameObject highlight;
            if (board.IsEmpty(move))
            {
                highlight = Instantiate(moveHighlightPrefab, tileTransform.position + Vector3.up * highlightsOffset, Quaternion.identity);
            }
            else
            {
                highlight = Instantiate(attackHighlightPrefab, tileTransform.position + Vector3.up * highlightsOffset, Quaternion.identity);
            }
            currentHighlights.Add(highlight);
        }
    }

    void ClearHighlights()
    {
        foreach (var h in currentHighlights)
        {
            Destroy(h);
        }
        currentHighlights.Clear();
    }
    
    void SwitchTurn()
    {
        currentTurn = (currentTurn == PlayerTurn.White) ? PlayerTurn.Black : PlayerTurn.White;
        UpdateTurnUI();
    }
    
    void UpdateTurnUI()
    {
        if (turnDisplay != null)
            turnDisplay.text = "Turn: " + currentTurn.ToString();
    }
}