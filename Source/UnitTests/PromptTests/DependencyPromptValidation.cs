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
    }
}
