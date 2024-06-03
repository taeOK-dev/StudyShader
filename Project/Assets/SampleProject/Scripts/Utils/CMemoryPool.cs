using System;
using System.Buffers;

public abstract class CMemoryPool<T> : MemoryPool<T>
{
    public new static MemoryPool<T> Shared
    {
        get
        {
            return CMemoryPool<T>.s_shared;
        }
    }

    private static readonly CArrayMemoryPool<T> s_shared = new CArrayMemoryPool<T>();
}

internal class CArrayMemoryPool<T> : MemoryPool<T>
{
    public override int MaxBufferSize
    {
        get
        {
            return 2147483647;
        }
    }

    public override IMemoryOwner<T> Rent(int minimumBufferSize = -1)
    {
        if (minimumBufferSize > 2147483647)
        {
            throw new ArgumentException($"{minimumBufferSize} > {MaxBufferSize}");
        }

        IMemoryOwner<T> buffer = new CArrayMemoryPool<T>.CArrayMemoryPoolBuffer(minimumBufferSize);
        buffer.Memory.Span.Clear();
        return buffer;
    }

    protected override void Dispose(bool disposing)
    {
    }

    private class CArrayMemoryPoolBuffer : IMemoryOwner<T>, IDisposable
    {
        int _size;
        private T[] _array;

        public CArrayMemoryPoolBuffer(int size)
        {
            _size = size;
            this._array = ArrayPool<T>.Shared.Rent(size);
        }

        public Memory<T> Memory
        {
            get
            {
                T[] array = this._array;

                if (array == null)
                {
                    throw new NullReferenceException();
                }

                return new Memory<T>(array, 0, _size);
            }
        }

        public void Dispose()
        {
            T[] array = this._array;
            if (array != null)
            {
                this._array = null;
                ArrayPool<T>.Shared.Return(array, true);
            }
        }
    }
}