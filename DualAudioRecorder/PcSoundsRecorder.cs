using NAudio.Wave;

namespace BlekAudioRecorder
{
    public class PcSoundsRecorder : IDisposable
    {
        private readonly WaveOutEvent waveOut;
        private readonly WasapiLoopbackCapture wasapiLoopbackCapture;
        private readonly MemoryStream recordedAudioStream;
        private readonly WaveFileWriter waveFileWriter;

        public PcSoundsRecorder()
        {
            // Create a new WaveOutEvent object
            waveOut = new WaveOutEvent();

            // Set up a new WasapiLoopbackCapture object
            wasapiLoopbackCapture = new WasapiLoopbackCapture();

            // Set the recording format to PCM 16-bit 44.1kHz stereo
            wasapiLoopbackCapture.WaveFormat = new WaveFormat(44100, 16, 2);

            // Create a new MemoryStream object to store the recorded audio
            recordedAudioStream = new MemoryStream();

            // Create a new WaveFileWriter object to write the recorded audio to a file
            waveFileWriter = new WaveFileWriter(recordedAudioStream, wasapiLoopbackCapture.WaveFormat);
        }

        public void StartRecording()
        {
            // Set up an event handler for the DataAvailable event of the WasapiLoopbackCapture object
            wasapiLoopbackCapture.DataAvailable += (s, e) =>
            {
                // Write the recorded audio to the MemoryStream object
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
            };

            // Start recording
            wasapiLoopbackCapture.StartRecording();
        }

        public void StopRecording()
        {
            // Stop recording
            wasapiLoopbackCapture.StopRecording();

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
            // Dispose of the WaveOutEvent object and the WasapiLoopbackCapture object
            waveOut.Dispose();
            wasapiLoopbackCapture.Dispose();
        }
    }
}