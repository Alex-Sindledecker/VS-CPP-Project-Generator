using Microsoft.VisualStudio.TestTools.UnitTesting;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.Prompts;

namespace UnitTests.PromptTests
{
    [TestClass]
    public class DependencyPromptValidation
    {
        [TestMethod]
        public void URLTestValidation()
        {
            const string testUrl1 = "https://github.com/Alex-Sindledecker/VS-CPP-Project-Generator/tree/development";
            const string testUrl2 = "https://github.com/canton7/Stylet";
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
        public void DependencyDirectoryValidation()
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
        public void DependencyDirectoryPopulateValidation()
        {
            DependencyModel dependencyModel = new DependencyModel();

            DependencyDirectoryPrompt includePrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Include);
            DependencyDirectoryPrompt libraryPrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Library);
            DependencyDirectoryPrompt dllPrompt = new DependencyDirectoryPrompt(DependencyDirectoryType.Dll);

            const string includeDir = "SFML-2.5.1/include/";
            const string libraryDir = "SFML-2.5.1/lib/";
            const string dllDir = "SFML-2.5.1/dll/";

            includePrompt.Validate(includeDir);
            libraryPrompt.Validate(libraryDir);
            dllPrompt.Validate(dllDir);

            includePrompt.Populate(dependencyModel);
            libraryPrompt.Populate(dependencyModel);
            dllPrompt.Populate(dependencyModel);

            Assert.AreEqual(dependencyModel.IncludeDir, includeDir, "Include prompt did not populate dependency model IncludeDir");
            Assert.AreEqual(dependencyModel.LibDir, libraryDir, "Library prompt did not populate dependency model LibDir");
            Assert.AreEqual(dependencyModel.DllDir, dllDir, "Dll prompt did not populate dependency model DllDir");
        }
    }
}
