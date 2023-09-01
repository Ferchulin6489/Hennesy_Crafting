// Decompiled with JetBrains decompiler
// Type: ExileCore.SettingsParser
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Attributes;
using ExileCore.Shared.Helpers;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using GameOffsets.Native;
using ImGuiNET;
using MoreLinq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;


#nullable enable
namespace ExileCore
{
  public static class SettingsParser
  {
    public static void Parse(
    #nullable disable
    ISettings settings, System.Collections.Generic.List<ISettingsHolder> draws, int id = -1)
    {
      int nextKey = -2;
      SettingsParser.Parse((object) settings, draws, id, ref nextKey);
    }

    private static void Parse(
      object settings,
      System.Collections.Generic.List<ISettingsHolder> draws,
      int id,
      ref int nextKey)
    {
      if (settings == null)
      {
        DebugWindow.LogError("Cant parse null settings.");
      }
      else
      {
        foreach (PropertyInfo property in settings.GetType().GetProperties())
        {
          if (property.GetCustomAttribute<IgnoreMenuAttribute>() == null)
          {
            MenuAttribute menuAttribute = property.GetCustomAttribute<MenuAttribute>();
            if (!(property.Name == "Enable") || menuAttribute != null)
            {
              string menuName = Regex.Replace(Regex.Replace(property.Name, "(((?<![A-Z])\\B[A-Z])|(\\B[A-Z](?![A-Z])))", " $1"), "(?<!^)\\b(of|the|a|and|or|to|in|as|at)\\b", (MatchEvaluator) (m => m.Value.ToLowerInvariant()), RegexOptions.IgnoreCase);
              if (menuAttribute == null)
                menuAttribute = new MenuAttribute(menuName);
              int parentIndex = menuAttribute.parentIndex == -1 ? id : menuAttribute.parentIndex;
              SettingsHolder holder = new SettingsHolder()
              {
                Name = menuAttribute.MenuName ?? menuName,
                Tooltip = menuAttribute.Tooltip,
                ID = menuAttribute.index == -1 ? nextKey-- : menuAttribute.index
              };
              object settings1 = property.GetValue(settings);
              ConditionalDisplayAttribute displayAttribute = property.GetCustomAttribute<ConditionalDisplayAttribute>() ?? (settings1 != null ? settings1.GetType().GetCustomAttribute<ConditionalDisplayAttribute>() : (ConditionalDisplayAttribute) null);
              if (displayAttribute != null)
              {
                string conditionMethodName = displayAttribute.ConditionMethodName;
                bool comparisonValue = displayAttribute.ComparisonValue;
                holder.DisplayCondition = SettingsParser.GetConditionMethodOrProperty(settings, conditionMethodName, comparisonValue);
              }
              SubmenuAttribute submenuAttribute = property.GetCustomAttribute<SubmenuAttribute>() ?? (settings1 != null ? settings1.GetType().GetCustomAttribute<SubmenuAttribute>() : (SubmenuAttribute) null);
              holder.CollapsedByDefault = submenuAttribute != null ? submenuAttribute.CollapsedByDefault : menuAttribute.CollapsedByDefault;
              if (settings1 is ISettings || submenuAttribute != null)
              {
                draws.Add((ISettingsHolder) holder);
                SettingsParser.Parse(settings1, draws, holder.ID, ref nextKey);
                bool flag = false;
                if (parentIndex != -1)
                {
                  ISettingsHolder settingsHolder = SettingsParser.GetAllDrawers(draws).Find((Predicate<ISettingsHolder>) (x => x.ID == parentIndex));
                  if (settingsHolder != null)
                  {
                    flag = true;
                    settingsHolder.Children.Add((ISettingsHolder) holder);
                  }
                }
                if (flag)
                  draws.Remove((ISettingsHolder) holder);
              }
              else
              {
                if (parentIndex != -1)
                  SettingsParser.GetAllDrawers(draws).Find((Predicate<ISettingsHolder>) (x => x.ID == parentIndex))?.Children.Add((ISettingsHolder) holder);
                else
                  draws.Add((ISettingsHolder) holder);
                ButtonNode buttonNode = settings1 as ButtonNode;
                if (buttonNode == null)
                {
                  switch (settings1)
                  {
                    case null:
                    case EmptyNode _:
                      continue;
                    default:
                      CustomNode customNode = settings1 as CustomNode;
                      if (customNode == null)
                      {
                        HotkeyNode hotkeyNode = settings1 as HotkeyNode;
                        if (hotkeyNode == null)
                        {
                          ToggleNode toggleNode = settings1 as ToggleNode;
                          if (toggleNode == null)
                          {
                            ColorNode colorNode = settings1 as ColorNode;
                            if (colorNode == null)
                            {
                              TextNode textNode = settings1 as TextNode;
                              if (textNode == null)
                              {
                                ListNode listNode = settings1 as ListNode;
                                if (listNode == null)
                                {
                                  FileNode fileNode = settings1 as FileNode;
                                  if (fileNode == null)
                                  {
                                    RangeNode<int> rangeNode1 = settings1 as RangeNode<int>;
                                    if (rangeNode1 == null)
                                    {
                                      RangeNode<float> rangeNode2 = settings1 as RangeNode<float>;
                                      if (rangeNode2 == null)
                                      {
                                        RangeNode<long> rangeNode3 = settings1 as RangeNode<long>;
                                        if (rangeNode3 == null)
                                        {
                                          RangeNode<Vector2> rangeNode4 = settings1 as RangeNode<Vector2>;
                                          if (rangeNode4 == null)
                                          {
                                            RangeNode<Vector2i> rangeNode5 = settings1 as RangeNode<Vector2i>;
                                            if (rangeNode5 != null)
                                            {
                                              holder.DrawDelegate = (Action) (() =>
                                              {
                                                Vector2i vector2i = rangeNode5.Value;
                                                ImGui.SliderInt2(holder.Unique, ref vector2i.X, rangeNode5.Min.X, rangeNode5.Max.X);
                                                rangeNode5.Value = vector2i;
                                              });
                                              continue;
                                            }
                                            ILogger logger = Core.Logger;
                                            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
                                            interpolatedStringHandler.AppendFormatted<object>(settings1);
                                            interpolatedStringHandler.AppendLiteral(" not supported for menu now. Ask developers to add this type.");
                                            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                                            logger.Warning(stringAndClear);
                                            continue;
                                          }
                                          holder.DrawDelegate = (Action) (() =>
                                          {
                                            Vector2 v = rangeNode4.Value;
                                            ImGui.SliderFloat2(holder.Unique, ref v, rangeNode4.Min.X, rangeNode4.Max.X);
                                            rangeNode4.Value = v;
                                          });
                                          continue;
                                        }
                                        holder.DrawDelegate = (Action) (() =>
                                        {
                                          int v = (int) rangeNode3.Value;
                                          ImGui.SliderInt(holder.Unique, ref v, (int) rangeNode3.Min, (int) rangeNode3.Max);
                                          rangeNode3.Value = (long) v;
                                        });
                                        continue;
                                      }
                                      holder.DrawDelegate = (Action) (() =>
                                      {
                                        float v = rangeNode2.Value;
                                        ImGui.SliderFloat(holder.Unique, ref v, rangeNode2.Min, rangeNode2.Max);
                                        rangeNode2.Value = v;
                                      });
                                      continue;
                                    }
                                    holder.DrawDelegate = (Action) (() =>
                                    {
                                      int v = rangeNode1.Value;
                                      ImGui.SliderInt(holder.Unique, ref v, rangeNode1.Min, rangeNode1.Max);
                                      rangeNode1.Value = v;
                                    });
                                    continue;
                                  }
                                  holder.DrawDelegate = (Action) (() =>
                                  {
                                    if (!ImGui.TreeNode(holder.Unique))
                                      return;
                                    string str = fileNode.Value;
                                    if (ImGui.BeginChildFrame(1U, new Vector2(0.0f, 300f)))
                                    {
                                      DirectoryInfo directoryInfo = new DirectoryInfo("config");
                                      if (directoryInfo.Exists)
                                      {
                                        foreach (FileInfo file in directoryInfo.GetFiles())
                                        {
                                          if (ImGui.Selectable(file.Name, str == file.FullName))
                                            fileNode.Value = file.FullName;
                                        }
                                      }
                                      ImGui.EndChildFrame();
                                    }
                                    ImGui.TreePop();
                                  });
                                  continue;
                                }
                                holder.DrawDelegate = (Action) (() =>
                                {
                                  if (!ImGui.BeginCombo(holder.Unique, listNode.Value))
                                    return;
                                  foreach (string label in listNode.Values)
                                  {
                                    if (ImGui.Selectable(label))
                                    {
                                      listNode.Value = label;
                                      ImGui.EndCombo();
                                      return;
                                    }
                                  }
                                  ImGui.EndCombo();
                                });
                                continue;
                              }
                              holder.DrawDelegate = (Action) (() =>
                              {
                                string input = textNode.Value ?? "";
                                ImGui.InputText(holder.Unique, ref input, 200U);
                                textNode.Value = input;
                              });
                              continue;
                            }
                            holder.DrawDelegate = (Action) (() =>
                            {
                              Vector4 vector4Num = colorNode.Value.ToVector4().ToVector4Num();
                              if (!ImGui.ColorEdit4(holder.Unique, ref vector4Num, ImGuiColorEditFlags.NoInputs | ImGuiColorEditFlags.AlphaBar | ImGuiColorEditFlags.AlphaPreviewHalf))
                                return;
                              colorNode.Value = vector4Num.ToSharpColor();
                            });
                            continue;
                          }
                          holder.DrawDelegate = (Action) (() =>
                          {
                            bool v = toggleNode.Value;
                            ImGui.Checkbox(holder.Unique, ref v);
                            toggleNode.Value = v;
                          });
                          continue;
                        }
                        holder.DrawDelegate = (Action) (() =>
                        {
                          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
                          interpolatedStringHandler.AppendFormatted(holder.Name);
                          interpolatedStringHandler.AppendLiteral(" ");
                          interpolatedStringHandler.AppendFormatted<Keys>(hotkeyNode.Value);
                          hotkeyNode.DrawPickerButton(interpolatedStringHandler.ToStringAndClear());
                        });
                        continue;
                      }
                      holder.DrawDelegate = (Action) (() =>
                      {
                        Action drawDelegate = customNode.DrawDelegate;
                        if (drawDelegate == null)
                          return;
                        drawDelegate();
                      });
                      continue;
                  }
                }
                else
                  holder.DrawDelegate = (Action) (() =>
                  {
                    if (!ImGui.Button(holder.Unique))
                      return;
                    buttonNode.OnPressed();
                  });
              }
            }
          }
        }
      }
    }

