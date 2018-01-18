using Minesweeper.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace Minesweeper.WPF.Models
{
    class GameViewModel : INotifyPropertyChanged
    {
        private readonly Game _game;
        private readonly Grid _gameGrid;

        #region [CTOR]

        public GameViewModel(Game game, Grid gameGrid)
        {
            _game = game;
            _gameGrid = gameGrid;
        }

        #endregion

        public bool IsOver { get { return _game.Win || _game.Loose; } }

        public int NumberOfBombs { get { return _game.NumberOfBombs; } }

        public int NumberOfMoves { get { return _game.NumberOfMoves; } }

        public string Status { get { return _game.Win ? "WIN" : (_game.Loose ? "GAME OVER" : String.Empty); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RevealField(int x, int y)
        {
            if (!IsOver)
            {
                var fields = _game.RevealField(x, y);

                foreach (var field in fields)
                {
                    if (field.IsRevealed)
                    {
                        var button = _gameGrid.Children
                                              .Cast<Button>()
                                              .FirstOrDefault(b => Grid.GetColumn(b) == field.X && Grid.GetRow(b) == field.Y);

                        if (button != null)
                        {
                            button.Content = GetButtonString(field.FieldIndicator);
                            var backgroundColor = field.FieldIndicator == FieldIndicator.Bomb ?
                                                  Color.FromRgb(200, 0, 0) :
                                                  Color.FromRgb(200, 200, 200);

                            button.Background = new SolidColorBrush(backgroundColor);
                        }
                    }
                }

                Refresh();
            }
        }

        public void ToggleFlag(int x, int y)
        {
            if (!IsOver)
            {
                if(_game.FlagField(x, y))
                {
                    var button = _gameGrid.Children
                                              .Cast<Button>()
                                              .FirstOrDefault(b => Grid.GetColumn(b) == x && Grid.GetRow(b) == y);

                    if (button != null)
                    {
                        button.Content = "?";
                    }

                    Refresh();
                }
                else
                {
                    if(_game.UnflagField(x, y))
                    {
                        var button = _gameGrid.Children
                                              .Cast<Button>()
                                              .FirstOrDefault(b => Grid.GetColumn(b) == x && Grid.GetRow(b) == y);

                        if (button != null)
                        {
                            button.Content = String.Empty;
                        }

                        Refresh();
                    }
                }
            }
        }

        #region [Private]

        private string GetButtonString(FieldIndicator indicator)
        {
            var result = string.Empty;

            switch (indicator)
            {
                case FieldIndicator.Bomb:
                    result = "#";
                    break;
                case FieldIndicator.Empty:
                    break;
                default:
                    result = ((int)indicator).ToString();
                    break;
            }

            return result;
        }

        private void Refresh()
        {
            PropertyChanged(this, new PropertyChangedEventArgs("NumberOfMoves"));
            PropertyChanged(this, new PropertyChangedEventArgs("Status"));
        }

        #endregion
    }
}
