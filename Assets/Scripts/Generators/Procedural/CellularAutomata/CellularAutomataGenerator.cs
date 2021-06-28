namespace Stealcase.Generators.Procedural.CellularAutomata
{
    public class CellularAutomataGenerator
    {
        private int wallDuplicationThreshold;
        private int randomFillPercent;
        private int width;
        private int height;
        public CellularAutomataGenerator(int width, int height, int wallDuplicationThreshold, int randomFillPercent)
        {
            this.wallDuplicationThreshold = wallDuplicationThreshold;
            this.randomFillPercent = randomFillPercent;
            this.width = width;
            this.height = height;
        }

        public int[,] SmoothMap(int[,] map)
        {
            int[,] smoothedMap = map;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighborWallTiles = GetSurroundingWallCount(smoothedMap,x, y);
                    if (neighborWallTiles > wallDuplicationThreshold)
                    {
                        smoothedMap[x, y] = 1;
                    }
                    else if (neighborWallTiles < wallDuplicationThreshold)
                    {
                        smoothedMap[x, y] = 0;
                    }
                }
            }
            return smoothedMap;
        }
        public int[,] GenerateMap(string seed = "")
        {
            int[,] map = new int[width, height];
            if (string.IsNullOrEmpty(seed))
            {
                seed = System.DateTime.UtcNow.ToString();
            }
            //PseudoRandomNumberGeneratior;
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            //2D Array iteration
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Catches the edges of the map
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        //.next gives number between Minimum value and maximum value, based on seed.

                        //If number is less than random fill percent, return 1. If more, return 0.
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                    }
                }
            }
            return map;
        }
        //Return the number of walls that surround a tile.
        int GetSurroundingWallCount(int[,] map, int gridX, int gridY)
        {
            int wallCount = 0;
            //Looping through a 3-by-3 grid!
            //Get the left neighbor, The current pos, then the Right neighbor
            for (int neighborX = gridX - 1; neighborX <= gridX + 1; neighborX++)
            {
                //Down neighbor, current pos, Up Neighbor;
                for (int neighborY = gridY - 1; neighborY <= gridY + 1; neighborY++)
                {
                    //Check if tile is INSIDE the  map, so we dont get an OUT OF INDEX ARRAY error
                    if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                    {
                        //IGnore middle Tile
                        if (neighborX != gridX || neighborY != gridY)
                        {
                            //Map is a number between 0 and 1. If there is a wall, increment wallcount.
                            wallCount += map[neighborX, neighborY];
                        }
                    }
                    else
                    {
                        //If the tile is outside the map, count that as a wall.
                        wallCount++;
                    }

                }

            }
            return wallCount;
        }

    }
}