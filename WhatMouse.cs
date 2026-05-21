using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new WhatMouseForm());
    }
}

internal sealed class WhatMouseForm : Form
{
    private readonly Button toggleButton;
    private readonly Label statusLabel;
    private readonly Timer timer;
    private bool running;

    private const int IntervalMilliseconds = 30000;
    private const int Pixels = 1;

    private const uint ES_CONTINUOUS = 0x80000000;
    private const uint ES_SYSTEM_REQUIRED = 0x00000001;
    private const uint ES_DISPLAY_REQUIRED = 0x00000002;

    public WhatMouseForm()
    {
        Text = "whatmouse";
        ClientSize = new Size(260, 150);
        MinimumSize = new Size(260, 150);
        MaximizeBox = false;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        StartPosition = FormStartPosition.CenterScreen;

        toggleButton = new Button();
        toggleButton.Text = "Start";
        toggleButton.Font = new Font(Font.FontFamily, 18.0f, FontStyle.Bold);
        toggleButton.Size = new Size(170, 64);
        toggleButton.Location = new Point(45, 28);
        toggleButton.Click += ToggleButtonClick;

        statusLabel = new Label();
        statusLabel.Text = "Off";
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        statusLabel.AutoSize = false;
        statusLabel.Size = new Size(220, 24);
        statusLabel.Location = new Point(20, 105);

        timer = new Timer();
        timer.Interval = IntervalMilliseconds;
        timer.Tick += TimerTick;

        Controls.Add(toggleButton);
        Controls.Add(statusLabel);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        StopJiggler();
        base.OnFormClosing(e);
    }

    private void ToggleButtonClick(object sender, EventArgs e)
    {
        if (running)
        {
            StopJiggler();
        }
        else
        {
            StartJiggler();
        }
    }

    private void StartJiggler()
    {
        running = true;
        toggleButton.Text = "Stop";
        statusLabel.Text = "On: moving every 30 seconds";
        KeepAwake();
        JiggleMouse();
        timer.Start();
    }

    private void StopJiggler()
    {
        timer.Stop();
        running = false;
        toggleButton.Text = "Start";
        statusLabel.Text = "Off";
        SetThreadExecutionState(ES_CONTINUOUS);
    }

    private void TimerTick(object sender, EventArgs e)
    {
        KeepAwake();
        JiggleMouse();
    }

    private static void KeepAwake()
    {
        SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
    }

    private static void JiggleMouse()
    {
        POINT point;
        if (!GetCursorPos(out point))
        {
            return;
        }

        int virtualLeft = GetSystemMetrics(76);
        int virtualRight = virtualLeft + GetSystemMetrics(78) - 1;
        int offset = point.X + Pixels > virtualRight ? -Pixels : Pixels;

        SetCursorPos(point.X + offset, point.Y);
        System.Threading.Thread.Sleep(120);
        SetCursorPos(point.X, point.Y);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int X;
        public int Y;
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("kernel32.dll")]
    private static extern uint SetThreadExecutionState(uint esFlags);
}
