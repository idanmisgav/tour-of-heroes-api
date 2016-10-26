using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Utils
{
    public class FileUtils
    {
        private const int newLineLength = 2;
        private const string newLineChars = "\r\n";

        public static void CleanEmptyLines(string filePath)
        {
            string fileContetnt = File.ReadAllText(filePath);
            string cleanedContent = RecursiveCleanLines(fileContetnt);
            File.WriteAllText(filePath, cleanedContent);
        }

        public static string RecursiveCleanLines(string text)
        {
            if(text.StartsWith(newLineChars))
            {
                string emptyLineCleaned = text.Substring(newLineLength);
                return RecursiveCleanLines(emptyLineCleaned);
            }
            if (text.EndsWith(newLineChars))
            {
                string cleanEndLine = text.Remove(text.Length - newLineLength);
                return RecursiveCleanLines(cleanEndLine);
            }
            if (!text.Contains(newLineChars + newLineChars))
            {
                return text;
            }
            else
            {
                string newText = text.Replace("\r\n\r\n", "\r\n");
                return RecursiveCleanLines(newText);
            }
        }
    }
}