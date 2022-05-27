using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly;
using VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types;

namespace UnitTests.PromptTests
{
    [TestClass]
    public class ProjectBuilderTests
    {
        private const string _tempDir = "C:/__VS_CPP_Project_Generator_Unit_Tests__/";
        private string _root = $"{_tempDir}TestProj/";

        [TestMethod]
        public void TestDirectoryStructure()
        {
            //This tests to make sure the directory is correctly built
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = ""
            };

            ProjectBuilder.BuildDirectoryStructure(model);

            bool rootExists = Directory.Exists(_root);
            bool sourceExists = Directory.Exists($"{_root}Source/");
            bool dependenciesExists = Directory.Exists($"{_root}Source/dependencies");
            bool projectExists = Directory.Exists($"{_root}Source/{model.Name}/");

            DeleteCreatedDirectories();

            Assert.AreEqual(rootExists, true, "Root directory not created! WARNING: The directory may have been created elsewhere and was not deleted!");
            Assert.AreEqual(sourceExists, true, "Source directory not created!");
            Assert.AreEqual(dependenciesExists, true, "Source directory not created!");
            Assert.AreEqual(projectExists, true, "Source directory not created!");
        }

        [TestMethod]
        public void TestTemplateFilesMoved()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = $"{PathTools.GetTemplateRootPath()}/SFMLSource/"
            };
            ProjectBuilder.BuildDirectoryStructure(model);

            ProjectBuilder.MoveTemplateFiles(model);

            bool mainInExpectedLocation = File.Exists($"{_root}Source/{model.Name}/main.cpp");

            DeleteCreatedDirectories();

            Assert.AreEqual(mainInExpectedLocation, true, "Source files not accurately moved!");
        }

        [TestMethod]
        public void TestSLNFileCreation()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = ""
            };

            SLNBuilder builder = new SLNBuilder(model, "v142");
            VCXProj tempProject = new VCXProj(model, "12.00");
            builder.AddProject(tempProject);

            ProjectBuilder.BuildDirectoryStructure(model);
            ProjectBuilder.CreateSLNFile(model, builder);

            bool slnCreated = File.Exists($"{_root}Source/{model.Name}.sln");

            DeleteCreatedDirectories();

            Assert.AreEqual(slnCreated, true, "SLN File Not Created!! Warning: This could mean it was created in an unexpected location...");
        }

        //TODO: Sln validation

        [TestMethod]
        public void TestProjectFileCreation()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = $"{PathTools.GetTemplateRootPath()}/SFMLSource/",
                Dependencies = new List<DependencyModel>() { new DependencyModel()
                    {
                        Url = "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip",
                        IncludeDir = "SFML/include/",
                        LibDir = "SFML/lib/",
                        DllDir = "SFML/bin/",
                        DebugLibNames = new List<string>(){ "sfml-system-d.lib", "sfml-window-d.lib", "sfml-graphics-d.lib" },
                        ReleaseLibNames = new List<string>(){ "sfml-system.lib", "sfml-window.lib", "sfml-graphics.lib" }
                    }
                }
            };

            VSProject project = new VCXProj(model, "12.00");

            ProjectBuilder.BuildDirectoryStructure(model);
            ProjectBuilder.CreateProjFile(model, project);

            bool vcxprojCreated = File.Exists($"{_root}Source/{model.Name}/{model.Name}.vcxproj");

            DeleteCreatedDirectories();

            Assert.AreEqual(vcxprojCreated, true, "Project file not created!! Warning: The file may have been created in an unexpected location...");
        }

        //TODO: vcxproj validation

        private void DeleteCreatedDirectories()
        {
            if (Directory.Exists(_root) == true)
                Directory.Delete(_tempDir, true);
        }
    }
}
