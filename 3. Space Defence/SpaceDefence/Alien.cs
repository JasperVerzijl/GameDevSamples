using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SpaceDefence
{
    internal class Alien : GameObject
    {
        private CircleCollider _circleCollider;
        private Texture2D _texture;
        private float playerClearance = 100;
        private float speed = 3.0f; // Speed of the alien
        GameManager gm = GameManager.GetGameManager();

        public Alien()
        {

        }

        public override void Load(ContentManager content)
        {
            base.Load(content);
            _texture = content.Load<Texture2D>("Alien");
            _circleCollider = new CircleCollider(Vector2.Zero, _texture.Width / 2);
            SetCollider(_circleCollider);
            RandomMove();
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Bullet)
            {
                RandomMove();
            }
            if (other is Laser)
            {
                RandomMove();
            }
            if (other == gm.Player)
            {
                speed = 2.5f;
                RandomMove();
            }
            base.OnCollision(other);
        }

        public void RandomMove()
        {
            speed += 0.5f;
            _circleCollider.Center = gm.RandomScreenLocation();

            Vector2 centerOfPlayer = gm.Player.GetPosition().Center.ToVector2();
            while ((_circleCollider.Center - centerOfPlayer).Length() < playerClearance)
                _circleCollider.Center = gm.RandomScreenLocation();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 playerPosition = gm.Player.GetPosition().Center.ToVector2();
            Vector2 direction = playerPosition - _circleCollider.Center;
            direction.Normalize();
            _circleCollider.Center += direction * speed;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _circleCollider.GetBoundingBox(), Color.White);
            base.Draw(gameTime, spriteBatch);
        }
    }
}
