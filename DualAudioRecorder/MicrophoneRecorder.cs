using NAudio.Wave;

namespace BlekAudioRecorder
{
    public class MicrophoneRecorder : IDisposable
    {
        private readonly WaveInEvent waveIn;
        private readonly MemoryStream recordedAudioStream;
        private readonly WaveFileWriter waveFileWriter;

        public MicrophoneRecorder()
        {
            // Create a new WaveInEvent object
            waveIn = new WaveInEvent();

            // Set up the recording format to PCM 16-bit 44.1kHz stereo
            waveIn.WaveFormat = new WaveFormat(44100, 16, 2);

            // Create a new MemoryStream object to store the recorded audio
            recordedAudioStream = new MemoryStream();

            // Create a new WaveFileWriter object to write the recorded audio to a file
            waveFileWriter = new WaveFileWriter(recordedAudioStream, waveIn.WaveFormat);
        }

        public void StartRecording(int deviceNumber)
        {
            // Set the device number for the WaveInEvent object
            waveIn.DeviceNumber = deviceNumber;

            // Set up an event handler for the DataAvailable event of the WaveInEvent object
            waveIn.DataAvailable += (s, e) =>
            {
                // Write the recorded audio to the MemoryStream object
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
            };

            // Start recording
            waveIn.StartRecording();
        }

        public void StopRecording()
        {
            // Stop recording
            waveIn.StopRecording();

            // Close the WaveFileWriter object
            waveFileWriter.Close();
        }

        public void Save(string filename)
        {
            // Save the recorded audio to a file
            File.WriteAllBytes(filename, recordedAudioStream.ToArray());
        }

        public void Dispose()
        {
            // Dispose of the WaveInEvent object
            waveIn.Dispose();
        }
    }
}
