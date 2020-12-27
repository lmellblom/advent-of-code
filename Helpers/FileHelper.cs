using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace adventOfCode
{
    public static class FileHelper
    {
        public static void WriteFile(string file, string content)
        {
            File.WriteAllText(file, content);
        }
    }
}