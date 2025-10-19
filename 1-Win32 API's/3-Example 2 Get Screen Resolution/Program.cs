using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _3_Example_2_Get_Screen_Resolution
{
    internal class Program
    {



        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(int index);


        static void Main(string[] args)
        {
            int ScreenWidth = GetSystemMetrics(0); // SM_CXScreen
            int ScreenHeigth = GetSystemMetrics(1); // SM_CYScreen

            Console.WriteLine("System width : {0} , Sreen height : {1}",ScreenWidth,ScreenHeigth);


            Console.ReadLine(); 



        }
    }
}
