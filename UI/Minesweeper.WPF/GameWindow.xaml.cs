using Minesweeper.Core;
using Minesweeper.WPF.Models;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper.WPF
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameViewModel _gameVM;
        private readonly int _buttonWidthHeight = 20;
        private readonly int _windowMinimalHeight = 200;
        private readonly int _windowRightAreaWidth = 100;

        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly GameDifficulty _difficulty;

        public bool restart = false;

        #region [CTOR]

        private GameWindow()
        {
            InitializeComponent();
        }

        public GameWindow(int sizeX, int sizeY, GameDifficulty difficulty) : this()
        {
            _sizeX = sizeX;
            _sizeY = sizeY;
            _difficulty = difficulty;

            this.Width = _sizeX * _buttonWidthHeight + _windowRightAreaWidth;
            this.Height = (_sizeY * _buttonWidthHeight) < _windowMinimalHeight ? _windowMinimalHeight : _sizeY * _buttonWidthHeight;

            var game = Game.CreateNew(_difficulty, sizeX: _sizeX, sizeY: _sizeY);
            CreateGrid(game.SizeX, game.SizeY);

            PopulateGrid(game.SizeX, game.SizeY);
            _gameVM = new GameViewModel(game, this.GameGrid);

            this.DataContext = _gameVM;
        }

        #endregion

        #region [Private]

        private void CreateGrid(int sizeX, int sizeY)
        {
            this.GameGrid.Width = sizeX * _buttonWidthHeight;
            this.GameGrid.Height = sizeY * _buttonWidthHeight;

            for (int x = 0; x < sizeX; x++)
            {
                this.GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < sizeY; y++)
            {
                this.GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            this.GameGrid.AddHandler(UIElement.PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(LeftClick));
            this.GameGrid.AddHandler(UIElement.MouseRightButtonUpEvent, new RoutedEventHandler(RightClick));
        }

        private void PopulateGrid(int sizeX, int sizeY)
        {
            this.GameGrid.Children.Clear();

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    var button = new Button() { Width = (double)_buttonWidthHeight, Height = (double)_buttonWidthHeight };

                    Grid.SetColumn(button, x);
                    Grid.SetRow(button, y);
                    GameGrid.Children.Add(button);
                }
            }
        }

        private void LeftClick(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;

            if (button != null)
            {
                var x = Grid.GetColumn(button);
                var y = Grid.GetRow(button);

                _gameVM.RevealField(x, y);
            }
        }

        private void RightClick(object sender, RoutedEventArgs e)
        {
            var button = e.Source as Button;

            if (button != null)
            {
                var x = Grid.GetColumn(button);
                var y = Grid.GetRow(button);

                _gameVM.ToggleFlag(x, y);
            }
        }

        private void Restart(object sender, RoutedEventArgs e)
        {
            this.restart = true;
            this.Close();
        }

        #endregion
    }
}