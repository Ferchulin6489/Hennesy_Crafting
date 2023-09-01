// Decompiled with JetBrains decompiler
// Type: FullRareSetManager.StashData
// Assembly: FullRareSetManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1E8E0CC2-44D8-492F-ABF4-DF4E019E4878
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\FullRareSetManager\FullRareSetManager.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FullRareSetManager
{
  public class StashData
  {
    private const string STASH_DATA_FILE = "StashData.json";
    public StashTabData PlayerInventory = new StashTabData();
    public Dictionary<string, StashTabData> StashTabs = new Dictionary<string, StashTabData>();

    public static StashData Load(FullRareSetManagerCore plugin)
    {
      string path = plugin.DirectoryFullName + "\\StashData.json";
      StashData data;
      if (File.Exists(path))
      {
        string str = File.ReadAllText(path);
        try
        {
          data = JsonConvert.DeserializeObject<StashData>(str);
        }
        catch (Exception ex)
        {
          return (StashData) null;
        }
      }
      else
      {
        data = new StashData();
        StashData.Save(plugin, data);
      }
      return data;
    }

    public static void Save(FullRareSetManagerCore plugin, StashData data)
    {
      try
      {
        if (data == null)
          return;
        string path = plugin.DirectoryFullName + "\\StashData.json";
        string directoryName = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        using (StreamWriter streamWriter = new StreamWriter((Stream) File.Create(path)))
        {
          string str = JsonConvert.SerializeObject((object) data, (Formatting) 1);
          streamWriter.Write(str);
        }
      }
      catch
      {
      }
    }
  }
}
