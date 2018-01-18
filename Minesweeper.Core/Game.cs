using System.Collections.Generic;

namespace Minesweeper.Core
{
    public class Game
    {
        private static readonly GameBuilder _builder = new GameBuilder();
        private readonly Board _board;

        #region [CTOR]

        /// <summary>
        /// Private constructor to prevent initialization with out building the game.
        /// </summary>
        internal Game(Board board)
        {
            _board = board;
        }

        #endregion

        #region [Public]

        public static Game CreateNew(GameDifficulty difficulty, int sizeX = 0, int sizeY = 0)
        {
            return (sizeX == 0 || sizeY == 0) ?
                _builder.CreateNewGame(difficulty) :
                _builder.CreateNewGame(sizeX, sizeY, difficulty);
        }

        public bool IsReady { get { return HasBombsIndicators && HasOtherIndicators; } }

        public int NumberOfMoves { get; private set; }

        public int SizeX { get { return _board.SizeX; } }

        public int SizeY { get { return _board.SizeY; } }

        public int NumberOfBombs { get { return _board.NumberOfBombs; } }

        public bool Win { get; private set; }

        public bool Loose { get; private set; }

        public List<Field> RevealField(int x, int y)
        {
            List<Field> result = new List<Field>();
            Field field = _board.GetField(x, y);

            if (field != null)
            {
                if (field.Reveal())
                {
                    NumberOfMoves++;

                    switch (field.FieldIndicator)
                    {
                        case FieldIndicator.Bomb:
                            Loose = true;
                            result.Add(field);
                            break;
                        case FieldIndicator.Empty:
                            result = RevealRecursivly(field, true);
                            break;
                        default:
                            result.Add(field);
                            break;
                    }

                    Win = !Loose && IsWon();
                }
            }

            return result;
        }

        public bool FlagField(int x, int y)
        {
            bool result = false;
            Field field = _board.GetField(x, y);

            if (field != null && !field.IsFlaged)
            {
                NumberOfMoves++;
                field.IsFlaged = true;

                result = true;
            }

            return result;
        }

        public bool UnflagField(int x, int y)
        {
            bool result = false;
            Field field = _board.GetField(x, y);

            if (field != null && field.IsFlaged)
            {
                NumberOfMoves++;
                field.IsFlaged = false;

                result = true;
            }

            return result;
        }

        #endregion

        #region [Private]

        internal bool HasBombsIndicators { get; set; }

        internal bool HasOtherIndicators { get; set; }

        private List<Field> RevealRecursivly(Field field, bool isFirst = false)
        {
            var result = new List<Field>();

            if (field != null && field.FieldIndicator != FieldIndicator.Bomb)
            {
                if (field.Reveal() || isFirst)
                {
                    result.Add(field);

                    if (field.FieldIndicator == FieldIndicator.Empty)
                    {
                        for (int x = field.X - 1; x <= field.X + 1; x++)
                        {
                            for (int y = field.Y - 1; y <= field.Y + 1; y++)
                            {
                                result.AddRange(RevealRecursivly(_board.GetField(x, y)));
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool IsWon()
        {
            // check if any non-revealed field is not a bomb
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    Field field = _board.GetField(x, y);

                    if (field != null)
                    {
                        if (!field.IsRevealed && field.FieldIndicator != FieldIndicator.Bomb)
                            return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
