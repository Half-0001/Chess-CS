using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_Client
{
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
        internal class Player2
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

            List<Player2> player2 = new List<Player2>();
            Vector2[,] board = new Vector2[8, 8]{ //board co-ords
                                {new Vector2(5, 5), new Vector2(105, 5), new Vector2(205, 5), new Vector2(305, 5), new Vector2(405, 5), new Vector2(505, 5), new Vector2(605, 5), new Vector2(705, 5)},
                                {new Vector2(5, 105), new Vector2(105, 105), new Vector2(205, 105), new Vector2(305, 105), new Vector2(405, 105), new Vector2(505, 105), new Vector2(605, 105), new Vector2(705, 105)},
                                {new Vector2(5, 205), new Vector2(105, 205), new Vector2(205, 205), new Vector2(305, 205), new Vector2(405, 205), new Vector2(505, 205), new Vector2(605, 205), new Vector2(705, 205)},
                                {new Vector2(5, 305), new Vector2(105, 305), new Vector2(205, 305), new Vector2(305, 305), new Vector2(405, 305), new Vector2(505, 305), new Vector2(605, 305), new Vector2(705, 305)},
                                {new Vector2(5, 405), new Vector2(105, 405), new Vector2(205, 405), new Vector2(305, 405), new Vector2(405, 405), new Vector2(505, 405), new Vector2(605, 405), new Vector2(705, 405)},
                                {new Vector2(5, 505), new Vector2(105, 505), new Vector2(205, 505), new Vector2(305, 505), new Vector2(405, 505), new Vector2(505, 505), new Vector2(605, 505), new Vector2(705, 505)},
                                {new Vector2(5, 605), new Vector2(105, 605), new Vector2(205, 605), new Vector2(305, 605), new Vector2(405, 605), new Vector2(505, 605), new Vector2(605, 605), new Vector2(705, 605)},
                                {new Vector2(5, 705), new Vector2(105, 705), new Vector2(205, 705), new Vector2(305, 705), new Vector2(405, 705), new Vector2(505, 705), new Vector2(605, 705), new Vector2(705, 705)},
                            };

            //Textures
            Texture2D B_Bishop;
            Texture2D B_King;
            Texture2D B_Rook;
            Texture2D B_Pawn;
            Texture2D B_Queen;
            Texture2D B_Knight;
            Texture2D _texture; //blank texture

            //other variable
            MouseState mState;
            MouseState mStateOld;

            int mouseBoardSquareX = 1;
            int mouseBoardSquareY = 1;
            int selectedPiece = 100;
            bool hasBeenMoved;
            int piecePosX;
            int piecePosY;

            List<int> validMovesX = new List<int>();
            List<int> validMovesY = new List<int>();

            List<Color> rectColours = new List<Color>();
            Color SemiTransparent = new Color(225, 232, 149, 255);


            public Player2()
            {
                //constructor for the start of game1.cs
            }
            public Player2(int startPosY, int startPosX, Type pieceType)
            {
                Debug.WriteLine("Player2 Constructor Triggered");
                piecePosX = startPosX;
                piecePosY = startPosY;
                position = board[startPosY, startPosX];
                pieceRect.X = (int)position.X;
                pieceRect.Y = (int)position.Y;
                type = pieceType;
                hasBeenMoved = false;
                Debug.WriteLine("Creating Type: " + pieceType);
            }

            public void LoadContent(ContentManager Content, GraphicsDevice _graphics)
            {
                //load pictures of pieces
                B_Bishop = Content.Load<Texture2D>("Black/B_Bishop");
                B_King = Content.Load<Texture2D>("Black/B_King");
                B_Rook = Content.Load<Texture2D>("Black/B_Rook");
                B_Pawn = Content.Load<Texture2D>("Black/B_Pawn");
                B_Queen = Content.Load<Texture2D>("Black/B_Queen");
                B_Knight = Content.Load<Texture2D>("Black/B_Knight");

                //blank texture 
                _texture = new Texture2D(_graphics, 1, 1);
                _texture.SetData(new Color[] { Color.White });

                //load pieces in their correct place
                for (int i = 0; i < 8; i++)
                {
                    player2.Add(new Player2(1, i, Type.Pawn));
                }
                for (int i = 0; i < 8; i += 7)
                {
                    player2.Add(new Player2(0, i, Type.Rook));
                }
                for (int i = 1; i < 7; i += 5)
                {
                    player2.Add(new Player2(0, i, Type.Knight));
                }
                for (int i = 2; i < 6; i += 3)
                {
                    player2.Add(new Player2(0, i, Type.Bishop));
                }
                player2.Add(new Player2(0, 3, Type.Queen));
                player2.Add(new Player2(0, 4, Type.King));
                for (int i = 0; i < 16; i++)
                {
                    rectColours.Add(Color.Black);
                }
            }

            public int Update(bool debugMode)
            {
                mState = Mouse.GetState();
                mousePos.X = (int)mState.X;
                mousePos.Y = (int)mState.Y;

                for (int i = 0; i < player2.Count; i++) //update every pieces rect to their position
                {
                    player2[i].pieceRect.X = (int)player2[i].position.X;
                    player2[i].pieceRect.Y = (int)player2[i].position.Y;
                }

                if (mState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
                {
                    for (int i = 0; i < player2.Count; i++)
                    {
                        if (mousePos.Intersects(player2[i].pieceRect))
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
                    for (int i = 0; i < validMovesX.Count; i++)
                    {
                        if (validMovesX[i] == mouseBoardSquareX && validMovesY[i] == mouseBoardSquareY)
                        {
                            Debug.WriteLine("Moving");
                            player2[selectedPiece].position.X = board[validMovesY[i], validMovesX[i]].X;
                            player2[selectedPiece].position.Y = board[validMovesY[i], validMovesX[i]].Y;
                            player2[selectedPiece].piecePosX = validMovesX[i];
                            player2[selectedPiece].piecePosY = validMovesY[i];
                            player2[selectedPiece].hasBeenMoved = true;
                            validMovesX.Clear();
                            validMovesY.Clear();
                            CheckValidMoves(debugMode);
                            for (int j = 0; j < player2.Count; j++) //update every pieces rect to their position
                            {
                                player2[j].pieceRect.X = (int)player2[j].position.X;
                                player2[j].pieceRect.Y = (int)player2[j].position.Y;
                            }
                            return 1;
                        }
                    }
                }
                float distanceOld = Vector2.Distance(board[0, 0], new Vector2(mState.Position.X, mState.Position.Y));
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        float distance = Vector2.Distance(new Vector2(board[i, j].X + 35, board[i, j].Y + 35), new Vector2(mState.Position.X, mState.Position.Y));
                        if (distance < distanceOld)
                        {
                            distanceOld = distance;
                            mouseBoardSquareX = j;
                            mouseBoardSquareY = i;
                        }
                    }
                }

                mStateOld = mState;
                return 2;
            }

            public void Draw(SpriteBatch _spriteBatch, int playerTurn)
            {
                //_spriteBatch.Draw(_texture, mousePos, Color.Black);
                //_spriteBatch.Draw(_texture, new Rectangle((int)board[0, mouseBoardSquareX].X, (int)board[mouseBoardSquareY, 0].Y, 80, 80), Color.LightCoral);

                for (int i = 0; i < player2.Count; i++) //draw pieces by type
                {
                    if (playerTurn == 2)
                    {
                        if (rectColours[i] != Color.Black)
                            _spriteBatch.Draw(_texture, player2[i].pieceRect, rectColours[i]);
                    }
                    if (player2[i].type == Type.Pawn)
                        _spriteBatch.Draw(B_Pawn, player2[i].pieceRect, Color.White);
                    if (player2[i].type == Type.Rook)
                        _spriteBatch.Draw(B_Rook, player2[i].pieceRect, Color.White);
                    if (player2[i].type == Type.Queen)
                        _spriteBatch.Draw(B_Queen, player2[i].pieceRect, Color.White);
                    if (player2[i].type == Type.King)
                        _spriteBatch.Draw(B_King, player2[i].pieceRect, Color.White);
                    if (player2[i].type == Type.Bishop)
                        _spriteBatch.Draw(B_Bishop, player2[i].pieceRect, Color.White);
                    if (player2[i].type == Type.Knight)
                        _spriteBatch.Draw(B_Knight, player2[i].pieceRect, Color.White);

                }
                if (playerTurn == 2)
                {
                    for (int i = 0; i < validMovesX.Count; i++)
                    {
                        _spriteBatch.Draw(_texture, new Rectangle((int)board[0, validMovesX[i]].X + 25, (int)board[validMovesY[i], 0].Y + 30, 40, 40), Color.Gray);
                    }
                }
            }

            public void CheckValidMoves(bool debugMode)
            {
                List<string> allPiecePositions = new List<string>();
                for (int i = 0; i < player2.Count; i++)
                {
                    if (i != selectedPiece)
                        allPiecePositions.Add(player2[i].piecePosX + "," + player2[i].piecePosY);
                }

                if (player2[selectedPiece].type == Type.Pawn)
                {
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);
                    validMovesX.Add(player2[selectedPiece].piecePosX);

                    if (player2[selectedPiece].hasBeenMoved == false)
                    {
                        validMovesY.Add(player2[selectedPiece].piecePosY + 2);
                        validMovesX.Add(player2[selectedPiece].piecePosX);
                    }
                }
                if (player2[selectedPiece].type == Type.Knight)
                {
                    validMovesX.Add(player2[selectedPiece].piecePosX - 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 2);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 2);

                    validMovesX.Add(player2[selectedPiece].piecePosX - 2);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 2);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX - 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 2);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 2);

                    validMovesX.Add(player2[selectedPiece].piecePosX - 2);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 2);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);
                }

                if (player2[selectedPiece].type == Type.King)
                {
                    validMovesX.Add(player2[selectedPiece].piecePosX - 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 0);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 0);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX + 0);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX - 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY - 1);

                    validMovesX.Add(player2[selectedPiece].piecePosX - 1);
                    validMovesY.Add(player2[selectedPiece].piecePosY + 0);
                }

                if (player2[selectedPiece].type == Type.Rook || player2[selectedPiece].type == Type.Queen)
                {

                    for (int i = 0; i < 8; i++)
                    {
                        if (!allPiecePositions.Contains(player2[selectedPiece].piecePosX + "," + (player2[selectedPiece].piecePosY + 1 + i)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX);
                            validMovesY.Add(player2[selectedPiece].piecePosY + 1 + i);
                        }
                        else
                            break;
                    }
                    int xd = 0;
                    for (int i = 8; i > 0; i--)
                    {
                        xd++;
                        if (!allPiecePositions.Contains(player2[selectedPiece].piecePosX + "," + (player2[selectedPiece].piecePosY - xd)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX);
                            validMovesY.Add(player2[selectedPiece].piecePosY - xd);
                        }
                        else
                        {
                            break;
                        }

                    }
                    for (int i = 0; i < 8; i++)
                    {
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX + 1 + i) + "," + player2[selectedPiece].piecePosY))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX + 1 + i);
                            validMovesY.Add(player2[selectedPiece].piecePosY);
                        }
                        else
                            break;
                    }
                    xd = 0;
                    for (int i = 8; i > 0; i--)
                    {
                        xd++;
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX - xd) + "," + player2[selectedPiece].piecePosY))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX - xd);
                            validMovesY.Add(player2[selectedPiece].piecePosY);
                        }
                        else
                        {
                            break;
                        }

                    }

                }

                if (player2[selectedPiece].type == Type.Bishop || player2[selectedPiece].type == Type.Queen)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX + 1 + i) + "," + (player2[selectedPiece].piecePosY + 1 + i)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX + 1 + i);
                            validMovesY.Add(player2[selectedPiece].piecePosY + 1 + i);
                        }
                        else
                            break;
                    }
                    int xd = 0;
                    for (int i = 8; i > 0; i--)
                    {
                        xd++;
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX + xd) + "," + (player2[selectedPiece].piecePosY - xd)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX + xd);
                            validMovesY.Add(player2[selectedPiece].piecePosY - xd);
                        }
                        else
                        {
                            break;
                        }

                    }
                    for (int i = 0; i < 8; i++)
                    {
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX - 1 - i) + "," + (player2[selectedPiece].piecePosY - 1 - i)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX - 1 - i);
                            validMovesY.Add(player2[selectedPiece].piecePosY - 1 - i);
                        }
                        else
                            break;
                    }
                    xd = 0;
                    for (int i = 8; i > 0; i--)
                    {
                        xd++;
                        if (!allPiecePositions.Contains((player2[selectedPiece].piecePosX - xd) + "," + (player2[selectedPiece].piecePosY + xd)))
                        {
                            validMovesX.Add(player2[selectedPiece].piecePosX - xd);
                            validMovesY.Add(player2[selectedPiece].piecePosY + xd);
                        }
                        else
                        {
                            break;
                        }

                    }
                }

                int k = 0;
                while (k < validMovesX.Count)  //remove moves that are outside of the board and on top of other pieces
                { //  - use while loop to ensure no pieces are missed
                    if (validMovesY[k] < 0 || validMovesY[k] > 7)
                    {
                        validMovesX.RemoveAt(k);
                        validMovesY.RemoveAt(k);
                        k = 0;
                        continue;
                    }
                    if (validMovesX[k] < 0 || validMovesX[k] > 7)
                    {
                        validMovesX.RemoveAt(k);
                        validMovesY.RemoveAt(k);
                        k = 0;
                        continue;
                    }
                    if (allPiecePositions.Contains(validMovesX[k] + "," + validMovesY[k]))
                    {
                        validMovesX.RemoveAt(k);
                        validMovesY.RemoveAt(k);
                        k = 0;
                        continue;
                    }
                    else
                    {
                        k++;
                    }

                }

                if (debugMode)
                    Debug.WriteLine("Valid Moves Added");
            }
        }
    }
}
