using System;

namespace SFB
{
    public struct ExtensionFilter
    {
        public string Name;
        public string[] Extensions;

        public ExtensionFilter(string filterName, params string[] filterExtensions)
        {
            Name = filterName;
            Extensions = filterExtensions;
        }
    }

    public class LoadImage
    {
        private static ILoadImage _platformWrapper = null;

        static LoadImage()
        {
#if UNITY_STANDALONE_OSX
            _platformWrapper = new LoadImageMac();
#elif UNITY_STANDALONE_WIN
            _platformWrapper = new LoadImageWindows();
#elif UNITY_STANDALONE_LINUX
            _platformWrapper = new LoadImageLinux();
#elif UNITY_EDITOR
            _platformWrapper = new LoadImageEditor();
#endif
        }


        public static string[] OpenFilePanel(string title, string directory, string extension, bool multiselect)
        {
            var extensions = string.IsNullOrEmpty(extension) ? null : new[] { new ExtensionFilter("", extension) };
            return OpenFilePanel(title, directory, extensions, multiselect);
        }

        public static string[] OpenFilePanel(string title, string directory, ExtensionFilter[] extensions, bool multiselect)
        {
            return _platformWrapper.OpenFilePanel(title, directory, extensions, multiselect);
        }

    }
}