using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;

namespace Zen_CSGO_Hack
{
    public partial class ESP_Overlay : Form
    {
        private WindowRenderTarget _device;
        private HwndRenderTargetProperties _renderProperties;
        private Factory _factory;

        private SolidColorBrush _brush;

        private TextFormat _font;
        private FontFactory _fontFactory;
        private const string FontFamily = "Arial";
        private const float FontSize = 12.0f;

        private Thread _sharpDxThread;

        public ESP_Overlay()
        {
            InitializeComponent();
        }

        private void ESP_Load(object sender, EventArgs e)
        {
            //This way we can click through the window without any problem
            var initialStyle = PinvokeWrapper.GetWindowLng(this.Handle, -20);
            PinvokeWrapper.SetWindowLng(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                     ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);

            _factory = new Factory();
            _fontFactory = new FontFactory();

            _renderProperties = new HwndRenderTargetProperties
            {
                Hwnd = Handle,
                PixelSize = new SharpDX.Size2(Size.Width, Size.Height),
                PresentOptions = PresentOptions.None
            };

            // Initialize DirectX
            _device = new WindowRenderTarget(_factory, new RenderTargetProperties(new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), _renderProperties);

            _brush = new SolidColorBrush(_device, new RawColor4(255, 0, 0, 1f));

            // Initialize Fonts
            _font = new TextFormat(_fontFactory, FontFamily, FontSize);

            _sharpDxThread = new Thread(DirectXThread)
            {
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };

            _sharpDxThread.Start();
        }

        private void DirectXThread(object obj)
        {
            while (Options.EspIsEnabled)
            {
                _device.BeginDraw();

                _device.Clear(new RawColor4(Color.Transparent.R, Color.Transparent.G, Color.Transparent.B,
                    Color.Transparent.A));
                _device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Aliased;

                //Get all players in the game
                var players = Utils.GetPlayers();

                //Get our local player
                var localPlayer = Utils.GetLocalPlayer();

                //If there aren't any players, we continue looping
                if (players.Count == 0)
                    continue;

                foreach (var player in players)
                {
                    //If the player is dead, or it's in our team or is dormant, it's not a valid one, so we continue looping
                    if (player.GetHealth() == 0 || player.GetTeam() == localPlayer.GetTeam() || player.IsDormant())
                        continue;
                     
                    //We get the screen coordinates for the player's right foot
                    var rightFoot = player.GetBonePos((int)Utils.PlayerBones.RightFoot);
                    //We get the screen coordinates for the player's left foot
                    var leftFoot = player.GetBonePos((int)Utils.PlayerBones.LeftFoot);
                    //We get the screen coordinates for the player's head
                    var head = player.GetBonePos((int)Utils.PlayerBones.Head);

                    //Bottom line
                    _device.DrawLine(new RawVector2(rightFoot.X, rightFoot.Y), new RawVector2(leftFoot.X, leftFoot.Y), _brush, 2f);
                    //Top line
                    _device.DrawLine(new RawVector2(rightFoot.X, head.Y), new RawVector2(leftFoot.X, head.Y), _brush, 2f);
                    //Right line
                    _device.DrawLine(new RawVector2(rightFoot.X, rightFoot.Y), new RawVector2(rightFoot.X, head.Y), _brush, 2f);
                    //Left line
                    _device.DrawLine(new RawVector2(leftFoot.X, leftFoot.Y), new RawVector2(leftFoot.X, head.Y), _brush, 2f);

                    //Health
                    _device.DrawTextLayout(new RawVector2(rightFoot.X, rightFoot.Y), new TextLayout(_fontFactory, "Health: " + player.GetHealth(), _font, 50, 12), _brush);
                    //Armor
                    _device.DrawTextLayout(new RawVector2(rightFoot.X, rightFoot.Y + 30), new TextLayout(_fontFactory, "Armor: " + player.GetArmor(), _font, 50, 12), _brush);
                }

                _device.EndDraw();

                Thread.Sleep(2);
            }
        }
    }
}
