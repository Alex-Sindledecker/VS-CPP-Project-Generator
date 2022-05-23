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
            string guid = System.Guid.NewGuid().ToString();
            string includeDirs = GetIncludeDirectoriesAsString(model);
            string libDirs = GetLibraryDirectoriesAsString(model);
            Tuple<string, string> libStrings = GetLibNamesAsStrings(model);

            StreamWriter projectWriter = new StreamWriter($"{model.DiskLocation}/Source/{model.Name}/{model.Name}.vcxproj");

            projectWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf - 8\"?>");
            projectWriter.WriteLine("<Project DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">");
            projectWriter.WriteLine(@"<ItemGroup Label=""ProjectConfigurations"">
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
  </ItemGroup>");
            projectWriter.WriteLine(@$"<PropertyGroup Label=""Globals"">
    <VCProjectVersion>16.0</VCProjectVersion>
    <ProjectGuid>{{{guid}}}</ProjectGuid>
    <WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion>
  </PropertyGroup>");
            projectWriter.WriteLine("<Import Project=\"$(VCTargetsPath)\\Microsoft.Cpp.Default.props\" />");
            projectWriter.WriteLine(@$"<PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>{platformToolset}</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>{platformToolset}</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>{platformToolset}</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>{platformToolset}</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>");
            projectWriter.WriteLine("<Import Project=\"$(VCTargetsPath)\\Microsoft.Cpp.props\" />");
            projectWriter.WriteLine(@"  <ImportGroup Label=""PropertySheets"" Condition=""'$(Configuration)|$(Platform)' =='Debug|Win32'"" >
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
  </ImportGroup>");
            projectWriter.WriteLine(@"  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
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
  </PropertyGroup>");
            projectWriter.WriteLine(@$"<ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>WIN32;_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>{includeDirs};%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs};%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item1};%(AdditionalDependencies)</AdditionalDependencies>
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
      <AdditionalIncludeDirectories>{includeDirs};%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs};%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item2};%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>{includeDirs};%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs};%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item1};%(AdditionalDependencies)</AdditionalDependencies>
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
      <AdditionalIncludeDirectories>{includeDirs};%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs};%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item2};%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>");
            projectWriter.WriteLine(@"  <ItemGroup>
    <ClCompile Include=""main.cpp"" />
  </ItemGroup> ");
            projectWriter.Write("</Project>");
            
            projectWriter.Close();

            return guid;
        }

        private static string GetIncludeDirectoriesAsString(ProjectModel projectModel)
        {
            string output = "";

            foreach (DependencyModel dependencyModel in projectModel.Dependencies)
            {
                output += $"$(SolutionDir)/dependencies/{dependencyModel.IncludeDir};";
            }

            return output;
        }

        private static string GetLibraryDirectoriesAsString(ProjectModel projectModel)
        {
            string output = "";

            foreach (DependencyModel dependencyModel in projectModel.Dependencies)
            {
                output += $"$(SolutionDir)/dependencies/{dependencyModel.LibDir};";
            }

            return output;
        }

        private static Tuple<string, string> GetLibNamesAsStrings(ProjectModel projectModel)
        {
            string debugNames = "";
            string releaseNames = "";

            foreach (DependencyModel dependencyModel in projectModel.Dependencies)
            {
                foreach (string name in dependencyModel.DebugLibNames)
                {
                    debugNames += name + ';';
                }
                foreach (string name in dependencyModel.ReleaseLibNames)
                {
                    releaseNames += name + ';';
                }
            }

            return new Tuple<string, string>(debugNames, releaseNames);
        }
    }
}
