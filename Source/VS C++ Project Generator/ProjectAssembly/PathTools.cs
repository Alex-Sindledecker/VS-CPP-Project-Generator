using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public static class PathTools
    {
        public static string GetTemplateRootPath()
        {
#if DEBUG
            //Gets the project template source files path by getting the source directory of the project

            string currentDir = Directory.GetCurrentDirectory();
            string currentDirName = new DirectoryInfo(currentDir).Name;
            while (currentDirName != "Source")
            {
                DirectoryInfo info = Directory.GetParent(currentDir);
                currentDir = info.FullName;
                currentDirName = info.Name;
            }

            return $"{currentDir}/VS C++ Project Generator/ProjectTemplateSourceFiles/";
#else
            return "ProjectTemplateSourceFiles/";
#endif
        }
    }
}
