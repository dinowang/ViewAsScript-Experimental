using System;
using System.IO;
using System.Text;

namespace ViewAsScript.Middlewares
{
    public class JavaScriptStream : Stream, IDisposable
    {
        private readonly Stream _stream;

        private bool _dirty;

        private static byte[] _NEWLINE = Encoding.UTF8.GetBytes("\\n");
        private static byte[] _QUOTE = Encoding.UTF8.GetBytes("\\\"");

        public JavaScriptStream(Stream stream)
        {
            _stream = stream;
            _dirty = false;
        }

        private void EnsurePrefix()
        {
            if (_dirty) 
                return;

            _dirty = true;

            var start = Encoding.UTF8.GetBytes("document.write(\"");
            _stream.WriteAsync(start);
        }

        private void EnsurePostfix()
        {
            if (!_dirty)
                return;

            var postfix = Encoding.UTF8.GetBytes("\");");
            _stream.WriteAsync(postfix);
        }

        public new void Dispose()
        {
            EnsurePostfix();

            _stream.Flush();
            _stream.Dispose();
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsurePrefix();

            var ptr = 0; 
            var length = 0;

            for (var i = offset; i < offset + count; i++)
            {
                switch (buffer[i])
                {
                    case 34: // ":
                        if (length > 0)
                        {
                            _stream.Write(buffer, ptr, length);
                        }
                        _stream.Write(_QUOTE, 0, _QUOTE.Length);
                        length = 0;
                        ptr = i + 1;
                        break;

                    case 10: // \n
                        if (length > 0)
                        {
                            _stream.Write(buffer, ptr, length);
                        }
                        _stream.Write(_NEWLINE, 0, _NEWLINE.Length);
                        length = 0;
                        ptr = i + 1;
                        break;

                    case 13: // \r
                        // ignore
                        if (length > 0)
                        {
                            _stream.Write(buffer, ptr, length);
                        }
                        i++;
                        length = 0;
                        ptr = i + 1;
                        break;

                    default:
                        length++;
                        break;
                }
            }
            if (length > 0)
            {
                _stream.Write(buffer, ptr, length);
            }
        }
    }
}