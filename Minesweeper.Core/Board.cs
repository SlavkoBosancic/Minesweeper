namespace Minesweeper.Core
{
    class Board
    {
        private readonly Field[,] _fields;
        private readonly int _sizeX;
        private readonly int _sizeY;

        #region [CTOR]

        /// <summary>
        /// Constructor with a arbitrary board size
        /// </summary>
        /// <param name="size"></param>
        internal Board(int sizeX, int sizeY)
        {
            _sizeX = sizeX > 0 ? sizeX : 1;
            _sizeY = sizeY > 0 ? sizeY : 1;

            _fields = new Field[_sizeX, _sizeY];
            PopulateEmptyFields();
        }

        #endregion

        #region [Public]

        public int SizeX { get { return _sizeX;  } }

        public int SizeY { get { return _sizeY; } }

        public int NumberOfBombs { get; private set; }

        public bool SetField(int x, int y, FieldIndicator indicator)
        {
            bool result = false;

            if (x >= 0 && x < _sizeX)
            {
                if(y >= 0 && y < _sizeY)
                {
                    _fields[x, y] = new Field(x, y, indicator);

                    if (indicator == FieldIndicator.Bomb)
                        NumberOfBombs++;

                    result = true;
                }
            }

            return result;
        }

        public Field GetField(int x, int y)
        {
            Field result = null;

            if (x >= 0 && x < _sizeX)
            {
                if (y >= 0 && y < _sizeY)
                {
                    result = _fields[x, y];
                }
            }

            return result;
        }

        #endregion

        #region [Private]

        private void PopulateEmptyFields()
        {
            for(int x = 0; x < _fields.GetLength(0); x++)
            {
                for(int y = 0; y < _fields.GetLength(1); y++)
                {
                    _fields[x, y] = new Field(x, y, FieldIndicator.Empty);
                }
            }
        }

        #endregion
    }
}
