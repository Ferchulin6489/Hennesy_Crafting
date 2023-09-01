// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.EntityList
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared;
using ExileCore.Shared.Enums;
using GameOffsets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class EntityList : RemoteMemoryObject
  {
    private readonly WaitTime collectEntities = new WaitTime(1);
    private readonly List<long> hashAddresses = new List<long>(1000);
    private readonly HashSet<long> hashSet = new HashSet<long>(256);
    private readonly object locker = new object();
    private readonly Queue<long> queue = new Queue<long>(256);
    private readonly HashSet<long> StoreIds = new HashSet<long>(256);
    private readonly Stopwatch sw = Stopwatch.StartNew();

    public int EntitiesProcessed { get; private set; }

    public unsafe IEnumerator CollectEntities(EntityCollectSettingsContainer container)
    {
      EntityList entityList = this;
      if (entityList.Address == 0L)
      {
        DebugWindow.LogError("EntityList -> Address is 0;");
        yield return (object) new WaitTime(100);
      }
      while (!container.NeedUpdate)
        yield return (object) entityList.collectEntities;
      entityList.sw.Restart();
      long num1 = container.EntitiesCount();
      double num2 = 0.0;
      long addr1 = entityList.M.Read<long>(entityList.Address + 8L);
      entityList.hashAddresses.Clear();
      entityList.hashSet.Clear();
      entityList.StoreIds.Clear();
      entityList.queue.Enqueue(addr1);
      EntityListOffsets entityListOffsets = entityList.M.Read<EntityListOffsets>(addr1);
      entityList.queue.Enqueue(entityListOffsets.FirstAddr);
      entityList.queue.Enqueue(entityListOffsets.SecondAddr);
      int num3 = 0;
      DefaultInterpolatedStringHandler interpolatedStringHandler1;
      while (entityList.queue.Count > 0)
      {
        if (num3 < 10000)
        {
          try
          {
            ++num3;
            long addr2 = entityList.queue.Dequeue();
            if (!entityList.hashSet.Contains(addr2))
            {
              entityList.hashSet.Add(addr2);
              if (addr2 != addr1)
              {
                if (addr2 != 0L)
                {
                  long entity = entityListOffsets.Entity;
                  if (entity > 4096L && entity < 139637976727552L)
                    entityList.hashAddresses.Add(entity);
                  entityListOffsets = entityList.M.Read<EntityListOffsets>(addr2);
                  entityList.queue.Enqueue(entityListOffsets.FirstAddr);
                  entityList.queue.Enqueue(entityListOffsets.SecondAddr);
                }
              }
            }
          }
          catch (Exception ex)
          {
            interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(23, 1);
            interpolatedStringHandler1.AppendLiteral("Entitylist while loop: ");
            interpolatedStringHandler1.AppendFormatted<Exception>(ex);
            DebugWindow.LogError(interpolatedStringHandler1.ToStringAndClear());
          }
        }
        else
          break;
      }
      entityList.EntitiesProcessed = entityList.hashAddresses.Count;
      if (num1 > 0L && (double) ((long) entityList.EntitiesProcessed / num1) > 1.5)
      {
        interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(38, 2);
        interpolatedStringHandler1.AppendLiteral("Something wrong we parse ");
        interpolatedStringHandler1.AppendFormatted<int>(entityList.EntitiesProcessed);
        interpolatedStringHandler1.AppendLiteral(" when expect ");
        interpolatedStringHandler1.AppendFormatted<long>(num1);
        DebugWindow.LogError(interpolatedStringHandler1.ToStringAndClear());
        entityList.TheGame.IngameState.UpdateData();
      }
      if (container.ParseEntitiesInMultiThread() && (bool) container.CollectEntitiesInParallelWhenMoreThanX && container.MultiThreadManager != null && entityList.EntitiesProcessed / container.MultiThreadManager.ThreadsCount >= 100)
      {
        int hashAddressesCount = entityList.hashAddresses.Count / container.MultiThreadManager.ThreadsCount;
        List<Job> jobs = new List<Job>(container.MultiThreadManager.ThreadsCount);
        for (int i = 1; i <= container.MultiThreadManager.ThreadsCount; ++i)
        {
          int i1 = i;
          MultiThreadManager multiThreadManager = container.MultiThreadManager;
          Action action = (Action) (() =>
          {
            try
            {
              int num4 = i != container.MultiThreadManager.ThreadsCount ? i1 * hashAddressesCount : this.hashAddresses.Count;
              int num5 = (i1 - 1) * hashAddressesCount;
              int length = num4 - num5;
              // ISSUE: untyped stack allocation
              Span<uint> span1 = new Span<uint>((void*) __untypedstackalloc(checked (unchecked ((IntPtr) (uint) length) * 4)), length);
              int index1 = 0;
              for (int index2 = num5; index2 < num4; ++index2)
              {
                long hashAddress = this.hashAddresses[index2];
                span1[index1] = this.ParseEntity(hashAddress, (IReadOnlyDictionary<uint, Entity>) container.EntityCache, container.EntitiesVersion, container.Simple);
                ++index1;
              }
              lock (this.locker)
              {
                Span<uint> span2 = span1;
                for (int index3 = 0; index3 < span2.Length; ++index3)
                  this.StoreIds.Add((long) span2[index3]);
              }
            }
            catch (Exception ex)
            {
              DefaultInterpolatedStringHandler interpolatedStringHandler2 = new DefaultInterpolatedStringHandler(0, 1);
              interpolatedStringHandler2.AppendFormatted<Exception>(ex);
              DebugWindow.LogError(interpolatedStringHandler2.ToStringAndClear());
            }
          });
          interpolatedStringHandler1 = new DefaultInterpolatedStringHandler(17, 1);
          interpolatedStringHandler1.AppendLiteral("EntityCollection ");
          interpolatedStringHandler1.AppendFormatted<int>(i);
          string stringAndClear = interpolatedStringHandler1.ToStringAndClear();
          jobs.Add(multiThreadManager.AddJob(action, stringAndClear));
        }
        while (!jobs.AllF<Job>((Predicate<Job>) (x => x.IsCompleted)))
        {
          entityList.sw.Stop();
          yield return (object) entityList.collectEntities;
          entityList.sw.Start();
        }
        num2 = jobs.SumF<Job>((Func<Job, double>) (x => x.ElapsedMs));
        jobs = (List<Job>) null;
      }
      else
      {
        foreach (long hashAddress in entityList.hashAddresses)
          entityList.StoreIds.Add((long) entityList.ParseEntity(hashAddress, (IReadOnlyDictionary<uint, Entity>) container.EntityCache, container.EntitiesVersion, container.Simple));
      }
      if (container.Break)
      {
        container.Break = false;
        ++container.EntitiesVersion;
        container.NeedUpdate = false;
        container.DebugInformation.Tick = entityList.sw.Elapsed.TotalMilliseconds;
      }
      else
      {
        foreach (KeyValuePair<uint, Entity> keyValuePair in container.EntityCache)
        {
          Entity entity = keyValuePair.Value;
          if (entityList.StoreIds.Contains((long) keyValuePair.Key))
          {
            entity.IsValid = true;
          }
          else
          {
            entity.IsValid = false;
            float distancePlayer = entity.DistancePlayer;
            if ((double) distancePlayer < 100.0)
            {
              if ((double) distancePlayer < 75.0)
              {
                if (entity.Type == EntityType.Chest && entity.League == LeagueType.Delve)
                {
                  if ((double) distancePlayer < 30.0)
                  {
                    container.KeyForDelete.Enqueue(keyValuePair.Key);
                    continue;
                  }
                }
                else
                {
                  container.KeyForDelete.Enqueue(keyValuePair.Key);
                  continue;
                }
              }
              if (entity.Type == EntityType.Monster && entity.IsAlive)
              {
                container.KeyForDelete.Enqueue(keyValuePair.Key);
                continue;
              }
            }
            if ((double) distancePlayer > 300.0 && keyValuePair.Value.Metadata.Equals("Metadata/Monsters/Totems/HeiTikiSextant", StringComparison.Ordinal))
              container.KeyForDelete.Enqueue(keyValuePair.Key);
            else if (entity.Type < EntityType.Monster)
              container.KeyForDelete.Enqueue(keyValuePair.Key);
            else if ((double) distancePlayer > 1000000.0 || keyValuePair.Value.GridPosNum == Vector2.Zero)
              container.KeyForDelete.Enqueue(keyValuePair.Key);
          }
        }
        ++container.EntitiesVersion;
        container.NeedUpdate = false;
        container.DebugInformation.Tick = entityList.sw.Elapsed.TotalMilliseconds + num2;
      }
    }

    private uint ParseEntity(
      long addrEntity,
      IReadOnlyDictionary<uint, Entity> entityCache,
      uint entitiesVersion,
      Stack<Entity> result)
    {
      uint entity1 = this.M.Read<uint>(addrEntity + (long) Entity.IdOffset);
      if (entity1 <= 0U)
        return 0;
      Entity entity2;
      if (entityCache.TryGetValue(entity1, out entity2))
      {
        if (entity2.Address != addrEntity)
        {
          entity2.UpdatePointer(addrEntity);
          if (entity2.Check(entity1))
          {
            entity2.Version = entitiesVersion;
            entity2.IsValid = true;
          }
        }
        else
        {
          entity2.Version = entitiesVersion;
          entity2.IsValid = true;
        }
      }
      else
      {
        entity2 = this.GetObject<Entity>(addrEntity);
        if (entity2.Check(entity1))
        {
          entity2.Version = entitiesVersion;
          result.Push(entity2);
          entity2.IsValid = true;
        }
      }
      return entity1;
    }
  }
}
