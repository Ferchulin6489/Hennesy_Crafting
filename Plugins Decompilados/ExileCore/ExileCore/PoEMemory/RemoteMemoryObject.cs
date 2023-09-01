// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.RemoteMemoryObject
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using ExileCore.Shared.Interfaces;
using System;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory
{
  public abstract class RemoteMemoryObject
  {
    private long _address;

    public long Address
    {
      get => this._address;
      protected set
      {
        if (this._address == value)
          return;
        this._address = value;
        this.OnAddressChange();
      }
    }

    public static ExileCore.Shared.Cache.Cache Cache => RemoteMemoryObject.pCache;

    public IMemory M => RemoteMemoryObject.pM;

    public TheGame TheGame => RemoteMemoryObject.pTheGame;

    public static TheGame pTheGame { get; protected set; }

    protected static ExileCore.Shared.Cache.Cache pCache { get; set; }

    protected static IMemory pM { get; set; }

    protected virtual void OnAddressChange()
    {
    }

    public T ReadObjectAt<T>(int offset) where T : RemoteMemoryObject, new() => this.ReadObject<T>(this.Address + (long) offset);

    public T ReadObject<T>(long addressPointer) where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(this.M.Read<long>(addressPointer));

    public T GetObjectAt<T>(int offset) where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(this.Address + (long) offset);

    public T GetObjectAt<T>(long offset) where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(this.Address + offset);

    public T GetObject<T>(long address) where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(address);

    public static T GetObjectStatic<T>(long address) where T : RemoteMemoryObject, new()
    {
      T objectStatic = new T();
      objectStatic.Address = address;
      return objectStatic;
    }

    public T GetObject<T>(IntPtr address) where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(address.ToInt64());

    public T AsObject<T>() where T : RemoteMemoryObject, new() => RemoteMemoryObject.GetObjectStatic<T>(this.Address);

    public override bool Equals(object obj) => obj is RemoteMemoryObject remoteMemoryObject && remoteMemoryObject.Address == this.Address;

    public override int GetHashCode() => (int) this.Address + this.GetType().Name.GetHashCode();

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
      interpolatedStringHandler.AppendFormatted<long>(this.Address, "X");
      return interpolatedStringHandler.ToStringAndClear();
    }

    protected FrameCache<T> CreateStructFrameCache<T>() where T : unmanaged => new FrameCache<T>((Func<T>) (() => this.M.Read<T>(this.Address)));
  }
}