    private static Func<bool> GetConditionMethodOrProperty(
      object settings,
      string methodName,
      bool comparisonValue)
    {
      System.Type type = settings.GetType();
      MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, System.Type.EmptyTypes);
      if (method != (MethodInfo) null)
      {
        if (method.ReturnType == typeof (bool))
          return (Func<bool>) (() => comparisonValue == (bool) method.Invoke(settings, new object[0]));
        MethodInfo conversionMethod = SettingsParser.GetBoolConversionMethod(method.ReturnType);
        if (conversionMethod != (MethodInfo) null)
          return (Func<bool>) (() => (comparisonValue ? 1 : 0) == ((bool) conversionMethod.Invoke((object) null, new object[1]
          {
            method.Invoke(settings, new object[0])
          }) ? 1 : 0));
      }
      else
      {
        PropertyInfo property = type.GetProperty(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != (PropertyInfo) null && property.PropertyType == typeof (bool))
          return (Func<bool>) (() => comparisonValue == (bool) property.GetValue(settings));
        MethodInfo conversionMethod = SettingsParser.GetBoolConversionMethod(property.PropertyType);
        if (conversionMethod != (MethodInfo) null)
          return (Func<bool>) (() => (comparisonValue ? 1 : 0) == ((bool) conversionMethod.Invoke((object) null, new object[1]
          {
            property.GetValue(settings)
          }) ? 1 : 0));
      }
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 2);
      interpolatedStringHandler.AppendLiteral("Wanted to use method or property ");
      interpolatedStringHandler.AppendFormatted(methodName);
      interpolatedStringHandler.AppendLiteral(" on type ");
      interpolatedStringHandler.AppendFormatted<System.Type>(type);
      interpolatedStringHandler.AppendLiteral(" as display condition but can't find it or it has a wrong type.");
      DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
      return (Func<bool>) null;
    }

    private static MethodInfo GetBoolConversionMethod(System.Type type) => ((IEnumerable<MethodInfo>) type.GetMethods(BindingFlags.Static | BindingFlags.Public)).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (x =>
    {
      bool flag = x.IsSpecialName;
      if (flag)
      {
        string name = x.Name;
        flag = name == "op_Implicit" || name == "op_Explicit";
      }
      if (flag && x.ReturnType == typeof (bool))
      {
        ParameterInfo[] parameters = x.GetParameters();
        if (parameters != null && parameters.Length == 1)
          return parameters[0].ParameterType == type;
      }
      return false;
    }));

    private static System.Collections.Generic.List<ISettingsHolder> GetAllDrawers(
      System.Collections.Generic.List<ISettingsHolder> SettingPropertyDrawers)
    {
      System.Collections.Generic.List<ISettingsHolder> result = new System.Collections.Generic.List<ISettingsHolder>();
      SettingsParser.GetDrawersRecurs((IList<ISettingsHolder>) SettingPropertyDrawers, (IList<ISettingsHolder>) result);
      return result;
    }

    private static void GetDrawersRecurs(
      IList<ISettingsHolder> drawers,
      IList<ISettingsHolder> result)
    {
      foreach (ISettingsHolder drawer in (IEnumerable<ISettingsHolder>) drawers)
      {
        if (!result.Contains(drawer))
        {
          result.Add(drawer);
        }
        else
        {
          ILogger logger = Core.Logger;
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(105, 2);
          interpolatedStringHandler.AppendLiteral(" Possible stashoverflow or duplicating drawers detected while generating menu. Drawer SettingName: ");
          interpolatedStringHandler.AppendFormatted(drawer.Name);
          interpolatedStringHandler.AppendLiteral(", Id: ");
          interpolatedStringHandler.AppendFormatted<int>(drawer.ID);
          string stringAndClear = interpolatedStringHandler.ToStringAndClear();
          logger.Error<int>(stringAndClear, 5);
        }
      }
      drawers.ForEach<ISettingsHolder>((Action<ISettingsHolder>) (x => SettingsParser.GetDrawersRecurs(x.Children, result)));
    }
  }
}
