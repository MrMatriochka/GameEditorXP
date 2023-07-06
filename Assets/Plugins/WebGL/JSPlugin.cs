
using System.Runtime.InteropServices;
public static class JSPlugin
{
    [DllImport("__Internal")]
    public static extern string FindGroup();

}
