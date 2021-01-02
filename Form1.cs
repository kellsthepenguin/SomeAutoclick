using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Gma.System.MouseKeyHook;

namespace SomeAutoclick {
    public partial class Form1 : Form {
        private IKeyboardMouseEvents m_GlobalHook;
        private bool isClicking = false;

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        public Form1() {
            InitializeComponent();
            Subscribe();
            timer1.Interval = 1;
        }

        private void Subscribe() {
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void StartClicking() {
            if (!isClicking) {
                timer1.Interval = (int) numericUpDown1.Value;
                timer1.Start();
                isClicking = true;
            }
        }
        
        private void StopClicking() {
            if (isClicking) {
                timer1.Stop();
                isClicking = false;
            }
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == ';') {
                StopClicking();
            }

            if (e.KeyChar == '[') {
                StartClicking();
            }
        }

        private void ClickMouse() {
            const uint LBUTTONDOWN = 0x0002;
            const uint LBUTTONUP = 0x0004;

            mouse_event(LBUTTONDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
            mouse_event(LBUTTONUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        }

        private void timer1_Tick(object sender, EventArgs e) {
            ClickMouse();
        }

        private void button1_Click(object sender, EventArgs e) {
            StartClicking();
        }

        private void button2_Click(object sender, EventArgs e) {
            StopClicking();
        }
    }
}
