using NAudio.Wave;

namespace BlekAudioRecorder
{
    internal class SoundsMixer
    {
        public static void MixFiles(string filename1, string filename2, string outputfilename)
        {
            // Load two audio files
            var file1 = new AudioFileReader(filename1);
            var file2 = new AudioFileReader(filename2);

            // Mix the two audio streams together
            var mixer = new WaveMixerStream32();
            mixer.AddInputStream(file1);
            mixer.AddInputStream(file2);

            // Create a new wave file writer to save the mixed audio
            var writer = new WaveFileWriter(outputfilename, mixer.WaveFormat);

            // Read samples from the mixer and write them to the output file
            byte[] buffer = new byte[8192];
            int bytesRead;
            while ((bytesRead = mixer.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, bytesRead);
            }

            // Close the writer and dispose of the audio streams
            writer.Close();
            mixer.Close();
            file1.Close();
            file2.Close();
        }
    }
}
