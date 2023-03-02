using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;

namespace BlekAudioRecorder
{
    internal class MicrophoneSelector
    {
        private static void PrintMicrophones()
        {
            // Get the number of microphones available
            // Ask for microphone input
            Console.WriteLine("Choose a microphone to record from:");
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {devices[i].FriendlyName}");
            }
        }

        public static int GetDesiredMicrophoneNumer()
        {
            PrintMicrophones();
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

            while (true)
            { 
                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    number--;
                    if (0 <= number && number < devices.Count)
                    { 
                        return number;
                    }
                }
                Console.WriteLine("You entered incorrect Microphone Numer.");
            }
        }
    }
}
