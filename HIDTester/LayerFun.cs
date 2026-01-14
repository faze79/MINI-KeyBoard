using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HIDTester;

public class LayerFun : UserControl
{
  private IContainer components;
  private RadioButton KEY_Layer1;
  private RadioButton KEY_Layer2;
  private RadioButton KEY_Layer3;
  private RadioButton KEY_FunLayer1;
  private RadioButton KEY_FunLayer2;
  private RadioButton KEY_FunLayer3;
  private Label lblTitle;

  // Modern UI Colors
  private readonly Color textColor = Color.FromArgb(241, 241, 241);
  private readonly Color panelBackColor = Color.FromArgb(37, 37, 38);
  private readonly Color layer1Color = Color.FromArgb(0, 122, 204);   // Blue
  private readonly Color layer2Color = Color.FromArgb(16, 124, 16);   // Green
  private readonly Color layer3Color = Color.FromArgb(194, 59, 34);   // Red

  public LayerFun()
  {
    this.InitializeComponent();
    this.ApplyModernTheme();
  }

  private void ApplyModernTheme()
  {
    this.BackColor = this.panelBackColor;

    // Style the title
    if (this.lblTitle != null)
    {
      this.lblTitle.ForeColor = this.textColor;
      this.lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
    }

    // Style layer buttons with distinct colors
    StyleLayerButton(this.KEY_FunLayer1, this.layer1Color, "Layer 1");
    StyleLayerButton(this.KEY_FunLayer2, this.layer2Color, "Layer 2");
    StyleLayerButton(this.KEY_FunLayer3, this.layer3Color, "Layer 3");
  }

  private void StyleLayerButton(RadioButton rb, Color layerColor, string text)
  {
    if (rb == null) return;

    rb.ForeColor = layerColor;
    rb.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
    rb.Text = text;
    rb.Cursor = Cursors.Hand;
    rb.AutoSize = true;
  }

  private void KEY_FunLayer1_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.KEY_FunLayer1.Checked)
      return;
    FormMain.KeyParam.KEY_Cur_Layer = (byte) 1;
    FormMain.KeyParam.PageBet_Inte_Cmd = (byte) 1;
  }

  private void KEY_FunLayer2_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.KEY_FunLayer2.Checked)
      return;
    FormMain.KeyParam.KEY_Cur_Layer = (byte) 2;
    FormMain.KeyParam.PageBet_Inte_Cmd = (byte) 1;
  }

  private void KEY_FunLayer3_CheckedChanged(object sender, EventArgs e)
  {
    if (!this.KEY_FunLayer3.Checked)
      return;
    FormMain.KeyParam.KEY_Cur_Layer = (byte) 3;
    FormMain.KeyParam.PageBet_Inte_Cmd = (byte) 1;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.KEY_Layer1 = new RadioButton();
    this.KEY_Layer2 = new RadioButton();
    this.KEY_Layer3 = new RadioButton();
    this.KEY_FunLayer1 = new RadioButton();
    this.KEY_FunLayer2 = new RadioButton();
    this.KEY_FunLayer3 = new RadioButton();
    this.lblTitle = new Label();
    this.SuspendLayout();

    // lblTitle
    this.lblTitle.AutoSize = true;
    this.lblTitle.Location = new Point(5, 5);
    this.lblTitle.Name = "lblTitle";
    this.lblTitle.Size = new Size(80, 15);
    this.lblTitle.Text = "Edit Layer:";

    // KEY_FunLayer1
    this.KEY_FunLayer1.AutoSize = true;
    this.KEY_FunLayer1.Checked = true;
    this.KEY_FunLayer1.Location = new Point(5, 28);
    this.KEY_FunLayer1.Name = "KEY_FunLayer1";
    this.KEY_FunLayer1.Size = new Size(70, 20);
    this.KEY_FunLayer1.TabIndex = 0;
    this.KEY_FunLayer1.TabStop = true;
    this.KEY_FunLayer1.Text = "Layer 1";
    this.KEY_FunLayer1.UseVisualStyleBackColor = true;
    this.KEY_FunLayer1.CheckedChanged += new EventHandler(this.KEY_FunLayer1_CheckedChanged);

    // KEY_FunLayer2
    this.KEY_FunLayer2.AutoSize = true;
    this.KEY_FunLayer2.Location = new Point(5, 53);
    this.KEY_FunLayer2.Name = "KEY_FunLayer2";
    this.KEY_FunLayer2.Size = new Size(70, 20);
    this.KEY_FunLayer2.TabIndex = 1;
    this.KEY_FunLayer2.Text = "Layer 2";
    this.KEY_FunLayer2.UseVisualStyleBackColor = true;
    this.KEY_FunLayer2.CheckedChanged += new EventHandler(this.KEY_FunLayer2_CheckedChanged);

    // KEY_FunLayer3
    this.KEY_FunLayer3.AutoSize = true;
    this.KEY_FunLayer3.Location = new Point(5, 78);
    this.KEY_FunLayer3.Name = "KEY_FunLayer3";
    this.KEY_FunLayer3.Size = new Size(70, 20);
    this.KEY_FunLayer3.TabIndex = 2;
    this.KEY_FunLayer3.Text = "Layer 3";
    this.KEY_FunLayer3.UseVisualStyleBackColor = true;
    this.KEY_FunLayer3.CheckedChanged += new EventHandler(this.KEY_FunLayer3_CheckedChanged);

    // LayerFun
    this.AutoScaleDimensions = new SizeF(6F, 13F);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add(this.lblTitle);
    this.Controls.Add(this.KEY_FunLayer3);
    this.Controls.Add(this.KEY_FunLayer2);
    this.Controls.Add(this.KEY_FunLayer1);
    this.ForeColor = SystemColors.InactiveCaptionText;
    this.Name = nameof(LayerFun);
    this.Size = new Size(100, 110);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
