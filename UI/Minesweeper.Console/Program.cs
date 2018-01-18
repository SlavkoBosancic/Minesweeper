using Minesweeper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Screen = System.Console;

namespace Minesweeper.Console
{
    class Program
    {
        // TO DO .... Everything

        static void Main(string[] args)
        {
            var x = 10;
            var y = 10;
            var rowLength = x * 2 + 1;

            Game game = Game.CreateNew(GameDifficulty.Normal, x, y);

            PrintRowLine(rowLength);

            for (int i = 0; i < y; i++)
            {
                Screen.Write("|");

                for (int j = 0; j < x; j++)
                {
                    var field = game.RevealField(j, i)
                                    .FirstOrDefault(f => f.X == j && f.Y == i);

                    if (field != null)
                    {
                        Screen.Write("{0}|", field.FieldIndicator == FieldIndicator.Bomb ?
                                             "*" :
                                             ((int)field.FieldIndicator)
                              .ToString());
                    }
                }

                Screen.WriteLine();
                PrintRowLine(rowLength);
            }

            Screen.ReadKey();
        }

        private static void PrintRowLine(int rowLength)
        {
            while (rowLength > 0) { Screen.Write("-"); rowLength--; }
            Screen.WriteLine();
        }
    }
}
