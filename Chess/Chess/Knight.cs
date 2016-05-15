using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class Knight : ChessPiece
    {
        public Knight(Player player) : base(player, "knight")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            int[] deltaX = { -2, -2, -1, 1, 2, 2, 1, -1 };
            int[] deltaY = { 1, -1, -2, -2, -1, 1, 2, 2 };

            for (int i = 0; i < deltaX.Length; i++)
            {
                Vector2 move = new Vector2(gridSpot.X + deltaX[i], gridSpot.Y + deltaY[i]);
                if (chessboard.isInBounds(move))
                {
                    if (chessboard.isEmpty(move) || chessboard.getChessPiecePlayerAt(move) != Player)
                    {
                        possibleMoves.Add(move);
                    }
                }
            }

            return possibleMoves;
        }
    }
}