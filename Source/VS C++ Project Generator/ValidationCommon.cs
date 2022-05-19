using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator
{
    public static class ValidationCommon
    {
        public static bool IsValidFilePath(string path)
        {
            char[] otherInvalidChars = { '?', '*', '"', '<', '>', '|' };
            if (path.Length == 0 || path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1 || path.IndexOfAny(otherInvalidChars) != -1)
                return false;

            return true;
        }
    }
}
