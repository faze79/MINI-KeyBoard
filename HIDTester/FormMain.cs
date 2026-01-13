// Decompiled with JetBrains decompiler
// Type: HIDTester.FormMain
// Assembly: MINI KeyBoard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 830E3432-592A-4FE8-A60E-4E46348E689C
// Assembly location: C:\Users\davide.fasolo\OneDrive - Salvagnini Italia SpA\Documents\MINI-KEYBOARD\MINI KeyBoard.exe

using HID;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace HIDTester;

public class FormMain : Form
{
  private ushort WriteMode = 1;
  private Hid myHid = new Hid();
  private IntPtr myHidPtr;
  private AutoSizeFormClass asc = new AutoSizeFormClass();
  private HidLib myHidLib = new HidLib();
  private byte[] RecDataBuffer = new byte[90];
  private int Display_Dowlaod_Char_TM;
  private readonly Color menuBackColor = Color.FromArgb(200, 200, 169);
  private readonly Color menuMouserOverColor = Color.FromArgb(230, 206, 172);
  private readonly string[] menuStr = new string[6]
  {
    "KEY",
    "Ctrl Shift Alt",
    "Multimedia",
    "LED",
    "Mouse",
    ""
  };
  private Dictionary<string, Form> menuDic = new Dictionary<string, Form>();
  private IContainer components;
  private Label stateLabel;
  private ToolStripMenuItem fdToolStripMenuItem;
  private Button Download;
  private Button KEY1;
  private Button KEY2;
  private Button KEY3;
  private Button KEY4;
  private Button K1_Left;
  private Button K1_Centre;
  private Button K1_Right;
  private TextBox SetText;
  private Button Key_Clear;
  private Button KEY8;
  private Button KEY7;
  private Button KEY6;
  private Button KEY5;
  private Button KEY12;
  private Button KEY11;
  private Button KEY10;
  private Button KEY9;
  private Button KEY16;
  private Button KEY15;
  private Button KEY14;
  private Button KEY13;
  private Button K2_Right;
  private Button K2_Centre;
  private Button K2_Left;
  private SplitContainer splitContainer1;
  private FlowLayoutPanel flowLayoutPanel1;
  private TextBox SetFunText;
  private Button K3_Left;
  private Button K3_Centre;
  private Button K3_Right;
  private FlowLayoutPanel flowLayoutPanel_LayerFun;
  private PictureBox pictureBox1;
  private Label label_Dowload_Dsp;

  public FormMain()
  {
    this.InitializeComponent();
    this.myHid.DataReceived += new EventHandler<report>(this.myhid_DataReceived);
    this.myHid.DeviceRemoved += new EventHandler(this.myhid_DeviceRemoved);
    this.MenuList();
    this.KEY_Colour_Init();
    this.Time_Display_Text();
    this.Hide_Dowload_Text();
    this.LayerFunList();
    this.Lanuage_Set_EN();
  }

  private void MenuList()
  {
    for (int index = 0; index < this.menuStr.Length; ++index)
    {
      Button button = new Button();
      button.Text = this.menuStr[index];
      button.FlatStyle = FlatStyle.Flat;
      button.FlatAppearance.MouseOverBackColor = this.menuMouserOverColor;
      button.FlatAppearance.BorderSize = 0;
      button.Width = this.flowLayoutPanel1.Width;
      button.Height = 40;
      button.Margin = new Padding() { All = 0 };
      button.MouseClick += new MouseEventHandler(this.Btn_OnClick);
      this.flowLayoutPanel1.Controls.Add((Control) button);
      this.flowLayoutPanel1.BackColor = this.menuBackColor;
    }
    BasicKeys basicKeys = new BasicKeys();
    basicKeys.Parent = (Control) this.splitContainer1.Panel2;
    basicKeys.Dock = DockStyle.Fill;
    basicKeys.Show();
    FormMain.KeyParam.KEY_Cur_Page = (byte) 1;
  }

  private void LayerFunList()
  {
    LayerFun layerFun = new LayerFun();
    this.flowLayoutPanel_LayerFun.Controls.Add((Control) layerFun);
    layerFun.Show();
  }

