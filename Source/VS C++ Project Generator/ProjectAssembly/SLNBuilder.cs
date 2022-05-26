using System;
using System.Collections.Generic;
using System.Text;
using VS_CPP_Project_Generator.Models;
using VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public class SLNBuilder
    {
        private string _version;
        private ProjectModel _projectModel;
        private List<VSProject> _projects;

        public SLNBuilder(ProjectModel projectModel, string formatVersion)
        {
            _version = formatVersion;
            _projectModel = projectModel;
        }

        public void AddProject(VSProject project)
        {
            _projects.Add(project);
        }

        public string BuildFileContent()
        {
            string output = "";

            output += $"Microsoft Visual Studio Solution File, Format Version {_version}";
            foreach (VSProject project in _projects)
            {
                output += $"Project(\"{{{project.GetProjectTypeGUID()}}}\") = \"{_projectModel.Name}\", \"{_projectModel.Name}/{_projectModel.Name}.vcxproj\", \"{{{project.GUID}}}\"";
                output += "EndProject";
            }

            return output;
        }
    }
}
