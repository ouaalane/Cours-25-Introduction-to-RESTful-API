using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _6_Example_Message_Box
{
    internal class Program
    {

        // Import message box function from user32.dll
        [DllImport("user32.dll",CharSet=CharSet.Unicode,SetLastError =true)]

        static extern int MessageBox(IntPtr intPtr, String text, String Caption, int type);
        static void Main(string[] args)
        {

            MessageBox(IntPtr.Zero,"Heloo guys","My message box",0);
        }
    }
}
