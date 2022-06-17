using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Prompts;

namespace UnitTests
{
    [TestClass]
    public class PromptTests
    {
        [TestMethod]
        public void URLValidationTest()
        {
            const string testUrl1 = "https://github.com/Alex-Sindledecker/VS-CPP-Project-Generator/tree/development";
            const string testUrl2 = "https://github.com/canton7/Stylet/";
            const string testUrl3 = "https://github.com/Alex-Sindledecker/VS-CPP-Project-Generator";
            const string testUrl4 = "https://www.w3schools.com/cs/cs_user_input.php";
            const string testUrl5 = "https://github.com/SFML/SFML/releases/download/2.5.1/SFML-2.5.1-windows-vc15-64-bit.zip";

            DependencyURLPrompt dependencyURLPrompt = new DependencyURLPrompt();
            bool test1 = dependencyURLPrompt.Validate(testUrl1);
            bool test2 = dependencyURLPrompt.Validate(testUrl2);
            bool test3 = dependencyURLPrompt.Validate(testUrl3);
            bool test4 = dependencyURLPrompt.Validate(testUrl4);
            bool test5 = dependencyURLPrompt.Validate(testUrl5);

            Assert.AreEqual(test1, false, $"Bad github url accepted ({testUrl1})");
            Assert.AreEqual(test2, true, $"Correct github url not accepted ({testUrl2})");
            Assert.AreEqual(test3, true, $"Correct github url not accepted ({testUrl3})");
            Assert.AreEqual(test4, false, $"Bad url accepted (php url) ({testUrl4})");
            Assert.AreEqual(test5, true, $"Zip file not accepted! ({testUrl5})");
        }

        [TestMethod]
        public void DependencyDirectoryValidationTest()
        {
            const string testDir1 = "SFML-2.5.1/include";
            const string testDir2 = "SFML-2.5.1/include/";
            const string testDir3 = "Hello?/";
            const string testDir4 = "Hello*/";
            const string testDir5 = "Hello\"/";
            const string testDir6 = "Hello</";
            const string testDir7 = "Hello>/";
            const string testDir8 = "Hello|/";
            const string testDir9 = "Hello";

            DependencyDirectoryPrompt dirPrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Include); //The dependency directory type doesn't matter
            bool test1 = dirPrompt.Validate(testDir1);
            bool test2 = dirPrompt.Validate(testDir2);
            bool test3 = dirPrompt.Validate(testDir3);
            bool test4 = dirPrompt.Validate(testDir4);
            bool test5 = dirPrompt.Validate(testDir5);
            bool test6 = dirPrompt.Validate(testDir6);
            bool test7 = dirPrompt.Validate(testDir7);
            bool test8 = dirPrompt.Validate(testDir8);
            bool test9 = dirPrompt.Validate(testDir9);

            Assert.AreEqual(test1, true, $"Valid directory not accepted ({testDir1})");
            Assert.AreEqual(test2, true, $"Valid directory not accepted ({testDir2})");
            Assert.AreEqual(test3, false, $"Invalid directory accepted ({testDir3})");
            Assert.AreEqual(test4, false, $"Invalid directory accepted ({testDir4})");
            Assert.AreEqual(test5, false, $"Invalid directory accepted ({testDir5})");
            Assert.AreEqual(test6, false, $"Invalid directory accepted ({testDir6})");
            Assert.AreEqual(test7, false, $"Invalid directory accepted ({testDir7})");
            Assert.AreEqual(test8, false, $"Invalid directory accepted ({testDir8})");
            Assert.AreEqual(test9, true, $"Valid directory not accepted ({testDir9})");
        }

