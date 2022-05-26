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

        public abstract string BuildXML();

    }
}
