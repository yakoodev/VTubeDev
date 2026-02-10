using System.Threading;

namespace app.Transport
{
    public class FrameStore
    {
        private readonly ReaderWriterLockSlim _lock = new();
        private FrameData? _latest;
        private long _version;

        public long Version
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _version;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }
        }

        public FrameData? GetLatest()
        {
            _lock.EnterReadLock();
            try
            {
                return _latest;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Update(FrameData frame)
        {
            _lock.EnterWriteLock();
            try
            {
                _latest = frame;
                _version++;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