  private void Btn_OnClick(object sender, MouseEventArgs e)
  {
    switch ((sender as Button).Text)
    {
      case "KEY":
        this.splitContainer1.Panel2.Controls.Clear();
        BasicKeys basicKeys = new BasicKeys();
        basicKeys.Parent = (Control) this.splitContainer1.Panel2;
        basicKeys.Dock = DockStyle.Fill;
        basicKeys.Show();
        FormMain.KeyParam.KEY_Cur_Page = (byte) 1;
        break;
      case "Ctrl Shift Alt":
        this.splitContainer1.Panel2.Controls.Clear();
        FunKey funKey = new FunKey();
        funKey.Parent = (Control) this.splitContainer1.Panel2;
        funKey.Dock = DockStyle.Fill;
        funKey.Show();
        FormMain.KeyParam.KEY_Cur_Page = (byte) 2;
        break;
      case "Multimedia":
        this.splitContainer1.Panel2.Controls.Clear();
        MULKey mulKey = new MULKey();
        mulKey.Parent = (Control) this.splitContainer1.Panel2;
        mulKey.Dock = DockStyle.Fill;
        mulKey.Show();
        FormMain.KeyParam.KEY_Cur_Page = (byte) 3;
        break;
      case "LED":
        this.splitContainer1.Panel2.Controls.Clear();
        LEDkey leDkey = new LEDkey();
        leDkey.Parent = (Control) this.splitContainer1.Panel2;
        leDkey.Dock = DockStyle.Fill;
        leDkey.Show();
        this.Key_Clear_Fun();
        FormMain.KeyParam.KEY_Cur_Page = (byte) 4;
        break;
      case "Mouse":
        this.splitContainer1.Panel2.Controls.Clear();
        MouseKey mouseKey = new MouseKey();
        mouseKey.Parent = (Control) this.splitContainer1.Panel2;
        mouseKey.Dock = DockStyle.Fill;
        mouseKey.Show();
        this.Key_Clear_Fun();
        FormMain.KeyParam.KEY_Cur_Page = (byte) 5;
        break;
    }
  }

  private void Show_Dowload_Text()
  {
    this.Display_Dowlaod_Char_TM = 20;
    this.label_Dowload_Dsp.Show();
  }

  private void Hide_Dowload_Text() => this.label_Dowload_Dsp.Hide();

  private void AutoCheckUsb()
  {
    if (this.WriteMode == (ushort) 0)
    {
      if (!this.myHid.Opened)
      {
        if ((int) (this.myHidPtr = this.myHid.OpenDevice((ushort) 4489, (ushort) 34960)) != -1)
        {
          this.KeyBoardVersion_Check();
          this.stateLabel.Text = "Connected";
          this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Green;
        }
        else
        {
          this.stateLabel.Text = "Not Connected";
          this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Red;
        }
      }
      else
      {
        this.stateLabel.Text = "Connected";
        this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Green;
      }
    }
    else
    {
      if (this.WriteMode != (ushort) 1)
        return;
      if (!this.myHidLib.Get_Dev_Sta())
      {
        if (this.myHidLib.Connect_Device())
        {
          this.KeyBoardVersion_Check();
          this.stateLabel.Text = "Connected";
          this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Green;
        }
        else
        {
          this.stateLabel.Text = "Not Connected";
          this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Red;
        }
      }
      else if (this.myHidLib.Check_Disconnect())
      {
        this.stateLabel.Text = "Connected";
        this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Green;
      }
      else
      {
        this.stateLabel.Text = "Not Connected";
        this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Red;
      }
    }
  }

  protected void myhid_DataReceived(object sender, report e)
  {
    this.RecDataBuffer = e.reportBuff;
    new ASCIIEncoding().GetString(this.RecDataBuffer);
  }

  protected void myhid_DeviceRemoved(object sender, EventArgs e)
  {
    this.stateLabel.Text = "????";
    this.stateLabel.BackColor = this.stateLabel.BackColor = Color.Red;
  }

  private void aboutMenu_Click(object sender, EventArgs e)
  {
    Process.Start("http://www.cnblogs.com/hebaichuanyeah/p/4504855.html");
  }

  private void Time_Display_Text()
  {
    System.Timers.Timer timer = new System.Timers.Timer();
    timer.Enabled = true;
    timer.Interval = 30.0;
    timer.Start();
    timer.Elapsed += new ElapsedEventHandler(this.Timer1_Elapsed);
  }

