// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.PassiveSkillTreePlanter
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements;
using ExileCore.Shared.AtlasHelper;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using PassiveSkillTreePlanter.SkillTreeJson;
using PassiveSkillTreePlanter.UrlDecoders;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


#nullable enable
namespace PassiveSkillTreePlanter
{
  public class PassiveSkillTreePlanter : BaseSettingsPlugin<
  #nullable disable
  PassiveSkillTreePlanterSettings>
  {
    private const string AtlasTreeDataFile = "AtlasTreeData.json";
    private const string SkillTreeDataFile = "SkillTreeData.json";
    private const string SkillTreeDir = "Builds";
    private readonly PoESkillTreeJsonDecoder _skillTreeData = new PoESkillTreeJsonDecoder();
    private readonly PoESkillTreeJsonDecoder _atlasTreeData = new PoESkillTreeJsonDecoder();
    private HashSet<ushort> _characterUrlNodeIds = new HashSet<ushort>();
    private HashSet<ushort> _atlasUrlNodeIds = new HashSet<ushort>();
    private int _selectedSettingsTab;
    private string _addNewBuildFile = "";
    private string _buildNameEditorValue;
    private AtlasTexture _ringImage;
    private TreeConfig.SkillTreeData _selectedBuildData = new TreeConfig.SkillTreeData();

    private List<string> BuildFiles { get; set; } = new List<string>();

    public string SkillTreeUrlFilesDir => Directory.CreateDirectory(Path.Join(this.ConfigDirectory, "Builds")).FullName;

    public override void OnLoad()
    {
      this._ringImage = this.GetAtlasTexture("AtlasMapCircle");
      this.Graphics.InitImage("Icons.png");
      this.ReloadGameTreeData();
      this.ReloadBuildList();
      if (string.IsNullOrWhiteSpace(this.Settings.SelectedBuild))
        this.Settings.SelectedBuild = "default";
      this.LoadBuild(this.Settings.SelectedBuild);
      this.LoadUrl(this.Settings.LastSelectedAtlasUrl);
      this.LoadUrl(this.Settings.LastSelectedCharacterUrl);
    }

    private void ReloadBuildList() => this.BuildFiles = TreeConfig.GetBuilds(this.SkillTreeUrlFilesDir);

    private void LoadBuild(string buildName)
    {
      this.Settings.SelectedBuild = buildName;
      this._selectedBuildData = TreeConfig.LoadBuild(this.SkillTreeUrlFilesDir, this.Settings.SelectedBuild) ?? new TreeConfig.SkillTreeData();
      this._characterUrlNodeIds = new HashSet<ushort>();
      this._atlasUrlNodeIds = new HashSet<ushort>();
      this._buildNameEditorValue = buildName;
    }

    private void LoadUrl(string url)
    {
      if (string.IsNullOrWhiteSpace(url))
        return;
      (HashSet<ushort> Nodes, ESkillTreeType Type) = TreeEncoder.DecodeUrl(PassiveSkillTreePlanter.PassiveSkillTreePlanter.RemoveAccName(url).Trim());
      if (Nodes == null)
      {
        this.LogMessage("PassiveSkillTree: Can't decode url " + url, 10f);
      }
      else
      {
        if (Type == ESkillTreeType.Character)
        {
          this._characterUrlNodeIds = Nodes;
          this.Settings.LastSelectedCharacterUrl = url;
          this.ValidateNodes(this._characterUrlNodeIds, this._skillTreeData.SkillNodes);
        }
        if (Type != ESkillTreeType.Atlas)
          return;
        this._atlasUrlNodeIds = Nodes;
        this.Settings.LastSelectedAtlasUrl = url;
        this.ValidateNodes(this._atlasUrlNodeIds, this._atlasTreeData.SkillNodes);
      }
    }

    public override bool Initialise() => true;

    private void ReloadGameTreeData()
    {
      string path1 = Path.Join(this.DirectoryFullName, "AtlasTreeData.json");
      if (!File.Exists(path1))
        this.LogMessage("Atlas passive skill tree: Can't find file " + path1 + " with atlas skill tree data.", 10f);
      else
        this._atlasTreeData.Decode(File.ReadAllText(path1));
      string path2 = Path.Join(this.DirectoryFullName, "SkillTreeData.json");
      if (!File.Exists(path2))
        this.LogMessage("Passive skill tree: Can't find file " + path2 + " with skill tree data.", 10f);
      else
        this._skillTreeData.Decode(File.ReadAllText(path2));
    }

    public override void Render()
    {
      this.DrawOverlays();
      this.DrawTreeSwitcher();
    }

    private void DrawTreeSwitcher()
    {
      if (!(bool) this.Settings.EnableEzTreeChanger || !this.GameController.Game.IngameState.IngameUi.TreePanel.IsVisible)
        return;
      bool p_open = true;
      ImGui.SetNextWindowPos(new System.Numerics.Vector2(0.0f, 0.0f), ImGuiCond.FirstUseEver);
      if (!ImGui.Begin("#treeSwitcher", ref p_open, ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize))
        return;
      List<TreeConfig.Tree> list = this._selectedBuildData.Trees.Where<TreeConfig.Tree>((Func<TreeConfig.Tree, bool>) (x => x.Type == ESkillTreeType.Character)).ToList<TreeConfig.Tree>();
      for (int index = 0; index < list.Count; ++index)
      {
        ImGui.BeginDisabled(this.Settings.LastSelectedCharacterUrl == list[index].SkillTreeUrl);
        if (ImGui.Button("Load " + list[index].Tag))
        {
          this._selectedBuildData.SelectedIndex = index;
          this.LoadUrl(list[index].SkillTreeUrl);
        }
        ImGui.EndDisabled();
      }
      ImGui.EndMenu();
    }

    private static string CleanFileName(string fileName) => ((IEnumerable<char>) Path.GetInvalidFileNameChars()).Aggregate<char, string>(fileName, (Func<string, char, string>) ((current, c) => current.Replace(c.ToString(), string.Empty)));

    private void RenameFile(string fileName, string oldFileName)
    {
      fileName = PassiveSkillTreePlanter.PassiveSkillTreePlanter.CleanFileName(fileName);
      File.Move(Path.Combine(this.SkillTreeUrlFilesDir, oldFileName + ".json"), Path.Combine(this.SkillTreeUrlFilesDir, fileName + ".json"));
      this.Settings.SelectedBuild = fileName;
      this.ReloadBuildList();
      this.LoadBuild(this.Settings.SelectedBuild);
    }

    private bool CanRename(string fileName) => !string.IsNullOrWhiteSpace(fileName) && !fileName.Intersect<char>((IEnumerable<char>) Path.GetInvalidFileNameChars()).Any<char>() && !File.Exists(Path.Combine(this.SkillTreeUrlFilesDir, fileName + ".json"));

    private static string RemoveAccName(string url)
    {
      url = url.Split("?accountName")[0];
      url = url.Split("?characterName")[0];
      return url;
    }

    public override void DrawSettings()
    {
      string[] strArray = new string[3]
      {
        "Build Selection",
        "Build Edit",
        "Settings"
      };
      if (ImGui.BeginChild("LeftSettings", new System.Numerics.Vector2(150f, ImGui.GetContentRegionAvail().Y), false, ImGuiWindowFlags.None))
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (ImGui.Selectable(strArray[index], this._selectedSettingsTab == index))
            this._selectedSettingsTab = index;
        }
      }
      ImGui.EndChild();
      ImGui.SameLine();
      ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5f);
      System.Numerics.Vector2 contentRegionAvail = ImGui.GetContentRegionAvail();
      if (ImGui.BeginChild("RightSettings", contentRegionAvail, true, ImGuiWindowFlags.None))
      {
        List<TreeConfig.Tree> trees = this._selectedBuildData.Trees;
        switch (strArray[this._selectedSettingsTab])
        {
          case "Build Selection":
            if (ImGui.Button("Open Build Folder"))
              Process.Start(this.SkillTreeUrlFilesDir);
            ImGui.SameLine();
            if (ImGui.Button("(Re)Load List"))
              this.ReloadBuildList();
            ImGui.SameLine();
            if (ImGui.Button("Open Forum Thread"))
              Process.Start(this._selectedBuildData.BuildLink);
            bool didChange;
            string buildName = ImGuiExtension.ComboBox("Builds", this.Settings.SelectedBuild, this.BuildFiles, out didChange, ImGuiComboFlags.HeightLarge);
            if (didChange)
              this.LoadBuild(buildName);
            ImGui.Separator();
            ImGui.Text("Currently Selected: " + this.Settings.SelectedBuild);
            ImGui.InputText("##CreationLabel", ref this._addNewBuildFile, 1024U, ImGuiInputTextFlags.EnterReturnsTrue);
            ImGui.BeginDisabled(!this.CanRename(this._addNewBuildFile));
            if (ImGui.Button("Add new build " + this._addNewBuildFile))
            {
              TreeConfig.SaveSettingFile<TreeConfig.SkillTreeData>(Path.Join(this.SkillTreeUrlFilesDir, this._addNewBuildFile), new TreeConfig.SkillTreeData());
              this._addNewBuildFile = string.Empty;
              this.ReloadBuildList();
            }
            ImGui.EndDisabled();
            ImGui.Separator();
            ImGui.Columns(3, "LoadColums", true);
            ImGui.SetColumnWidth(0, 51f);
            ImGui.SetColumnWidth(1, 38f);
            ImGui.Text("");
            ImGui.NextColumn();
            ImGui.Text("Type");
            ImGui.NextColumn();
            ImGui.Text("Tree Name");
            ImGui.NextColumn();
            if (trees.Count != 0)
              ImGui.Separator();
            for (int index1 = 0; index1 < trees.Count; ++index1)
            {
              DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
              interpolatedStringHandler.AppendLiteral("LOAD##LOADRULE");
              interpolatedStringHandler.AppendFormatted<int>(index1);
              if (ImGui.Button(interpolatedStringHandler.ToStringAndClear()))
                this.LoadUrl(trees[index1].SkillTreeUrl);
              ImGui.NextColumn();
              ESkillTreeType type = trees[index1].Type;
              MapIconsIndex index2;
              switch (type)
              {
                case ESkillTreeType.Unknown:
                  index2 = MapIconsIndex.QuestObject;
                  break;
                case ESkillTreeType.Character:
                  index2 = MapIconsIndex.MyPlayer;
                  break;
                case ESkillTreeType.Atlas:
                  index2 = MapIconsIndex.TangleAltar;
                  break;
                default:
                  // ISSUE: reference to a compiler-generated method
                  \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
                  break;
              }
              RectangleF uv = SpriteHelper.GetUV(index2);
              ImGui.Image(this.Graphics.GetTextureId("Icons.png"), new System.Numerics.Vector2(ImGui.CalcTextSize("A").Y), uv.TopLeft.ToVector2Num(), uv.BottomRight.ToVector2Num());
              ImGui.NextColumn();
              ImGui.Text(trees[index1].Tag);
              ImGui.NextColumn();
              ImGui.PopItemWidth();
            }
            ImGui.Columns(1, "", false);
            ImGui.Separator();
            ImGui.Text("NOTES:");
            ImGuiNative.igPushTextWrapPos(0.0f);
            ImGui.TextUnformatted(this._selectedBuildData.Notes);
            ImGuiNative.igPopTextWrapPos();
            break;
          case "Build Edit":
            if (trees.Count > 0)
            {
              ImGui.Separator();
              string buildLink = this._selectedBuildData.BuildLink;
              if (ImGui.InputText("Forum Thread", ref buildLink, 1024U, ImGuiInputTextFlags.None))
              {
                this._selectedBuildData.BuildLink = buildLink.Replace("\0", (string) null);
                this._selectedBuildData.Modified = true;
              }
              ImGui.Text("Notes");
              string notes = this._selectedBuildData.Notes;
              if (ImGui.InputTextMultiline("##Notes", ref notes, 150000U, new System.Numerics.Vector2(contentRegionAvail.X - 20f, 200f)))
              {
                this._selectedBuildData.Notes = notes.Replace("\0", (string) null);
                this._selectedBuildData.Modified = true;
              }
              ImGui.Separator();
              ImGui.Columns(5, "EditColums", true);
              ImGui.SetColumnWidth(0, 30f);
              ImGui.SetColumnWidth(1, 50f);
              ImGui.SetColumnWidth(3, 38f);
              ImGui.Text("");
              ImGui.NextColumn();
              ImGui.Text("Move");
              ImGui.NextColumn();
              ImGui.Text("Tree Name");
              ImGui.NextColumn();
              ImGui.Text("Type");
              ImGui.NextColumn();
              ImGui.Text("Skill Tree");
              ImGui.NextColumn();
              if (trees.Count != 0)
                ImGui.Separator();
              for (int index3 = 0; index3 < trees.Count; ++index3)
              {
                DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 1);
                interpolatedStringHandler.AppendFormatted<int>(index3);
                ImGui.PushID(interpolatedStringHandler.ToStringAndClear());
                if (ImGui.Button("X##REMOVERULE"))
                {
                  trees.RemoveAt(index3);
                  this._selectedBuildData.Modified = true;
                }
                else
                {
                  ImGui.NextColumn();
                  ImGui.BeginDisabled(index3 == 0);
                  if (ImGui.Button("^##MOVERULEUPEDIT"))
                  {
                    PassiveSkillTreePlanter.PassiveSkillTreePlanter.MoveElement<TreeConfig.Tree>(trees, index3, true);
                    this._selectedBuildData.Modified = true;
                  }
                  ImGui.EndDisabled();
                  ImGui.SameLine();
                  ImGui.BeginDisabled(index3 == trees.Count - 1);
                  if (ImGui.Button("v##MOVERULEDOWNEDIT"))
                  {
                    PassiveSkillTreePlanter.PassiveSkillTreePlanter.MoveElement<TreeConfig.Tree>(trees, index3, false);
                    this._selectedBuildData.Modified = true;
                  }
                  ImGui.EndDisabled();
                  ImGui.NextColumn();
                  ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
                  ImGui.InputText("##TAG", ref trees[index3].Tag, 1024U, ImGuiInputTextFlags.AutoSelectAll);
                  ImGui.PopItemWidth();
                  ImGui.NextColumn();
                  ESkillTreeType type = trees[index3].Type;
                  MapIconsIndex index4;
                  switch (type)
                  {
                    case ESkillTreeType.Unknown:
                      index4 = MapIconsIndex.QuestObject;
                      break;
                    case ESkillTreeType.Character:
                      index4 = MapIconsIndex.MyPlayer;
                      break;
                    case ESkillTreeType.Atlas:
                      index4 = MapIconsIndex.TangleAltar;
                      break;
                    default:
                      // ISSUE: reference to a compiler-generated method
                      \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) type);
                      break;
                  }
                  RectangleF uv = SpriteHelper.GetUV(index4);
                  ImGui.Image(this.Graphics.GetTextureId("Icons.png"), new System.Numerics.Vector2(ImGui.CalcTextSize("A").Y), uv.TopLeft.ToVector2Num(), uv.BottomRight.ToVector2Num());
                  ImGui.NextColumn();
                  ImGui.PushItemWidth(ImGui.GetContentRegionAvail().X);
                  if (ImGui.InputText("##GN", ref trees[index3].SkillTreeUrl, 1024U, ImGuiInputTextFlags.AutoSelectAll))
                  {
                    trees[index3].ResetType();
                    this._selectedBuildData.Modified = true;
                  }
                  ImGui.PopItemWidth();
                  ImGui.NextColumn();
                  ImGui.PopID();
                }
              }
              ImGui.Separator();
              ImGui.Columns(1, "", false);
            }
            else
              ImGui.Text("No Data Selected");
            if (ImGui.Button("+##AN"))
            {
              trees.Add(new TreeConfig.Tree());
              this._selectedBuildData.Modified = true;
            }
            ImGui.Text("Export current build");
            ImGui.SameLine();
            RectangleF uv1 = SpriteHelper.GetUV(MapIconsIndex.MyPlayer);
            ImGui.PushID("charBtn");
            if (ImGui.ImageButton(this.Graphics.GetTextureId("Icons.png"), new System.Numerics.Vector2(ImGui.CalcTextSize("A").Y), uv1.TopLeft.ToVector2Num(), uv1.BottomRight.ToVector2Num()))
            {
              trees.Add(new TreeConfig.Tree()
              {
                Tag = "Current character tree",
                SkillTreeUrl = PathOfExileUrlDecoder.Encode(this.GameController.Game.IngameState.ServerData.PassiveSkillIds.ToHashSet<ushort>(), ESkillTreeType.Character)
              });
              this._selectedBuildData.Modified = true;
            }
            ImGui.PopID();
            ImGui.SameLine();
            RectangleF uv2 = SpriteHelper.GetUV(MapIconsIndex.TangleAltar);
            ImGui.PushID("atlasBtn");
            if (ImGui.ImageButton(this.Graphics.GetTextureId("Icons.png"), new System.Numerics.Vector2(ImGui.CalcTextSize("A").Y), uv2.TopLeft.ToVector2Num(), uv2.BottomRight.ToVector2Num()))
            {
              trees.Add(new TreeConfig.Tree()
              {
                Tag = "Current atlas tree",
                SkillTreeUrl = PathOfExileUrlDecoder.Encode(this.GameController.Game.IngameState.ServerData.AtlasPassiveSkillIds.ToHashSet<ushort>(), ESkillTreeType.Atlas)
              });
              this._selectedBuildData.Modified = true;
            }
            ImGui.PopID();
            ImGui.Separator();
            ImGui.InputText("##RenameLabel", ref this._buildNameEditorValue, 200U, ImGuiInputTextFlags.None);
            ImGui.SameLine();
            ImGui.BeginDisabled(!this.CanRename(this._buildNameEditorValue));
            if (ImGui.Button("Rename Build"))
              this.RenameFile(this._buildNameEditorValue, this.Settings.SelectedBuild);
            ImGui.EndDisabled();
            if (ImGui.Button("Save Build to File: " + this.Settings.SelectedBuild) || this._selectedBuildData.Modified && (bool) this.Settings.SaveChangesAutomatically)
            {
              this._selectedBuildData.Modified = false;
              TreeConfig.SaveSettingFile<TreeConfig.SkillTreeData>(Path.Join(this.SkillTreeUrlFilesDir, this.Settings.SelectedBuild), this._selectedBuildData);
              this.ReloadBuildList();
            }
            if (this._selectedBuildData.Modified)
            {
              ImGui.TextColored(Color.Red.ToImguiVec4(), "Unsaved changes detected");
              break;
            }
            break;
          case "Settings":
            base.DrawSettings();
            break;
        }
      }
      ImGui.PopStyleVar();
      ImGui.EndChild();
    }

    private static void MoveElement<T>(List<T> list, int changeIndex, bool moveUp)
    {
      if (moveUp)
      {
        if (changeIndex <= 0)
          return;
        List<T> objList1 = list;
        int index1 = changeIndex;
        List<T> objList2 = list;
        int num = changeIndex - 1;
        T obj1 = list[changeIndex - 1];
        T obj2 = list[changeIndex];
        objList1[index1] = obj1;
        int index2 = num;
        T obj3 = obj2;
        objList2[index2] = obj3;
      }
      else
      {
        if (changeIndex >= list.Count - 1)
          return;
        List<T> objList3 = list;
        int index3 = changeIndex;
        List<T> objList4 = list;
        int num = changeIndex + 1;
        T obj4 = list[changeIndex + 1];
        T obj5 = list[changeIndex];
        objList3[index3] = obj4;
        int index4 = num;
        T obj6 = obj5;
        objList4[index4] = obj6;
      }
    }

    private void ValidateNodes(HashSet<ushort> currentNodes, Dictionary<ushort, SkillNode> nodeDict)
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler;
      foreach (ushort currentNode in currentNodes)
      {
        SkillNode skillNode;
        if (!nodeDict.TryGetValue(currentNode, out skillNode))
        {
          interpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
          interpolatedStringHandler.AppendLiteral("PassiveSkillTree: Can't find passive skill tree node with id: ");
          interpolatedStringHandler.AppendFormatted<ushort>(currentNode);
          this.LogError(interpolatedStringHandler.ToStringAndClear(), 5f);
        }
        else
        {
          foreach (ushort key in skillNode.linkedNodes.Where<ushort>(new Func<ushort, bool>(currentNodes.Contains)))
          {
            if (!nodeDict.ContainsKey(key))
            {
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(79, 1);
              interpolatedStringHandler.AppendLiteral("PassiveSkillTree: Can't find passive skill tree node with id: ");
              interpolatedStringHandler.AppendFormatted<ushort>(key);
              interpolatedStringHandler.AppendLiteral(" to draw the link");
              this.LogError(interpolatedStringHandler.ToStringAndClear(), 5f);
            }
          }
        }
      }
    }

    private void DrawOverlays()
    {
      this.DrawTreeOverlay(this.GameController.Game.IngameState.IngameUi.TreePanel.AsObject<TreePanel>(), this._skillTreeData, (IReadOnlySet<ushort>) this._characterUrlNodeIds, (Func<IReadOnlySet<ushort>>) (() => (IReadOnlySet<ushort>) this.GameController.Game.IngameState.ServerData.PassiveSkillIds.ToHashSet<ushort>()));
      this.DrawTreeOverlay(this.GameController.Game.IngameState.IngameUi.AtlasTreePanel.AsObject<TreePanel>(), this._atlasTreeData, (IReadOnlySet<ushort>) this._atlasUrlNodeIds, (Func<IReadOnlySet<ushort>>) (() => (IReadOnlySet<ushort>) this.GameController.Game.IngameState.ServerData.AtlasPassiveSkillIds.ToHashSet<ushort>()));
    }

    private void DrawTreeOverlay(
      TreePanel treePanel,
      PoESkillTreeJsonDecoder treeData,
      IReadOnlySet<ushort> targetNodeIds,
      Func<IReadOnlySet<ushort>> allocatedNodeIds)
    {
      if (targetNodeIds == null || targetNodeIds.Count <= 0 || !treePanel.IsVisible)
        return;
      Element canvasElement = treePanel.CanvasElement;
      System.Numerics.Vector2 baseOffset = new System.Numerics.Vector2((float) canvasElement.Center.X, (float) canvasElement.Center.Y);
      this.DrawTreeOverlay(allocatedNodeIds(), targetNodeIds, treeData, canvasElement.Scale, baseOffset);
    }

    private void DrawTreeOverlay(
      IReadOnlySet<ushort> allocatedNodeIds,
      IReadOnlySet<ushort> targetNodeIds,
      PoESkillTreeJsonDecoder treeData,
      float scale,
      System.Numerics.Vector2 baseOffset)
    {
      HashSet<ushort> wrongNodes = allocatedNodeIds.Except<ushort>((IEnumerable<ushort>) targetNodeIds).ToHashSet<ushort>();
      HashSet<ushort> missingNodes = targetNodeIds.Except<ushort>((IEnumerable<ushort>) allocatedNodeIds).ToHashSet<ushort>();
      List<SkillNode> list1 = targetNodeIds.Union<ushort>((IEnumerable<ushort>) allocatedNodeIds).Select<ushort, SkillNode>((Func<ushort, SkillNode>) (x => treeData.SkillNodes.GetValueOrDefault<ushort, SkillNode>(x))).Where<SkillNode>((Func<SkillNode, bool>) (x => x != null)).ToList<SkillNode>();
      List<((ushort, ushort), PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType)> list2 = list1.SelectMany<SkillNode, (ushort, ushort)>((Func<SkillNode, IEnumerable<(ushort, ushort)>>) (node => node.linkedNodes.Where<ushort>(new Func<ushort, bool>(treeData.SkillNodes.ContainsKey)).Where<ushort>((Func<ushort, bool>) (id => targetNodeIds.Contains(id) || allocatedNodeIds.Contains(id))).Select<ushort, (ushort, ushort)>((Func<ushort, (ushort, ushort)>) (linkedNode => (Math.Min(node.Id, linkedNode), Math.Max(node.Id, linkedNode)))))).Distinct<(ushort, ushort)>().Select<(ushort, ushort), ((ushort, ushort), PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType)>((Func<(ushort, ushort), ((ushort, ushort), PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType)>) (pair =>
      {
        (ushort, ushort) valueTuple = pair;
        (ushort num3, ushort num4) = pair;
        PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType connectionType;
        if (wrongNodes.Contains(num3) || wrongNodes.Contains(num4))
        {
          connectionType = PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Deallocate;
        }
        else
        {
          ushort num5 = num3;
          ushort num6 = num4;
          connectionType = missingNodes.Contains(num5) || missingNodes.Contains(num6) ? PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Allocate : PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Allocated;
        }
        int num7 = (int) connectionType;
        return (valueTuple, (PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType) num7);
      })).ToList<((ushort, ushort), PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType)>();
      foreach (SkillNode skillNode in list1)
      {
        float num8 = skillNode.DrawSize * scale;
        float num9 = baseOffset.X + skillNode.DrawPosition.X * scale;
        float num10 = baseOffset.Y + skillNode.DrawPosition.Y * scale;
        int num11 = allocatedNodeIds.Contains(skillNode.Id) ? 1 : 0;
        bool flag = targetNodeIds.Contains(skillNode.Id);
        Color color = num11 == 0 ? (flag ? this.Settings.UnpickedBorderColor.Value : Color.Orange) : (flag ? this.Settings.PickedBorderColor.Value : this.Settings.WrongPickedBorderColor.Value);
        this.Graphics.DrawImage(this._ringImage, new RectangleF(num9 - num8 / 2f, num10 - num8 / 2f, num8, num8), color);
      }
      if ((int) this.Settings.LineWidth > 0)
      {
        foreach (((ushort, ushort), PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType) tuple in list2)
        {
          SkillNode skillNode1 = treeData.SkillNodes[tuple.Item1.Item1];
          SkillNode skillNode2 = treeData.SkillNodes[tuple.Item1.Item2];
          System.Numerics.Vector2 vector2_1 = baseOffset + skillNode1.DrawPosition * scale;
          System.Numerics.Vector2 vector2_2 = baseOffset + skillNode2.DrawPosition * scale;
          System.Numerics.Vector2 vector2_3 = System.Numerics.Vector2.Normalize(vector2_2 - vector2_1);
          System.Numerics.Vector2 vector2_4 = vector2_1 + vector2_3 * skillNode1.DrawSize * scale / 2f;
          System.Numerics.Vector2 vector2_5 = vector2_2 - vector2_3 * skillNode2.DrawSize * scale / 2f;
          Graphics graphics = this.Graphics;
          System.Numerics.Vector2 p1 = vector2_4;
          System.Numerics.Vector2 p2 = vector2_5;
          float lineWidth = (float) (int) this.Settings.LineWidth;
          Color color;
          switch (tuple.Item2)
          {
            case PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Deallocate:
              color = (Color) this.Settings.WrongPickedBorderColor;
              break;
            case PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Allocate:
              color = (Color) this.Settings.UnpickedBorderColor;
              break;
            case PassiveSkillTreePlanter.PassiveSkillTreePlanter.ConnectionType.Allocated:
              color = (Color) this.Settings.PickedBorderColor;
              break;
            default:
              color = Color.Orange;
              break;
          }
          graphics.DrawLine(p1, p2, lineWidth, color);
        }
      }
      System.Numerics.Vector2 vector2 = new System.Numerics.Vector2(50f, 300f);
      Graphics graphics1 = this.Graphics;
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
      interpolatedStringHandler.AppendLiteral("Total Tree Nodes: ");
      interpolatedStringHandler.AppendFormatted<int>(targetNodeIds.Count);
      string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
      System.Numerics.Vector2 position1 = vector2;
      Color white = Color.White;
      graphics1.DrawText(stringAndClear1, position1, white, 15);
      vector2.Y += 20f;
      Graphics graphics2 = this.Graphics;
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(14, 1);
      interpolatedStringHandler.AppendLiteral("Picked Nodes: ");
      interpolatedStringHandler.AppendFormatted<int>(allocatedNodeIds.Count);
      string stringAndClear2 = interpolatedStringHandler.ToStringAndClear();
      System.Numerics.Vector2 position2 = vector2;
      Color green = Color.Green;
      graphics2.DrawText(stringAndClear2, position2, green, 15);
      vector2.Y += 20f;
      Graphics graphics3 = this.Graphics;
      interpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
      interpolatedStringHandler.AppendLiteral("Wrong Picked Nodes: ");
      interpolatedStringHandler.AppendFormatted<int>(wrongNodes.Count);
      string stringAndClear3 = interpolatedStringHandler.ToStringAndClear();
      System.Numerics.Vector2 position3 = vector2;
      Color red = Color.Red;
      graphics3.DrawText(stringAndClear3, position3, red, 15);
    }

    private enum ConnectionType
    {
      Deallocate,
      Allocate,
      Allocated,
    }
  }
}
