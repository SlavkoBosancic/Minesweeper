using System;
using System.Collections.Generic;

namespace Minesweeper.Core
{
    class GameBuilder
    {
        #region [Public]

        /// <summary>
        /// Default board size if not specified
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public Game CreateNewGame(GameDifficulty difficulty)
        {
            return CreateNewGame(8, 8, difficulty);
        }

        public Game CreateNewGame(int sizeX, int sizeY, GameDifficulty difficulty)
        {
            Board board = new Board(sizeX, sizeY);
            Game game = new Game(board);

            List<XY> bombs = GenerateBombFields(board, difficulty);

            game.HasBombsIndicators = PopulateBombIndicators(bombs, board);
            game.HasOtherIndicators = PopulateOtherIndicators(bombs, board);

            return game;
        }

        #endregion

        #region [Private]

        private bool PopulateBombIndicators(List<XY> bombs, Board board)
        {
            bool result = true;

            foreach(XY bomb in bombs)
            {
                result = result && 
                         board.SetField(bomb.X, bomb.Y, FieldIndicator.Bomb);
            }

            return result;
        }

        private bool PopulateOtherIndicators(List<XY> bombs, Board board)
        {
            bool result = true;

            foreach (XY bomb in bombs)
            {
                for(int x = bomb.X - 1; x <= bomb.X + 1; x++)
                {
                    for (int y = bomb.Y - 1; y <= bomb.Y + 1; y++)
                    {
                        Field field = board.GetField(x, y);

                        if(field != null)
                        {
                            if(field.FieldIndicator != FieldIndicator.Bomb)
                            {
                                int newfieldType = ((int)field.FieldIndicator) + 1;

                                if(Enum.IsDefined(typeof(FieldIndicator), newfieldType))
                                {
                                    result = result &&
                                             board.SetField(x, y, (FieldIndicator)newfieldType);
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private List<XY> GenerateBombFields(Board board, GameDifficulty difficulty)
        {
            List<XY> result = new List<XY>();

            var boardSize = board.SizeX * board.SizeY;
            var numberOfBombs = (int)Math.Ceiling(boardSize / (decimal)difficulty);
            Random rnd = new Random();

            while(numberOfBombs > 0)
            {
                int bombField = rnd.Next(0, boardSize - 1);

                var x = bombField % board.SizeX;
                var y = (int)Math.Floor((decimal)bombField / board.SizeX);

                XY newBombField = new XY(x, y);

                if (!result.Contains(newBombField))
                {
                    result.Add(newBombField);
                    numberOfBombs--;
                }
            }

            return result;
        }

        #endregion
    }
}
