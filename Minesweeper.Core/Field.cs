namespace Minesweeper.Core
{
    public class Field
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public FieldIndicator FieldIndicator { get; private set; }
        public bool IsRevealed { get; private set; }
        public bool IsFlaged { get; set; }

        #region [CTOR]

        public Field(int x, int y, FieldIndicator fieldIndicator)
        {
            X = x;
            Y = y;

            FieldIndicator = fieldIndicator;
            IsRevealed = false;
            IsFlaged = false;
        }

        #endregion

        #region [Public]

        public bool Reveal()
        {
            bool result = false;
            
            if (!IsFlaged && !IsRevealed)
            {
                IsRevealed = true;
                result = true;
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            bool result = false;

            if(obj is Field)
            {
                result = this.X == ((Field)obj).X &&
                         this.Y == ((Field)obj).Y;
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
