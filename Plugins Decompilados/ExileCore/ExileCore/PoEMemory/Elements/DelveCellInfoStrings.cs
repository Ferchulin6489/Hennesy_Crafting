// Decompiled with JetBrains decompiler
// Type: ExileCore.PoEMemory.Elements.DelveCellInfoStrings
// Assembly: ExileCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 482AC8CF-0A43-4326-80AE-90909BA2E467
// Assembly location: C:\Users\Dev_H\OneDrive\Documentos\Cosas para jueguitos\PoEHelper-3.22.0.1\ExileCore.dll

using ExileCore.Shared.Helpers;
using System;

namespace ExileCore.PoEMemory.Elements
{
  public class DelveCellInfoStrings : RemoteMemoryObject
  {
    private bool _interesting;
    private string _testString;
    private string _testString2;
    private string _testString3;
    private string _testString4;
    private string _testString5;
    private string _testStringGood;

    public string TestString => this._testString = this._testString ?? this.M.ReadStringU(this.M.Read<long>(this.Address));

    public string TestStringGood => this._testStringGood = this._testStringGood ?? this._testString.InsertBeforeUpperCase(Environment.NewLine);

    public string TestString2 => this._testString2 = this._testString2 ?? this.M.ReadStringU(this.M.Read<long>(this.Address + 8L));

    public string TestString3 => this._testString3 = this._testString3 ?? this.M.ReadStringU(this.M.Read<long>(this.Address + 64L));

    public string TestString4 => this._testString4 = this._testString4 ?? this.M.ReadStringU(this.M.Read<long>(this.Address + 88L));

    public string TestString5
    {
      get
      {
        string testString5 = this._testString5;
        if (testString5 != null)
          return testString5;
        this._testString5 = this.M.ReadStringU(this.M.Read<long>(this.Address + 96L));
        return this._testString5;
      }
    }

    public bool Interesting
    {
      get
      {
        if (this._testString5 == null)
        {
          string testString5 = this.TestString5;
          if (testString5.Length > 1 && !testString5.EndsWith("Azurite") && !this.TestString.StartsWith("Azurite3") && !testString5.EndsWith("Weapons") && !testString5.EndsWith("Armour") && !testString5.EndsWith("Jewellery") && !testString5.EndsWith("Items"))
            this._interesting = true;
          else if (this.TestString.StartsWith("Obstruction"))
            this._interesting = true;
        }
        return this._interesting;
      }
    }
  }
}
