using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Client
{
    internal class Player1
    {
        //piece variables
        Vector2 position;
        public enum Type
        {
            Pawn,
            Bishop, 
            King, 
            Rook, 
            Queen, 
            Knight,
            Empty,
        }

        public Type type;
        Texture2D pieceTexture;
        Rectangle pieceRect = new Rectangle(0, 0, 60, 60);

        List<Player1> player1 = new List<Player1>();
        Vector2[,] board = new Vector2[8, 8]{ //board co-ords
                                {new Vector2(20, 20), new Vector2(120, 20), new Vector2(220, 20), new Vector2(320, 20), new Vector2(420, 20), new Vector2(520, 20), new Vector2(620, 20), new Vector2(720, 20)},
                                {new Vector2(20, 120), new Vector2(120, 120), new Vector2(220, 120), new Vector2(320, 120), new Vector2(420, 120), new Vector2(520, 120), new Vector2(620, 120), new Vector2(720, 120)},
                                {new Vector2(20, 220), new Vector2(120, 220), new Vector2(220, 220), new Vector2(320, 220), new Vector2(420, 220), new Vector2(520, 220), new Vector2(620, 220), new Vector2(720, 220)},
                                {new Vector2(20, 320), new Vector2(120, 320), new Vector2(220, 320), new Vector2(320, 320), new Vector2(420, 320), new Vector2(520, 320), new Vector2(620, 320), new Vector2(720, 320)},
                                {new Vector2(20, 420), new Vector2(120, 420), new Vector2(220, 420), new Vector2(320, 420), new Vector2(420, 420), new Vector2(520, 420), new Vector2(620, 420), new Vector2(720, 420)},
                                {new Vector2(20, 520), new Vector2(120, 550), new Vector2(220, 520), new Vector2(320, 520), new Vector2(420, 520), new Vector2(520, 520), new Vector2(620, 520), new Vector2(720, 520)},
                                {new Vector2(20, 620), new Vector2(120, 620), new Vector2(220, 620), new Vector2(320, 620), new Vector2(420, 620), new Vector2(520, 620), new Vector2(620, 620), new Vector2(720, 620)},
                                {new Vector2(20, 720), new Vector2(120, 720), new Vector2(220, 720), new Vector2(320, 720), new Vector2(420, 720), new Vector2(520, 720), new Vector2(620, 720), new Vector2(720, 720)},
                            };

        //pieces
        Texture2D W_Bishop;
        Texture2D W_King;
        Texture2D W_Rook;
        Texture2D W_Pawn;
        Texture2D W_Queen;
        Texture2D W_Knight;
        
        public Player1()
        {

        }
        public Player1(Vector2 startPosition, Type pieceType)
        {
            Debug.WriteLine("Player1 Constructor Triggered");
            position = startPosition;
            pieceRect.X = (int)position.X;
            pieceRect.Y = (int)position.Y;
            type = pieceType;
            Debug.WriteLine(pieceType + ", "+ type);

            switch (type)
            {
                case Type.Bishop:
                    pieceTexture = W_Bishop;
                    break;
                case Type.King:
                    pieceTexture = W_King;
                    break;
                case Type.Rook:
                    pieceTexture = W_Rook;
                    break;
                case Type.Pawn:
                    pieceTexture = W_Pawn;
                    Debug.WriteLine("Texture Set Correctly");
                    break;
                case Type.Queen:
                    pieceTexture = W_Queen;
                    break;
                case Type.Knight:
                    pieceTexture = W_Knight;
                    break;
                default:
                    Debug.WriteLine("Error Initializing Texture");
                    break;
            }
        }

        public void LoadContent(ContentManager Content)
        {
            //load pictures of pieces
            W_Bishop = Content.Load<Texture2D>("White/W_Bishop");
            W_King = Content.Load<Texture2D>("White/W_King");
            W_Rook = Content.Load<Texture2D>("White/W_Rook");
            W_Pawn = Content.Load<Texture2D>("White/W_Pawn");
            W_Queen = Content.Load<Texture2D>("White/W_Queen");
            W_Knight = Content.Load<Texture2D>("White/W_Knight");

            //load pieces in their correct place
            for (int i = 0; i < 8; i++)
            {
                player1.Add(new Player1(board[6, i], Type.Pawn));
            }
        }

        public void Update()
        {
            for (int i = 0; i < player1.Count; i++)
            {
                player1[i].pieceRect.X = (int)player1[i].position.X;
                player1[i].pieceRect.Y = (int)player1[i].position.Y;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < player1.Count; i++)
            {
                _spriteBatch.Draw(W_Pawn, player1[i].pieceRect, Color.White);
            }
        }
    }
}
