using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types
{
    //This class doesn't really need to exist right now because its only doing one thing, but in the future more compelx project types and configurations might be 
    //availible so this would allow for that to be implemented a bit easier
    public class VCXProj : VSProject
    {
        private List<string> _includeFiles;
        private ProjectModel _projectModel;
        private string _platformToolset;

        public VCXProj(ProjectModel model, string platformToolset)
        {
            _includeFiles = new List<string> { "main.cpp" };
            _projectModel = model;
            _platformToolset = platformToolset;

            foreach (DependencyModel dependencyModel in model.Dependencies)
            {
                foreach (string includeFile in dependencyModel.IncludeInProject)
                {
                    _includeFiles.Add($"$(SolutionDir)Dependencies/{includeFile}");
                }
            }
        }

        public override string GetProjectTypeGUID()
        {
            return "8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942"; //GUID for C++ console applications
        }

        public override string GetFileExtension()
        {
            return "vcxproj";
        }

        public override string BuildXML()
        {
            return BuildStandardXML();
        }

        //Returns xml for the .vcxproj.user file
        public string GetUserFileXML()
        {
            string[] configs = { "'Debug|Win32'", "'Release|Win32'", "'Debug|x64'", "'Release|x64'" };

            string output = "";

            string binPaths = "";
            foreach (DependencyModel dependencyModel in _projectModel.Dependencies)
                binPaths += $"$(SolutionDir)Dependencies\\{dependencyModel.DllDir};";

            output += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
            output += "<Project ToolsVersion=\"Current\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\n";
            foreach (string config in configs)
            {
                output += $"  <PropertyGroup Condition=\"'$(Configuration)|$(Platform)'=={config}\">\n";
                output += $"    <LocalDebuggerEnvironment>PATH=%PATH%;{binPaths}</LocalDebuggerEnvironment>\n";
                output += "    <DebuggerFlavor>WindowsLocalDebugger</DebuggerFlavor>\n";
                output += "  </PropertyGroup>\n";
            }
            output += "</Project>";

            return output;
        }

        //Returns an xml string that will ultimately be written to a file
        private string BuildStandardXML()
        {
            string outputXML = "";

            string includeDirs = GetIncludeDirectoriesAsString();
            string libDirs = GetLibraryDirectoriesAsString();
            Tuple<string, string> libStrings = GetLibNamesAsStrings();

            outputXML += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
            outputXML += "<Project DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\n";
            outputXML += @"  <ItemGroup Label=""ProjectConfigurations"">
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
  </ItemGroup>" + '\n';
            outputXML += @$"  <PropertyGroup Label=""Globals"">
    <VCProjectVersion>16.0</VCProjectVersion>
    <ProjectGuid>{{{GUID}}}</ProjectGuid>
    <WindowsTargetPlatformVersion>10.0</WindowsTargetPlatformVersion>
  </PropertyGroup>" + '\n';
            outputXML += "  <Import Project=\"$(VCTargetsPath)\\Microsoft.Cpp.Default.props\" />\n";
            outputXML += @$"  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>{_platformToolset}</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|Win32'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>{_platformToolset}</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>true</UseDebugLibraries>
    <PlatformToolset>{_platformToolset}</PlatformToolset>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>
  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Release|x64'"" Label=""Configuration"">
    <ConfigurationType>Application</ConfigurationType>
    <UseDebugLibraries>false</UseDebugLibraries>
    <PlatformToolset>{_platformToolset}</PlatformToolset>
    <WholeProgramOptimization>true</WholeProgramOptimization>
    <CharacterSet>Unicode</CharacterSet>
  </PropertyGroup>" + '\n';
            outputXML += "  <Import Project=\"$(VCTargetsPath)\\Microsoft.Cpp.props\" />\n";
            outputXML += @"  <ImportGroup Label=""PropertySheets"" Condition=""'$(Configuration)|$(Platform)' =='Debug|Win32'"" >
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
  </ImportGroup>" + '\n';
            outputXML += @"  <PropertyGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
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
  </PropertyGroup>" + '\n';
            outputXML += @$"  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|Win32'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>WIN32;_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>{includeDirs}%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs}%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item1}%(AdditionalDependencies)</AdditionalDependencies>
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
      <AdditionalIncludeDirectories>{includeDirs}%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs}%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item2}%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>
  <ItemDefinitionGroup Condition=""'$(Configuration)|$(Platform)'=='Debug|x64'"">
    <ClCompile>
      <WarningLevel>Level3</WarningLevel>
      <SDLCheck>true</SDLCheck>
      <PreprocessorDefinitions>_DEBUG;_CONSOLE;%(PreprocessorDefinitions)</PreprocessorDefinitions>
      <ConformanceMode>true</ConformanceMode>
      <AdditionalIncludeDirectories>{includeDirs}%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs}%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item1}%(AdditionalDependencies)</AdditionalDependencies>
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
      <AdditionalIncludeDirectories>{includeDirs}%(AdditionalIncludeDirectories)</AdditionalIncludeDirectories>
    </ClCompile>
    <Link>
      <SubSystem>Console</SubSystem>
      <EnableCOMDATFolding>true</EnableCOMDATFolding>
      <OptimizeReferences>true</OptimizeReferences>
      <GenerateDebugInformation>true</GenerateDebugInformation>
      <AdditionalLibraryDirectories>{libDirs}%(AdditionalLibraryDirectories)</AdditionalLibraryDirectories>
      <AdditionalDependencies>{libStrings.Item2}%(AdditionalDependencies)</AdditionalDependencies>
    </Link>
  </ItemDefinitionGroup>" + '\n';
            outputXML += "  <ItemGroup>\n";
            foreach (string file in _includeFiles)
                outputXML += @$"    <ClCompile Include=""{file}"" />" + '\n';
            outputXML += "  </ItemGroup> " + '\n';
            outputXML += "  <Import Project=\"$(VCTargetsPath)\\Microsoft.Cpp.targets\" />\n";
            outputXML += "</Project>\n";

            return outputXML;
        }

        private string GetIncludeDirectoriesAsString()
        {
            string output = "";

            foreach (DependencyModel dependencyModel in _projectModel.Dependencies)
            {
                output += $"$(SolutionDir)/dependencies/{dependencyModel.IncludeDir};";
            }

            return output;
        }

        private string GetLibraryDirectoriesAsString()
        {
            string output = "";

            foreach (DependencyModel dependencyModel in _projectModel.Dependencies)
            {
                output += $"$(SolutionDir)/dependencies/{dependencyModel.LibDir};";
            }

            return output;
        }

        private Tuple<string, string> GetLibNamesAsStrings()
        {
            string debugNames = "";
            string releaseNames = "";

            foreach (DependencyModel dependencyModel in _projectModel.Dependencies)
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
