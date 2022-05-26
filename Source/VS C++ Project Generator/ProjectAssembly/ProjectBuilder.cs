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

            string guid = CreateVCXProj(model);
            CreateSLN(model, guid);
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

        public static void CreateSLN(ProjectModel model, string projectGUID)
        {
            const string formatVersion = "12.00";

            StreamWriter slnWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}.sln");

            string projectTypeGUID = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";

            slnWriter.WriteLine($"Microsoft Visual Studio Solution File, Format Version {formatVersion}");
            slnWriter.WriteLine($"Project(\"{projectTypeGUID}\") = \"{model.Name}\", \"{model.Name}/{model.Name}.vcxproj\", \"{{{projectGUID}}}\"");
            slnWriter.WriteLine("EndProject");

            slnWriter.Close();
        }

        public static string CreateVCXProj(ProjectModel model)
        {
            const string platformToolset = "v142";

            StreamWriter projectWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}/{model.Name}.vcxproj");

            VCXProj project = new VCXProj(model, platformToolset);
            projectWriter.Write(project.BuildStandardXML());

            projectWriter.Close();

            return project.GUID.ToString();
        }
    }
}
