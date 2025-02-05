using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Minesweeper
{
    public class Grid
    {
        #region Properties and attributes
        public Vector2 Position { get; set; }
        public int Width { get; }
        public int Height { get; }
        public int TileSize { get; set; } = 32;
        public Tile[,] Tiles { get; private set; }
        private MouseState prevMState;
        #endregion

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Tiles = new Tile[width, height];
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Tiles[col, row] = new Tile();
                }
            }
        }

        public void Generate(int mineCount)
        {
            int remainingMines = mineCount;
            Random random = new Random();

            // place mines
            do
            {
                for (int col = 0; col < Width; col++)
                {
                    if (remainingMines == 0)
                        break;
                    for (int row = 0; row < Height; row++)
                    {
                        if (remainingMines > 0 && random.Next(0, Width * Height / mineCount) == 0)
                        {
                            if (Tiles[col, row].TileType != TileType.Mine)
                            {
                                Tiles[col, row].TileType = TileType.Mine;
                                remainingMines--;

                                if (remainingMines == 0)
                                    break;
                            }
                        }
                    }
                }
            } while (remainingMines > 0);

            int[,] numbers = new int[Width, Height];

            // place numbers
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    if (Tiles[col, row].TileType == TileType.Mine)
                    {
                        for (int x = -1; x < 2; x++)
                        {
                            for (int y = -1; y < 2; y++)
                            {
                                if (x == 0 && y == 0)
                                    continue;

                                try
                                {
                                    numbers[col + x, row + y]++;
                                }
                                catch (Exception e)
                                {
                                }
                            }
                        }
                    }
                }
            }

            // assign textures
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    if (Tiles[col, row].TileType != TileType.Mine)
                    {
                        Tiles[col, row].TileType = numbers[col, row] switch
                        {
                            0 => TileType.Blank,
                            1 => TileType.Adjacent1,
                            2 => TileType.Adjacent2,
                            3 => TileType.Adjacent3,
                            4 => TileType.Adjacent4,
                            5 => TileType.Adjacent5,
                            6 => TileType.Adjacent6,
                            7 => TileType.Adjacent7,
                            8 => TileType.Adjacent8,
                            _ => TileType.Blank
                        };
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleMouse();
            prevMState = Mouse.GetState();
        }

        private void HandleMouse()
        {
            MouseState mState = Mouse.GetState();
            Point mousePosition = mState.Position;

            int col, row;

            if (mousePosition.X > 0 && mousePosition.Y > 0)
            {
                col = mousePosition.X / TileSize;
                row = mousePosition.Y / TileSize;
            }
            else
            {
                col = row = -1;
            }
            

            if (col >= 0 && col < Width && row >= 0 && row < Height)
            {
                if (mState.LeftButton == ButtonState.Pressed && prevMState.LeftButton == ButtonState.Released && Tiles[col, row].TileState != TileState.Flagged)
                {
                    UncoverTile(col, row);
                }
                else if (mState.RightButton == ButtonState.Pressed && prevMState.RightButton == ButtonState.Released && Tiles[col, row].TileState != TileState.Uncovered)
                {
                    Flag(col, row);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[] textures)
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Texture2D texture;

                    try
                    {
                        if (Tiles[col, row].TileState == TileState.Covered)
                        {
                            texture = textures[11];
                        }
                        else if (Tiles[col, row].TileState == TileState.Flagged)
                        {
                            texture = textures[12];
                        }
                        else
                        {
                            switch (Tiles[col, row].TileType)
                            {
                                case TileType.Blank:
                                    texture = textures[0];
                                    break;
                                case TileType.Adjacent1:
                                    texture = textures[1];
                                    break;
                                case TileType.Adjacent2:
                                    texture = textures[2];
                                    break;
                                case TileType.Adjacent3:
                                    texture = textures[3];
                                    break;
                                case TileType.Adjacent4:
                                    texture = textures[4];
                                    break;
                                case TileType.Adjacent5:
                                    texture = textures[5];
                                    break;
                                case TileType.Adjacent6:
                                    texture = textures[6];
                                    break;
                                case TileType.Adjacent7:
                                    texture = textures[7];
                                    break;
                                case TileType.Adjacent8:
                                    texture = textures[8];
                                    break;
                                case TileType.Mine:
                                    texture = textures[9];
                                    break;
                                case TileType.Explosion:
                                    texture = textures[10];
                                    break;
                                default:
                                    texture = null;
                                    break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception has occured: {0}", e);
                        texture = null;
                    }

                    if (texture is not null)
                        spriteBatch.Draw(texture, new Rectangle((int)Position.X + col * TileSize, (int)Position.Y + row * TileSize, TileSize, TileSize), Color.White);
                }
            }
        }

        public void Flag(int col, int row)
        {
            if (Tiles[col, row].TileState == TileState.Covered)
            {
                Tiles[col, row].TileState = TileState.Flagged;
            }
            else if (Tiles[col, row].TileState == TileState.Flagged)
            {
                Tiles[col, row].TileState = TileState.Covered;
            }
        }

        // uncovers a single tile
        private void UncoverTile(int col, int row)
        {
            Tiles[col, row].TileState = TileState.Uncovered;
            if (Tiles[col, row].TileType == TileType.Mine)
            {
                Tiles[col, row].TileType = TileType.Explosion;
                UncoverAll();
            }
            else if (Tiles[col, row].TileType == TileType.Blank)
            {
                UncoverBlankAdjacent(col, row);
            }
        }

        // in case of uncovering a blank tile, it uncovers all the adjacent tiles, uses recursion
        private void UncoverBlankAdjacent(int col, int row)
        {
            for (int x = col - 1; x < (col + 2); x++)
            {
                for (int y = row - 1; y < (row + 2); y++)
                {
                    if (x == col && y == row)
                        continue;

                    if (x >= 0 && y >= 0 && x < Width && y < Height)
                    {
                        if (Tiles[x, y].TileState == TileState.Covered)
                        {
                            Tiles[x, y].TileState = TileState.Uncovered;

                            if (Tiles[x, y].TileType == TileType.Blank)
                            {
                                UncoverBlankAdjacent(x, y);
                            }
                        }                       
                    }
                }
            }
        }

        private void UncoverAll()
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Tiles[col, row].TileState = TileState.Uncovered;
                }
            }
        }

        public bool IsMineUncovered()
        {
            for (int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    if (Tiles[col, row].TileType == TileType.Explosion && Tiles[col, row].TileState == TileState.Uncovered)
                        return true;
                }
            }

            return false;
        }
    }

}
