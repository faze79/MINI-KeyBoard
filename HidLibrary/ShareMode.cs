// Decompiled with JetBrains decompiler
// Type: HidLibrary.ShareMode
// Assembly: HidLibrary, Version=3.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 33B673C5-FCB9-4E91-8D12-5CABA4235493
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\HidLibrary.dll

using System;

#nullable disable
namespace HidLibrary;

[Flags]
public enum ShareMode
{
  Exclusive = 0,
  ShareRead = 1,
  ShareWrite = 2,
}
