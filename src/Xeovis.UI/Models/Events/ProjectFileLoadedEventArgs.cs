using System;
using Xeovis.Persistence;

namespace Xeovis.UI.Models.Events
{
    public class ProjectFileLoadedEventArgs : EventArgs
    {
        public ProjectFile Project { get; set; }
        public string Path { get; set; }

        public ProjectFileLoadedEventArgs(ProjectFile file, string? path = null)
        {
            Project = file;
            Path = path ?? string.Empty;
        }
    }
}
