using Chess_Client.Chess_Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess_Client
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Player1 player1 = new Player1();
        Player2 player2 = new Player2();

        Texture2D board;
        Texture2D boardCoords;

        bool debugMode;
        KeyboardState kStateOld;


        int playerTurn = 1;
        int playerTurnOld;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            board = Content.Load<Texture2D>("board");
            boardCoords = Content.Load<Texture2D>("board coords");
            player1.LoadContent(Content, GraphicsDevice);
            player2.LoadContent(Content, GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.G) && !kStateOld.IsKeyDown(Keys.G))
            {
                debugMode = !debugMode;
                Debug.WriteLine("Debug Mode: " + debugMode);
            }

            if (playerTurn == 1)
                playerTurn = player1.Update(debugMode, player2.allPiecePositions);
            if (playerTurn == 2)
                playerTurn = player2.Update(debugMode, player1.allPiecePositions);

            if (playerTurn == 1 && playerTurnOld == 2)
            {
                player2.SetPiecePositions();
                player1.CheckForCaptures(player2.lastPiecePosX, player2.lastPiecePosY);
            }
            if (playerTurn == 2 && playerTurnOld == 1)
            {
                player1.SetPiecePositions();
                player2.CheckForCaptures(player1.lastPiecePosX, player1.lastPiecePosY);
            }
            
            

            playerTurnOld = playerTurn;
            kStateOld = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _spriteBatch.Draw(board, new Rectangle(0, 0, 800, 800), Color.White);

            player1.Draw(_spriteBatch, playerTurn);
            player2.Draw(_spriteBatch, playerTurn);
            player1.DrawValidMoves(_spriteBatch, playerTurn);
            player2.DrawValidMoves(_spriteBatch, playerTurn);

            if (debugMode)
                _spriteBatch.Draw(boardCoords, new Rectangle(0, 0, 800, 800), Color.White);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}