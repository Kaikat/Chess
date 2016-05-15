using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Chess
{
    public class ChessGame
    {
        private Chessboard chessboard;
        private Player currentPlayer;
        private SpriteFont font;
        private String gameMessage;

        private const int WINDOW_SIZE = 650;

        public ChessGame()
        {
            chessboard = new Chessboard();
            currentPlayer = Player.PLAYER1;
            gameMessage = "Player 1's Turn";
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("ArialBold");
            chessboard.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            chessboard.Draw(spriteBatch);
            spriteBatch.DrawString(font, gameMessage,
                new Vector2(WINDOW_SIZE / 2 - font.MeasureString(gameMessage).X / 2,
                            WINDOW_SIZE - font.MeasureString(gameMessage).Y),
                Color.White);
        }

        public void ProcessMousePosition(Vector2 mousePosition)
        {
            if (chessboard.isMouseClickInBoard(mousePosition))
            {
                Vector2 mouseGridPosition = chessboard.calculateMouseGridPosition(mousePosition);

                gameMessage = currentPlayer == Player.PLAYER1 ? "Player 1's Turn" : "Player 2's Turn";

                if ((!chessboard.isEmpty(mouseGridPosition)) && chessboard.getChessPiecePlayerAt(mouseGridPosition) == currentPlayer)
                {
                    chessboard.showPossibleMoves(mouseGridPosition);
                }
                else if (chessboard.moveChessPieceSuccessful(mouseGridPosition))
                {
                    currentPlayer = currentPlayer == Player.PLAYER1 ? Player.PLAYER2 : Player.PLAYER1;
                }

                if (!chessboard.movesLeft(currentPlayer))
                {
                    gameMessage = currentPlayer == Player.PLAYER1 ? "Player 2 " : "Player 1 ";
                    gameMessage += "Won! Congratulations!";
                }
            }
        }
    }
}