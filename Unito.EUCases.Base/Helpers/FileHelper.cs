using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Unito.EUCases.Base.Helpers
{
    public static class FileHelper
    {
        public static IEnumerable<string> ReadLines(string filename)
        {
            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }                        
    }
}
