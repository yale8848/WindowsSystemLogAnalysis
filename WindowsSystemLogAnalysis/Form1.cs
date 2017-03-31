using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WindowsSystemLogAnalysis
{
    public partial class Form1 : Form
    {
      private List<DateTime> mTimeList = null;
      public Form1()
      {
        
        InitializeComponent();
        mTimeList = new List<DateTime>();

        numericUpDown1.Value = 18;
        numericUpDown2.Value = 30;
         
      }

      private void button1_Click(object sender, EventArgs e)
      {
          label5.Visible = false;
          button1.Text = "正在获取日志...";
          button1.Enabled = false;
          fillData(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month,
            (int)numericUpDown1.Value, (int)numericUpDown2.Value);
          button1.Enabled = true;
          button1.Text = "获取日志";

      }
      private string getDateDiff(DateTime dt, DateTime dd)
      {

          TimeSpan df = dt - dd;


          if (df.Hours < 0 || df.Minutes < 0)
          {
              return "0";
          }

          return df.Hours + "小时" + df.Minutes + "分钟";

      }
      private string getDateDiff(DateTime dt,int startHour,int startMin) 
      {
        DateTime dStart = new DateTime(dt.Year, dt.Month, dt.Day, startHour, startMin, 0);

        return getDateDiff(dt, dStart);
        
      }
      private string getDateDiff(string head,DateTime tail)
      {
        DateTime dStart = DateTime.Parse(head);


        return getDateDiff(tail, dStart);

      }
      private string getDateDiffWithWeeken(string head, DateTime dt,int startHour,int startMin)
      {
          string time = head + "    " + dt.ToString("HH:mm:ss");
          if (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
          {
              time += ("    " + getDateDiff(head, dt) + "    " + dt.DayOfWeek.ToString());
          }
          else
          {
              time += ("    " + getDateDiff(dt, startHour, startMin));
          }

          return time;

      }
      private void fillData(int year, int month, int startHour, int startMin)
      {
        listBox1.Items.Clear();
        mTimeList.Clear();

         

        EventLog myLog = new EventLog();
        myLog.Log = "System";
        foreach (EventLogEntry entry in myLog.Entries)
        {

            if (entry.TimeGenerated.Year == year && entry.TimeGenerated.Month == month)
            {
              mTimeList.Add(entry.TimeGenerated);
            }

           
           
        }
        if (mTimeList.Count==0)
        {
            label5.Visible = true;
            return;
        }
        List<DateTime> list = mTimeList.OrderBy(a => a).ToList();

        string head = list[0].ToString();

        for (int i = 0; i < list.Count;i++ )
        {
 
            if (i > 0 && list[i - 1].Day != list[i].Day)
            {

                this.listBox1.Items.Add(getDateDiffWithWeeken(head,list[i-1],startHour,startMin));
              head = list[i].ToString();
            }
        }

        this.listBox1.Items.Add(getDateDiffWithWeeken(head, list[list.Count - 1], startHour, startMin));

      }

      private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
      {
      }



 


    }
}
