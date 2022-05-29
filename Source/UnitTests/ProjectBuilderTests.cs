using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly;
using VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types;

namespace UnitTests
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

            SLNBuilder builder = new SLNBuilder(model, "12.00");
            VCXProj tempProject = new VCXProj(model, "v142");
            builder.AddProject(tempProject);

            ProjectBuilder.BuildDirectoryStructure(model);
            ProjectBuilder.CreateSLNFile(model, builder);

            bool slnCreated = File.Exists($"{_root}Source/{model.Name}.sln");

            DeleteCreatedDirectories();

            Assert.AreEqual(slnCreated, true, "SLN File Not Created!! Warning: This could mean it was created in an unexpected location...");
        }

        //Ensures the file contents of an sln file are as expected
        [TestMethod]
        public void SLNFileContentValidation()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = ""
            };

            SLNBuilder builder = new SLNBuilder(model, "12.00");
            VCXProj tempProject = new VCXProj(model, "v142");
            builder.AddProject(tempProject);

            string content = builder.BuildFileContent();

            string expectedContent = @$"Microsoft Visual Studio Solution File, Format Version 12.00
Project(""{{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}}"") = ""TestProj"", ""TestProj/TestProj.vcxproj"", ""{{{tempProject.GUID.ToString()}}}""
EndProject
".Replace("\r", "");
            Assert.AreEqual(content, expectedContent, "Invalid sln generated!!");
        }

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
                        ReleaseLibNames = new List<string>(){ "sfml-system.lib", "sfml-window.lib", "sfml-graphics.lib" },
                        IncludeInProject = new List<string>()
                    }
                }
            };

            VSProject project = new VCXProj(model, "v142");

            ProjectBuilder.BuildDirectoryStructure(model);
            ProjectBuilder.CreateProjFile(model, project);

            bool vcxprojCreated = File.Exists($"{_root}Source/{model.Name}/{model.Name}.vcxproj");

            DeleteCreatedDirectories();

            Assert.AreEqual(vcxprojCreated, true, "Project file not created!! Warning: The file may have been created in an unexpected location...");
        }

        [TestMethod]
        public void VCXProjValidation()
        {
            ProjectModel model = new ProjectModel()
            {
                Name = "TestProj",
                DiskLocation = $"{_root}",
                TemplateSourcePath = $"{PathTools.GetTemplateRootPath()}/SFMLSource/",
                Dependencies = new List<DependencyModel>() { new DependencyModel()
                    {
                        Url = "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip",
                        IncludeDir = "SFML-2.5.1/include/",
                        LibDir = "SFML-2.5.1/lib/",
                        DllDir = "SFML-2.5.1/bin/",
                        DebugLibNames = new List<string>(){ "sfml-graphics-d.lib", "sfml-window-d.lib", "sfml-system-d.lib" },
                        ReleaseLibNames = new List<string>(){ "sfml-graphics.lib", "sfml-window.lib", "sfml-system.lib" },
                        IncludeInProject = new List<string>()
                    }
                }
            };

            VCXProj project = new VCXProj(model, "v142");

            string actualXML = project.BuildXML().Replace("\r", "").Replace("\n", "");
            string expectedXML = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<Project DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <ItemGroup Label=""ProjectConfigurations"">
    <ProjectConfiguration Include=""Debug|Win32"">
      <Configuration>Debug</Configuration>
      <Platform>Win32</Platform>
    </ProjectConfiguration>
    <ProjectConfiguration Include=""Release|Win32"">
      <Configuration>Release</Configuration>
      <Platform>Win32</Platform>
    </ProjectConfiguration>
    <ProjectConfiguration Include=""Debug|x64"">
      <Configuration>Debug</Configuration>
      <Platform>x64</Platform>
    </ProjectConfiguration>
    <ProjectConfiguration Include=""Release|x64"">
      <Configuration>Release</Configuration>
      <Platform>x64</Platform>
    </ProjectConfiguration>
  </ItemGroup>
  <PropertyGroup Label=""Globals"">
    <VCProjectVersion>16.0</VCProjectVersion>
    <ProjectGuid>{{{project.GUID.ToString()}}}</ProjectGuid>
    <WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion>
  </PropertyGroup>
  <Import Project=""$(VCTargetsPath)\Microsoft.Cpp.Default.props"" />
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>v142</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>v142</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>v142</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>v142</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <Import Project=""$(VCTargetsPath)\Microsoft.Cpp.props"" />
  <ImportGroup Label=""PropertySheets"" Condition=""'$(Configuration)|$(Platform)' =='Debug|Win32'"" >
    <Import Project=""$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props"" Condition =""exists('$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props')"" Label =""LocalAppDataPlatform"" />
  </ImportGroup>
  <ImportGroup Label=""PropertySheets"" Condition =""'$(Configuration)|$(Platform)' =='Release|Win32'"" >
    <Import Project=""$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props"" Condition =""exists('$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props')"" Label =""LocalAppDataPlatform"" />
  </ImportGroup>
  <ImportGroup Label=""PropertySheets"" Condition =""'$(Configuration)|$(Platform)' =='Debug|x64'"" >
    <Import Project=""$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props"" Condition =""exists('$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props')"" Label =""LocalAppDataPlatform"" />
  </ImportGroup>
  <ImportGroup Label=""PropertySheets"" Condition =""'$(Configuration)|$(Platform)' =='Release|x64'"" >
    <Import Project=""$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props"" Condition =""exists('$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props')"" Label =""LocalAppDataPlatform"" />
  </ImportGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
    <LinkIncremental>true</LinkIncremental>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|Win32'"">
    <LinkIncremental>false</LinkIncremental>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"">
    <LinkIncremental>true</LinkIncremental>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|x64'"">
    <LinkIncremental>false</LinkIncremental>
  </PropertyGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>WIN32;_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/include/;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/lib/;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>sfml-graphics-d.lib;sfml-window-d.lib;sfml-system-d.lib;%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Release|Win32'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <FunctionLevelLinking>true</FunctionLevelLinking>
      <IntrinsicFunctions>true</IntrinsicFunctions>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>WIN32;NDEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/include/;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/lib/;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>sfml-graphics.lib;sfml-window.lib;sfml-system.lib;%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/include/;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/lib/;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>sfml-graphics-d.lib;sfml-window-d.lib;sfml-system-d.lib;%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Release|x64'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <FunctionLevelLinking>true</FunctionLevelLinking>
      <IntrinsicFunctions>true</IntrinsicFunctions>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>NDEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/include/;%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>$(SolutionDir)/dependencies/SFML-2.5.1/lib/;%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>sfml-graphics.lib;sfml-window.lib;sfml-system.lib;%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemGroup>
    <ClCompile Include=""main.cpp"" />
  </ItemGroup> 
  <Import Project=""$(VCTargetsPath)\Microsoft.Cpp.targets"" />
</Project>
".Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(actualXML, expectedXML, "Actual XML and expected XML are not the same!");
            Assert.AreEqual(project.GetFileExtension(), "vcxproj", "VCXProj returned an incorrect file extension!");
        }

        private void DeleteCreatedDirectories()
        {
            if (Directory.Exists(_root) == true)
                Directory.Delete(_tempDir, true);
        }
    }
}
