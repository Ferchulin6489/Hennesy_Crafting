// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.TreeEncoder
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using ExileCore;
using PassiveSkillTreePlanter.UrlDecoders;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PassiveSkillTreePlanter
{
  public class TreeEncoder
  {
    public static (HashSet<ushort> Nodes, ESkillTreeType Type) DecodeUrl(string url)
    {
      try
      {
        if (PoePlannerUrlDecoder.UrlMatch(url))
          return (PoePlannerUrlDecoder.Decode(url), ESkillTreeType.Character);
        ESkillTreeType type;
        HashSet<ushort> passiveIds;
        if (PathOfExileUrlDecoder.TryMatch(url, out type, out passiveIds))
          return (passiveIds, type);
      }
      catch (Exception ex)
      {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 2);
        interpolatedStringHandler.AppendLiteral("Failed to decode url ");
        interpolatedStringHandler.AppendFormatted(url);
        interpolatedStringHandler.AppendLiteral(":\n");
        interpolatedStringHandler.AppendFormatted<Exception>(ex);
        DebugWindow.LogError(interpolatedStringHandler.ToStringAndClear());
        return ();
      }
      return ();
    }
  }
}
