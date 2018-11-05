using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using osuTK;
using osuTK.Platform;
using Android.Opengl;
using osu.Framework.Platform.Android;
using myAndroidGraphics = Android.Graphics;
using osuTK.Input;
using System.ComponentModel;
using System.Drawing;

namespace osu.Framework.Platform.Android
{
    public class AndroidNativeGameWindow : AndroidGLSurfaceView, INativeWindow
    {
        public AndroidNativeGameWindow(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public AndroidNativeGameWindow(Context context) :
            base(context)
        {
            Initialize();
        }

        public event EventHandler<EventArgs> Move;
        public event EventHandler<EventArgs> Resize;
        public event EventHandler<CancelEventArgs> Closing;
        public event EventHandler<EventArgs> Closed;
        public event EventHandler<EventArgs> Disposed;
        public event EventHandler<EventArgs> IconChanged;
        public event EventHandler<EventArgs> TitleChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> FocusedChanged;
        public event EventHandler<EventArgs> WindowBorderChanged;
        public event EventHandler<EventArgs> WindowStateChanged;
        public event EventHandler<KeyboardKeyEventArgs> KeyDown;
        public event EventHandler<KeyPressEventArgs> KeyPress;
        public event EventHandler<KeyboardKeyEventArgs> KeyUp;
        public event EventHandler<EventArgs> MouseLeave;
        public event EventHandler<EventArgs> MouseEnter;
        public event EventHandler<MouseButtonEventArgs> MouseDown;
        public event EventHandler<MouseButtonEventArgs> MouseUp;
        public event EventHandler<MouseMoveEventArgs> MouseMove;
        public event EventHandler<MouseWheelEventArgs> MouseWheel;
        public event EventHandler<FileDropEventArgs> FileDrop;

        //this can't be changed to little letter cause of android keywords
        private void Initialize()
        {
            
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void ProcessEvents()
        {
            throw new NotImplementedException();
        }

        public Point PointToClient(Point point)
        {
            throw new NotImplementedException();
        }

        public Point PointToScreen(Point point)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int Width
        {
            get
            {
                //myAndroidGraphics.Point mysize = new myAndroidGraphics.Point();
                //base.GetDiplay().GetRealSize(mysize);
                //return mysize.X;
                return base.GetWidth();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Height
        {
            get
            {
                //myAndroidGraphics.Point mysize = new myAndroidGraphics.Point();
                //base.GetDiplay().GetRealSize(mysize);
                //return mysize.Y;
                return base.GetHeight();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public osuTK.Icon Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Focused => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Exists => throw new NotImplementedException();

        public IWindowInfo WindowInfo => throw new NotImplementedException();

        public WindowState WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WindowBorder WindowBorder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle Bounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public System.Drawing.Size Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int X { get => (int) base.GetX(); set => throw new NotImplementedException(); }
        public int Y { get => (int) base.GetY(); set => throw new NotImplementedException(); }
        public Rectangle ClientRectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public System.Drawing.Size ClientSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseCursor Cursor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CursorGrabbed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //osuTK.Icon INativeWindow.Icon { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
