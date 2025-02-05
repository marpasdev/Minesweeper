# Minesweeper

A simple Minesweeper clone made in C# and MonoGame.

The textures are my own work.

At its current state, the game is pretty much playable and fun.

features:
- 10x10 grid
- random placement of 10 mines
- ability to place flags on suspicious tiles
- uncovering a mine will trigger an explosion and will uncover the whole grid while highlighting the exploded mine
- resetting the game at any point (this makes the most sense after gameover and victory)
- uncovering all adjacent blank tiles when a blank tile has been uncovered

missing features (to be added later):
- GUI (mines count, timer)
- an indication of victory (the only way to tell the game has been won is to count 10 placed flags while no unflagged tiles are covered)
- sounds
- options (variable difficulty and grid size)
- leaderboards

The controls are as follows:
- left mouse click to uncover tiles
- right mouse click to place/remove flags
- to reset the game, press ENTER

How to play:
- uncover tiles
- the number on an uncovered tile signifies the number of adjacent mines (there are at most 8 adjacent tiles for a single tile)
- place flags on tiles that you think hide a mine
- avoid uncovering mines

Hope you have fun!
