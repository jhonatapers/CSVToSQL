using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToSQL.dto
{
    public class Config
    {
        public string OutputFolder { get; set; }

        public Csv CsvConfig { get; set; }

        public Table TableConfig { get; set; }
    }

    public class Csv
    {
        public string FileName { get; set; }

        public string Encoding { get; set; }

        public string ColumnSplitter { get; set; }

        public string LineSplitter { get; set; }

        public bool ContainHeader { get; set; }
    }

    public class Table
    {
        public string Schema { get; set; }

        public string Name { get; set; }

        public List<Column> Columns { get; set; }

    }

    public class Column
    {
        public string Name { get; set; }

        public string TypeName { get; set; }

        public string Format { get; set; }
    }
}
