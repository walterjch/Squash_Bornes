/*
 * Nom : Nguyen Kelly
 * Date : 15.10.19
 * P3B
 * v.2
 * Squash games
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SquashClass
{
    public class Game1 : Game
    {
        //CONST
        const float ACCELERATION = 0.1f;
        const float PLAYERSPEED = 300f;
        const float BALLSPEED = 100f;

        //Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D background;
        Texture2D arrows;

        Player p1;
        Player p2;
        Ball b;

        bool lose = false;
        float dist;
        float sin;
        float rotation;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            p1 = new Player();
            p2 = new Player();
            b = new Ball();

            b.MoveBall = false;
            p1.TurnPlayer = true;
            p2.TurnPlayer = false;
            b.TouchBorderRight = false;
            p1.PlayerSpeed = PLAYERSPEED;
            p2.PlayerSpeed = PLAYERSPEED;
            b.ballSpeed = BALLSPEED;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            p1.position = new Vector2(10, 150);
            p1.Initialize(Content.Load<Texture2D>("player"), p1.position);

            p2.position = new Vector2(10, GraphicsDevice.Viewport.Height - 150);
            p2.Initialize(Content.Load<Texture2D>("player"), p2.position);

            b.position = new Vector2(GraphicsDevice.Viewport.Width - 50, GraphicsDevice.Viewport.Height / 2);
            b.Initialize(Content.Load<Texture2D>("ball"), b.position);
            b.ballDirection = new Vector2(5, 0);

            font = Content.Load<SpriteFont>("font");
            background = Content.Load<Texture2D>("ground");
            arrows = Content.Load<Texture2D>("arrow");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kstate = Keyboard.GetState();
            Keys[] tabKey = new Keys[] { Keys.D8, Keys.Space, Keys.F, Keys.G, Keys.H, Keys.V, Keys.B, Keys.N, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6 };

            UpdateCollision(gameTime);
            rotation += ACCELERATION;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || Keyboard.GetState().IsKeyDown(Keys.D0))
                Exit();

            if (kstate.IsKeyDown(Keys.D6))
                resetScore();

            if (lose)
                return;

            startGame(tabKey);
            p1.move(gameTime, kstate, GraphicsDevice, Keys.Up, Keys.Left, Keys.Down, Keys.Right);
            p2.move(gameTime, kstate, GraphicsDevice, Keys.W, Keys.A, Keys.S, Keys.D);
            b.deplacementBall(gameTime, GraphicsDevice);

            //Collision a droite de l'écran
            if (b.position.X >= GraphicsDevice.Viewport.Width - b.Width)
            {
                b.ballDirection.X = -b.ballDirection.X;
                b.TouchBorderRight = true;
            }

            //Collision haut et bas de l'écran
            if (b.position.Y >= GraphicsDevice.Viewport.Height - b.Height / 2 || b.position.Y <= b.Height / 2)
                b.ballDirection.Y = -b.ballDirection.Y;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.DrawString(font, p1.ScorePlayer.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 120, 0), Color.Aquamarine);
            spriteBatch.DrawString(font, p2.ScorePlayer.ToString(), new Vector2(GraphicsDevice.Viewport.Width - 60, 0), Color.DeepPink);

            p1.Draw(spriteBatch, Color.Aquamarine);
            p2.Draw(spriteBatch, Color.LightPink);

            if (!b.MoveBall)
                spriteBatch.Draw(arrows, new Vector2((GraphicsDevice.Viewport.Width - arrows.Width) / 2, GraphicsDevice.Viewport.Height - arrows.Height), Color.White);
            
            //Change couleur de la balle en fonction du tour du joueur
            if (p1.TurnPlayer)
                b.Draw(spriteBatch, Color.Aquamarine, rotation);
            else
                b.Draw(spriteBatch, Color.LightPink, rotation);

            //Collision gauche
            if (b.position.X < 0)
            {
                //Affiche le joueur qui a perdu
                if (p1.TurnPlayer)
                {
                    spriteBatch.DrawString(font, "Player 1 Lose", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 50), Color.Black);
                    p2.ScorePlayer++;
                }

                if (p2.TurnPlayer)
                {
                    spriteBatch.DrawString(font, "Player 2 Lose", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, GraphicsDevice.Viewport.Height / 2 - 50), Color.Black);
                    p1.ScorePlayer++;
                }
                lose = true;
                b.MoveBall = false;
                reset();
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void UpdateCollision(GameTime gametime)
        {
            Rectangle playerRect = p1.getPlayerRect();
            Rectangle player2Rect = p2.getPlayerRect();
            Rectangle ballRect = b.getBallRect();

            dist = ballRect.Y - playerRect.Y;

            //Si le joueur entre en collision avec la balle alors
            if (playerRect.Intersects(ballRect) && p1.TurnPlayer && b.TouchBorderRight)
            {
                b.ballSpeed += ACCELERATION;
                p1.PlayerSpeed += ACCELERATION;
                p2.PlayerSpeed += ACCELERATION;
                b.TouchBorderRight = false;
                if (dist < p1.Height / 2)
                {
                    sin = (float)Math.Asin(dist / 100);
                    sin *= -57.2958f;
                    b.ballDirection.Y = sin / 10;
                }

                if (dist > p1.Height / 2)
                {
                    sin = (float)Math.Asin(dist / 100);
                    sin *= 57.2958f;
                    b.ballDirection.Y = sin / 10;
                }
                b.ballDirection.X = -b.ballDirection.X;
                p1.TurnPlayer = false;
                p2.TurnPlayer = true;
            }

            dist = ballRect.Y - player2Rect.Y;

            if (player2Rect.Intersects(ballRect) && p2.TurnPlayer && b.TouchBorderRight)
            {
                b.ballSpeed += ACCELERATION;
                p1.PlayerSpeed += ACCELERATION;
                p2.PlayerSpeed += ACCELERATION;
                b.TouchBorderRight = false;
                if (dist < p2.Height / 2)
                {
                    sin = (float)Math.Asin(dist / 100);
                    sin *= -57.2958f;
                    b.ballDirection.Y = sin / 10;
                }

                if (dist > p2.Height / 2)
                {
                    sin = (float)Math.Asin(dist / 100);
                    sin *= 57.2958f;
                    b.ballDirection.Y = sin / 10;
                }

                b.ballDirection.X = -b.ballDirection.X;
                p1.TurnPlayer = true;
                p2.TurnPlayer = false;
            }
        }

        private void resetScore()
        {
            p1.ScorePlayer = 0;
            p2.ScorePlayer = 0;
            reset();
        }

        private void reset()
        {
            lose = false;
            b.TouchBorderRight = true;
            b.MoveBall = false;

            p1.TurnPlayer = true;
            p2.TurnPlayer = false;
            b.ballSpeed = BALLSPEED;
            p1.PlayerSpeed = PLAYERSPEED;
            p2.PlayerSpeed = PLAYERSPEED;

            b.ballDirection = new Vector2(5, 0);
            b.position = new Vector2(GraphicsDevice.Viewport.Width - 50, GraphicsDevice.Viewport.Height / 2);
            p1.position = new Vector2(0, 150);
            p2.position = new Vector2(0, GraphicsDevice.Viewport.Height - 150);
        }
        private void startGame(params Keys[] keys)
        {
            var kstate = Keyboard.GetState();
            foreach (Keys k in keys)
            {
                if (kstate.IsKeyDown(k))
                    b.MoveBall = true;
            }
        }
    }
}
