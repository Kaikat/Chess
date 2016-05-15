using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Chess
{
    public class Chessboard
    {
        private const int CHESSBOARD_BORDER = 45;
        private const int GRID_LENGTH = 8;
        private const int SQUARE_WIDTH = 70;
        private const int PIECE_OFFSET_X = 17;
        private const int PIECE_OFFSET_Y = -7;

        private Texture2D chessboardTexture;
        private Texture2D availableMoveTexture;

        private ChessPiece[,] board;
        private Vector2 selectedPieceGridSpot;
        private List<Vector2> possibleMoves;

        public Chessboard()
        {
            board = new ChessPiece[GRID_LENGTH, GRID_LENGTH];
            possibleMoves = new List<Vector2>();
            setUpBoard();
        }

        private Chessboard(ChessPiece[,] theBoard)
        {
            board = new ChessPiece[GRID_LENGTH, GRID_LENGTH];
            for (int i = 0; i < GRID_LENGTH; i++)
            {
                for (int j = 0; j < GRID_LENGTH; j++)
                {
                    board[i, j] = theBoard[i, j];
                }
            }
            if (board[0, 0] == null) Console.WriteLine("ERROR COPYING THE BOARDS");
        }

        public Chessboard Clone()
        {
            return new Chessboard(board);
        }

        public void LoadContent(ContentManager content)
        {
            chessboardTexture = content.Load<Texture2D>("chessboard");
            availableMoveTexture = content.Load<Texture2D>("available");

            foreach (ChessPiece piece in board)
            {
                piece?.LoadContent(content);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(chessboardTexture, new Vector2(0, 0), Color.White);
            DrawPossibleMoves(spriteBatch);
            DrawChessPieces(spriteBatch);
        }

        public void DrawPossibleMoves(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                spriteBatch.Draw(availableMoveTexture, calculateBoardSquarePixelPosition(possibleMoves[i]), Color.White);
            }
        }

        public void DrawChessPieces(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GRID_LENGTH; i++)
            {
                for (int j = 0; j < GRID_LENGTH; j++)
                {
                    if (board[i, j] != null)
                    {
                        board[i, j].Draw(spriteBatch, calculatePiecePixelPosition(new Vector2(i, j)));
                    }
                }
            }
        }

        public bool isEmpty(Vector2 gridSpot)
        {
            return getChessPieceAt(gridSpot) == null;
        }

        public bool isInBounds(Vector2 gridSpot)
        {
            return !(gridSpot.X > 7 || gridSpot.Y > 7 ||
                      gridSpot.X < 0 || gridSpot.Y < 0);
        }

        public Player getChessPiecePlayerAt(Vector2 gridLocation)
        {
            return getChessPieceAt(gridLocation).Player;
        }

        public bool movesLeft(Player player)
        {
            for (int i = 0; i < GRID_LENGTH; i++)
            {
                for (int j = 0; j < GRID_LENGTH; j++)
                {
                    Vector2 spot = new Vector2(i, j);
                    if ((!isEmpty(spot)) && getChessPiecePlayerAt(spot) == player)
                    {
                        List<Vector2> possMoves = new List<Vector2>();
                        possMoves = getChessPieceAt(spot).FindPossibleMoves(this, spot);
                        possMoves = removeMovesThatPutKingInDanger(spot, possMoves);
                        if (possMoves.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool isMouseClickInBoard(Vector2 mousePosition)
        {
            return mousePosition.X > CHESSBOARD_BORDER && mousePosition.X < (CHESSBOARD_BORDER + (GRID_LENGTH * SQUARE_WIDTH)) &&
                   mousePosition.Y > CHESSBOARD_BORDER && mousePosition.Y < (CHESSBOARD_BORDER + (GRID_LENGTH * SQUARE_WIDTH));
        }

        public Vector2 calculateMouseGridPosition(Vector2 pixelPosition)
        {
            int x = -1;
            int y = -1;

            for (int i = 0; i < GRID_LENGTH; i++)
            {
                int start = CHESSBOARD_BORDER + (SQUARE_WIDTH * i);
                int end = CHESSBOARD_BORDER + (SQUARE_WIDTH * (i + 1));

                if (pixelPosition.X > start && pixelPosition.X <= end)
                {
                    x = i;
                }
                if (pixelPosition.Y > start && pixelPosition.Y <= end)
                {
                    y = i;
                }
            }

            return new Vector2(x, y);
        }

        public void showPossibleMoves(Vector2 gridSpot)
        {
            if (!isEmpty(gridSpot))
            {
                selectedPieceGridSpot = gridSpot;
                possibleMoves = getChessPieceAt(gridSpot).FindPossibleMoves(this, gridSpot);
                possibleMoves = removeMovesThatPutKingInDanger(gridSpot, possibleMoves);
            }
        }

        private List<Vector2> removeMovesThatPutKingInDanger(Vector2 position, List<Vector2> posMov)
        {
            for (int i = posMov.Count - 1; i >= 0; i--)
            {
                bool eatable = false;
                Chessboard copy = this.boardCopyWithMove(position, posMov[i]);
                for (int m = 0; m < GRID_LENGTH; m++)
                {
                    for (int n = 0; n < GRID_LENGTH; n++)
                    {
                        if ((copy.getChessPieceAt(new Vector2(m, n)) != null) &&
                             copy.getChessPiecePlayerAt(new Vector2(m, n)) != getChessPiecePlayerAt(position))
                        {
                            List<Vector2> enemyMoves = copy.getChessPieceAt(new Vector2(m, n)).FindPossibleMoves(copy, new Vector2(m, n));
                            for (int k = 0; k < enemyMoves.Count; k++)
                            {
                                if (copy.getChessPieceAt(enemyMoves[k]) is King)
                                {
                                    eatable = true;
                                }
                            }
                        }
                    }
                }
                if (eatable)
                {
                    posMov.RemoveAt(i);
                }
            }

            return posMov;
        }

        public bool moveChessPieceSuccessful(Vector2 newGridSpot)
        {
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                if (newGridSpot == possibleMoves[i])
                {
                    ChessPiece pieceToMove = getChessPieceAt(selectedPieceGridSpot);

                    board[(int)selectedPieceGridSpot.X, (int)selectedPieceGridSpot.Y] = null;
                    board[(int)newGridSpot.X, (int)newGridSpot.Y] = pieceToMove;
                    possibleMoves = new List<Vector2>();
                    return true;
                }
            }

            return false;
        }

        public Chessboard boardCopyWithMove(Vector2 currentPosition, Vector2 newPosition)
        {
            Chessboard boardCopy = Clone();
            ChessPiece piece = boardCopy.getChessPieceAt(currentPosition);
            boardCopy.board[(int)currentPosition.X, (int)currentPosition.Y] = null;
            boardCopy.board[(int)newPosition.X, (int)newPosition.Y] = piece;

            return boardCopy;
        }

        private void setUpBoard()
        {
            addRowOfChessPieces(0, Player.PLAYER1);

            for (int i = 0; i < GRID_LENGTH; i++)
            {
                board[i, 1] = new Pawn(Player.PLAYER1);
                board[i, 6] = new Pawn(Player.PLAYER2);
            }

            addRowOfChessPieces(7, Player.PLAYER2);
        }

        private void addRowOfChessPieces(int boardY, Player player)
        {
            board[0, boardY] = new Rook(player);
            board[1, boardY] = new Knight(player);
            board[2, boardY] = new Bishop(player);
            board[3, boardY] = new King(player);
            board[4, boardY] = new Queen(player);
            board[5, boardY] = new Bishop(player);
            board[6, boardY] = new Knight(player);
            board[7, boardY] = new Rook(player);
        }

        private ChessPiece getChessPieceAt(Vector2 gridLocation)
        {
            return board[(int)gridLocation.X, (int)gridLocation.Y];
        }

        private Vector2 calculatePiecePixelPosition(Vector2 gridSpot)
        {
            return new Vector2(CHESSBOARD_BORDER + PIECE_OFFSET_X + (gridSpot.X * SQUARE_WIDTH),
                               CHESSBOARD_BORDER + PIECE_OFFSET_Y + (gridSpot.Y * SQUARE_WIDTH));
        }

        private Vector2 calculateBoardSquarePixelPosition(Vector2 gridSpot)
        {
            return new Vector2(CHESSBOARD_BORDER + (gridSpot.X * SQUARE_WIDTH),
                               CHESSBOARD_BORDER + (gridSpot.Y * SQUARE_WIDTH));
        }
    }
}