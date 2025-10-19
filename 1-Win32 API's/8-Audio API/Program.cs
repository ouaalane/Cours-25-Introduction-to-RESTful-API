using System;
using NAudio.CoreAudioApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_Audio_API
{
    internal class Program
    {



        public static void PrintMenue()
        {
            Console.WriteLine("Welcome to the Volume Control app");
            Console.WriteLine("Choose an Option : ");
            Console.WriteLine("1 : Increase Volume");
            Console.WriteLine("2 : Decrease Volume");
            Console.WriteLine("3 : SetVolume (0-100)");
            Console.WriteLine("4 : Exit");
        }
        static void Main(string[] args)
        {
            MMDeviceEnumerator deviceenumerato = new MMDeviceEnumerator();

            var device  = deviceenumerato.GetDefaultAudioEndpoint(DataFlow.Render,Role.Multimedia);
            Console.WriteLine("Welcome to volume control App : ");

            bool IsRunning = true; 
            while (IsRunning)
            {
                Console.Clear();
                PrintMenue();
                Console.WriteLine("Choose an option :");
                var  option =  Console.ReadLine();
                

                switch (Convert.ToInt32(option))
                {
                    case 1:
                        device.AudioEndpointVolume.VolumeStepUp();
                        Console.WriteLine($"Volume is Increased {device.AudioEndpointVolume.MasterVolumeLevelScalar*100}");
                        Console.ReadLine();
                        break;

                    case 2:
                        device.AudioEndpointVolume.VolumeStepDown();
                        Console.WriteLine($"Volume is decreased {device.AudioEndpointVolume.MasterVolumeLevelScalar*100}");
                        Console.ReadLine();
                        break;

                    case 3:
                        float volume = float.Parse(Console.ReadLine())/100.0f;
                        device.AudioEndpointVolume.MasterVolumeLevelScalar = volume;
                        Console.WriteLine($"Volume set to {device.AudioEndpointVolume.MasterVolumeLevelScalar*100}");
                        Console.ReadLine();
                        break;

                    case 4:
                        IsRunning = false;
                        break;

                }
            }



        }
    }
}
