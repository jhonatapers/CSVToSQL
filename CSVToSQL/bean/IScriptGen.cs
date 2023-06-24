using CSVToSQL.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToSQL.bean
{
    internal interface IScriptGen
    {
        string Language { get; }

        void Gen(Config config);
    }
}
