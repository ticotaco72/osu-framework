using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osuTK;
using osuTK.Input;
using osuTK.Platform;
using osuTK.Platform.Android;

namespace osu.Framework.Platform.Android
{
    public class AndroidPlatformGameWindow : IGameWindow
    {
        private readonly AndroidGameView gameView;

        public AndroidPlatformGameWindow(AndroidGameView gameView)
        {
            this.gameView = gameView;

            gameView.Load += (o, e) => Load?.Invoke(o, e);
            gameView.Unload += (o, e) => Unload?.Invoke(o, e);
            gameView.UpdateFrame += (o, e) => UpdateFrame?.Invoke(o, e);
            gameView.RenderFrame += (o, e) => RenderFrame?.Invoke(o, e);
            gameView.Resize += (o, e) => Resize?.Invoke(o, e);
            gameView.Closed += (o, e) => Closed?.Invoke(o, e);
            gameView.Disposed += (o, e) => Disposed?.Invoke(o, e);
            gameView.TitleChanged += (o, e) => TitleChanged?.Invoke(o, e);
            gameView.VisibleChanged += (o, e) => VisibleChanged?.Invoke(o, e);
            gameView.WindowStateChanged += (o, e) => WindowStateChanged?.Invoke(o, e);
        }
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Focused => throw new NotImplementedException();

        public bool Visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Exists => throw new NotImplementedException();

        public IWindowInfo WindowInfo => throw new NotImplementedException();

        public WindowState WindowState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public WindowBorder WindowBorder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle Bounds { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point Location { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Size Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int X { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Rectangle ClientRectangle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Size ClientSize { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public MouseCursor Cursor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CursorVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool CursorGrabbed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<EventArgs> Load;
        public event EventHandler<EventArgs> Unload;
        public event EventHandler<FrameEventArgs> UpdateFrame;
        public event EventHandler<FrameEventArgs> RenderFrame;
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

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void MakeCurrent()
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

        public void ProcessEvents()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Run(double updateRate)
        {
            throw new NotImplementedException();
        }

        public void SwapBuffers()
        {
            throw new NotImplementedException();
        }
    }
}
