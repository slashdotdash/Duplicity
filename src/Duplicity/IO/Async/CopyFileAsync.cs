using System;
using System.IO;
using System.Threading.Tasks;

namespace Duplicity.IO.Async
{
    public sealed class CopyFileAsync
    {
        private readonly string _source;
        private readonly string _destination;
        private readonly bool _overwrite;

        public CopyFileAsync(string source, string destination, bool overwrite)
        {
            if (string.IsNullOrWhiteSpace(source)) throw new ArgumentNullException("source");
            if (string.IsNullOrWhiteSpace(destination)) throw new ArgumentNullException("source");

            _source = source;
            _destination = destination;
            _overwrite = overwrite;                       
        }

        public Task Execute()
        {
            PreventOverwrittingDestinationFile();

            // Note: Create FileStream with "use async" enabled
            var input = FileAsync.OpenRead(_source);
            var output = FileAsync.OpenWrite(_destination);

            // Copy the stream and when complete, close both streams and propagate any exceptions
            return input.CopyStreamToStreamAsync(output).ContinueWith(t => 
            {
                var e = t.Exception;
                
                input.Close();
                output.Close();

                if (e != null) throw e;
            }, TaskContinuationOptions.ExecuteSynchronously);
        }
        
        /// <summary>
        /// Prevent overwritting of existing files when overwrite option not set.
        /// </summary>
        private void PreventOverwrittingDestinationFile()
        {
            if (_overwrite) return;

            if (File.Exists(_destination))
                throw new IOException(string.Format(@"File ""{0}"" already exists", _destination));
        }
    }
}