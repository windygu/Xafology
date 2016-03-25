﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Win
{

    public class CopyParser : ICopyParser
    {
        private readonly ICopiedText copiedText;

        public CopyParser(ICopiedText copiedText)
        {
            this.copiedText = copiedText;
        }

        public string[][] ToArray()
        {
            string[] data = copiedText.Data.Split('\n');
            if (data.Length < 1) return null;
            string[][] parsed = new string[data.Length][];
            for (int i = 0; i < parsed.Length; i++)
            {
                parsed[i] = GetRowData(data[i]);
            }
            return parsed;
        }

        // split tab-delimited string into array
        private string[] GetRowData(string data)
        {
            string[] rowData = data.Split(new char[] { '\r', '\x09' });
            return rowData;
        }
    }
}
