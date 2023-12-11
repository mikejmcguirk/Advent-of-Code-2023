using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode {
    public static class PartTwo {
        public static void Run(string inputFile) {
            List<List<char>> lines = new();

            try {
                lines = File.ReadAllLines(inputFile)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.ToList())
                    .ToList();
            }
            catch (Exception ex) {
                Console.WriteLine($"Unable to read file: {ex.Message}");
            }

            List<int> emptyRows = lines
                .Select((line, index) => (line, index))
                .Where(t => t.line.All(c => c == '.'))
                .Select(t => t.index)
                .ToList();

            List<int> emptyCols = Enumerable.Range(0, lines[0].Count)
                .Where(colIndex => lines.All(line => line[colIndex] != '#'))
                .ToList();

            List<(int, int)> galaxies = lines
                .SelectMany((line, i) => line
                    .Select((thisChar, j) => (thisChar, i, j))
                    .Where(t => t.thisChar == '#')
                    .Select(t => (t.i, t.j)))
                .ToList();

            long totDistance = galaxies
                .SelectMany((startGal, i) => galaxies.Skip(i + 1)
                    .Select(endGal => {
                        int minRow = Math.Min(endGal.Item1, startGal.Item1);
                        int maxRow = Math.Max(endGal.Item1, startGal.Item1);
                        int minCol = Math.Min(endGal.Item2, startGal.Item2);
                        int maxCol = Math.Max(endGal.Item2, startGal.Item2);

                        int rowCrosses = emptyRows.Aggregate(0, (acc, cur) => {
                            if (cur > minRow && cur < maxRow) {
                                return acc + 1;
                            }

                            return acc;
                        });

                        int colCrosses = emptyCols.Aggregate(0, (acc, cur) => {
                            if (cur > minCol && cur < maxCol) {
                                return acc + 1;
                            }

                            return acc;
                        });

                        long expansionFactor = 1000000;
                        long expansionUnits = expansionFactor - 1;

                        long rowExpansion = rowCrosses * expansionUnits;
                        long colExpansion = colCrosses * expansionUnits;

                        int startRow = startGal.Item1;
                        int startCol = startGal.Item2;
                        int endRow = endGal.Item1;
                        int endCol = endGal.Item2;

                        long rowDistance = Math.Abs(endRow - startRow);
                        rowDistance += rowExpansion;
                        long colDistance = Math.Abs(endCol - startCol);
                        colDistance += colExpansion;

                        return rowDistance + colDistance;
                    }))
                .Sum();

            Console.WriteLine($"Total distance: {totDistance}");
        }
    }
}
