using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class King : ChessPiece
    {
        public King(Player player) : base(player, "king")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2 move = new Vector2(gridSpot.X + i, gridSpot.Y + j);
                    if (chessboard.isInBounds(move))
                    {
                        if (chessboard.isEmpty(move))
                        {
                            possibleMoves.Add(move);
                        }
                        else if (chessboard.getChessPiecePlayerAt(move) != Player)
                        {
                            possibleMoves.Add(move);
                        }
                    }
                }
            }

            return possibleMoves;
        }
    }
}