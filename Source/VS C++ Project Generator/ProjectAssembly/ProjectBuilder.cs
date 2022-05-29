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
            VCXProj mainProject = new VCXProj(model, "v142");
            SLNBuilder slnBuilder = new SLNBuilder(model, "12.00");
            slnBuilder.AddProject(mainProject);

            Console.WriteLine("\tBuilding directories...");
            BuildDirectoryStructure(model);
            Console.WriteLine("\tMoving template files...");
            MoveTemplateFiles(model);
            Console.WriteLine("\tCreating proj files...");
            CreateProjFile(model, mainProject);
            Console.WriteLine("\tCreating sln files...");
            CreateSLNFile(model, slnBuilder);
            DownloadAndInstallDependencies(model);
            Console.WriteLine("\tFinished!");
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
        
            //We should create a .vcxproj.user file if the type is a C++ console application
            if (project.GetProjectTypeGUID() == "8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942")
            {
                using (StreamWriter userWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}/{model.Name}.{project.GetFileExtension()}.user"))
                {
                    VCXProj vcxproj = (VCXProj)project;
                    userWriter.Write(vcxproj.GetUserFileXML());
                }
            }
        }

        public static void DownloadAndInstallDependencies(ProjectModel model)
        {
            string intDir = $"{model.DiskLocation}Source/__intermediate_install__/";
            DependencyManager dependencyManager = new DependencyManager(model, intDir);

            Directory.CreateDirectory(intDir);

            Console.WriteLine("\tDownloading dependencies...");
            dependencyManager.AquireDependencies();
            Console.WriteLine("\tExtracting dependencies...");
            dependencyManager.ExtractDependencies();
            Console.WriteLine("\tDistributing dependencies...");
            dependencyManager.DistributeDependencies();
        }
    }
}
