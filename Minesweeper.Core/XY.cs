namespace Minesweeper.Core
{
    struct XY
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        #region [CTOR]

        public XY(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region [Public]

        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is XY)
            {
                result = this.X == ((XY)obj).X &&
                         this.Y == ((XY)obj).Y;
            }

            return result;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}", this.X, this.Y)
                         .GetHashCode();
        }

        #endregion
    }
}
