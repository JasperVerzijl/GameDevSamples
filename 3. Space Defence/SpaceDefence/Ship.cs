using System;
using SpaceDefence.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceDefence
{
    public class Ship : GameObject
    {
        private Texture2D ship_body;
        private Texture2D base_turret;
        private Texture2D laser_turret;
        private float buffTimer = 10;
        private float buffDuration = 10f;
        private RectangleCollider _rectangleCollider;
        private Point target;
        private float _rotationAngle;
        public float RotationAngle
        {
            get { return _rotationAngle; }
            set { _rotationAngle = value; }
        }

        /// <summary>
        /// The player character
        /// </summary>
        /// <param name="Position">The ship's starting position</param>
        public Ship(Point Position)
        {
            _rectangleCollider = new RectangleCollider(new Rectangle(Position, Point.Zero));
            SetCollider(_rectangleCollider);
        }

        public override void Load(ContentManager content)
        {
            // Ship sprites from: https://zintoki.itch.io/space-breaker
            ship_body = content.Load<Texture2D>("ship_body");
            base_turret = content.Load<Texture2D>("base_turret");
            laser_turret = content.Load<Texture2D>("laser_turret");
            _rectangleCollider.shape.Size = ship_body.Bounds.Size;
            _rectangleCollider.shape.Location -= new Point(ship_body.Width/2, ship_body.Height/2);
            base.Load(content);
        }

        public override void HandleInput(InputManager inputManager)
        {
            base.HandleInput(inputManager);
            target = inputManager.CurrentMouseState.Position;
            if(inputManager.LeftMousePress())
            {

                Vector2 aimDirection = LinePieceCollider.GetDirection(GetPosition().Center, target);
                Vector2 turretExit = _rectangleCollider.shape.Center.ToVector2() + aimDirection * base_turret.Height / 2f;
                if (buffTimer <= 0)
                {
                    GameManager.GetGameManager().AddGameObject(new Bullet(turretExit, aimDirection, 500));
                }
                else
                {
                    GameManager.GetGameManager().AddGameObject(new Laser(new LinePieceCollider(turretExit, target.ToVector2()),2300));
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update the Buff timer
            if (buffTimer > 0)
                buffTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                ship_body,
                _rectangleCollider.shape.Center.ToVector2(),
                null,
                Color.White,
                _rotationAngle,
                new Vector2(ship_body.Width / 2, ship_body.Height / 2),
                1.0f,
                SpriteEffects.None,
                0f
            );

            // Draw the turret with the same rotation as the ship
            Vector2 turretOrigin = new Vector2(base_turret.Width / 2, base_turret.Height / 2);
            Vector2 turretPosition = _rectangleCollider.shape.Center.ToVector2();

            float aimAngle = LinePieceCollider.GetAngle(LinePieceCollider.GetDirection(GetPosition().Center, target));
            if (buffTimer <= 0)
            {
                Rectangle turretLocation = base_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(base_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            else
            {
                Rectangle turretLocation = laser_turret.Bounds;
                turretLocation.Location = _rectangleCollider.shape.Center;
                spriteBatch.Draw(laser_turret, turretLocation, null, Color.White, aimAngle, turretLocation.Size.ToVector2() / 2f, SpriteEffects.None, 0);
            }
            base.Draw(gameTime, spriteBatch);
        }

        public void MoveUp()
        {
            _rectangleCollider.shape.Y -= 5;
            _rotationAngle = 0; // 0 degrees in radians
        }

        public void MoveDown()
        {
            _rectangleCollider.shape.Y += 5;
            _rotationAngle = MathHelper.Pi; // 180 degrees in radians
        }

        public void MoveLeft()
        {
            _rectangleCollider.shape.X -= 5;
            _rotationAngle = -MathHelper.PiOver2; // 90 degrees in radians
        }

        public void MoveRight()
        {
            _rectangleCollider.shape.X += 5;
            _rotationAngle = MathHelper.PiOver2; // -90 degrees in radians
        }

        public void UpdatePosition(Point newPosition)
        {
            _rectangleCollider.shape.Location = newPosition;
        }

        public int GetWidth()
        {
            return _rectangleCollider.shape.Width;
        }

        public int GetHeight()
        {
            return _rectangleCollider.shape.Height;
        }

        public void Buff()
        {
            buffTimer = buffDuration;
        }

        public Rectangle GetPosition()
        {
            return _rectangleCollider.shape;
        }
    }
}
