using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace M5MouseController.Controller
{
    class NativeController
    {
        class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public extern static int SendInput(int nInputs, ref Input pInputs, int cbsize);

            [DllImport("user32.dll", SetLastError = true)]
            public extern static IntPtr GetMessageExtraInfo();

            [DllImport("user32.dll")]
            public extern static bool SetCursorPos(int x, int y);

            [DllImport("user32.dll")]
            public extern static bool GetCursorPos(out POINT lppoint);

        }

        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        const int MOUSEEVENTF_MIDDLEUP = 0x0030;
        const int MOUSEEVENTF_WHEEL = 0x0800;
        const int MOUSEEVENTF_HWHEEL = 0x01000;
        const int MOUSEEVENTF_VIRTUALDESK = 0x4000;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        [StructLayout(LayoutKind.Sequential)]
        struct MouseInput
        {
            public int X;
            public int Y;
            public int Data;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KeyboardInput
        {
            public short VirtualKey;
            public short ScanCode;
            public int Flags;
            public int Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HardwareInput
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct Input
        {
            public int Type;
            public InputUnion ui;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct InputUnion
        {
            [FieldOffset(0)]
            public MouseInput Mouse;
            [FieldOffset(0)]
            public KeyboardInput Keyboard;
            [FieldOffset(0)]
            public HardwareInput Hardware;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct POINT
        {
            public int X { get; set; }
            public int Y { get; set; }
            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        public static Point GetCursorPosition()
        {
            var pt = new POINT();
            NativeMethods.GetCursorPos(out pt);
            return pt;
        }

        static Input MouseMoveData(int x, int y, IntPtr extraInfo)
        {
            Input input = new Input();
            input.Type = 0; // MOUSE = 0
            input.ui.Mouse.Flags = MOUSEEVENTF_MOVE;// | MOUSEEVENTF_ABSOLUTE; // | MOUSEEVENTF_VIRTUALDESK
            input.ui.Mouse.Data = 0;
            input.ui.Mouse.X = x;
            input.ui.Mouse.Y = y;
            input.ui.Mouse.Time = 0;
            input.ui.Mouse.ExtraInfo = extraInfo;
            return input;
        }

        static Input MouseDataWithoutMove(int flags, IntPtr extraInfo)
        {
            Input input = new Input();
            input.Type = 0; // MOUSE = 0
            input.ui.Mouse.Flags = flags;
            input.ui.Mouse.Data = 0;
            input.ui.Mouse.X = 0;
            input.ui.Mouse.Y = 0;
            input.ui.Mouse.Time = 0;
            input.ui.Mouse.ExtraInfo = extraInfo;
            return input;
        }

        static Input MouseDataWheel(int flags, int wheel, IntPtr extraInfo)
        {
            Input input = new Input();
            input.Type = 0; // MOUSE = 0
            input.ui.Mouse.Flags = flags;
            input.ui.Mouse.Data = wheel;
            input.ui.Mouse.X = 0;
            input.ui.Mouse.Y = 0;
            input.ui.Mouse.Time = 0;
            input.ui.Mouse.ExtraInfo = extraInfo;
            return input;
        }

        public static void SendMouseMove(int x, int y)
        {
            var pt = GetCursorPosition();
            x += (int)pt.X;
            y += (int)pt.Y;
            NativeMethods.SetCursorPos(x, y);
        }

        public static void SendMouseDown()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_LEFTDOWN, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseUp()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_LEFTUP, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseDownR()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_RIGHTDOWN, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseUpR()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_RIGHTUP, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseDownM()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_MIDDLEDOWN, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseUpM()
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWithoutMove(MOUSEEVENTF_MIDDLEUP, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseWheel(int wheel)
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWheel(MOUSEEVENTF_WHEEL, wheel, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
        public static void SendMouseHWheel(int wheel)
        {
            IntPtr extraInfo = NativeMethods.GetMessageExtraInfo();
            Input inputs = MouseDataWheel(MOUSEEVENTF_HWHEEL, wheel, extraInfo);
            int ret = NativeMethods.SendInput(1, ref inputs, Marshal.SizeOf(inputs));
        }
    }
}
