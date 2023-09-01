// Decompiled with JetBrains decompiler
// Type: PassiveSkillTreePlanter.SkillNode
// Assembly: PassiveSkillTreePlanter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 379B794A-55C9-4148-B69C-E32612D56DE0
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\Plugins\Compiled\PassiveSkillTreePlanter\PassiveSkillTreePlanter.dll

using PassiveSkillTreePlanter.SkillTreeJson;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PassiveSkillTreePlanter
{
  public class SkillNode
  {
    public bool bJevel;
    public bool bKeyStone;
    public bool bMastery;
    public bool bMult;
    public bool bNotable;
    public Vector2 DrawPosition;
    public float DrawSize = 100f;
    public ushort Id;
    public List<ushort> linkedNodes = new List<ushort>();
    public string Name;
    public int Orbit;
    public long OrbitIndex;
    public SkillNodeGroup SkillNodeGroup;
    private static readonly int[] Angles16 = new int[16]
    {
      0,
      30,
      45,
      60,
      90,
      120,
      135,
      150,
      180,
      210,
      225,
      240,
      270,
      300,
      315,
      330
    };
    private static readonly int[] Angles40 = new int[40]
    {
      0,
      10,
      20,
      30,
      40,
      45,
      50,
      60,
      70,
      80,
      90,
      100,
      110,
      120,
      130,
      135,
      140,
      150,
      160,
      170,
      180,
      190,
      200,
      210,
      220,
      225,
      230,
      240,
      250,
      260,
      270,
      280,
      290,
      300,
      310,
      315,
      320,
      330,
      340,
      350
    };

    public Constants Constants { private get; init; }

    public List<int> OrbitRadii => this.Constants.OrbitRadii;

    public List<int> SkillsPerOrbit => this.Constants.SkillsPerOrbit;

    public Vector2 Position => this.GetPositionAtAngle(this.Arc);

    public Vector2 GetPositionAtAngle(float angle) => this.SkillNodeGroup == null ? new Vector2() : this.SkillNodeGroup.Position + (float) this.OrbitRadius * SkillNode.GetAngleVector(angle);

    public int OrbitRadius => this.OrbitRadii[this.Orbit];

    public float Arc => SkillNode.GetOrbitAngle(this.OrbitIndex, (long) this.SkillsPerOrbit[this.Orbit]);

    public void Init()
    {
      this.DrawPosition = this.Position;
      if (this.bJevel)
        this.DrawSize = 160f;
      if (this.bNotable)
        this.DrawSize = 170f;
      if (!this.bKeyStone)
        return;
      this.DrawSize = 250f;
    }

    private static float GetOrbitAngle(long orbitIndex, long maxNodePositions)
    {
      float orbitAngle;
      switch (maxNodePositions)
      {
        case 16:
          orbitAngle = (float) ((double) SkillNode.Angles16[orbitIndex] * 3.1415927410125732 / 180.0);
          break;
        case 40:
          orbitAngle = (float) ((double) SkillNode.Angles40[orbitIndex] * 3.1415927410125732 / 180.0);
          break;
        default:
          orbitAngle = 6.28318548f * (float) orbitIndex / (float) maxNodePositions;
          break;
      }
      return orbitAngle;
    }

    public static Vector2 GetAngleVector(float angle) => new Vector2(MathF.Sin(angle), -MathF.Cos(angle));
  }
}
