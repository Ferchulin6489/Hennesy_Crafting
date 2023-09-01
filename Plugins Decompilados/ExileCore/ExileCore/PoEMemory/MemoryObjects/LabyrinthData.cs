// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.MemoryObjects.LabyrinthData
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using SharpDX;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExileCore.PoEMemory.MemoryObjects
{
  public class LabyrinthData : RemoteMemoryObject
  {
    internal static Dictionary<long, LabyrinthRoom> CachedRoomsDictionary;

    public IList<LabyrinthRoom> Rooms
    {
      get
      {
        long num1 = this.M.Read<long>(this.Address);
        long num2 = this.M.Read<long>(this.Address + 8L);
        List<LabyrinthRoom> rooms = new List<LabyrinthRoom>();
        LabyrinthData.CachedRoomsDictionary = new Dictionary<long, LabyrinthRoom>();
        int num3 = 0;
        for (long index = num1; index < num2; index += 96L)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(12, 2);
          interpolatedStringHandler.AppendLiteral("Room ");
          interpolatedStringHandler.AppendFormatted<int>(num3);
          interpolatedStringHandler.AppendLiteral(" Addr: ");
          interpolatedStringHandler.AppendFormatted(index.ToString("x"));
          DebugWindow.LogMsg(interpolatedStringHandler.ToStringAndClear(), 0.0f, Color.White);
          if (index != 0L)
          {
            LabyrinthRoom labyrinthRoom = new LabyrinthRoom(this.M, index, this.TheGame.Files.WorldAreas)
            {
              Id = num3++
            };
            rooms.Add(labyrinthRoom);
            LabyrinthData.CachedRoomsDictionary.Add(index, labyrinthRoom);
          }
        }
        return (IList<LabyrinthRoom>) rooms;
      }
    }

    internal static LabyrinthRoom GetRoomById(long roomId)
    {
      LabyrinthRoom roomById;
      LabyrinthData.CachedRoomsDictionary.TryGetValue(roomId, out roomById);
      return roomById;
    }
  }
}
