using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace HIDTester;

internal class AutoSizeFormClass
{
  public List<AutoSizeFormClass.controlRect> oldCtrl = new List<AutoSizeFormClass.controlRect>();
  private int ctrlNo;

  public void controllInitializeSize(Control mForm)
  {
    AutoSizeFormClass.controlRect controlRect;
    controlRect.Left = mForm.Left;
    controlRect.Top = mForm.Top;
    controlRect.Width = mForm.Width;
    controlRect.Height = mForm.Height;
    this.oldCtrl.Add(controlRect);
    this.AddControl(mForm);
  }

  private void AddControl(Control ctl)
  {
    foreach (Control control in (ArrangedElementCollection) ctl.Controls)
    {
      AutoSizeFormClass.controlRect controlRect;
      controlRect.Left = control.Left;
      controlRect.Top = control.Top;
      controlRect.Width = control.Width;
      controlRect.Height = control.Height;
      this.oldCtrl.Add(controlRect);
      if (control.Controls.Count > 0)
        this.AddControl(control);
    }
  }

  public void controlAutoSize(Control mForm)
  {
    if (this.ctrlNo == 0)
    {
      AutoSizeFormClass.controlRect controlRect;
      controlRect.Left = 0;
      controlRect.Top = 0;
      controlRect.Width = mForm.PreferredSize.Width;
      controlRect.Height = mForm.PreferredSize.Height;
      this.oldCtrl.Add(controlRect);
      this.AddControl(mForm);
    }
    float wScale = (float) mForm.Width / (float) this.oldCtrl[0].Width;
    float hScale = (float) mForm.Height / (float) this.oldCtrl[0].Height;
    this.ctrlNo = 1;
    this.AutoScaleControl(mForm, wScale, hScale);
  }

  private void AutoScaleControl(Control ctl, float wScale, float hScale)
  {
    foreach (Control control in (ArrangedElementCollection) ctl.Controls)
    {
      int left = this.oldCtrl[this.ctrlNo].Left;
      int top = this.oldCtrl[this.ctrlNo].Top;
      int width1 = this.oldCtrl[this.ctrlNo].Width;
      int height = this.oldCtrl[this.ctrlNo].Height;
      control.Left = (int) ((double) left * (double) wScale);
      control.Top = (int) ((double) top * (double) hScale);
      control.Width = (int) ((double) width1 * (double) wScale);
      control.Height = (int) ((double) height * (double) hScale);
      ++this.ctrlNo;
      if (control.Controls.Count > 0)
        this.AutoScaleControl(control, wScale, hScale);
      if (ctl is DataGridView)
      {
        DataGridView dataGridView = ctl as DataGridView;
        Cursor.Current = Cursors.WaitCursor;
        int num1 = 0;
        for (int index = 0; index < dataGridView.Columns.Count; ++index)
        {
          dataGridView.AutoResizeColumn(index, DataGridViewAutoSizeColumnMode.AllCells);
          num1 += dataGridView.Columns[index].Width;
        }
        int num2 = num1;
        int width2 = ctl.Size.Width;
        dataGridView.AutoSizeColumnsMode = num2 < width2 ? DataGridViewAutoSizeColumnsMode.Fill : DataGridViewAutoSizeColumnsMode.DisplayedCells;
        Cursor.Current = Cursors.Default;
      }
    }
  }

  public struct controlRect
  {
    public int Left;
    public int Top;
    public int Width;
    public int Height;
  }
}
