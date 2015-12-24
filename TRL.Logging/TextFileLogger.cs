using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TRL.Logging
{
    public class TextFileLogger:ILogger, IDisposable
    {
        private string prefix;

        private int fileCounter;

        private long byteLimit;

        private StreamWriter streamWriter;

        public TextFileLogger():this("default", 10000000) { }

        public TextFileLogger(string namePrefix, long byteLimit)
        {
            this.fileCounter = 0;

            this.prefix = namePrefix;

            this.streamWriter = MakeStreamWriter();

            this.byteLimit = byteLimit;
        }

        public TextFileLogger(string namePrefix, long byteLimit, bool create)
        {
            this.fileCounter = 0;

            this.prefix = namePrefix;

            if (create)
            {
                this.streamWriter = NewStreamWriter();
            }
            else 
            {
                this.streamWriter = MakeStreamWriter();
            }
            this.byteLimit = byteLimit;
        }

        public void Log(string message)
        {
            if (LimitExceed())
            {
                CloseStream();
                this.streamWriter = MakeStreamWriter();
            }

            this.streamWriter.WriteLine(message);
            this.streamWriter.Flush();
        }

        private bool LimitExceed()
        {
            return this.streamWriter.BaseStream.Length > this.byteLimit;
        }

        private void CloseStream()
        {
            this.streamWriter.Close();
            this.streamWriter.Dispose();
            this.fileCounter++;
        }

        private string NewFileName()
        {
            return String.Format("{0}-{1}.log", this.prefix, this.fileCounter);
        }

        private StreamWriter MakeStreamWriter()
        {
            return new StreamWriter(new FileStream(NewFileName(), FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
        }

        private StreamWriter NewStreamWriter()
        {
            return new StreamWriter(new FileStream(NewFileName(), FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
        }

        public void Dispose()
        {
            this.streamWriter.Close();
            this.streamWriter.Dispose();
        }
    }
}
