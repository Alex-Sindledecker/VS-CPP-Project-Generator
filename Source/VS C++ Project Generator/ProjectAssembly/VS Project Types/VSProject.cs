using System;
using System.Collections.Generic;
using System.Text;

namespace VS_CPP_Project_Generator.ProjectAssembly.VS_Project_Types
{
    //Abstract class for creating visual studio project files (.vcxproj or .csproj for example)
    public abstract class VSProject
    {
        private Guid _guid;
        public Guid GUID { get { return _guid; } }

        public VSProject()
        {
            _guid = Guid.NewGuid();
        }

        //Builds the xml for a project
        public abstract string BuildXML();
        //Note that this is different than the GUID member above
        public abstract string GetProjectTypeGUID();
        //Returns the file extension (csproj or vcxproj for example)
        public abstract string GetFileExtension();
    }
}
