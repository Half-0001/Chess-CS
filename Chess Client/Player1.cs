using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess_Client
{
    internal class Player1
    {
        Vector2 position;

        //enum
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
        
        //rectangles
        Rectangle pieceRect = new Rectangle(0, 0, 90, 90);
        Rectangle mousePos = new Rectangle(0, 0, 10, 10);

        List<Player1> player1 = new List<Player1>();
        Vector2[,] board = new Vector2[8, 8]{ //board co-ords
                                {new Vector2(5, 5), new Vector2(105, 5), new Vector2(205, 5), new Vector2(320, 5), new Vector2(420, 5), new Vector2(520, 5), new Vector2(620, 5), new Vector2(720, 5)},
                                {new Vector2(5, 105), new Vector2(105, 105), new Vector2(205, 105), new Vector2(305, 105), new Vector2(405, 105), new Vector2(505, 105), new Vector2(605, 105), new Vector2(705, 105)},
                                {new Vector2(5, 205), new Vector2(105, 205), new Vector2(205, 205), new Vector2(305, 205), new Vector2(405, 205), new Vector2(505, 205), new Vector2(605, 205), new Vector2(705, 205)},
                                {new Vector2(5, 305), new Vector2(105, 305), new Vector2(205, 305), new Vector2(305, 305), new Vector2(405, 305), new Vector2(505, 305), new Vector2(605, 305), new Vector2(705, 305)},
                                {new Vector2(5, 405), new Vector2(105, 405), new Vector2(205, 405), new Vector2(305, 405), new Vector2(405, 405), new Vector2(505, 405), new Vector2(605, 405), new Vector2(705, 405)},
                                {new Vector2(5, 505), new Vector2(105, 505), new Vector2(205, 505), new Vector2(305, 505), new Vector2(405, 505), new Vector2(505, 505), new Vector2(605, 505), new Vector2(705, 505)},
                                {new Vector2(5, 605), new Vector2(105, 605), new Vector2(205, 605), new Vector2(305, 605), new Vector2(405, 605), new Vector2(505, 605), new Vector2(605, 605), new Vector2(705, 605)},
                                {new Vector2(5, 705), new Vector2(105, 705), new Vector2(205, 705), new Vector2(305, 705), new Vector2(405, 705), new Vector2(505, 705), new Vector2(605, 705), new Vector2(705, 705)},
                            };

        //Textures
        Texture2D W_Bishop;
        Texture2D W_King;
        Texture2D W_Rook;
        Texture2D W_Pawn;
        Texture2D W_Queen;
        Texture2D W_Knight;
        Texture2D _texture; //blank texture

        //other variable
        MouseState mState;
        MouseState mStateOld;

        int selectedPiece = 100;
        bool hasBeenMoved;
        int piecePosX;
        int piecePosY;

        List<int> validMovesX = new List<int>();
        List<int> validMovesY = new List<int>();

        List<Color> rectColours = new List<Color>();
        Color SemiTransparent = new Color(225, 232, 149, 255);


        public Player1()
        {
            //constructor for the start of game1.cs
        }
        public Player1(int startPosY, int startPosX, Type pieceType)
        {
            Debug.WriteLine("Player1 Constructor Triggered");
            piecePosX = startPosX;
            piecePosY = startPosY;
            position = board[startPosY, startPosX];
            pieceRect.X = (int)position.X;
            pieceRect.Y = (int)position.Y;
            type = pieceType;
            hasBeenMoved = false;
            Debug.WriteLine("Creating Type: "+pieceType);
        }

        public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
        {
            //load pictures of pieces
            W_Bishop = Content.Load<Texture2D>("White/W_Bishop");
            W_King = Content.Load<Texture2D>("White/W_King");
            W_Rook = Content.Load<Texture2D>("White/W_Rook");
            W_Pawn = Content.Load<Texture2D>("White/W_Pawn");
            W_Queen = Content.Load<Texture2D>("White/W_Queen");
            W_Knight = Content.Load<Texture2D>("White/W_Knight");

            //blank texture 
            _texture = new Texture2D(_graphics, 1, 1);
            _texture.SetData(new Color[] { Color.White });

            //load pieces in their correct place
            for (int i = 0; i < 8; i++)
            {
                player1.Add(new Player1(6, i, Type.Pawn));
            }
            for (int i = 0; i < 8; i += 7)
            {
                player1.Add(new Player1(3, i, Type.Rook));
            }
            for (int i = 1; i < 7; i += 5)
            {
                player1.Add(new Player1(7, i, Type.Knight));
            }
            for (int i = 2; i < 6; i += 3)
            {
                player1.Add(new Player1(7, i, Type.Bishop));
            }
            player1.Add(new Player1(7, 3, Type.Queen));
            player1.Add(new Player1(7, 4, Type.King));
            for (int i = 0; i < 16; i++)
            {
                rectColours.Add(Color.Black); 
            }
        }

        public void Update(bool debugMode)
        {
            mState = Mouse.GetState();
            mousePos.X = (int)mState.X;
            mousePos.Y = (int)mState.Y;

            for (int i = 0; i < player1.Count; i++) //update every pieces rect to their position
            {
                player1[i].pieceRect.X = (int)player1[i].position.X;
                player1[i].pieceRect.Y = (int)player1[i].position.Y;
            }
            
            if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < player1.Count; i++)
                {
                    if (mousePos.Intersects(player1[i].pieceRect))
                    {
                        if (i != selectedPiece) //if a new piece is selected
                        {
                            validMovesX.Clear();
                            validMovesY.Clear();
                            if (selectedPiece != 100)
                                rectColours[selectedPiece] = Color.Black;
                            selectedPiece = i;
                            rectColours[i] = SemiTransparent;
                            CheckValidMoves(debugMode);
                            if (debugMode)
                            {
                                for (int j = 0; j < validMovesX.Count; j++)
                                {
                                    Debug.WriteLine("Valid Moves: " + validMovesX[j] + "," + validMovesY[j]);
                                }
                                break;
                            }
                        }
                    }
                }
            }

            mStateOld = mState;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            //_spriteBatch.Draw(_texture, mousePos, Color.Black);
            for (int i = 0; i < player1.Count; i++) //draw pieces by type
            {
                if (rectColours[i] != Color.Black)
                    _spriteBatch.Draw(_texture, player1[i].pieceRect, rectColours[i]);
                if (player1[i].type == Type.Pawn)
                    _spriteBatch.Draw(W_Pawn, player1[i].pieceRect, Color.White);
                if (player1[i].type == Type.Rook)
                    _spriteBatch.Draw(W_Rook, player1[i].pieceRect, Color.White);
                if (player1[i].type == Type.Queen)
                    _spriteBatch.Draw(W_Queen, player1[i].pieceRect, Color.White);
                if (player1[i].type == Type.King)
                    _spriteBatch.Draw(W_King, player1[i].pieceRect, Color.White);
                if (player1[i].type == Type.Bishop)
                    _spriteBatch.Draw(W_Bishop, player1[i].pieceRect, Color.White);
                if (player1[i].type == Type.Knight)
                    _spriteBatch.Draw(W_Knight, player1[i].pieceRect, Color.White);

            }
        }

        public void CheckValidMoves(bool debugMode)
        {
            if (player1[selectedPiece].type == Type.Pawn)
            {
                if (player1[selectedPiece].hasBeenMoved == false)
                {
                    validMovesY.Add(player1[selectedPiece].piecePosY - 1);
                    validMovesX.Add(player1[selectedPiece].piecePosX);
                    validMovesY.Add(player1[selectedPiece].piecePosY - 2);
                    validMovesX.Add(player1[selectedPiece].piecePosX);
                }
                if (player1[selectedPiece].hasBeenMoved == true)
                {
                    validMovesY.Add(player1[selectedPiece].piecePosY - 1);
                    validMovesX.Add(player1[selectedPiece].piecePosX);
                }
            }
            if (player1[selectedPiece].type == Type.Knight)
            {
                validMovesX.Add(player1[selectedPiece].piecePosX - 1);
                validMovesY.Add(player1[selectedPiece].piecePosY - 2);

                validMovesX.Add(player1[selectedPiece].piecePosX + 1);
                validMovesY.Add(player1[selectedPiece].piecePosY - 2);

                validMovesX.Add(player1[selectedPiece].piecePosX - 2);
                validMovesY.Add(player1[selectedPiece].piecePosY - 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 2);
                validMovesY.Add(player1[selectedPiece].piecePosY - 1);

                validMovesX.Add(player1[selectedPiece].piecePosX - 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 2);

                validMovesX.Add(player1[selectedPiece].piecePosX + 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 2);

                validMovesX.Add(player1[selectedPiece].piecePosX - 2);
                validMovesY.Add(player1[selectedPiece].piecePosY + 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 2);
                validMovesY.Add(player1[selectedPiece].piecePosY + 1);
            }

            if (player1[selectedPiece].type == Type.King)
            {
                validMovesX.Add(player1[selectedPiece].piecePosX - 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 0);
                validMovesY.Add(player1[selectedPiece].piecePosY + 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 0);

                validMovesX.Add(player1[selectedPiece].piecePosX + 1);
                validMovesY.Add(player1[selectedPiece].piecePosY - 1);

                validMovesX.Add(player1[selectedPiece].piecePosX + 0);
                validMovesY.Add(player1[selectedPiece].piecePosY - 1);

                validMovesX.Add(player1[selectedPiece].piecePosX - 1);
                validMovesY.Add(player1[selectedPiece].piecePosY - 1);

                validMovesX.Add(player1[selectedPiece].piecePosX - 1);
                validMovesY.Add(player1[selectedPiece].piecePosY + 0);
            }

            if (player1[selectedPiece].type == Type.Rook)
            {
                List<string> allPiecePositions = new List<string>();
                for (int i = 0; i < player1.Count; i++)
                {
                    if (i != selectedPiece)
                        allPiecePositions.Add(player1[i].piecePosX +"," + player1[i].piecePosY); 
                }
                for (int i = 0; i < 8; i++)
                {
                    if (!allPiecePositions.Contains(player1[selectedPiece].piecePosX + "," + player1[selectedPiece].piecePosY + 1))
                    {
                        validMovesX.Add(player1[selectedPiece].piecePosX);
                        validMovesY.Add(player1[selectedPiece].piecePosY + 1 + i);
                    }
                    else
                        break;
                }
                for (int i = 8; i < 0; i--)
                {
                    if (!allPiecePositions.Contains(player1[selectedPiece].piecePosX + "," + player1[selectedPiece].piecePosY + 1))
                    {
                        validMovesX.Add(player1[selectedPiece].piecePosX);
                        validMovesY.Add(player1[selectedPiece].piecePosY - 1);
                    }
                    else
                        break;
                }

            }





            int k = 0;
            while (k < validMovesX.Count)  //remove moves that are outside of the board - use while loop to ensure no pieces are missed
            {
                if (validMovesX[k] < 0 || validMovesX[k] > 7)
                {
                    validMovesX.RemoveAt(k);
                    validMovesY.RemoveAt(k);
                    k = 0;
                }
                if (validMovesY[k] > 7 || validMovesY[k] < 0)
                {
                    validMovesX.RemoveAt(k);
                    validMovesY.RemoveAt(k);
                    k = 0;
                }
                else
                    k++;
            }

            if (debugMode)
                Debug.WriteLine("Valid Moves Added");
        }
    }
}
