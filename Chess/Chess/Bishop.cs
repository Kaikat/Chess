using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class Bishop : ChessPiece
    {
        public Bishop(Player player) : base(player, "bishop")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            int[] deltaX = { -1, 1, 1, -1 };
            int[] deltaY = { -1, -1, 1, 1 };

            for (int i = 0; i < deltaX.Length; i++)
            {
                Vector2 movingGridPosition = gridSpot;
                Vector2 move = new Vector2(movingGridPosition.X + deltaX[i], movingGridPosition.Y + deltaY[i]);

                while (chessboard.isInBounds(move))
                {
                    if (chessboard.isEmpty(move))
                    {
                        possibleMoves.Add(move);
                    }
                    else if (chessboard.getChessPiecePlayerAt(move) != Player)
                    {
                        possibleMoves.Add(move);
                        break;
                    }
                    else if (chessboard.getChessPiecePlayerAt(move) == Player)
                    {
                        break;
                    }

                    movingGridPosition = move;
                    move = new Vector2(movingGridPosition.X + deltaX[i], movingGridPosition.Y + deltaY[i]);
                }
            }

            return possibleMoves;
        }
    }
}