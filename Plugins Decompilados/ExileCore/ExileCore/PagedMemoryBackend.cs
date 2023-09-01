// Decompiled with JetBrains decompiler
// Type: ExileCore.PagedMemoryBackend
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Helpers;
using MoreLinq.Extensions;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExileCore
{
  public class PagedMemoryBackend : IMemoryBackend, IDisposable
  {
    private readonly ConcurrentDictionary<IntPtr, IMemoryOwner<byte>> _cachedPages = new ConcurrentDictionary<IntPtr, IMemoryOwner<byte>>();
    private readonly ConcurrentDictionary<IntPtr, bool> _pagesRequestedOnLastIteration = new ConcurrentDictionary<IntPtr, bool>();
    private CancellationTokenSource _nextFrameCts;
    private bool _disposed;
    private const int PageSize = 4096;
    private readonly IMemoryBackend _pageBackend;
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public PagedMemoryBackend(IMemoryBackend pageBackend) => this._pageBackend = pageBackend;

    private IntPtr GetPageAddress(IntPtr address) => (IntPtr) ((long) (address.GetValue() / 4096UL) * 4096L);

    private IMemoryOwner<byte> GetRealPage(IntPtr address) => this.GetRealPage(address, 1);

    private IMemoryOwner<byte> GetRealPage(IntPtr address, int pageCount)
    {
      int num = 4096 * pageCount;
      IMemoryOwner<byte> realPage = MemoryPool<byte>.Shared.Rent(num);
      Span<byte> span = realPage.Memory.Slice(0, num).Span;
      if (this._pageBackend.TryReadMemory(address, span))
        return realPage;
      realPage.Dispose();
      return (IMemoryOwner<byte>) null;
    }

    public bool TryReadMemory(IntPtr address, Span<byte> target)
    {
      this._lock.EnterReadLock();
      try
      {
        if (target.Length == 0)
          return true;
        IntPtr pageAddress1 = this.GetPageAddress(address);
        IntPtr pageAddress2 = this.GetPageAddress(address + target.Length - 1);
        if (pageAddress1.GetValue() > pageAddress2.GetValue())
          return false;
        int start1 = checked ((int) (address.GetValue() - pageAddress1.GetValue()));
        IntPtr num1 = pageAddress1;
        while (num1.GetValue() < pageAddress2.GetValue())
        {
          IMemoryOwner<byte> page = this.GetPage(num1);
          if (page == null)
            return false;
          Span<byte> span = page.Memory.Span;
          ref Span<byte> local1 = ref span;
          int num2 = start1;
          int start2 = num2;
          int length = 4096 - num2;
          local1.Slice(start2, length).CopyTo(target);
          num1 += 4096;
          ref Span<byte> local2 = ref target;
          int start3 = 4096 - start1;
          target = local2.Slice(start3, local2.Length - start3);
          start1 = 0;
        }
        IMemoryOwner<byte> page1 = this.GetPage(pageAddress2);
        if (page1 == null)
          return false;
        page1.Memory.Span.Slice(start1, target.Length).CopyTo(target);
      }
      finally
      {
        this._lock.ExitReadLock();
      }
      return true;
    }

    private IMemoryOwner<byte> GetPage(IntPtr pageAddress)
    {
      IMemoryOwner<byte> orAdd = this._cachedPages.GetOrAdd(pageAddress, new Func<IntPtr, IMemoryOwner<byte>>(this.GetRealPage));
      this._pagesRequestedOnLastIteration.TryAdd(pageAddress, orAdd != null);
      return orAdd;
    }

    public void NotifyFrame()
    {
      this._lock.EnterWriteLock();
      try
      {
        this._nextFrameCts?.Cancel();
        CancellationTokenSource thisFrameCts = new CancellationTokenSource();
        this._nextFrameCts = thisFrameCts;
        this._pageBackend.NotifyFrame();
        List<(IntPtr, int)> pagesToPreRead = this._pagesRequestedOnLastIteration.Where<KeyValuePair<IntPtr, bool>>((Func<KeyValuePair<IntPtr, bool>, bool>) (x => x.Value)).Select<KeyValuePair<IntPtr, bool>, IntPtr>((Func<KeyValuePair<IntPtr, bool>, IntPtr>) (x => x.Key)).OrderBy<IntPtr, IntPtr>((Func<IntPtr, IntPtr>) (x => x)).Segment<IntPtr>((Func<IntPtr, IntPtr, int, bool>) ((b, a, _) => (long) b.GetValue() - (long) a.GetValue() != 4096L)).Select<IEnumerable<IntPtr>, (IntPtr, int)>((Func<IEnumerable<IntPtr>, (IntPtr, int)>) (x => (x.First<IntPtr>(), x.Count<IntPtr>()))).ToList<(IntPtr, int)>();
        this._pagesRequestedOnLastIteration.Clear();
        this.DropRentedPages();
        Task.Run((Action) (() =>
        {
          foreach ((IntPtr address, int pageCount) in pagesToPreRead)
          {
            if (thisFrameCts.IsCancellationRequested)
              break;
            using (IMemoryOwner<byte> realPage = this.GetRealPage(address, pageCount))
            {
              if (thisFrameCts.IsCancellationRequested)
                break;
              if (realPage != null)
              {
                for (int start = 0; start < pageCount * 4096; start += 4096)
                {
                  IMemoryOwner<byte> memoryOwner = MemoryPool<byte>.Shared.Rent(4096);
                  realPage.Memory.Slice(start, 4096).CopyTo(memoryOwner.Memory.Slice(0, 4096));
                  this._lock.EnterReadLock();
                  try
                  {
                    if (!this._disposed)
                    {
                      if (this._cachedPages.TryAdd(address + start, memoryOwner))
                        continue;
                    }
                    memoryOwner.Dispose();
                  }
                  finally
                  {
                    this._lock.ExitReadLock();
                  }
                }
              }
            }
          }
        }), thisFrameCts.Token);
      }
      finally
      {
        this._lock.ExitWriteLock();
      }
    }

    private void DropRentedPages()
    {
      foreach (KeyValuePair<IntPtr, IMemoryOwner<byte>> cachedPage in this._cachedPages)
      {
        if (this._cachedPages.TryRemove(cachedPage))
          cachedPage.Value?.Dispose();
      }
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._lock.EnterWriteLock();
      try
      {
        if (this._disposed)
          return;
        this._disposed = true;
        this._nextFrameCts?.Cancel();
        this.DropRentedPages();
      }
      finally
      {
        this._lock.ExitWriteLock();
      }
    }
  }
}
