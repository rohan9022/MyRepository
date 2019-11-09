using System.Runtime.InteropServices;
using System.Text;

namespace IMS.Helper
{
    internal class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
        string key,
        string val,
        string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
        string key,
        string def,
        StringBuilder retVal,
        int size,
        string filePath);

        public IniFile(string filePath)
        {
            this.FilePath = filePath;
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value.ToLower(), this.FilePath);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification = "<Pending>")]
        public string Read(string section, string key)
        {
            StringBuilder SB = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", SB, 255, this.FilePath);
            return SB.ToString();
        }

        public string FilePath { get; set; }
    }
}