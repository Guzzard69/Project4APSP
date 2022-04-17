using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static bool IsGridSame(List<List<decimal>> firstGrid, List<List<decimal>> secondGrid)
        {
            if (firstGrid.Count == 0 && secondGrid.Count == 0) return true;

            for (int row = 0; row < firstGrid.Count; row++)
            {
                for (int column = 0; column < firstGrid[0].Count; column++)
                {
                    if (firstGrid[row][column] != secondGrid[row][column]) return false;
                }
            }

            return true;
        }

        static decimal MinSum(int row, int column, List<List<decimal>> grid)
        {
            var resultList = new List<decimal>();
            for (int i = 0; i < grid[row].Count; i++)
            {
                var firstValue = grid[row][i];
                var secondValue = grid[i][column];
                var addedDistance = firstValue == decimal.MaxValue || secondValue == decimal.MaxValue ? decimal.MaxValue : firstValue + secondValue;
                resultList.Add(addedDistance);
            }
            return resultList.OrderBy(x => x).First();
        }

        static void CopyGrid(List<List<decimal>> CopiedFrom, List<List<decimal>> CopiedTo)
        {
            CopiedTo.Clear();
            for (int row = 0; row < CopiedFrom.Count; row++)
            {
                CopiedTo.Add(new List<decimal>());
                for (int column = 0; column < CopiedFrom[row].Count; column++)
                {
                    CopiedTo[row].Add(CopiedFrom[row][column]);
                }
            }
        }

        static List<List<decimal>> GetNewNextStep(List<List<decimal>> grid)
        {
            var nextStep = new List<List<decimal>>();
            for (int row = 0; row < grid.Count; row++)
            {
                nextStep.Add(new List<decimal>());
                for (int column = 0; column < grid[0].Count; column++)
                {
                    nextStep[row].Add(MinSum(row, column, grid));
                }
            }
            return nextStep;
        }

        static void Main(string[] args)
        {
            // reads file
            if (args.Length == 0)
            {
                Console.WriteLine("No argument passed, you must supply a file name");
                return;
            }
            var fileName = $"{Directory.GetCurrentDirectory()}\\{args[0]}";

            var grid = new List<List<decimal>>();
            var lines = System.IO.File.ReadAllLines(fileName);
            for (int row = 0; row < lines.Length; row++)
            {
                grid.Add(new List<decimal>());

                var columns = lines[row].Split(',');
                foreach (var square in columns)
                {
                    var squareValue = square == "inf" ? decimal.MaxValue : decimal.Parse(square);
                    grid[row].Add(squareValue);
                }
            }

            // only run number of vertices times (we can't have a path that visits a city twice)
            for (int i = 0; i < grid.Count; i++)
            //for (int i = 0; i < Math.Pow(grid.Count, 2); i++)
            {
                var nextStep = GetNewNextStep(grid);
                CopyGrid(nextStep, grid);
            }

            for (int row = 0; row < grid.Count; row++)
            {
                for (int column = 0; column < grid[0].Count; column++)
                {
                    var squareValue = grid[row][column];
                    var comma = column == grid[0].Count - 1 ? "" : ",";
                    Console.Write($"{(squareValue == decimal.MaxValue ? "inf" : squareValue.ToString())}{comma}");
                }
                Console.WriteLine();
            }
        }
    }
}
