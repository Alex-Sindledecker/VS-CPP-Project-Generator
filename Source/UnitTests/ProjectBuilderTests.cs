using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly;

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
        public void TestSLNCreation()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = ""
            };

            ProjectBuilder.BuildDirectoryStructure(model);
            ProjectBuilder.CreateSLN(model);

            bool slnCreated = File.Exists($"{_root}Source/{model.Name}.sln");

            DeleteCreatedDirectories();

            Assert.AreEqual(slnCreated, true, "SLN File Not Created!! Warning: This could mean it was created in an unexpected location...");
        }

        //TODO: Sln validation

        private void DeleteCreatedDirectories()
        {
            if (Directory.Exists(_root) == true)
                Directory.Delete(_tempDir, true);
        }
    }
}
