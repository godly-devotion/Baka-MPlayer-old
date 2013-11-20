using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Baka_MPlayer.Controls
{
    /// <summary>
    /// Encapsulates control that visualy displays certain integer value and allows user to change it within desired range. It imitates <see cref="System.Windows.Forms.TrackBar"/> as far as mouse usage is concerned.
    /// </summary>
    [ToolboxBitmap(typeof(TrackBar))]
    [DefaultEvent("Scroll"), DefaultProperty("BarColor")]
    public partial class ColorSlider : Control
    {
        #region Events

        /// <summary>
        /// Fires when Slider position has changed
        /// </summary>
        [Description("Event fires when the Value property changes")]
        [Category("Action")]
        public event EventHandler ValueChanged;

        /// <summary>
        /// Fires when user scrolls the Slider
        /// </summary>
        [Description("Event fires when the Slider position is changed")]
        [Category("Behavior")]
        public event ScrollEventHandler Scroll;

        #endregion

        #region Properties

        private Rectangle thumbRect; //bounding rectangle of thumb area
        /// <summary>
        /// Gets the thumb rect. Usefull to determine bounding rectangle when creating custom thumb shape.
        /// </summary>
        /// <value>The thumb rect.</value>
        [Browsable(false)]
        public Rectangle ThumbRect
        {
            get { return thumbRect; }
        }

        private Rectangle barRect; //bounding rectangle of bar area
        private Rectangle thumbHalfRect;
        private Rectangle elapsedRect; //bounding rectangle of elapsed area

        private Size thumbSize = new Size(15, 15);
        /// <summary>
        /// Gets or sets the size of the thumb.
        /// </summary>
        /// <value>The size of the thumb.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is lower than zero or grather than half of appropiate dimension</exception>
        [Description("Set Slider thumb size")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Size), "15, 15")]
        public Size ThumbSize
        {
            get { return thumbSize; }
            set
            {
                if (value.Width > 0 && value.Height > 0)
                    thumbSize = value;
                else
                    throw new ArgumentOutOfRangeException(
                        "TrackSize has to be greather than zero and lower than half of Slider width");
                Invalidate();
            }
        }

        private GraphicsPath thumbCustomShape = null;
        /// <summary>
        /// Gets or sets the thumb custom shape. Use ThumbRect property to determine bounding rectangle.
        /// </summary>
        /// <value>The thumb custom shape. null means default shape</value>
        [Description("Set Slider's thumb's custom shape")]
        [Category("ColorSlider")]
        [Browsable(false)]
        [DefaultValue(typeof(GraphicsPath), "null")]
        public GraphicsPath ThumbCustomShape
        {
            get { return thumbCustomShape; }
            set
            {
                thumbCustomShape = value;
                thumbSize.Width = (int)value.GetBounds().Width + 1;
                Invalidate();
            }
        }

        private Size thumbRoundRectSize = new Size(8, 8);
        /// <summary>
        /// Gets or sets the size of the thumb round rectangle edges.
        /// </summary>
        /// <value>The size of the thumb round rectangle edges.</value>
        [Description("Set Slider's thumb round rect size")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Size), "8; 8")]
        public Size ThumbRoundRectSize
        {
            get { return thumbRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                thumbRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private Size borderRoundRectSize = new Size(8, 8);
        /// <summary>
        /// Gets or sets the size of the border round rect.
        /// </summary>
        /// <value>The size of the border round rect.</value>
        [Description("Set Slider's border round rect size")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Size), "8; 8")]
        public Size BorderRoundRectSize
        {
            get { return borderRoundRectSize; }
            set
            {
                int h = value.Height, w = value.Width;
                if (h <= 0) h = 1;
                if (w <= 0) w = 1;
                borderRoundRectSize = new Size(w, h);
                Invalidate();
            }
        }

        private int trackerValue = 50;
        /// <summary>
        /// Gets or sets the value of Slider.
        /// </summary>
        /// <value>The value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when value is outside appropriate range (min, max)</exception>
        [Description("Set Slider value")]
        [Category("ColorSlider")]
        [DefaultValue(50)]
        public int Value
        {
            get { return trackerValue; }
            set
            {
                if (value >= barMinimum & value <= barMaximum)
                {
                    trackerValue = value;
                    if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    Invalidate();
                }
                else
                {
                    Console.WriteLine("Value is outside appropriate range (min, max)");
                    //throw new ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
                }
            }
        }

        private int barMinimum = 0;
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when minimal value is greather than maximal one</exception>
        [Description("Set Slider minimal point")]
        [Category("ColorSlider")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return barMinimum; }
            set
            {
                if (value < barMaximum)
                {
                    barMinimum = value;
                    if (trackerValue < barMinimum)
                    {
                        trackerValue = barMinimum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Minimal value is greather than maximal one");
            }
        }

        private int barMaximum = 100;
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        /// <exception cref="T:System.ArgumentOutOfRangeException">exception thrown when maximal value is lower than minimal one</exception>
        [Description("Set Slider maximal point")]
        [Category("ColorSlider")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return barMaximum; }
            set
            {
                if (value > barMinimum)
                {
                    barMaximum = value;
                    if (trackerValue > barMaximum)
                    {
                        trackerValue = barMaximum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                else throw new ArgumentOutOfRangeException("Maximal value is lower than minimal one");
            }
        }

        private uint smallChange = 1;
        /// <summary>
        /// Gets or sets trackbar's small change. It affects how to behave when directional keys are pressed
        /// </summary>
        /// <value>The small change value.</value>
        [Description("Set trackbar's small change")]
        [Category("ColorSlider")]
        [DefaultValue(1)]
        public uint SmallChange
        {
            get { return smallChange; }
            set { smallChange = value; }
        }

        private uint largeChange = 5;

        /// <summary>
        /// Gets or sets trackbar's large change. It affects how to behave when PageUp/PageDown keys are pressed
        /// </summary>
        /// <value>The large change value.</value>
        [Description("Set trackbar's large change")]
        [Category("ColorSlider")]
        [DefaultValue(5)]
        public uint LargeChange
        {
            get { return largeChange; }
            set { largeChange = value; }
        }

        private bool drawSemitransparentThumb = true;
        /// <summary>
        /// Gets or sets a value indicating whether to draw semitransparent thumb.
        /// </summary>
        /// <value><c>true</c> if semitransparent thumb should be drawn; otherwise, <c>false</c>.</value>
        [Description("Set whether to draw semitransparent thumb")]
        [Category("ColorSlider")]
        [DefaultValue(true)]
        public bool DrawSemitransparentThumb
        {
            get { return drawSemitransparentThumb; }
            set
            {
                drawSemitransparentThumb = value;
                Invalidate();
            }
        }

        private Color thumbFirstColor = Color.White;
        /// <summary>
        /// Gets or sets the first thumb color.
        /// </summary>
        /// <value>The first thumb color.</value>
        [Description("Set first thumb color on Slider")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "White")]
        public Color ThumbFirstColor
        {
            get { return thumbFirstColor; }
            set
            {
                thumbFirstColor = value;
                Invalidate();
            }
        }

        private Color thumbSecondColor = Color.Gainsboro;
        /// <summary>
        /// Gets or sets the second thumb color.
        /// </summary>
        /// <value>The second thumb color.</value>
        [Description("Set second thumb color on Slider")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color ThumbSecondColor
        {
            get { return thumbSecondColor; }
            set
            {
                thumbSecondColor = value;
                Invalidate();
            }
        }

        private Color thumbBorderColor = Color.Silver;
        /// <summary>
        /// Gets or sets the border color of the thumb.
        /// </summary>
        /// <value>The color border of the thumb.</value>
        [Description("Set Slider thumb pen color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color ThumbBorderColor
        {
            get { return thumbBorderColor; }
            set
            {
                thumbBorderColor = value;
                Invalidate();
            }
        }

        private int barHeight = 2;
        /// <summary>
        /// Gets or sets the bar height.
        /// </summary>
        /// <value>The size of the bar height.</value>
        [Description("Set Slider's bar height")]
        [Category("ColorSlider")]
        [DefaultValue(2)]
        public int BarHeight
        {
            get { return barHeight; }
            set
            {
                barHeight = value;
                Invalidate();
            }
        }

        private Color barColor = Color.Gray;
        /// <summary>
        /// Gets or sets the bar color.
        /// </summary>
        /// <value>The color of the bar.</value>
        [Description("Set Slider's bar color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color BarColor
        {
            get { return barColor; }
            set
            {
                barColor = value;
                Invalidate();
            }
        }

        private Color elapsedBarColor = Color.DodgerBlue;
        /// <summary>
        /// Gets or sets the elapsed color.
        /// </summary>
        /// <value>The color of the elapsed.</value>
        [Description("Set Slider's elapsed color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "DodgerBlue")]
        public Color ElapsedBarColor
        {
            get { return elapsedBarColor; }
            set
            {
                elapsedBarColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="value">The current value.</param>
        public ColorSlider(int min, int max, int value)
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor | ControlStyles.UserMouse |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;

            Minimum = min;
            Maximum = max;
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> class.
        /// </summary>
        public ColorSlider() : this(0, 100, 50) { }

        #endregion

        #region Functions

        private List<long> marks = new List<long>();
        private double maxMarkValue;

        /// <summary>
        /// Creates marks on the bar
        /// </summary>
        public void AddMarks(List<long> marks, double maxMarkValue)
        {
            this.marks = marks;
            this.maxMarkValue = maxMarkValue;
            Invalidate();
        }

        public void ClearMarks()
        {
            marks.Clear();
            Invalidate();
        }

        #endregion

        #region Paint

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Enabled)
            {
                Color[] desaturatedColors = DesaturateColors(thumbFirstColor, thumbSecondColor, thumbBorderColor,
                                                             barColor, elapsedBarColor);
                DrawColorSlider(e, desaturatedColors[0], desaturatedColors[1], desaturatedColors[2],
                                desaturatedColors[3], desaturatedColors[4]);
            }
            else
            {
                DrawColorSlider(e, thumbFirstColor, thumbSecondColor, thumbBorderColor,
                                barColor, elapsedBarColor);
            }
        }

        /// <summary>
        /// Draws the colorslider control using passed colors.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        /// <param name="thumbFirstColorPaint">The first color of the thumb.</param>
        /// <param name="thumbSecondColorPaint">The second color of the thumb.</param>
        /// <param name="thumbBorderColorPaint">The thumb border color.</param>
        /// <param name="barColorPaint">The bar color paint.</param>
        /// <param name="elapsedBarColorPaint">The elapsed color paint.</param>
        private void DrawColorSlider(PaintEventArgs e, Color thumbFirstColorPaint, Color thumbSecondColorPaint,
                                     Color thumbBorderColorPaint, Color barColorPaint, Color elapsedBarColorPaint)
        {
            try
            {
                // set up thumbRect aproprietly
                int TrackX = (((trackerValue - barMinimum) * (ClientRectangle.Width - thumbSize.Width)) / (barMaximum - barMinimum));
                thumbRect = new Rectangle(TrackX, (ClientRectangle.Height - thumbSize.Height) / 2, thumbSize.Width - 1, thumbSize.Height);

                // get thumb shape path 
                GraphicsPath thumbPath;
                if (thumbCustomShape == null)
                {
                    thumbPath = CreateRoundRectPath(thumbRect, thumbRoundRectSize);
                }
                else
                {
                    thumbPath = thumbCustomShape;
                    var m = new Matrix();
                    m.Translate(thumbRect.Left - thumbPath.GetBounds().Left, thumbRect.Top - thumbPath.GetBounds().Top);
                    thumbPath.Transform(m);
                }

                thumbHalfRect = thumbRect;
                thumbHalfRect.Height /= 2;

                // draw bar

                // adjust drawing rects
                barRect = ClientRectangle;
                
                barRect.Inflate(0, (barHeight - barRect.Height) / 2);
                elapsedRect = barRect;
                elapsedRect.Width = thumbRect.Left + thumbSize.Width / 2;

                using (var barBrush = new SolidBrush(barColorPaint))
                {
                    e.Graphics.FillRectangle(barBrush, barRect);

                    // draw elapsed bar
                    using (var elapsedBrush = new SolidBrush(elapsedBarColorPaint))
                    {
                        if (Capture && drawSemitransparentThumb)
                        {
                            var elapsedReg = new Region(elapsedRect);
                            elapsedReg.Exclude(thumbPath);
                            e.Graphics.FillRegion(elapsedBrush, elapsedReg);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(elapsedBrush, elapsedRect);
                        }
                    }

                    // create tick marks (for chapter marks)
                    foreach (var m in marks)
                    {
                        var x = (m * this.ClientRectangle.Width) / maxMarkValue;
                        var r = new Rectangle((int)x, barRect.Y, barRect.Height, barRect.Height);
                        r.Inflate(0, 2);

                        e.Graphics.FillRectangle(barBrush, r);
                    }
                }

                // draw thumb
                Color newthumbFirstColorPaint = thumbFirstColorPaint, newthumbSecondColorPaint = thumbSecondColorPaint;
                if (Capture && drawSemitransparentThumb)
                {
                    newthumbFirstColorPaint = Color.FromArgb(175, thumbFirstColorPaint);
                    newthumbSecondColorPaint = Color.FromArgb(175, thumbSecondColorPaint);
                }
                using (var lgbThumb = new LinearGradientBrush(thumbRect,
                    newthumbFirstColorPaint, newthumbSecondColorPaint, LinearGradientMode.Vertical))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(lgbThumb, thumbPath);
                    // draw thumb border
                    Color newThumbBorderColor = thumbBorderColorPaint;

                    using (var thumbPen = new Pen(newThumbBorderColor))
                    {
                        e.Graphics.DrawPath(thumbPen, thumbPath);
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("ColorSlider.cs: DrawBackground Error in {0}: {1}", Name, err.Message);
            }
        }

        #endregion

        #region Overrided events

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                Capture = true;
                if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.ThumbTrack, trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
                OnMouseMove(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Capture & e.Button == MouseButtons.Left)
            {
                ScrollEventType set = ScrollEventType.ThumbPosition;
                Point pt = e.Location;
                int margin = thumbSize.Width >> 1;
                int p = pt.X - margin;
                float coef = (float)(barMaximum - barMinimum) /
                             (ClientSize.Width - 2 * margin);
                trackerValue = (int)(p * coef + barMinimum);

                if (trackerValue <= barMinimum)
                {
                    trackerValue = barMinimum;
                    set = ScrollEventType.First;
                }
                else if (trackerValue >= barMaximum)
                {
                    trackerValue = barMaximum;
                    set = ScrollEventType.Last;
                }

                if (Scroll != null) Scroll(this, new ScrollEventArgs(set, trackerValue));
                if (ValueChanged != null) ValueChanged(this, new EventArgs());
            }
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Capture = false;
            if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.EndScroll, trackerValue));
            if (ValueChanged != null) ValueChanged(this, new EventArgs());
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                Capture = false;
            }
        }

        #endregion

        #region Help routines

        /// <summary>
        /// Creates the round rect path.
        /// </summary>
        /// <param name="rect">The rectangle on which graphics path will be spanned.</param>
        /// <param name="size">The size of rounded rectangle edges.</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundRectPath(Rectangle rect, Size size)
        {
            var gp = new GraphicsPath();
            gp.AddLine(rect.Left + size.Width / 2, rect.Top, rect.Right - size.Width / 2, rect.Top);
            gp.AddArc(rect.Right - size.Width, rect.Top, size.Width, size.Height, 270, 90);

            gp.AddLine(rect.Right, rect.Top + size.Height / 2, rect.Right, rect.Bottom - size.Width / 2);
            gp.AddArc(rect.Right - size.Width, rect.Bottom - size.Height, size.Width, size.Height, 0, 90);

            gp.AddLine(rect.Right - size.Width / 2, rect.Bottom, rect.Left + size.Width / 2, rect.Bottom);
            gp.AddArc(rect.Left, rect.Bottom - size.Height, size.Width, size.Height, 90, 90);

            gp.AddLine(rect.Left, rect.Bottom - size.Height / 2, rect.Left, rect.Top + size.Height / 2);
            gp.AddArc(rect.Left, rect.Top, size.Width, size.Height, 180, 90);
            return gp;
        }

        /// <summary>
        /// Desaturates colors from given array.
        /// </summary>
        /// <param name="colorsToDesaturate">The colors to be desaturated.</param>
        /// <returns></returns>
        public static Color[] DesaturateColors(params Color[] colorsToDesaturate)
        {
            var colorsToReturn = new Color[colorsToDesaturate.Length];
            for (int i = 0; i < colorsToDesaturate.Length; i++)
            {
                //use NTSC weighted avarage
                var gray =
                    (int)(colorsToDesaturate[i].R * 0.3 + colorsToDesaturate[i].G * 0.6 + colorsToDesaturate[i].B * 0.1);
                colorsToReturn[i] = Color.FromArgb(-0x010101 * (255 - gray) - 1);
            }
            return colorsToReturn;
        }

        /// <summary>
        /// Lightens colors from given array.
        /// </summary>
        /// <param name="colorsToLighten">The colors to lighten.</param>
        /// <returns></returns>
        public static Color[] LightenColors(params Color[] colorsToLighten)
        {
            var colorsToReturn = new Color[colorsToLighten.Length];
            for (int i = 0; i < colorsToLighten.Length; i++)
            {
                colorsToReturn[i] = ControlPaint.Light(colorsToLighten[i]);
            }
            return colorsToReturn;
        }

        #endregion
    }
}
