using CSVToSQL.bean;
using CSVToSQL.bean.impl;
using CSVToSQL.dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CSVToSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IScriptGen gen = new PostgreSQL();

            foreach (DirectoryInfo dirInfo in new DirectoryInfo(args[0]).GetDirectories())
            {
                List<FileInfo> files = dirInfo.GetFiles().ToList();
                Config config = JsonConvert.DeserializeObject<Config>(System.IO.File.ReadAllText(files.FirstOrDefault(file => file.Extension.ToLower().Equals(".json")).FullName));
                config.OutputFolder = dirInfo.FullName;
                config.CsvConfig.FileName = files.Find(file => file.Extension.ToLower().Equals(".csv")).FullName;

                gen.Gen(config);
            }
        }

        internal static string FindArg(List<string> args, string argToFind)
        {
            return args.FirstOrDefault(arg => arg.Split(':')[0].Equals(argToFind));
        }
    }



}

