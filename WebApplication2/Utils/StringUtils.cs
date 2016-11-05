using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Utils
{
    public class StringUtils
    {
        public static int findLengthUntilChar(string text, int startIndex, char c)
        {
            if (string.IsNullOrEmpty(text))
            {
                return -1;
            }
            int counter = 1;
            for (int i = startIndex; i < text.Length; i++)
            {
                if (text[i] == c)
                    return counter;
                counter++;
            }
            return -1;
        }
    }
}