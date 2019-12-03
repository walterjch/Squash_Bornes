using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SquashClass
{
    class Ball
    {
        private Texture2D texture;
        private bool touchBorderRight;
        private bool moveBall;

        public Vector2 ballDirection;
        public Vector2 position;
        public float ballSpeed;

        public Texture2D Texture { get => texture; set => texture = value; }
        public int Width { get => Texture.Width; }
        public int Height { get => Texture.Height; }
        public bool TouchBorderRight { get => touchBorderRight; set => touchBorderRight = value; }
        public bool MoveBall { get => moveBall; set => moveBall = value; }

        public void Initialize(Texture2D _texture, Vector2 _position)
        {
            this.Texture = _texture;
            this.position = _position;
        }

        public void Draw(SpriteBatch spritebatch, Color color, float rot)
        {
            spritebatch.Draw(Texture, position, null, color, rot, new Vector2(Texture.Width / 2, Texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }

        public void deplacementBall(GameTime gameTime, GraphicsDevice gd)
        {
            if (MoveBall)
                position += ballDirection * ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            borderBlock(gd);
        }

        public void borderBlock(GraphicsDevice gd)
        {
            position.X = MathHelper.Clamp(position.X, -10, gd.Viewport.Width - Width / 2);
            position.Y = Math.Min(Math.Max(Height / 2, position.Y), gd.Viewport.Height -Height / 2);
        }

        public Rectangle getBallRect()
        {
            return new Rectangle((int)position.X, (int)position.Y, Width, Height);
        }
    }
}
