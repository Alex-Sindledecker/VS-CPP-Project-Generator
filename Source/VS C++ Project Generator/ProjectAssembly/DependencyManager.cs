using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using VS_CPP_Project_Generator.Models;

namespace VS_CPP_Project_Generator.ProjectAssembly
{
    public class DependencyManager
    {
        private string _intDir;
        private ProjectModel _model;
        private List<string> _extractions;

        public DependencyManager(ProjectModel model, string intDir)
        {
            _intDir = intDir;
            _model = model;
            _extractions = new List<string>();
        }

        public void AquireDependencies()
        {
            foreach (DependencyModel dependencyModel in _model.Dependencies)
            {
                using (WebClient client = new WebClient())
                {
                    string name = Path.GetFileNameWithoutExtension(dependencyModel.Url);
                    client.DownloadFile(dependencyModel.Url, $"{_intDir}{name}.zip");
                    _extractions.Add(name);
                }
            }
        }

        public void ExtractDependencies()
        {
            foreach (string name in _extractions)
            {
                ZipFile.ExtractToDirectory($"{_intDir}{name}.zip", $"{_intDir}{name}/");
            }
        }

        public void DistributeDependencies()
        {
            string dest = $"{_model.DiskLocation}Source/Dependencies/";
            for (int i = 0; i < _model.Dependencies.Count; i++)
            {
                string name = _extractions[i];
                Directory.Move($"{_intDir}{name}/", $"{dest}{name}");
            }

            Directory.Delete(_intDir, true);
        }
    }
}
