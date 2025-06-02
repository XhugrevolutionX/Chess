using UnityEngine;

namespace Pieces
{
    public class Piece : MonoBehaviour
    {
        protected Board Board;
        
        public enum Type
        {
            Pawn,
            Rook,
            Knight,
            Bishop,
            King,
            Queen
        } 
        public enum Color
        {
            Black,
            White
        }
    
        public Type type;
        public Color color;
        
        protected int BoardPosition;

        private void Start()
        {
            Board = GetComponentInParent<Board>();

            GetInitialPositions();
            
        }
        private void GetInitialPositions()
        {
            for (int i = 0; i < Board.positions.Length; i++)
            {
                if (Board.positions[i].localPosition == transform.localPosition)
                {
                    BoardPosition = i;
                }
            }
        }
        
        
        
        protected virtual void CheckLegalMoves(){}
        protected virtual void Move(int move){}
        protected virtual void Attack(int attack){}

    }
}