  private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
  {
    this.AutoCheckUsb();
    if (FormMain.KeyParam.PageBet_Inte_Cmd != (byte) 0 && FormMain.KeyParam.PageBet_Inte_Cmd == (byte) 1)
    {
      FormMain.KeyParam.PageBet_Inte_Cmd = (byte) 0;
      this.Key_Clear_Fun();
    }
    if (this.Display_Dowlaod_Char_TM-- == 0)
      this.Hide_Dowload_Text();
    if (FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] != (byte) 0)
    {
      string str1 = "";
      string str2 = "";
      this.SetText.Text = str1 + FormMain.KeyParam.KeyChar[0] + " " + FormMain.KeyParam.KeyChar[2] + " " + FormMain.KeyParam.KeyChar[4] + " " + FormMain.KeyParam.KeyChar[6] + " " + FormMain.KeyParam.KeyChar[8];
      this.SetFunText.Text = str2 + FormMain.KeyParam.FunKeyChar[0] + " " + FormMain.KeyParam.FunKeyChar[1] + " " + FormMain.KeyParam.FunKeyChar[2] + " " + FormMain.KeyParam.FunKeyChar[3];
    }
    else
    {
      string str3 = "";
      string str4 = "";
      this.SetText.Text = str3;
      this.SetFunText.Text = str4;
    }
  }

  private void KeyBoardVersion_Check()
  {
    byte[] arrayBuff = new byte[65];
    FormMain.KeyParam.ReportID = (byte) 0;
    arrayBuff[0] = (byte) 0;
    arrayBuff[1] = (byte) 0;
    if (this.WriteMode == (ushort) 0)
    {
      if ((byte) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff)) == (byte) 0)
      {
        FormMain.KeyParam.ReportID = (byte) 0;
      }
      else
      {
        FormMain.KeyParam.ReportID = (byte) 2;
        if ((byte) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff)) == (byte) 0)
          FormMain.KeyParam.ReportID = (byte) 2;
        else
          FormMain.KeyParam.ReportID = (byte) 3;
      }
    }
    else
    {
      if (this.WriteMode != (ushort) 1)
        return;
      FormMain.KeyParam.ReportID = (byte) 3;
      if (this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff))
      {
        FormMain.KeyParam.ReportID = (byte) 3;
      }
      else
      {
        FormMain.KeyParam.ReportID = (byte) 0;
        if (this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff))
        {
          FormMain.KeyParam.ReportID = (byte) 0;
        }
        else
        {
          FormMain.KeyParam.ReportID = (byte) 2;
          if (!this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff))
            return;
          FormMain.KeyParam.ReportID = (byte) 2;
        }
      }
    }
  }

  private void Send_WriteFlash_Cmd()
  {
    byte[] arrayBuff = new byte[65];
    arrayBuff[0] = (byte) 170;
    arrayBuff[1] = (byte) 170;
    if (this.WriteMode == (ushort) 0)
    {
      if ((byte) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff)) == (byte) 0)
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download success";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Green;
        this.Show_Dowload_Text();
      }
      else
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download failed";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Red;
        this.Show_Dowload_Text();
      }
    }
    else
    {
      if (this.WriteMode != (ushort) 1)
        return;
      if (this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff))
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download success";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Green;
        this.Show_Dowload_Text();
      }
      else
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download failed";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Red;
        this.Show_Dowload_Text();
      }
    }
  }

  private void Send_WriteFlashLED_Cmd()
  {
    byte[] arrayBuff = new byte[65];
    arrayBuff[0] = (byte) 170;
    arrayBuff[1] = (byte) 161;
    if (this.WriteMode == (ushort) 0)
    {
      if ((byte) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff)) == (byte) 0)
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download success";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Green;
        this.Show_Dowload_Text();
      }
      else
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download failed";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Red;
        this.Show_Dowload_Text();
      }
    }
    else
    {
      if (this.WriteMode != (ushort) 1)
        return;
      if (this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff))
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download success";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Green;
        this.Show_Dowload_Text();
      }
      else
      {
        switch (FormMain.KeyParam.Language_Set)
        {
          case 0:
            this.label_Dowload_Dsp.Text = "Download failed";
            break;
          case 1:
            this.label_Dowload_Dsp.Text = "????";
            break;
        }
        this.label_Dowload_Dsp.BackColor = this.label_Dowload_Dsp.BackColor = Color.Red;
        this.Show_Dowload_Text();
      }
    }
  }

  private void Send_SwLayer()
  {
    byte[] arrayBuff = new byte[65];
    arrayBuff[0] = (byte) 161;
    arrayBuff[1] = FormMain.KeyParam.KEY_Cur_Layer;
    if (arrayBuff[1] == (byte) 0)
      arrayBuff[1] = (byte) 1;
    if (this.WriteMode == (ushort) 0)
    {
      int num = (int) (byte) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff));
    }
    else
    {
      if (this.WriteMode != (ushort) 1)
        return;
      this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff);
    }
  }

  private void Download_Click(object sender, EventArgs e)
  {
    byte[] arrayBuff = new byte[65];
    if (!this.myHidLib.Get_Dev_Sta())
      return;
    arrayBuff[0] = FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum];
    if (arrayBuff[0] == (byte) 0)
      return;
    if (FormMain.KeyParam.ReportID == (byte) 0)
    {
      arrayBuff[1] = (FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] &= (byte) 15);
    }
    else
    {
      this.Send_SwLayer();
      arrayBuff[1] = FormMain.KeyParam.KEY_Cur_Layer;
      arrayBuff[1] <<= 4;
      arrayBuff[1] |= FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num];
    }
    if (((int) FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] & 15) == 1)
    {
      arrayBuff[2] = FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyGroupCharNum];
      for (byte index = 0; (int) index <= (int) FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyGroupCharNum]; ++index)
      {
        arrayBuff[3] = index;
        // Extended to support up to 15 characters (index 0-14)
        // Each character uses 2 bytes: modifier (byte 4) and keycode (byte 5)
        // Data layout: byte 4 = modifier for char 0, bytes 5+ = pairs of (modifier, keycode)
        if (index == 0)
        {
          arrayBuff[4] = FormMain.KeyParam.Data_Send_Buff[4];
          arrayBuff[5] = (byte) 0;
        }
        else
        {
          int dataOffset = 4 + ((index - 1) * 2);
          arrayBuff[4] = FormMain.KeyParam.Data_Send_Buff[dataOffset];
          arrayBuff[5] = FormMain.KeyParam.Data_Send_Buff[dataOffset + 1];
        }
        if (this.WriteMode == (ushort) 0)
        {
          int num = (int) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff));
        }
        else if (this.WriteMode == (ushort) 1)
          this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff);
      }
      this.Send_WriteFlash_Cmd();
    }
    else if (((int) FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] & 15) == 2)
    {
      arrayBuff[2] = FormMain.KeyParam.Data_Send_Buff[5];
      arrayBuff[3] = FormMain.KeyParam.Data_Send_Buff[6];
      if (this.WriteMode == (ushort) 0)
      {
        int num = (int) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff));
      }
      else if (this.WriteMode == (ushort) 1)
        this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff);
      this.Send_WriteFlash_Cmd();
    }
    else if (((int) FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] & 15) == 8)
    {
      arrayBuff[2] = FormMain.KeyParam.Data_Send_Buff[2];
      if (this.WriteMode == (ushort) 0)
      {
        int num = (int) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff));
      }
      else if (this.WriteMode == (ushort) 1)
        this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff);
      this.Send_WriteFlashLED_Cmd();
    }
    else
    {
      if (((int) FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] & 15) != 3)
        return;
      arrayBuff[2] = FormMain.KeyParam.Data_Send_Buff[5];
      arrayBuff[3] = FormMain.KeyParam.Data_Send_Buff[6];
      arrayBuff[4] = FormMain.KeyParam.Data_Send_Buff[7];
      arrayBuff[5] = FormMain.KeyParam.Data_Send_Buff[8];
      arrayBuff[6] = FormMain.KeyParam.Data_Send_Buff[9];
      if (this.WriteMode == (ushort) 0)
      {
        int num = (int) this.myHid.Write(new report(FormMain.KeyParam.ReportID, arrayBuff));
      }
      else if (this.WriteMode == (ushort) 1)
        this.myHidLib.WriteDevice(FormMain.KeyParam.ReportID, arrayBuff);
      this.Send_WriteFlash_Cmd();
    }
  }

  private void Key_Clear_Click(object sender, EventArgs e) => this.Key_Clear_Fun();

  private void Key_Clear_Fun()
  {
    this.Clear_Key_Char();
    this.Set_Key_Init();
    this.KEY_Colour_Init();
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 0;
  }

  private void Clear_Key_Char()
  {
    for (int index = 0; index < 100; ++index)
    {
      FormMain.KeyParam.KeyChar[index] = (string) null;
      FormMain.KeyParam.FunKeyChar[index] = (string) null;
      FormMain.KeyParam.FunKEY_Char_Num = (byte) 0;
    }
  }

  private void Set_Key_Init()
  {
    FormMain.KeyParam.KEY_Char_Num = (byte) 5;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyGroupCharNum] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyValNum] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.Key_Fun_Num] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 1] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 2] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 3] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 4] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 5] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 6] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 7] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 8] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 9] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 10] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 11] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 12] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 13] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 14] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 15] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 16 /*0x10*/] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 17] = (byte) 0;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KEY_Char_Num + 18] = (byte) 0;
  }

  private void KEY_Colour_Init()
  {
    int red = 152;
    int green = 251;
    int blue = 152;
    this.KEY1.BackColor = Color.FromArgb(red, green, blue);
    this.KEY2.BackColor = Color.FromArgb(red, green, blue);
    this.KEY3.BackColor = Color.FromArgb(red, green, blue);
    this.KEY4.BackColor = Color.FromArgb(red, green, blue);
    this.KEY5.BackColor = Color.FromArgb(red, green, blue);
    this.KEY6.BackColor = Color.FromArgb(red, green, blue);
    this.KEY7.BackColor = Color.FromArgb(red, green, blue);
    this.KEY8.BackColor = Color.FromArgb(red, green, blue);
    this.KEY9.BackColor = Color.FromArgb(red, green, blue);
    this.KEY10.BackColor = Color.FromArgb(red, green, blue);
    this.KEY11.BackColor = Color.FromArgb(red, green, blue);
    this.KEY12.BackColor = Color.FromArgb(red, green, blue);
    this.KEY13.BackColor = Color.FromArgb(red, green, blue);
    this.KEY14.BackColor = Color.FromArgb(red, green, blue);
    this.KEY15.BackColor = Color.FromArgb(red, green, blue);
    this.KEY16.BackColor = Color.FromArgb(red, green, blue);
    this.K1_Left.BackColor = Color.FromArgb(red, green, blue);
    this.K1_Centre.BackColor = Color.FromArgb(red, green, blue);
    this.K1_Right.BackColor = Color.FromArgb(red, green, blue);
    this.K2_Left.BackColor = Color.FromArgb(red, green, blue);
    this.K2_Centre.BackColor = Color.FromArgb(red, green, blue);
    this.K2_Right.BackColor = Color.FromArgb(red, green, blue);
    this.K3_Left.BackColor = Color.FromArgb(red, green, blue);
    this.K3_Centre.BackColor = Color.FromArgb(red, green, blue);
    this.K3_Right.BackColor = Color.FromArgb(red, green, blue);
  }

  private void KEY1_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 1;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY1.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY2_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 2;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY2.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY3_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 3;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY3.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY4_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 4;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY4.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY5_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 5;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY5.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY6_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 6;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY6.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY7_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 7;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY7.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY8_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 8;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY8.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY9_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 9;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY9.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY10_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 10;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY10.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY11_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 11;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY11.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void KEY12_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 12;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.KEY12.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K1_Left_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 13;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K1_Left.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K1_Centre_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 14;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K1_Centre.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K1_Right_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 15;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K1_Right.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K2_Left_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 16 /*0x10*/;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K2_Left.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K2_Centre_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 17;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K2_Centre.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void K2_Right_Click(object sender, EventArgs e)
  {
    if (FormMain.KeyParam.KEY_Cur_Page == (byte) 4)
      return;
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 18;
    this.Set_Key_Init();
    this.Clear_Key_Char();
    this.KEY_Colour_Init();
    this.K2_Right.BackColor = Color.FromArgb((int) byte.MaxValue, 48 /*0x30*/, 48 /*0x30*/);
  }

  private void FormMain_Load(object sender, EventArgs e)
  {
    this.asc.controllInitializeSize((Control) this);
  }

  private void MainPage_SizeChanged(object sender, EventArgs e)
  {
    this.asc.controlAutoSize((Control) this);
  }

  private void Lanuage_Set_ZH()
  {
    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
    this.ApplyResource();
  }

  private void Lanuage_Set_EN()
  {
    Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
    this.ApplyResource();
  }

  private void ApplyResource()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormMain));
    foreach (Control control in (ArrangedElementCollection) this.Controls)
      componentResourceManager.ApplyResources((object) control, control.Name);
    this.ResumeLayout(false);
    this.PerformLayout();
    componentResourceManager.ApplyResources((object) this, "$this");
  }

  private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
  {
  }

  private void SetFunText_TextChanged(object sender, EventArgs e)
  {
  }

  private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
  {
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormMain));
    this.splitContainer1 = new SplitContainer();
    this.flowLayoutPanel1 = new FlowLayoutPanel();
    this.stateLabel = new Label();
    this.fdToolStripMenuItem = new ToolStripMenuItem();
    this.Download = new Button();
    this.KEY1 = new Button();
    this.KEY2 = new Button();
    this.KEY3 = new Button();
    this.KEY4 = new Button();
    this.K1_Left = new Button();
    this.K1_Centre = new Button();
    this.K1_Right = new Button();
    this.SetText = new TextBox();
    this.Key_Clear = new Button();
    this.KEY8 = new Button();
    this.KEY7 = new Button();
    this.KEY6 = new Button();
    this.KEY5 = new Button();
    this.KEY12 = new Button();
    this.KEY11 = new Button();
    this.KEY10 = new Button();
    this.KEY9 = new Button();
    this.KEY16 = new Button();
    this.KEY15 = new Button();
    this.KEY14 = new Button();
    this.KEY13 = new Button();
    this.K2_Right = new Button();
    this.K2_Centre = new Button();
    this.K2_Left = new Button();
    this.SetFunText = new TextBox();
    this.K3_Left = new Button();
    this.K3_Centre = new Button();
    this.K3_Right = new Button();
    this.flowLayoutPanel_LayerFun = new FlowLayoutPanel();
    this.pictureBox1 = new PictureBox();
    this.label_Dowload_Dsp = new Label();
    this.splitContainer1.BeginInit();
    this.splitContainer1.SuspendLayout();
    ((ISupportInitialize) this.pictureBox1).BeginInit();
    this.SuspendLayout();
    componentResourceManager.ApplyResources((object) this.splitContainer1, "splitContainer1");
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Paint += new PaintEventHandler(this.splitContainer1_Panel1_Paint);
    this.splitContainer1.Panel2.Paint += new PaintEventHandler(this.splitContainer1_Panel2_Paint);
    componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
    this.flowLayoutPanel1.Name = "flowLayoutPanel1";
    componentResourceManager.ApplyResources((object) this.stateLabel, "stateLabel");
    this.stateLabel.BackColor = SystemColors.ActiveCaption;
    this.stateLabel.Name = "stateLabel";
    this.fdToolStripMenuItem.Name = "fdToolStripMenuItem";
    componentResourceManager.ApplyResources((object) this.fdToolStripMenuItem, "fdToolStripMenuItem");
    componentResourceManager.ApplyResources((object) this.Download, "Download");
    this.Download.Name = "Download";
    this.Download.UseVisualStyleBackColor = true;
    this.Download.Click += new EventHandler(this.Download_Click);
    componentResourceManager.ApplyResources((object) this.KEY1, "KEY1");
    this.KEY1.Name = "KEY1";
    this.KEY1.UseVisualStyleBackColor = true;
    this.KEY1.Click += new EventHandler(this.KEY1_Click);
    componentResourceManager.ApplyResources((object) this.KEY2, "KEY2");
    this.KEY2.Name = "KEY2";
    this.KEY2.UseVisualStyleBackColor = true;
    this.KEY2.Click += new EventHandler(this.KEY2_Click);
    componentResourceManager.ApplyResources((object) this.KEY3, "KEY3");
    this.KEY3.Name = "KEY3";
    this.KEY3.UseVisualStyleBackColor = true;
    this.KEY3.Click += new EventHandler(this.KEY3_Click);
    componentResourceManager.ApplyResources((object) this.KEY4, "KEY4");
    this.KEY4.Name = "KEY4";
    this.KEY4.UseVisualStyleBackColor = true;
    this.KEY4.Click += new EventHandler(this.KEY4_Click);
    componentResourceManager.ApplyResources((object) this.K1_Left, "K1_Left");
    this.K1_Left.Name = "K1_Left";
    this.K1_Left.UseVisualStyleBackColor = true;
    this.K1_Left.Click += new EventHandler(this.K1_Left_Click);
    componentResourceManager.ApplyResources((object) this.K1_Centre, "K1_Centre");
    this.K1_Centre.Name = "K1_Centre";
    this.K1_Centre.UseVisualStyleBackColor = true;
    this.K1_Centre.Click += new EventHandler(this.K1_Centre_Click);
    componentResourceManager.ApplyResources((object) this.K1_Right, "K1_Right");
    this.K1_Right.Name = "K1_Right";
    this.K1_Right.UseVisualStyleBackColor = true;
    this.K1_Right.Click += new EventHandler(this.K1_Right_Click);
    componentResourceManager.ApplyResources((object) this.SetText, "SetText");
    this.SetText.Name = "SetText";
    componentResourceManager.ApplyResources((object) this.Key_Clear, "Key_Clear");
    this.Key_Clear.Name = "Key_Clear";
    this.Key_Clear.UseVisualStyleBackColor = true;
    this.Key_Clear.Click += new EventHandler(this.Key_Clear_Click);
    componentResourceManager.ApplyResources((object) this.KEY8, "KEY8");
    this.KEY8.Name = "KEY8";
    this.KEY8.UseVisualStyleBackColor = true;
    this.KEY8.Click += new EventHandler(this.KEY8_Click);
    componentResourceManager.ApplyResources((object) this.KEY7, "KEY7");
    this.KEY7.Name = "KEY7";
    this.KEY7.UseVisualStyleBackColor = true;
    this.KEY7.Click += new EventHandler(this.KEY7_Click);
    componentResourceManager.ApplyResources((object) this.KEY6, "KEY6");
    this.KEY6.Name = "KEY6";
    this.KEY6.UseVisualStyleBackColor = true;
    this.KEY6.Click += new EventHandler(this.KEY6_Click);
    componentResourceManager.ApplyResources((object) this.KEY5, "KEY5");
    this.KEY5.Name = "KEY5";
    this.KEY5.UseVisualStyleBackColor = true;
    this.KEY5.Click += new EventHandler(this.KEY5_Click);
    componentResourceManager.ApplyResources((object) this.KEY12, "KEY12");
    this.KEY12.Name = "KEY12";
    this.KEY12.UseVisualStyleBackColor = true;
    this.KEY12.Click += new EventHandler(this.KEY12_Click);
    componentResourceManager.ApplyResources((object) this.KEY11, "KEY11");
    this.KEY11.Name = "KEY11";
    this.KEY11.UseVisualStyleBackColor = true;
    this.KEY11.Click += new EventHandler(this.KEY11_Click);
    componentResourceManager.ApplyResources((object) this.KEY10, "KEY10");
    this.KEY10.Name = "KEY10";
    this.KEY10.UseVisualStyleBackColor = true;
    this.KEY10.Click += new EventHandler(this.KEY10_Click);
    componentResourceManager.ApplyResources((object) this.KEY9, "KEY9");
    this.KEY9.Name = "KEY9";
    this.KEY9.UseVisualStyleBackColor = true;
    this.KEY9.Click += new EventHandler(this.KEY9_Click);
    componentResourceManager.ApplyResources((object) this.KEY16, "KEY16");
    this.KEY16.Name = "KEY16";
    this.KEY16.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.KEY15, "KEY15");
    this.KEY15.Name = "KEY15";
    this.KEY15.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.KEY14, "KEY14");
    this.KEY14.Name = "KEY14";
    this.KEY14.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.KEY13, "KEY13");
    this.KEY13.Name = "KEY13";
    this.KEY13.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.K2_Right, "K2_Right");
    this.K2_Right.Name = "K2_Right";
    this.K2_Right.UseVisualStyleBackColor = true;
    this.K2_Right.Click += new EventHandler(this.K2_Right_Click);
    componentResourceManager.ApplyResources((object) this.K2_Centre, "K2_Centre");
    this.K2_Centre.Name = "K2_Centre";
    this.K2_Centre.UseVisualStyleBackColor = true;
    this.K2_Centre.Click += new EventHandler(this.K2_Centre_Click);
    componentResourceManager.ApplyResources((object) this.K2_Left, "K2_Left");
    this.K2_Left.Name = "K2_Left";
    this.K2_Left.UseVisualStyleBackColor = true;
    this.K2_Left.Click += new EventHandler(this.K2_Left_Click);
    componentResourceManager.ApplyResources((object) this.SetFunText, "SetFunText");
    this.SetFunText.Name = "SetFunText";
    this.SetFunText.TextChanged += new EventHandler(this.SetFunText_TextChanged);
    componentResourceManager.ApplyResources((object) this.K3_Left, "K3_Left");
    this.K3_Left.Name = "K3_Left";
    this.K3_Left.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.K3_Centre, "K3_Centre");
    this.K3_Centre.Name = "K3_Centre";
    this.K3_Centre.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.K3_Right, "K3_Right");
    this.K3_Right.Name = "K3_Right";
    this.K3_Right.UseVisualStyleBackColor = true;
    componentResourceManager.ApplyResources((object) this.flowLayoutPanel_LayerFun, "flowLayoutPanel_LayerFun");
    this.flowLayoutPanel_LayerFun.Name = "flowLayoutPanel_LayerFun";
    this.pictureBox1.BackColor = Color.FromArgb(192 /*0xC0*/, 192 /*0xC0*/, (int) byte.MaxValue);
    componentResourceManager.ApplyResources((object) this.pictureBox1, "pictureBox1");
    this.pictureBox1.Name = "pictureBox1";
    this.pictureBox1.TabStop = false;
    componentResourceManager.ApplyResources((object) this.label_Dowload_Dsp, "label_Dowload_Dsp");
    this.label_Dowload_Dsp.Name = "label_Dowload_Dsp";
    componentResourceManager.ApplyResources((object) this, "$this");
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.flowLayoutPanel1);
    this.Controls.Add((Control) this.label_Dowload_Dsp);
    this.Controls.Add((Control) this.flowLayoutPanel_LayerFun);
    this.Controls.Add((Control) this.SetFunText);
    this.Controls.Add((Control) this.K3_Right);
    this.Controls.Add((Control) this.K3_Centre);
    this.Controls.Add((Control) this.K3_Left);
    this.Controls.Add((Control) this.K2_Right);
    this.Controls.Add((Control) this.K2_Centre);
    this.Controls.Add((Control) this.K2_Left);
    this.Controls.Add((Control) this.KEY16);
    this.Controls.Add((Control) this.KEY15);
    this.Controls.Add((Control) this.KEY14);
    this.Controls.Add((Control) this.KEY13);
    this.Controls.Add((Control) this.KEY12);
    this.Controls.Add((Control) this.KEY11);
    this.Controls.Add((Control) this.KEY10);
    this.Controls.Add((Control) this.KEY9);
    this.Controls.Add((Control) this.KEY8);
    this.Controls.Add((Control) this.KEY7);
    this.Controls.Add((Control) this.KEY6);
    this.Controls.Add((Control) this.KEY5);
    this.Controls.Add((Control) this.Key_Clear);
    this.Controls.Add((Control) this.SetText);
    this.Controls.Add((Control) this.K1_Right);
    this.Controls.Add((Control) this.K1_Centre);
    this.Controls.Add((Control) this.K1_Left);
    this.Controls.Add((Control) this.KEY4);
    this.Controls.Add((Control) this.KEY3);
    this.Controls.Add((Control) this.KEY2);
    this.Controls.Add((Control) this.KEY1);
    this.Controls.Add((Control) this.Download);
    this.Controls.Add((Control) this.stateLabel);
    this.Controls.Add((Control) this.splitContainer1);
    this.Controls.Add((Control) this.pictureBox1);
    this.Cursor = Cursors.Default;
    this.FormBorderStyle = FormBorderStyle.Fixed3D;
    this.Name = nameof (FormMain);
    this.Load += new EventHandler(this.FormMain_Load);
    this.splitContainer1.EndInit();
    this.splitContainer1.ResumeLayout(false);
    ((ISupportInitialize) this.pictureBox1).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public class KeyParam
  {
    public static byte[] Data_Send_Buff = new byte[65];
    public static byte KeySet_KeyNum = 0;
    public static byte KeyType_Num = 1;
    public static byte KeyGroupCharNum = 2;
    public static byte KeySet_KeyValNum = 3;
    public static byte Key_Fun_Num = 4;
    public static byte KEY_Char_Num = 5;
    public static byte PageBet_Inte_Cmd = 0;
    public static string[] KeyChar = new string[100];
    public static string[] FunKeyChar = new string[100];
    public static byte FunKEY_Char_Num = 0;
    public static byte ReportID = 0;
    public static byte KEY_Cur_Layer = 1;
    public static byte KEY_Cur_Page = 1;
    public static byte Language_Set = 0;
  }
}
