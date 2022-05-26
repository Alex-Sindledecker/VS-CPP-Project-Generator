using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public static class ProjectBuilder
    { 
        public static void BuildFromModel(ProjectModel model)
        {
            BuildDirectoryStructure(model);
            MoveTemplateFiles(model);

            VSProject mainProject = new VCXProj(model, "v142");

            SLNBuilder slnBuilder = new SLNBuilder(model, "12.00");
            slnBuilder.AddProject(mainProject);

            CreateProjFile(model, mainProject);
            CreateSLNFile(model, slnBuilder);
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

        public static void CreateSLNFile(ProjectModel model, SLNBuilder builder)
        {
            StreamWriter slnWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}.sln");

            slnWriter.Write(builder.BuildFileContent());

            slnWriter.Close();
        }

        public static void CreateProjFile(ProjectModel model, VSProject project)
        {
            StreamWriter projectWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}/{model.Name}.{project.GetFileExtension()}");

            projectWriter.Write(project.BuildXML());

            projectWriter.Close();
        }
    }
}
