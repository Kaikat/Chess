using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class Queen : ChessPiece
    {
        public Queen(Player player) : base(player, "queen")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            int[] deltaX = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] deltaY = { 0, -1, -1, -1, 0, 1, 1, 1 };

            for (int i = 0; i < deltaX.Length; i++)
            {
                Vector2 move = gridSpot + new Vector2(deltaX[i], deltaY[i]);

                while (chessboard.isInBounds(move) && (chessboard.isEmpty(move) || chessboard.getChessPiecePlayerAt(move) != Player))
                {
                    possibleMoves.Add(move);

                    if (!chessboard.isEmpty(move) && chessboard.getChessPiecePlayerAt(move) != Player)
                    {
                        break;
                    }

                    move += new Vector2(deltaX[i], deltaY[i]);
                }
            }

            return possibleMoves;
        }
    }
}