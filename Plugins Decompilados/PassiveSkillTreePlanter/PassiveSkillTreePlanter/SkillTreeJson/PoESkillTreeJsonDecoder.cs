// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillTreeJson.PoESkillTreeJsonDecoder
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ExileCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Numerics;


#nullable enable
namespace PassiveSkillTreePlanter.SkillTreeJson
{
  public class PoESkillTreeJsonDecoder
  {
    private 
    #nullable disable
    List<SkillNodeGroup> NodeGroups = new List<SkillNodeGroup>();
    private List<SkillNode> Nodes = new List<SkillNode>();
    public Dictionary<ushort, SkillNode> SkillNodes = new Dictionary<ushort, SkillNode>();
    private PoESkillTree SkillTree;

    public void Decode(string jsonTree)
    {
      this.Nodes = new List<SkillNode>();
      JsonSerializerSettings settings = new JsonSerializerSettings()
      {
        Error = (EventHandler<ErrorEventArgs>) ((sender, args) =>
        {
          if (args.ErrorContext.Path == null || !args.ErrorContext.Path.EndsWith(".oo"))
            Logger.Log.Error("Exception while deserializing Json tree" + args.ErrorContext.Error?.ToString());
          args.ErrorContext.Handled = true;
        })
      };
      this.SkillTree = JsonConvert.DeserializeObject<PoESkillTree>(jsonTree, settings);
      this.SkillNodes = new Dictionary<ushort, SkillNode>();
      this.NodeGroups = new List<SkillNodeGroup>();
      foreach (KeyValuePair<string, Node> node in this.SkillTree.Nodes)
      {
        SkillNode skillNode = new SkillNode()
        {
          Id = (ushort) node.Value.Skill,
          Name = node.Value.Name,
          Orbit = node.Value.Orbit,
          OrbitIndex = node.Value.OrbitIndex,
          bJevel = node.Value.IsJewelSocket,
          bMastery = node.Value.IsMastery,
          bMult = node.Value.IsMultipleChoice,
          linkedNodes = node.Value.Out,
          bKeyStone = node.Value.IsKeystone,
          Constants = this.SkillTree.Constants
        };
        this.Nodes.Add(skillNode);
        this.SkillNodes.Add((ushort) node.Value.Skill, skillNode);
      }
      this.NodeGroups = new List<SkillNodeGroup>();
      foreach (KeyValuePair<string, Group> group in this.SkillTree.Groups)
      {
        SkillNodeGroup skillNodeGroup = new SkillNodeGroup()
        {
          Position = new Vector2((float) group.Value.X, (float) group.Value.Y)
        };
        foreach (ushort node in group.Value.Nodes)
        {
          SkillNode skillNode = this.SkillNodes[node];
          skillNodeGroup.Nodes.Add(skillNode);
          skillNode.SkillNodeGroup = skillNodeGroup;
        }
        this.NodeGroups.Add(skillNodeGroup);
      }
      foreach (SkillNode node in this.Nodes)
        node.Init();
    }
  }
}
