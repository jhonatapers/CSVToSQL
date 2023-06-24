using CSVToSQL.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CSVToSQL.bean.impl
{
    internal class PostgreSQL : IScriptGen
    {
        public string Language { get { return "postgresql"; } }

        public void Gen(Config config)
        {
            Encoding csvEncoding = Encoding.GetEncoding(config.CsvConfig.EncodingCodePage);

            string script = String.Format("{0};",
                string.Join($";{Environment.NewLine}", CsvReader
                .ReadLines(File.ReadAllBytes(config.CsvConfig.FileName), csvEncoding, config.CsvConfig.ColumnSplitter)
                .Select(line =>
                {
                    var columns = string.Join(", ", config.TableConfig.Columns.Select(c => c.Name));
                    var scrp = $"INSERT INTO {config.TableConfig.Schema}.{config.TableConfig.Name} ({columns}) VALUES ({ToValuesScript(line, config.TableConfig.Columns)})";

                    return scrp;
                })));

            byte[] bScrpit = Encoding.Convert(csvEncoding, Encoding.UTF8, csvEncoding.GetBytes(script));
            string utf8Script = Encoding.UTF8.GetString(bScrpit);

            using (StreamWriter sw = new StreamWriter($"{config.OutputFolder}{Path.DirectorySeparatorChar}{config.TableConfig.Schema}_{config.TableConfig.Name}.sql", false, Encoding.UTF8))
            {
                sw.Write(utf8Script);
                sw.Flush();
                sw.Close();
            }
        }

        private string ToValuesScript(string[] line, List<Column> columns)
        {
            return string.Join(", ",
           Enumerable.Range(0, columns.Count)
                .Select(i => ToValueScript(line[i], columns[i])));
        }

        private string ToValueScript(string value, Column column)
        {
            if (value.Equals(""))
                return "null";

            switch (column.TypeName)
            {
                case "String":
                    return $"'{value.Replace("'", "''")}'";
                case "DateTime":
                    return $"TO_DATE('{value}', '{column.Format}')";
                default:
                    return value.Replace(',', '.');
            }
        }
    }
}
