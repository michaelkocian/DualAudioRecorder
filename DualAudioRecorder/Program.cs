using BlekAudioRecorder;

class Program
{
    static DateTime? recordingStart;
    static void Main(string[] args)
    {
        int microphoneNumber = MicrophoneSelector.GetDesiredMicrophoneNumer();
        System.Timers.Timer timer = new(TimeSpan.FromMilliseconds(16));
        timer.Elapsed += Timer_Elapsed;

        while (true)
        { 
            PcSoundsRecorder pcSoundsRecorder = new PcSoundsRecorder();
            MicrophoneRecorder microphoneRecorder = new MicrophoneRecorder();

            Console.Write("Press any key to ");
            Console.ForegroundColor = ConsoleColor.Green; 
            Console.Write("start");
            Console.ResetColor();
            Console.WriteLine(" recording.");
            Console.ReadKey();

            // Start recording
            pcSoundsRecorder.StartRecording();
            microphoneRecorder.StartRecording(microphoneNumber);
            recordingStart = DateTime.Now;

            Console.Write("Press any key to ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("stop");
            Console.ResetColor();
            Console.WriteLine(" new recording.");
            Console.WriteLine();
            timer.Start();
            Console.ReadKey();


            // Stop recording
            pcSoundsRecorder.StopRecording();
            microphoneRecorder.StopRecording();
            recordingStart = null;
            timer.Stop();

            Console.WriteLine("Enter recording name:");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "sound_from";
            }
            name += DateTime.Now.ToString("_yyyy-MM-ddTHH-mm-ss");
            string pcSoundsFilename = name + ".pc.wav";
            string microphoneFilename = name + ".mic.wav";
            string mixedFilename = name + ".mix.wav";

            // Save files
            pcSoundsRecorder.Save(pcSoundsFilename);
            microphoneRecorder.Save(microphoneFilename);

            // Dispose of the objects
            pcSoundsRecorder.Dispose();
            microphoneRecorder.Dispose();

            // Combine the PC and MIC sounds
            SoundsMixer.MixFiles(pcSoundsFilename, microphoneFilename, mixedFilename);

            Console.WriteLine($"PC sounds saved to {pcSoundsFilename}.");
            Console.WriteLine($"Microphone audio saved to {microphoneFilename}.");
            Console.WriteLine($"Mixed sound saved to {mixedFilename}.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        if (recordingStart is null)
        {
            return;
        }
               
        // Move the cursor back to the beginning of the line
        Console.SetCursorPosition(0, Console.CursorTop-1);
        Console.WriteLine((e.SignalTime - recordingStart).ToString());
    }
}