        [TestMethod]
        public void DependencyDirectoryPopulateTest()
        {
            DependencyModel dependencyModel = new DependencyModel();

            DependencyDirectoryPrompt includePrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Include);
            DependencyDirectoryPrompt libraryPrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Library);
            DependencyDirectoryPrompt dllPrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Dll);

            const string includeDir = "SFML-2.5.1/include/";
            const string libraryDir = "SFML-2.5.1/lib/";
            const string dllDir = "";

            includePrompt.Validate("SFML-2.5.1/include/");
            libraryPrompt.Validate("SFML-2.5.1/lib");
            dllPrompt.Validate("");

            includePrompt.Populate(dependencyModel);
            libraryPrompt.Populate(dependencyModel);
            dllPrompt.Populate(dependencyModel);

            Assert.AreEqual(dependencyModel.IncludeDir, includeDir, "Include prompt did not populate dependency model IncludeDir");
            Assert.AreEqual(dependencyModel.LibDir, libraryDir, "Library prompt did not populate dependency model LibDir");
            Assert.AreEqual(dependencyModel.DllDir, dllDir, "Dll prompt did not populate dependency model DllDir");
        }

        [TestMethod]
        public void LibraryNamesValidationTest()
        {
            const string testNames1 = "sfml.lib";
            const string testNames2 = "sfml-d.lib";
            const string testNames3 = "sfml-d.lib, sfml.lib";
            const string testNames4 = "sfml-d.lib, sfml.lib, sfml-graphics.lib";
            const string testNames5 = "sfml.qlib";
            const string testNames6 = "sfml.lib, sfml.qlib";
            const string testNames7 = ".lib";
            const string testNames8 = "lib";

            LibraryNamesPrompt prompt = new LibraryNamesPrompt(DependencyLibraryConfiguration.Debug); // The configuration doesn't matter

            bool test1 = prompt.Validate(testNames1);
            bool test2 = prompt.Validate(testNames2);
            bool test3 = prompt.Validate(testNames3);
            bool test4 = prompt.Validate(testNames4);
            bool test5 = prompt.Validate(testNames5);
            bool test6 = prompt.Validate(testNames6);
            bool test7 = prompt.Validate(testNames7);
            bool test8 = prompt.Validate(testNames8);

            Assert.AreEqual(test1, true, $"{testNames1} was marked invalid!");
            Assert.AreEqual(test2, true, $"{testNames2} was marked invalid!");
            Assert.AreEqual(test3, true, $"{testNames3} was marked invalid!");
            Assert.AreEqual(test4, true, $"{testNames4} was marked invalid!");
            Assert.AreEqual(test5, false, $"{testNames5} was marked valid!");
            Assert.AreEqual(test6, false, $"{testNames6} was marked valid!");
            Assert.AreEqual(test7, false, $"{testNames7} was marked valid!");
            Assert.AreEqual(test8, false, $"{testNames8} was marked valid!");
        }

        [TestMethod]
        public void LibraryNamesPopulateTest()
        {
            DependencyModel dependencyModel = new DependencyModel();

            LibraryNamesPrompt debugPrompt = new LibraryNamesPrompt(DependencyLibraryConfiguration.Debug);
            LibraryNamesPrompt releasePrompt = new LibraryNamesPrompt(DependencyLibraryConfiguration.Release);

            const string debugLibList = "sfml-system-d.lib, sfml-window-d.lib, sfml-graphics-d.lib";
            const string releaseLibList = "sfml-system.lib";

            debugPrompt.Validate(debugLibList);
            releasePrompt.Validate(releaseLibList);

            debugPrompt.Populate(dependencyModel);
            releasePrompt.Populate(dependencyModel);

            List<string> expectedDebugResult = new List<string>() { "sfml-system-d.lib", "sfml-window-d.lib", "sfml-graphics-d.lib" };
            List<string> expectedReleaseResult = new List<string>() { "sfml-system.lib" };

            CollectionAssert.AreEqual(dependencyModel.DebugLibNames, expectedDebugResult, "Dependency model debug lib names not correctly populated");
            CollectionAssert.AreEqual(dependencyModel.ReleaseLibNames, expectedReleaseResult, "Dependency model release lib names not correctly populated");
        }

        [TestMethod]
        public void TestAdditionalIncludeFiles()
        {
            DependencyModel dependencyModel = new DependencyModel();

            IncludeFilesPrompt includeFilesPrompt = new IncludeFilesPrompt();

            bool test1 = includeFilesPrompt.Validate("");
            bool test2 = includeFilesPrompt.Validate("file1.cpp");
            bool test3 = includeFilesPrompt.Validate("path/file2.cpp");
            bool test4 = includeFilesPrompt.Validate("file3");
            bool test5 = includeFilesPrompt.Validate("path/file4");

            Assert.AreEqual(test1, true, "Include prompts should allow empty field!");
            Assert.AreEqual(test2, true, "Include prompt not allowing \"file1.cpp\"");
            Assert.AreEqual(test3, true, "Include prompt not allowing \"path/file2.cpp\"");
            Assert.AreEqual(test4, false, "Include prompt allowing \"file3\"");
            Assert.AreEqual(test5, false, "Include prompt allowing \"path/file4\"");
        }
    }
}
