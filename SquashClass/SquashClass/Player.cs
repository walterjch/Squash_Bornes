using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SquashClass
{
    class Player
    {
        private Texture2D texture;
        private float playerSpeed;
        private bool turnPlayer;
        public Vector2 position;
        private int scorePlayer;

        public Texture2D Texture { get => texture; set => texture = value; }
        public int Width { get => Texture.Width; }
        public int Height { get => Texture.Height; }
        public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
        public bool TurnPlayer { get => turnPlayer; set => turnPlayer = value; }
        public int ScorePlayer { get => scorePlayer; set => scorePlayer = value; }

        public void Initialize(Texture2D _texture, Vector2 _position)
        {
            this.Texture = _texture;
            this.position = _position;
            ScorePlayer = 0;
        }

        public void Draw(SpriteBatch spritebatch, Color color)
        {
            spritebatch.Draw(Texture, position, null, color, 0f, new Vector2(Texture.Width / 2, Texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }

        public void move(GameTime gameTime, KeyboardState kstate, GraphicsDevice gd, Keys up, Keys left, Keys down, Keys right)
        {
            if (kstate.IsKeyDown(up))
                position.Y -= PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(down))
                position.Y += PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(left))
                position.X -= PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(right))
                position.X += PlayerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            borderBlock(gd);
        }

        public void borderBlock(GraphicsDevice gd)
        {
            position.X = Math.Min(Math.Max(Width / 2, position.X), gd.Viewport.Width - Width / 2);
            position.Y = Math.Min(Math.Max(Height / 2, position.Y), gd.Viewport.Height - Height / 2);
        }

        public Rectangle getPlayerRect()
        {
            return new Rectangle(((int)position.X - Width / 2) + 8, ((int)position.Y - Height / 2) + 8, Width, Height);
        }
    }
}
