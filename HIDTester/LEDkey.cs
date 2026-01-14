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

  // Extended LED controls
  private Button KEY_LEDMode3;
  private Button KEY_LEDMode4;
  private Button KEY_LEDMode5;
  private Button KEY_LEDMode6;
  private Button KEY_LEDMode7;
  private NumericUpDown numCustomMode;
  private Button btnCustomMode;
  private Label lblCustomMode;
  private Label lblExperimental;

  // Modern UI Colors
  private readonly Color keyBackColor = Color.FromArgb(60, 60, 65);
  private readonly Color keyHoverColor = Color.FromArgb(80, 80, 85);
  private readonly Color accentColor = Color.FromArgb(0, 122, 204);
  private readonly Color textColor = Color.FromArgb(241, 241, 241);
  private readonly Color panelBackColor = Color.FromArgb(37, 37, 38);

  public LEDkey()
  {
    this.InitializeComponent();
    this.ApplyModernTheme();
    this.KEY_Colour_Init();
  }

  private void ApplyModernTheme()
  {
    this.BackColor = this.panelBackColor;
    foreach (Control ctrl in this.Controls)
    {
      if (ctrl is Button btn)
      {
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 75);
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.MouseOverBackColor = this.keyHoverColor;
        btn.FlatAppearance.MouseDownBackColor = this.accentColor;
        btn.BackColor = this.keyBackColor;
        btn.ForeColor = this.textColor;
        btn.Cursor = Cursors.Hand;
      }
      else if (ctrl is Label lbl)
      {
        lbl.ForeColor = this.textColor;
      }
      else if (ctrl is NumericUpDown num)
      {
        num.BackColor = this.keyBackColor;
        num.ForeColor = this.textColor;
      }
    }
  }

  private void KEY_Colour_Init()
  {
    this.KEY_LEDMode0.BackColor = this.keyBackColor;
    this.KEY_LEDMode1.BackColor = this.keyBackColor;
    this.KEY_LEDMode2.BackColor = this.keyBackColor;
    this.KEY_LEDMode3.BackColor = this.keyBackColor;
    this.KEY_LEDMode4.BackColor = this.keyBackColor;
    this.KEY_LEDMode5.BackColor = this.keyBackColor;
    this.KEY_LEDMode6.BackColor = this.keyBackColor;
    this.KEY_LEDMode7.BackColor = this.keyBackColor;
    this.btnCustomMode.BackColor = this.keyBackColor;
  }

  private void LEDGeneral_Char_Set()
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeyType_Num] |= (byte) 8;
  }

  private void SetLEDMode(byte mode, Button selectedButton, string modeName)
  {
    FormMain.KeyParam.Data_Send_Buff[(int) FormMain.KeyParam.KeySet_KeyNum] = (byte) 176 /*0xB0*/;
    this.LEDGeneral_Char_Set();
    FormMain.KeyParam.Data_Send_Buff[2] = mode;
    FormMain.KeyParam.KeyChar[(int) FormMain.KeyParam.KEY_Char_Num - 5] = modeName;
    this.KEY_Colour_Init();
    if (selectedButton != null)
      selectedButton.BackColor = this.accentColor;
  }

  private void KEY_LEDMode0_Click(object sender, EventArgs e)
  {
    SetLEDMode(0, this.KEY_LEDMode0, "LED Off");
  }

  private void KEY_LEDMode1_Click(object sender, EventArgs e)
  {
    SetLEDMode(1, this.KEY_LEDMode1, "LED Mode 1");
  }

  private void KEY_LEDMode2_Click(object sender, EventArgs e)
  {
    SetLEDMode(2, this.KEY_LEDMode2, "LED Mode 2");
  }

  private void KEY_LEDMode3_Click(object sender, EventArgs e)
  {
    SetLEDMode(3, this.KEY_LEDMode3, "LED Mode 3");
  }

  private void KEY_LEDMode4_Click(object sender, EventArgs e)
  {
    SetLEDMode(4, this.KEY_LEDMode4, "LED Mode 4");
  }

  private void KEY_LEDMode5_Click(object sender, EventArgs e)
  {
    SetLEDMode(5, this.KEY_LEDMode5, "LED Mode 5");
  }

  private void KEY_LEDMode6_Click(object sender, EventArgs e)
  {
    SetLEDMode(6, this.KEY_LEDMode6, "LED Mode 6");
  }

  private void KEY_LEDMode7_Click(object sender, EventArgs e)
  {
    SetLEDMode(7, this.KEY_LEDMode7, "LED Mode 7");
  }

  private void btnCustomMode_Click(object sender, EventArgs e)
  {
    byte mode = (byte)this.numCustomMode.Value;
    SetLEDMode(mode, this.btnCustomMode, $"LED Mode {mode}");
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.KEY_LEDMode0 = new Button();
    this.KEY_LEDMode1 = new Button();
    this.KEY_LEDMode2 = new Button();
    this.KEY_LEDMode3 = new Button();
    this.KEY_LEDMode4 = new Button();
    this.KEY_LEDMode5 = new Button();
    this.KEY_LEDMode6 = new Button();
    this.KEY_LEDMode7 = new Button();
    this.numCustomMode = new NumericUpDown();
    this.btnCustomMode = new Button();
    this.lblCustomMode = new Label();
    this.lblExperimental = new Label();
    ((ISupportInitialize)(this.numCustomMode)).BeginInit();
    this.SuspendLayout();

    // Label for original modes
    Label lblOriginal = new Label();
    lblOriginal.AutoSize = true;
    lblOriginal.Location = new Point(10, 10);
    lblOriginal.Text = "Standard Modes:";
    lblOriginal.ForeColor = this.textColor;

    // KEY_LEDMode0 - Off
    this.KEY_LEDMode0.Location = new Point(10, 35);
    this.KEY_LEDMode0.Name = "KEY_LEDMode0";
    this.KEY_LEDMode0.Size = new Size(80, 30);
    this.KEY_LEDMode0.TabIndex = 0;
    this.KEY_LEDMode0.Text = "Off";
    this.KEY_LEDMode0.UseVisualStyleBackColor = true;
    this.KEY_LEDMode0.Click += new EventHandler(this.KEY_LEDMode0_Click);

    // KEY_LEDMode1
    this.KEY_LEDMode1.Location = new Point(100, 35);
    this.KEY_LEDMode1.Name = "KEY_LEDMode1";
    this.KEY_LEDMode1.Size = new Size(80, 30);
    this.KEY_LEDMode1.TabIndex = 1;
    this.KEY_LEDMode1.Text = "Mode 1";
    this.KEY_LEDMode1.UseVisualStyleBackColor = true;
    this.KEY_LEDMode1.Click += new EventHandler(this.KEY_LEDMode1_Click);

    // KEY_LEDMode2
    this.KEY_LEDMode2.Location = new Point(190, 35);
    this.KEY_LEDMode2.Name = "KEY_LEDMode2";
    this.KEY_LEDMode2.Size = new Size(80, 30);
    this.KEY_LEDMode2.TabIndex = 2;
    this.KEY_LEDMode2.Text = "Mode 2";
    this.KEY_LEDMode2.UseVisualStyleBackColor = true;
    this.KEY_LEDMode2.Click += new EventHandler(this.KEY_LEDMode2_Click);

    // Experimental label
    this.lblExperimental.AutoSize = true;
    this.lblExperimental.Location = new Point(10, 80);
    this.lblExperimental.Text = "Extended Modes (experimental):";
    this.lblExperimental.ForeColor = Color.FromArgb(255, 180, 100);

    // KEY_LEDMode3
    this.KEY_LEDMode3.Location = new Point(10, 105);
    this.KEY_LEDMode3.Name = "KEY_LEDMode3";
    this.KEY_LEDMode3.Size = new Size(80, 30);
    this.KEY_LEDMode3.TabIndex = 3;
    this.KEY_LEDMode3.Text = "Mode 3";
    this.KEY_LEDMode3.UseVisualStyleBackColor = true;
    this.KEY_LEDMode3.Click += new EventHandler(this.KEY_LEDMode3_Click);

    // KEY_LEDMode4
    this.KEY_LEDMode4.Location = new Point(100, 105);
    this.KEY_LEDMode4.Name = "KEY_LEDMode4";
    this.KEY_LEDMode4.Size = new Size(80, 30);
    this.KEY_LEDMode4.TabIndex = 4;
    this.KEY_LEDMode4.Text = "Mode 4";
    this.KEY_LEDMode4.UseVisualStyleBackColor = true;
    this.KEY_LEDMode4.Click += new EventHandler(this.KEY_LEDMode4_Click);

    // KEY_LEDMode5
    this.KEY_LEDMode5.Location = new Point(190, 105);
    this.KEY_LEDMode5.Name = "KEY_LEDMode5";
    this.KEY_LEDMode5.Size = new Size(80, 30);
    this.KEY_LEDMode5.TabIndex = 5;
    this.KEY_LEDMode5.Text = "Mode 5";
    this.KEY_LEDMode5.UseVisualStyleBackColor = true;
    this.KEY_LEDMode5.Click += new EventHandler(this.KEY_LEDMode5_Click);

    // KEY_LEDMode6
    this.KEY_LEDMode6.Location = new Point(10, 145);
    this.KEY_LEDMode6.Name = "KEY_LEDMode6";
    this.KEY_LEDMode6.Size = new Size(80, 30);
    this.KEY_LEDMode6.TabIndex = 6;
    this.KEY_LEDMode6.Text = "Mode 6";
    this.KEY_LEDMode6.UseVisualStyleBackColor = true;
    this.KEY_LEDMode6.Click += new EventHandler(this.KEY_LEDMode6_Click);

    // KEY_LEDMode7
    this.KEY_LEDMode7.Location = new Point(100, 145);
    this.KEY_LEDMode7.Name = "KEY_LEDMode7";
    this.KEY_LEDMode7.Size = new Size(80, 30);
    this.KEY_LEDMode7.TabIndex = 7;
    this.KEY_LEDMode7.Text = "Mode 7";
    this.KEY_LEDMode7.UseVisualStyleBackColor = true;
    this.KEY_LEDMode7.Click += new EventHandler(this.KEY_LEDMode7_Click);

    // Custom mode label
    this.lblCustomMode.AutoSize = true;
    this.lblCustomMode.Location = new Point(10, 195);
    this.lblCustomMode.Text = "Custom Mode (0-255):";
    this.lblCustomMode.ForeColor = this.textColor;

    // Custom mode numeric
    this.numCustomMode.Location = new Point(10, 218);
    this.numCustomMode.Name = "numCustomMode";
    this.numCustomMode.Size = new Size(70, 23);
    this.numCustomMode.Minimum = 0;
    this.numCustomMode.Maximum = 255;
    this.numCustomMode.Value = 0;

    // Custom mode button
    this.btnCustomMode.Location = new Point(90, 215);
    this.btnCustomMode.Name = "btnCustomMode";
    this.btnCustomMode.Size = new Size(80, 30);
    this.btnCustomMode.TabIndex = 8;
    this.btnCustomMode.Text = "Apply";
    this.btnCustomMode.UseVisualStyleBackColor = true;
    this.btnCustomMode.Click += new EventHandler(this.btnCustomMode_Click);

    // UserControl
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Size = new Size(300, 270);
    this.Controls.Add(lblOriginal);
    this.Controls.Add(this.KEY_LEDMode0);
    this.Controls.Add(this.KEY_LEDMode1);
    this.Controls.Add(this.KEY_LEDMode2);
    this.Controls.Add(this.lblExperimental);
    this.Controls.Add(this.KEY_LEDMode3);
    this.Controls.Add(this.KEY_LEDMode4);
    this.Controls.Add(this.KEY_LEDMode5);
    this.Controls.Add(this.KEY_LEDMode6);
    this.Controls.Add(this.KEY_LEDMode7);
    this.Controls.Add(this.lblCustomMode);
    this.Controls.Add(this.numCustomMode);
    this.Controls.Add(this.btnCustomMode);
    this.Name = nameof(LEDkey);

    ((ISupportInitialize)(this.numCustomMode)).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
