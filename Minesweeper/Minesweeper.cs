using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper
{
    public class Minesweeper : Game
    {
        #region Properties and attributes
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Grid grid;
        private Texture2D blank, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5, adjacent6, adjacent7, adjacent8;
        private Texture2D mine, explosion, covered, flagged;
        
        private int mineCount = 10;
        private bool isGameOver = false;

        private KeyboardState prevKState;
        #endregion

        public Minesweeper()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 320;
            graphics.PreferredBackBufferHeight = 320;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            grid = new Grid(10, 10);

            graphics.PreferredBackBufferWidth = grid.Width * grid.TileSize;
            graphics.PreferredBackBufferHeight = grid.Height * grid.TileSize;
            graphics.ApplyChanges();

            grid.Generate(10);

            blank = Content.Load<Texture2D>("blank");
            adjacent1 = Content.Load<Texture2D>("adjacent1");
            adjacent2 = Content.Load<Texture2D>("adjacent2");
            adjacent3 = Content.Load<Texture2D>("adjacent3");
            adjacent4 = Content.Load<Texture2D>("adjacent4");
            adjacent5 = Content.Load<Texture2D>("adjacent5");
            adjacent6 = Content.Load<Texture2D>("adjacent6");
            adjacent7 = Content.Load<Texture2D>("adjacent7");
            adjacent8 = Content.Load<Texture2D>("adjacent8");
            mine = Content.Load<Texture2D>("mine");
            explosion = Content.Load<Texture2D>("explosion");
            covered = Content.Load<Texture2D>("covered");
            flagged = Content.Load<Texture2D>("flagged");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState kState = Keyboard.GetState();

            // resetting
            if (kState.IsKeyDown(Keys.Enter) && !prevKState.IsKeyDown(Keys.Enter))
            {
                ResetGrid();
            }

            if (grid.IsMineUncovered())
            {
                isGameOver = true;
            }
            if (!isGameOver)
            {
                grid.Update(gameTime);
            }

            base.Update(gameTime);

            prevKState = kState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            grid.Draw(spriteBatch, new Texture2D[]
            {
                blank, adjacent1, adjacent2, adjacent3, adjacent4, adjacent5, adjacent6, adjacent7, adjacent8, mine, explosion, covered, flagged
            });

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGrid()
        {
            grid = new Grid(grid.Width, grid.Height);
            grid.Generate(mineCount);
            isGameOver = false;
        }
    }
}
