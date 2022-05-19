using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator
{
    public static class ValidationCommon
    {
        public static bool IsValidFilePath(string path)
        {
            if (path.Length == 0 || path.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
                return false;

            return true;
        }
    }
}
