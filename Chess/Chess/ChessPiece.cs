using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Chess
{
    abstract public class ChessPiece
    {
        protected Texture2D chessPieceTexture;
        private string name;

        public Player Player { get; private set; }

        protected ChessPiece(Player player, string name)
        {
            Player = player;
            this.name = name;
        }

        public void LoadContent(ContentManager content)
        {
            chessPieceTexture = Player == Player.PLAYER1 ? content.Load<Texture2D>(name + "_black") :
                                                           content.Load<Texture2D>(name + "_brown");
        }

        public abstract List<Vector2> FindPossibleMoves(Chessboard chessboard, Vector2 gridSpot);

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(chessPieceTexture, position, Color.White);
        }
    }
}