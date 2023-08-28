using System.IO;
using System.Reflection;

namespace ETModel
{
    public static class DllHelper
    {
        public static Assembly GetHotfixAssembly()
        {
            var dllBytes = File.ReadAllBytes("./Hotfix.dll");
            var pdbBytes = File.ReadAllBytes("./Hotfix.pdb");
            var assembly = Assembly.Load(dllBytes, pdbBytes);
            return assembly;
        }
    }
}