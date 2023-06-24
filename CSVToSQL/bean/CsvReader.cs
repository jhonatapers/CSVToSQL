using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToSQL.bean
{
    internal static class CsvReader
    {
        internal static List<string[]> ReadLines(byte[] bFile, Encoding encoding, string columnSplitter = ",", string lineSplitter = null, bool containHeader = true)
        {
            lineSplitter = lineSplitter == null ? Environment.NewLine : lineSplitter;
            List<string> allLines = getAllLines(encoding.GetString(bFile), lineSplitter).ToList();

            if (containHeader)
                allLines.RemoveAt(0);

            return allLines
                .Where(line => !line.Trim().Equals(""))
                .Select(line => getColumns(line, columnSplitter))
                .ToList();
        }

        private static string[] getAllLines(String csv, String splitter)
        {
            List<String> csvLines = new List<String>();

            int jump = splitter.Length;
            int startPosition = 0;
            bool isInQuotes = false;
            for (int currentPosition = 0; currentPosition < csv.Length; currentPosition++)
            {
                if (csv.Length < currentPosition + jump)
                    break;

                if (csv[currentPosition] == '\"')
                {
                    isInQuotes = !isInQuotes;
                }
                else if (csv.Substring(currentPosition, jump).Equals(splitter) && !isInQuotes)
                {
                    csvLines.Add(csv.Substring(startPosition, (currentPosition) - startPosition));
                    startPosition = currentPosition + jump;
                    currentPosition += jump - 1;
                }
            }

            csvLines.Add(csv.Substring(startPosition));

            return csvLines.ToArray();
        }

        private static string[] getColumns(string csvLine, String splitter)
        {
            List<string> csvColumns = new List<string>();
            int startPosition = 0;
            int jump = splitter.Length;
            bool isInQuotes = false;
            for (int currentPosition = 0; currentPosition < csvLine.Length; currentPosition++)
            {
                if (csvLine.Length < currentPosition + jump)
                    break;

                if (csvLine[currentPosition] == '\"')
                {
                    isInQuotes = !isInQuotes;
                }
                else if (csvLine.Substring(currentPosition, jump).Equals(splitter) && !isInQuotes)
                {
                    if (csvLine[startPosition] == '\"' && csvLine[startPosition] == '\"')
                    {
                        csvColumns.Add(csvLine.Substring(startPosition + 1, ((currentPosition) - startPosition) - 2));
                    }
                    else
                    {
                        csvColumns.Add(csvLine.Substring(startPosition, (currentPosition) - startPosition));
                    }
                    startPosition = currentPosition + jump;
                    currentPosition += jump - 1;
                }
            }

            if (csvLine.Substring(startPosition).Equals(splitter))
            {
                csvColumns.Add("");
            }
            else
            {
                string aux = csvLine.Substring(startPosition);
                if (aux.StartsWith("\"") && aux.EndsWith("\""))
                {
                    csvColumns.Add(aux.Substring(1, aux.Length - 2));
                }
                else
                {
                    csvColumns.Add(csvLine.Substring(startPosition));
                }
            }

            return csvColumns.ToArray();
        }

    }
}
