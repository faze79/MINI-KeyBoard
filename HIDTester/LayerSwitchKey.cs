// Layer Switch Key - allows programming a key to switch layers
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HIDTester;

public class LayerSwitchKey : UserControl
{
  private IContainer components;
  private RadioButton rbSwitchToLayer1;
  private RadioButton rbSwitchToLayer2;
  private RadioButton rbSwitchToLayer3;
  private RadioButton rbToggleLayers;
  private Label lblDescription;
  private Label lblInfo;

  // Modern UI Colors
  private readonly Color textColor = Color.FromArgb(241, 241, 241);
  private readonly Color panelBackColor = Color.FromArgb(37, 37, 38);
  private readonly Color accentColor = Color.FromArgb(0, 122, 204);
  private readonly Color layer1Color = Color.FromArgb(0, 122, 204);
  private readonly Color layer2Color = Color.FromArgb(16, 124, 16);
  private readonly Color layer3Color = Color.FromArgb(194, 59, 34);

  public LayerSwitchKey()
  {
    this.InitializeComponent();
    this.ApplyModernTheme();
  }

  private void ApplyModernTheme()
  {
    this.BackColor = this.panelBackColor;

    // Style radio buttons
    foreach (Control ctrl in this.Controls)
    {
      if (ctrl is RadioButton rb)
      {
        rb.ForeColor = this.textColor;
        rb.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
        rb.Cursor = Cursors.Hand;
      }
      else if (ctrl is Label lbl)
      {
        lbl.ForeColor = this.textColor;
      }
    }

    // Color-code the layer buttons
    this.rbSwitchToLayer1.ForeColor = this.layer1Color;
    this.rbSwitchToLayer2.ForeColor = this.layer2Color;
    this.rbSwitchToLayer3.ForeColor = this.layer3Color;
    this.rbToggleLayers.ForeColor = Color.FromArgb(180, 180, 180);
  }

