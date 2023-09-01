// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.TreeConfig
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PassiveSkillTreePlanter
{
  public class TreeConfig
  {
    public static List<string> GetBuilds(string buildDirectory) => ((IEnumerable<FileInfo>) new DirectoryInfo(buildDirectory).GetFiles("*.json")).Select<FileInfo, string>((Func<FileInfo, string>) (x => Path.GetFileNameWithoutExtension(x.Name))).ToList<string>();

    public static TreeConfig.SkillTreeData LoadBuild(string buildDirectory, string buildName) => TreeConfig.LoadSettingFile<TreeConfig.SkillTreeData>(Path.Join(buildDirectory, buildName + ".json"));

    public static TSettingType LoadSettingFile<TSettingType>(string fileName) => !File.Exists(fileName) ? default (TSettingType) : JsonConvert.DeserializeObject<TSettingType>(File.ReadAllText(fileName));

    public static void SaveSettingFile<TSettingType>(string fileName, TSettingType setting) => File.WriteAllText(fileName + ".json", JsonConvert.SerializeObject((object) setting, Formatting.Indented));

    public class Tree
    {
      public string Tag = "";
      public string SkillTreeUrl = "";
      private ESkillTreeType? _type;

      [JsonIgnore]
      public ESkillTreeType Type
      {
        get
        {
          ESkillTreeType valueOrDefault = this._type.GetValueOrDefault();
          ESkillTreeType type;
          if (this._type.HasValue)
          {
            type = valueOrDefault;
          }
          else
          {
            (HashSet<ushort> Nodes, ESkillTreeType Type) tuple = TreeEncoder.DecodeUrl(this.SkillTreeUrl);
            ESkillTreeType eskillTreeType = tuple.Nodes == null ? ESkillTreeType.Unknown : tuple.Type;
            this._type = new ESkillTreeType?(eskillTreeType);
            type = eskillTreeType;
          }
          return type;
        }
      }

      public void ResetType() => this._type = new ESkillTreeType?();
    }

    public class SkillTreeData
    {
      public string Notes { get; set; } = "";

      public List<TreeConfig.Tree> Trees { get; set; } = new List<TreeConfig.Tree>();

      public string BuildLink { get; set; } = "";

      public int SelectedIndex { get; set; }

      internal bool Modified { get; set; }
    }
  }
}
