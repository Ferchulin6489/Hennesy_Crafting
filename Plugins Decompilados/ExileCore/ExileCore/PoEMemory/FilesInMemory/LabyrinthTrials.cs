// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.FilesInMemory.LabyrinthTrials
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExileCore.PoEMemory.FilesInMemory
{
  public class LabyrinthTrials : UniversalFileWrapper<LabyrinthTrial>
  {
    public static string[] LabyrinthTrialAreaIds = new string[18]
    {
      "1_1_7_1",
      "1_2_5_1",
      "1_2_6_2",
      "1_3_3_1",
      "1_3_6",
      "1_3_15",
      "2_6_7_1",
      "2_7_4",
      "2_7_5_2",
      "2_8_5",
      "2_9_7",
      "2_10_9",
      "EndGame_Labyrinth_trials_spikes",
      "EndGame_Labyrinth_trials_spinners",
      "EndGame_Labyrinth_trials_sawblades_#",
      "EndGame_Labyrinth_trials_lava_#",
      "EndGame_Labyrinth_trials_roombas",
      "EndGame_Labyrinth_trials_arrows"
    };

    public LabyrinthTrials(IMemory m, Func<long> address)
      : base(m, address)
    {
    }

    public IList<LabyrinthTrial> EntriesList => (IList<LabyrinthTrial>) base.EntriesList.ToList<LabyrinthTrial>();

    public LabyrinthTrial GetLabyrinthTrialByAreaId(string id) => this.EntriesList.FirstOrDefault<LabyrinthTrial>((Func<LabyrinthTrial, bool>) (x => x.Area.Id == id));

    public LabyrinthTrial GetLabyrinthTrialById(int index) => this.EntriesList.FirstOrDefault<LabyrinthTrial>((Func<LabyrinthTrial, bool>) (x => x.Id == index));

    public LabyrinthTrial GetLabyrinthTrialByArea(WorldArea area) => this.EntriesList.FirstOrDefault<LabyrinthTrial>((Func<LabyrinthTrial, bool>) (x => object.Equals((object) x.Area, (object) area)));

    public new LabyrinthTrial GetByAddress(long address) => base.GetByAddress(address);
  }
}