  private void RbSwitchToLayer1_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.rbSwitchToLayer1.Checked)
      return;

    // Set key type to 4 (layer switch) and target layer to 1
    SetLayerSwitchData(1);
    this.lblInfo.Text = "Key will switch to Layer 1 (Blue)";
    this.lblInfo.ForeColor = this.layer1Color;
  }

  private void RbSwitchToLayer2_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.rbSwitchToLayer2.Checked)
      return;

    SetLayerSwitchData(2);
    this.lblInfo.Text = "Key will switch to Layer 2 (Green)";
    this.lblInfo.ForeColor = this.layer2Color;
  }

  private void RbSwitchToLayer3_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.rbSwitchToLayer3.Checked)
      return;

    SetLayerSwitchData(3);
    this.lblInfo.Text = "Key will switch to Layer 3 (Red)";
    this.lblInfo.ForeColor = this.layer3Color;
  }

  private void RbToggleLayers_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.rbToggleLayers.Checked)
      return;

    // Toggle cycles through 1->2->3->1
    SetLayerSwitchData(0);
    this.lblInfo.Text = "Key will cycle through layers (1 -> 2 -> 3 -> 1)";
    this.lblInfo.ForeColor = Color.FromArgb(180, 180, 180);
  }

  private void SetLayerSwitchData(byte targetLayer)
  {
    // Key type 4 = layer switch (based on protocol analysis)
    // The firmware should recognize this as a special function key
    FormMain.KeyParam.Data_Send_Buff[(int)FormMain.KeyParam.KeyType_Num] = (byte)4;

    // Store target layer in the data buffer
    // Using position 5 (first data byte after type info) for target layer
    FormMain.KeyParam.Data_Send_Buff[5] = targetLayer;

    // Also store a marker in position 6 to indicate it's a layer switch command
    FormMain.KeyParam.Data_Send_Buff[6] = (byte)0xA1; // Layer switch command marker

    // Update the display in the main form textbox
    // Clear previous key chars
    for (int i = 0; i < 10; i++)
    {
      FormMain.KeyParam.KeyChar[i] = null;
    }

    // Show layer switch info in the key display
    if (targetLayer == 0)
    {
      FormMain.KeyParam.KeyChar[0] = "[Layer Toggle]";
    }
    else
    {
      FormMain.KeyParam.KeyChar[0] = $"[Layer {targetLayer}]";
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.rbSwitchToLayer1 = new RadioButton();
    this.rbSwitchToLayer2 = new RadioButton();
    this.rbSwitchToLayer3 = new RadioButton();
    this.rbToggleLayers = new RadioButton();
    this.lblDescription = new Label();
    this.lblInfo = new Label();
    this.SuspendLayout();

    // lblDescription
    this.lblDescription.AutoSize = true;
    this.lblDescription.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
    this.lblDescription.Location = new Point(20, 15);
    this.lblDescription.Name = "lblDescription";
    this.lblDescription.Size = new Size(300, 25);
    this.lblDescription.Text = "Layer Switch - Program key to change layers";

    // rbSwitchToLayer1
    this.rbSwitchToLayer1.AutoSize = true;
    this.rbSwitchToLayer1.Location = new Point(30, 60);
    this.rbSwitchToLayer1.Name = "rbSwitchToLayer1";
    this.rbSwitchToLayer1.Size = new Size(180, 25);
    this.rbSwitchToLayer1.Text = "Switch to Layer 1";
    this.rbSwitchToLayer1.UseVisualStyleBackColor = true;
    this.rbSwitchToLayer1.CheckedChanged += new EventHandler(this.RbSwitchToLayer1_CheckedChanged);

    // rbSwitchToLayer2
    this.rbSwitchToLayer2.AutoSize = true;
    this.rbSwitchToLayer2.Location = new Point(30, 95);
    this.rbSwitchToLayer2.Name = "rbSwitchToLayer2";
    this.rbSwitchToLayer2.Size = new Size(180, 25);
    this.rbSwitchToLayer2.Text = "Switch to Layer 2";
    this.rbSwitchToLayer2.UseVisualStyleBackColor = true;
    this.rbSwitchToLayer2.CheckedChanged += new EventHandler(this.RbSwitchToLayer2_CheckedChanged);

    // rbSwitchToLayer3
    this.rbSwitchToLayer3.AutoSize = true;
    this.rbSwitchToLayer3.Location = new Point(30, 130);
    this.rbSwitchToLayer3.Name = "rbSwitchToLayer3";
    this.rbSwitchToLayer3.Size = new Size(180, 25);
    this.rbSwitchToLayer3.Text = "Switch to Layer 3";
    this.rbSwitchToLayer3.UseVisualStyleBackColor = true;
    this.rbSwitchToLayer3.CheckedChanged += new EventHandler(this.RbSwitchToLayer3_CheckedChanged);

    // rbToggleLayers
    this.rbToggleLayers.AutoSize = true;
    this.rbToggleLayers.Location = new Point(30, 175);
    this.rbToggleLayers.Name = "rbToggleLayers";
    this.rbToggleLayers.Size = new Size(200, 25);
    this.rbToggleLayers.Text = "Toggle Layers (Cycle 1-2-3)";
    this.rbToggleLayers.UseVisualStyleBackColor = true;
    this.rbToggleLayers.CheckedChanged += new EventHandler(this.RbToggleLayers_CheckedChanged);

    // lblInfo
    this.lblInfo.AutoSize = true;
    this.lblInfo.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
    this.lblInfo.ForeColor = Color.Gray;
    this.lblInfo.Location = new Point(30, 220);
    this.lblInfo.Name = "lblInfo";
    this.lblInfo.Size = new Size(250, 20);
    this.lblInfo.Text = "Select a layer switch option above";

    // LayerSwitchKey
    this.AutoScaleDimensions = new SizeF(6F, 13F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add(this.lblDescription);
    this.Controls.Add(this.rbSwitchToLayer1);
    this.Controls.Add(this.rbSwitchToLayer2);
    this.Controls.Add(this.rbSwitchToLayer3);
    this.Controls.Add(this.rbToggleLayers);
    this.Controls.Add(this.lblInfo);
    this.Name = "LayerSwitchKey";
    this.Size = new Size(600, 300);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
