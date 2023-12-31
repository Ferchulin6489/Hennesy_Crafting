﻿// Decompiled with JetBrains decompiler
// Type: ExileCore.Shared.CircularBuffer`1
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.Shared
{
  public class CircularBuffer<T> : IEnumerable<T>, IEnumerable
  {
    private readonly T[] _buffer;
    private int _end;
    private int _size;
    private int _start;

    public CircularBuffer(int capacity)
      : this(capacity, new T[0])
    {
    }

    public CircularBuffer(int capacity, T[] items)
    {
      if (capacity < 1)
        throw new ArgumentException("Circular buffer cannot have negative or zero capacity.", nameof (capacity));
      if (items == null)
        throw new ArgumentNullException(nameof (items));
      if (items.Length > capacity)
        throw new ArgumentException("Too many items to fit circular buffer", nameof (items));
      this._buffer = new T[capacity];
      Array.Copy((Array) items, (Array) this._buffer, items.Length);
      this._size = items.Length;
      this._start = 0;
      this._end = this._size == capacity ? 0 : this._size;
    }

    public int Capacity => this._buffer.Length;

    public bool IsFull => this.Size == this.Capacity;

    public bool IsEmpty => this.Size == 0;

    public int Size => this._size;

    public T this[int index]
    {
      get
      {
        if (this.IsEmpty)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
          interpolatedStringHandler.AppendLiteral("Cannot access index ");
          interpolatedStringHandler.AppendFormatted<int>(index);
          interpolatedStringHandler.AppendLiteral(". Buffer is empty");
          throw new IndexOutOfRangeException(interpolatedStringHandler.ToStringAndClear());
        }
        if (index >= this._size)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
          interpolatedStringHandler.AppendLiteral("Cannot access index ");
          interpolatedStringHandler.AppendFormatted<int>(index);
          interpolatedStringHandler.AppendLiteral(". Buffer size is ");
          interpolatedStringHandler.AppendFormatted<int>(this._size);
          throw new IndexOutOfRangeException(interpolatedStringHandler.ToStringAndClear());
        }
        return this._buffer[this.InternalIndex(index)];
      }
      set
      {
        if (this.IsEmpty)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
          interpolatedStringHandler.AppendLiteral("Cannot access index ");
          interpolatedStringHandler.AppendFormatted<int>(index);
          interpolatedStringHandler.AppendLiteral(". Buffer is empty");
          throw new IndexOutOfRangeException(interpolatedStringHandler.ToStringAndClear());
        }
        if (index >= this._size)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
          interpolatedStringHandler.AppendLiteral("Cannot access index ");
          interpolatedStringHandler.AppendFormatted<int>(index);
          interpolatedStringHandler.AppendLiteral(". Buffer size is ");
          interpolatedStringHandler.AppendFormatted<int>(this._size);
          throw new IndexOutOfRangeException(interpolatedStringHandler.ToStringAndClear());
        }
        this._buffer[this.InternalIndex(index)] = value;
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      ArraySegment<T>[] arraySegmentArray = new ArraySegment<T>[2]
      {
        this.ArrayOne(),
        this.ArrayTwo()
      };
      for (int index = 0; index < arraySegmentArray.Length; ++index)
      {
        ArraySegment<T> segment = arraySegmentArray[index];
        for (int i = 0; i < segment.Count; ++i)
          yield return segment.Array[segment.Offset + i];
        segment = new ArraySegment<T>();
      }
      arraySegmentArray = (ArraySegment<T>[]) null;
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public T Front()
    {
      this.ThrowIfEmpty();
      return this._buffer[this._start];
    }

    public T Back()
    {
      this.ThrowIfEmpty();
      return this._buffer[(this._end != 0 ? this._end : this.Capacity) - 1];
    }

    public void PushBack(T item)
    {
      if (this.IsFull)
      {
        this._buffer[this._end] = item;
        this.Increment(ref this._end);
        this._start = this._end;
      }
      else
      {
        this._buffer[this._end] = item;
        this.Increment(ref this._end);
        ++this._size;
      }
    }

    public void PushFront(T item)
    {
      if (this.IsFull)
      {
        this.Decrement(ref this._start);
        this._end = this._start;
        this._buffer[this._start] = item;
      }
      else
      {
        this.Decrement(ref this._start);
        this._buffer[this._start] = item;
        ++this._size;
      }
    }

    public void PopBack()
    {
      this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
      this.Decrement(ref this._end);
      this._buffer[this._end] = default (T);
      --this._size;
    }

    public void PopFront()
    {
      this.ThrowIfEmpty("Cannot take elements from an empty buffer.");
      this._buffer[this._start] = default (T);
      this.Increment(ref this._start);
      --this._size;
    }

    public T[] ToArray()
    {
      T[] destinationArray = new T[this.Size];
      int destinationIndex = 0;
      ArraySegment<T>[] arraySegmentArray = new ArraySegment<T>[2]
      {
        this.ArrayOne(),
        this.ArrayTwo()
      };
      foreach (ArraySegment<T> arraySegment in arraySegmentArray)
      {
        Array.Copy((Array) arraySegment.Array, arraySegment.Offset, (Array) destinationArray, destinationIndex, arraySegment.Count);
        destinationIndex += arraySegment.Count;
      }
      return destinationArray;
    }

    private void ThrowIfEmpty(string message = "Cannot access an empty buffer.")
    {
      if (this.IsEmpty)
        throw new InvalidOperationException(message);
    }

    private void Increment(ref int index)
    {
      if (++index != this.Capacity)
        return;
      index = 0;
    }

    private void Decrement(ref int index)
    {
      if (index == 0)
        index = this.Capacity;
      --index;
    }

    private int InternalIndex(int index) => this._start + (index < this.Capacity - this._start ? index : index - this.Capacity);

    private ArraySegment<T> ArrayOne() => this._start < this._end ? new ArraySegment<T>(this._buffer, this._start, this._end - this._start) : new ArraySegment<T>(this._buffer, this._start, this._buffer.Length - this._start);

    private ArraySegment<T> ArrayTwo() => this._start < this._end ? new ArraySegment<T>(this._buffer, this._end, 0) : new ArraySegment<T>(this._buffer, 0, this._end);
  }
}
