using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public static class ProjectBuilder
    { 
        public static void BuildFromModel(ProjectModel model)
        {
            BuildDirectoryStructure(model);
            MoveTemplateFiles(model);
            CreateSLN(model);
        }

        public static void BuildDirectoryStructure(ProjectModel model)
        {
            Directory.CreateDirectory(model.DiskLocation);                                 //Root directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/");                    //Source directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/Dependencies/");       //Dependency directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/{model.Name}/");       //Project directory
        }

        public static void MoveTemplateFiles(ProjectModel model)
        {
            if (model.TemplateSourcePath != "")
            {
                string[] files = Directory.GetFiles(model.TemplateSourcePath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine($"{model.DiskLocation}/Source/{model.Name}/", fileName);
                    File.Copy(file, destFile, true);
                }
            }
        }

        public static void CreateSLN(ProjectModel model)
        {
            const string formatVersion = "12.00";

            StreamWriter slnWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}.sln");

            //This ends up getting turned into some sort of hash by visual studio. I have no idea why or how though
            string preHash = model.Name + DateTime.Now.ToString();

            slnWriter.WriteLine($"Microsoft Visual Studio Solution File, Format Version {formatVersion}");
            slnWriter.WriteLine($"Project(\"{preHash}\") = \"{model.Name}\", \"{model.Name}/{model.Name}.vcxproj\", \"{preHash}\"");
            slnWriter.WriteLine("EndProject");

            slnWriter.Close();
        }
    }
}
