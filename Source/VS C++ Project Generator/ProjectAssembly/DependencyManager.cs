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
        public delegate void DependencyProcFinishedDelegate(int current, int total);

        //TODO: Un-static-ify these events. (They're only static right now because it works and I need to get this done)
        public static event DependencyProcFinishedDelegate DependencyAqusationEvent;
        public static event DependencyProcFinishedDelegate DependencyExtractingEvent;

        private string _destDir;
        private string _intDir;
        private ProjectModel _model;
        private List<string> _extractions;

        public DependencyManager(ProjectModel model, string intDir)
        {
            _intDir = intDir;
            _model = model;
            _destDir = $"{_model.DiskLocation}Source/Dependencies/";
            _extractions = new List<string>();
        }

        public void AquireDependencies()
        {
            for (int i = 0; i < _model.Dependencies.Count; i++)
            {
                DependencyAqusationEvent?.Invoke(i + 1, _model.Dependencies.Count);

                DependencyModel dependencyModel = _model.Dependencies[i];
                using (WebClient client = new WebClient())
                {
                    string name = Path.GetFileNameWithoutExtension(dependencyModel.Url);
                    if (dependencyModel.Url.EndsWith(".zip"))
                    {
                        //Zip file
                        client.DownloadFile(dependencyModel.Url, $"{_intDir}{name}.zip");
                        _extractions.Add(name);
                    }
                    else
                    {
                        //Github repo
                        System.Diagnostics.Process cmdProc = new System.Diagnostics.Process();
                        cmdProc.StartInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/c git clone --recursive {dependencyModel.Url} {_destDir}{name}");
                        cmdProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        cmdProc.StartInfo.UseShellExecute = true;
                        cmdProc.Start();
                        cmdProc.WaitForExit();
                    }
                }
            }
        }

        public void ExtractDependencies()
        {
            int current = 1;
            foreach (string name in _extractions)
            {
                DependencyExtractingEvent?.Invoke(current, _extractions.Count);
                ZipFile.ExtractToDirectory($"{_intDir}{name}.zip", $"{_destDir}{name}/");
                current++;
            }
        }
    }
}
