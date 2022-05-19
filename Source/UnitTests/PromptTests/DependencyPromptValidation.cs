using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void IncludeDirValidation()
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

            IncludeDirPrompt includeDirPrompt = new IncludeDirPrompt();
            bool test1 = includeDirPrompt.Validate(testDir1);
            bool test2 = includeDirPrompt.Validate(testDir2);
            bool test3 = includeDirPrompt.Validate(testDir3);
            bool test4 = includeDirPrompt.Validate(testDir4);
            bool test5 = includeDirPrompt.Validate(testDir5);
            bool test6 = includeDirPrompt.Validate(testDir6);
            bool test7 = includeDirPrompt.Validate(testDir7);
            bool test8 = includeDirPrompt.Validate(testDir8);
            bool test9 = includeDirPrompt.Validate(testDir9);

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
    }
}
