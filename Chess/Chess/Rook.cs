using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class Rook : ChessPiece
    {
        public Rook(Player player) : base(player, "rook")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            Vector2 movingGridPosition = gridSpot;

            for (int i = 0; i <= 1; i++)
            {
                Vector2 moveBy;

                for (int j = -1; j <= 1; j = j + 2)
                {
                    moveBy = (i == 0) ? new Vector2(j, 0) : new Vector2(0, j);

                    Vector2 move = movingGridPosition + moveBy;
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
                        move = movingGridPosition + moveBy;
                    }

                    movingGridPosition = gridSpot;
                }
            }

            return possibleMoves;
        }
    }
}