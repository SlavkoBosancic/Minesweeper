using Minesweeper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Minesweeper.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Brush _normalBorderBrush;
        private Brush _errorBorderBrush;
        private GameWindow _gameWindow;

        private int _sizeX;
        private int _sizeY;
        private int _difficulty;

        #region [CTOR]

        public MainWindow()
        {
            InitializeComponent();
            _normalBorderBrush = this.HeightInput.BorderBrush.Clone();
            _errorBorderBrush = new SolidColorBrush(Color.FromRgb(Byte.MaxValue, Byte.MinValue, Byte.MinValue));
        }

        #endregion

        #region [Private]

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int sizeX;
            int sizeY;
            List<bool> allGood = new List<bool>();

            if (!Int32.TryParse(WidthInput.Text, out sizeX))
            {
                WidthInput.BorderBrush = _errorBorderBrush;
                allGood.Add(false);
            }
            else
            {
                WidthInput.BorderBrush = _normalBorderBrush;
                allGood.Add(true);
            }

            if (!Int32.TryParse(HeightInput.Text, out sizeY))
            {
                HeightInput.BorderBrush = _errorBorderBrush;
                allGood.Add(false);
            }
            else
            {
                HeightInput.BorderBrush = _normalBorderBrush;
                allGood.Add(true);
            }

            if (allGood.Any(x => x != true))
            {
                ErrorLabel.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ErrorLabel.Visibility = Visibility.Hidden;

                _sizeX = sizeX;
                _sizeY = sizeY;
                _difficulty = 7;    //TODO: Extract difficulty level, now hardcoded

                CreateNewGameWindow();
            }
        }

        private void CreateNewGameWindow(bool gameRestart = false)
        {
            _gameWindow = new GameWindow(_sizeX, _sizeY, (GameDifficulty)_difficulty);
            _gameWindow.Closed += _gameWindow_Closed;

            if (!gameRestart)
            {
                if (this.IsVisible)
                    this.Hide();
            }

            _gameWindow.Show();
        }

        private void _gameWindow_Closed(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (sender is GameWindow)
                {
                    var game = sender as GameWindow;
                    _gameWindow = null;

                    if (game.restart)
                    {
                        // Restarted Game Window
                        CreateNewGameWindow(true);
                    }
                    else
                    {
                        // Closed Game Window
                        if (!this.IsVisible)
                            this.Show();
                    }
                }
            }
        }

        #endregion
    }
}
