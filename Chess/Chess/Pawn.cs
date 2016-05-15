using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        public Pawn(Player player) : base(player, "pawn")
        {
        }

        public override List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot)
        {
            List<Vector2> possibleMoves = new List<Vector2>();

            for (int i = -1; i <= 1; i++)
            {
                Vector2 move;
                move = (Player == Player.PLAYER1) ? new Vector2(gridSpot.X + i, gridSpot.Y + 1) :
                                                    new Vector2(gridSpot.X + i, gridSpot.Y - 1);

                if (chessboard.isInBounds(move))
                {
                    switch (i)
                    {
                        case 0:
                            if (chessboard.isEmpty(move))
                            {
                                possibleMoves.Add(move);
                            }

                            break;

                        default:
                            if (!chessboard.isEmpty(move) && chessboard.getChessPiecePlayerAt(move) != Player)
                            {
                                possibleMoves.Add(move);
                            }

                            break;
                    }
                }
            }

            if (Player == Player.PLAYER1 && gridSpot.Y == 1)
            {
                if (chessboard.isEmpty(new Vector2(gridSpot.X, gridSpot.Y + 1)) &&
                    chessboard.isEmpty(new Vector2(gridSpot.X, gridSpot.Y + 2)))
                {
                    Vector2 move = new Vector2(gridSpot.X, gridSpot.Y + 2);
                    possibleMoves.Add(move);
                }
            }

            if (Player == Player.PLAYER2 && gridSpot.Y == 6)
            {
                if (chessboard.isEmpty(new Vector2(gridSpot.X, gridSpot.Y - 1)) &&
                    chessboard.isEmpty(new Vector2(gridSpot.X, gridSpot.Y - 2)))
                {
                    Vector2 move = new Vector2(gridSpot.X, gridSpot.Y - 2);
                    possibleMoves.Add(move);
                }
            }

            return possibleMoves;
        }
    }
}