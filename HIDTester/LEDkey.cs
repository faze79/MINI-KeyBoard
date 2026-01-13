// Decompiled with JetBrains decompiler
// Type: HIDTester.LEDkey
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HIDTester;

public class LEDkey : UserControl
{
  private IContainer components;
  private Button KEY_LEDMode0;
  private Button KEY_LEDMode1;
  private Button KEY_LEDMode2;

  public LEDkey()
  {
    this.InitializeComponent();
    this.KEY_Colour_Init();
  }

  private void KEY_Colour_Init()
  {
    int red = 152;
    int green = 251;
    int blue = 152;
    this.KEY_LEDMode0.BackColor = Color.FromArgb(red, green, blue);
    this.KEY_LEDMode1.BackColor = Color.FromArgb(red, green, blue);
    this.KEY_LEDMode2.BackColor = Color.FromArgb(red, green, blue);
  }

  private void LEDGeneral_Char_Set()
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] |= (byte) 8;
  }

  private void KEY_LEDMode0_Click(object sender, EventArgs e)
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 176 /*0xB0*/;
    this.LEDGeneral_Char_Set();
    FormMain.KeyParam.Data_Send_Buff[2] = (byte) 0;
    FormMain.KeyParam.KeyChar[(int) FormMain.KeyParam.KEY_Char_Num - 5] = this.KEY_LEDMode0.Text;
    this.KEY_Colour_Init();
    this.KEY_LEDMode0.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY_LEDMode1_Click(object sender, EventArgs e)
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 176 /*0xB0*/;
    this.LEDGeneral_Char_Set();
    FormMain.KeyParam.Data_Send_Buff[2] = (byte) 1;
    FormMain.KeyParam.KeyChar[(int) FormMain.KeyParam.KEY_Char_Num - 5] = this.KEY_LEDMode1.Text;
    this.KEY_Colour_Init();
    this.KEY_LEDMode1.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY_LEDMode2_Click(object sender, EventArgs e)
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 176 /*0xB0*/;
    this.LEDGeneral_Char_Set();
    FormMain.KeyParam.Data_Send_Buff[2] = (byte) 2;
    FormMain.KeyParam.KeyChar[(int) FormMain.KeyParam.KEY_Char_Num - 5] = this.KEY_LEDMode2.Text;
    this.KEY_Colour_Init();
    this.KEY_LEDMode2.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LEDkey));
    this.KEY_LEDMode0 = new Button();
    this.KEY_LEDMode1 = new Button();
    this.KEY_LEDMode2 = new Button();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.KEY_LEDMode0, "KEY_LEDMode0");
    this.KEY_LEDMode0.Name = "KEY_LEDMode0";
    this.KEY_LEDMode0.UseVisualStyleBackColor = true;
    this.KEY_LEDMode0.Click += new EventHandler(this.KEY_LEDMode0_Click);
    componentResourceManager.ApplyResources((object) this.KEY_LEDMode1, "KEY_LEDMode1");
    this.KEY_LEDMode1.Name = "KEY_LEDMode1";
    this.KEY_LEDMode1.UseVisualStyleBackColor = true;
    this.KEY_LEDMode1.Click += new EventHandler(this.KEY_LEDMode1_Click);
    componentResourceManager.ApplyResources((object) this.KEY_LEDMode2, "KEY_LEDMode2");
    this.KEY_LEDMode2.Name = "KEY_LEDMode2";
    this.KEY_LEDMode2.UseVisualStyleBackColor = true;
    this.KEY_LEDMode2.Click += new EventHandler(this.KEY_LEDMode2_Click);
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.KEY_LEDMode2);
    this.Controls.Add((Control) this.KEY_LEDMode1);
    this.Controls.Add((Control) this.KEY_LEDMode0);
    this.Name = nameof (LEDkey);
    this.ResumeLayout(false);
  }
}
