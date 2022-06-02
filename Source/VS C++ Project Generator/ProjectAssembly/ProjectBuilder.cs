using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public class ProjectBuilder
    {
        public delegate void BuildStepDelegate();

        public event BuildStepDelegate DirectoryBuildEvent;
        public event BuildStepDelegate MoveTemplateEvent;
        public event BuildStepDelegate ProjFileBuildEvent;
        public event BuildStepDelegate SlnFileBuildEvent;

        private string _vcxVersion;
        private string _slnVersion;

        public ProjectBuilder(string vcxVersion, string slnVersion)
        {
            _vcxVersion = vcxVersion;
            _slnVersion = slnVersion;
        }

        public void BuildFromModel(ProjectModel model)
        {
            //Setup a vcxproj template for the given project
            VCXProj mainProject = new VCXProj(model, _vcxVersion);
            SLNBuilder slnBuilder = new SLNBuilder(model, _slnVersion);

            //Add the project to the sln builder
            slnBuilder.AddProject(mainProject);

            //Creating the project on disk:
            
            BuildDirectoryStructure(model);
            
            MoveTemplateFiles(model);
            CreateProjFile(model, mainProject);
            CreateSLNFile(model, slnBuilder);
            DownloadAndInstallDependencies(model);
        }

        public void BuildDirectoryStructure(ProjectModel model)
        {
            DirectoryBuildEvent?.Invoke();

            Directory.CreateDirectory(model.DiskLocation);                                 //Root directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/");                    //Source directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/Dependencies/");       //Dependency directory
            Directory.CreateDirectory($"{model.DiskLocation}/Source/{model.Name}/");       //Project directory
        }

        public void MoveTemplateFiles(ProjectModel model)
        {
            if (model.TemplateSourcePath != "")
            {
                MoveTemplateEvent?.Invoke();

                //Get list of files in template directory
                string[] files = Directory.GetFiles(model.TemplateSourcePath);

                //Copy each template file to the new project directory
                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine($"{model.DiskLocation}/Source/{model.Name}/", fileName);
                    File.Copy(file, destFile, true);
                }
            }
        }

        public void CreateSLNFile(ProjectModel model, SLNBuilder builder)
        {
            SlnFileBuildEvent?.Invoke();

            //Write the sln file
            using (StreamWriter slnWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}.sln"))
                slnWriter.Write(builder.BuildFileContent());
        }

        public void CreateProjFile(ProjectModel model, VSProject project)
        {
            ProjFileBuildEvent?.Invoke();

            //Write project xml to a file
            using (StreamWriter projectWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}/{model.Name}.{project.GetFileExtension()}"))
                projectWriter.Write(project.BuildXML());
        
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

        public void DownloadAndInstallDependencies(ProjectModel model)
        {
            //Create intermediate directory (this is where downloaded zip files will be stored and extracted from)
            string intDir = $"{model.DiskLocation}Source/__intermediate_install__/";
            Directory.CreateDirectory(intDir);

            //Manages downloading and extracting dependencies
            DependencyManager dependencyManager = new DependencyManager(model, intDir);

            dependencyManager.AquireDependencies();
            dependencyManager.ExtractDependencies();

            //Delete intermediate directory
            Directory.Delete(intDir, true);
        }
    }
}
