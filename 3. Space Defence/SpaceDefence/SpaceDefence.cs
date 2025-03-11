using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace SpaceDefence
{
    public class SpaceDefence : Game
    {
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        public SpaceDefence()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;

            // Set the size of the screen
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1000;
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Initialize the GameManager
            _gameManager = GameManager.GetGameManager();
            base.Initialize();

            // Place the player at the center of the screen
            Ship player = new Ship(new Point(GraphicsDevice.Viewport.Width/2,GraphicsDevice.Viewport.Height/2));

            // Add the starting objects to the GameManager
            _gameManager.Initialize(Content, this, player);
            _gameManager.AddGameObject(player);
            _gameManager.AddGameObject(new Alien());
            _gameManager.AddGameObject(new Supply());
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameManager.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            _gameManager.Update(gameTime);
            base.Update(gameTime);
            KeyboardState kstate = Keyboard.GetState();
            // move the spaceship
            if (kstate.IsKeyDown(Keys.Up))
            {
                _gameManager.Player.MoveUp();
            }
            if (kstate.IsKeyDown(Keys.Down))
            {
                _gameManager.Player.MoveDown();
            }
            if (kstate.IsKeyDown(Keys.Left))
            {
                _gameManager.Player.MoveLeft();
            }
            if (kstate.IsKeyDown(Keys.Right))
            {
                _gameManager.Player.MoveRight();
            }


            // Screen wrapping logic
            Rectangle playerPosition = _gameManager.Player.GetPosition();
            Point newPosition = playerPosition.Location;
            int shipWidth = _gameManager.Player.GetWidth();
            int shipHeight = _gameManager.Player.GetHeight();

            if (playerPosition.Y + shipHeight < 0)
            {
                newPosition.Y = GraphicsDevice.Viewport.Height;
            }
            else if (playerPosition.Y > GraphicsDevice.Viewport.Height)
            {
                newPosition.Y = -shipHeight;
            }

            if (playerPosition.X + shipWidth < 0)
            {
                newPosition.X = GraphicsDevice.Viewport.Width;
            }
            else if (playerPosition.X > GraphicsDevice.Viewport.Width)
            {
                newPosition.X = -shipWidth;
            }

            // Update the player's position
            _gameManager.Player.UpdatePosition(newPosition);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _gameManager.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
