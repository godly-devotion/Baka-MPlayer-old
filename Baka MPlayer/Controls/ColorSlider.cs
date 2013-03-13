using System;
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
    [DefaultEvent("Scroll"), DefaultProperty("BarInnerColor")]
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
        private Rectangle barHalfRect;
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
                    Console.WriteLine("Value is outside appropriate range (min, max)");
                //throw new ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
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

        private bool mouseEffects = true;
        /// <summary>
        /// Gets or sets whether mouse entry and exit actions have impact on how control look.
        /// </summary>
        /// <value><c>true</c> if mouse entry and exit actions have impact on how control look; otherwise, <c>false</c>.</value>
        [Description("Set whether mouse entry and exit actions have impact on how control look")]
        [Category("ColorSlider")]
        [DefaultValue(true)]
        public bool MouseEffects
        {
            get { return mouseEffects; }
            set
            {
                mouseEffects = value;
                Invalidate();
            }
        }

        private Color thumbOuterColor = Color.White;
        /// <summary>
        /// Gets or sets the thumb outer color .
        /// </summary>
        /// <value>The thumb outer color.</value>
        [Description("Set Slider thumb outer color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "White")]
        public Color ThumbOuterColor
        {
            get { return thumbOuterColor; }
            set
            {
                thumbOuterColor = value;
                Invalidate();
            }
        }


        private Color thumbInnerColor = Color.Gainsboro;
        /// <summary>
        /// Gets or sets the inner color of the thumb.
        /// </summary>
        /// <value>The inner color of the thumb.</value>
        [Description("Set Slider thumb inner color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color ThumbInnerColor
        {
            get { return thumbInnerColor; }
            set
            {
                thumbInnerColor = value;
                Invalidate();
            }
        }


        private Color thumbPenColor = Color.Silver;
        /// <summary>
        /// Gets or sets the color of the thumb pen.
        /// </summary>
        /// <value>The color of the thumb pen.</value>
        [Description("Set Slider thumb pen color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Silver")]
        public Color ThumbPenColor
        {
            get { return thumbPenColor; }
            set
            {
                thumbPenColor = value;
                Invalidate();
            }
        }


        private Color barOuterColor = Color.SkyBlue;
        /// <summary>
        /// Gets or sets the outer color of the bar.
        /// </summary>
        /// <value>The outer color of the bar.</value>
        [Description("Set Slider bar outer color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "SkyBlue")]
        public Color BarOuterColor
        {
            get { return barOuterColor; }
            set
            {
                barOuterColor = value;
                Invalidate();
            }
        }


        private Color barInnerColor = Color.DarkSlateBlue;
        /// <summary>
        /// Gets or sets the inner color of the bar.
        /// </summary>
        /// <value>The inner color of the bar.</value>
        [Description("Set Slider bar inner color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "DarkSlateBlue")]
        public Color BarInnerColor
        {
            get { return barInnerColor; }
            set
            {
                barInnerColor = value;
                Invalidate();
            }
        }


        private Color barPenColor = Color.Gainsboro;
        /// <summary>
        /// Gets or sets the color of the bar pen.
        /// </summary>
        /// <value>The color of the bar pen.</value>
        [Description("Set Slider bar pen color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color BarPenColor
        {
            get { return barPenColor; }
            set
            {
                barPenColor = value;
                Invalidate();
            }
        }

        private Color elapsedOuterColor = Color.DarkGreen;
        /// <summary>
        /// Gets or sets the outer color of the elapsed.
        /// </summary>
        /// <value>The outer color of the elapsed.</value>
        [Description("Set Slider's elapsed part outer color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "DarkGreen")]
        public Color ElapsedOuterColor
        {
            get { return elapsedOuterColor; }
            set
            {
                elapsedOuterColor = value;
                Invalidate();
            }
        }

        private Color elapsedInnerColor = Color.Chartreuse;
        /// <summary>
        /// Gets or sets the inner color of the elapsed.
        /// </summary>
        /// <value>The inner color of the elapsed.</value>
        [Description("Set Slider's elapsed part inner color")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(Color), "Chartreuse")]
        public Color ElapsedInnerColor
        {
            get { return elapsedInnerColor; }
            set
            {
                elapsedInnerColor = value;
                Invalidate();
            }
        }

        private bool solidBackgroundColor = false;
        /// <summary>
        /// Gets or sets whether or not the background will have a solid color.
        /// </summary>
        /// <value>True for solid color, False for gradient color.</value>
        [Description("Uses a solid background color (Note: uses BarInnerColor as a solid color)")]
        [Category("ColorSlider")]
        [DefaultValue(typeof(bool), "false")]
        public bool SolidBackground
        {
            get { return solidBackgroundColor; }
            set { solidBackgroundColor = value; Invalidate(); }
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

        #region Paint

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Enabled)
            {
                Color[] desaturatedColors = DesaturateColors(thumbOuterColor, thumbInnerColor, thumbPenColor,
                                                             barOuterColor, barInnerColor, barPenColor,
                                                             elapsedOuterColor, elapsedInnerColor);
                DrawColorSlider(e, desaturatedColors[0], desaturatedColors[1], desaturatedColors[2],
                                desaturatedColors[3],
                                desaturatedColors[4], desaturatedColors[5], desaturatedColors[6], desaturatedColors[7]);
            }
            else
            {
                if (mouseEffects && mouseInRegion)
                {
                    Color[] lightenedColors = LightenColors(thumbOuterColor, thumbInnerColor, thumbPenColor,
                                                            barOuterColor, barInnerColor, barPenColor,
                                                            elapsedOuterColor, elapsedInnerColor);
                    DrawColorSlider(e, lightenedColors[0], lightenedColors[1], lightenedColors[2], lightenedColors[3],
                                    lightenedColors[4], lightenedColors[5], lightenedColors[6], lightenedColors[7]);
                }
                else
                {
                    DrawColorSlider(e, thumbOuterColor, thumbInnerColor, thumbPenColor,
                                    barOuterColor, barInnerColor, barPenColor,
                                    elapsedOuterColor, elapsedInnerColor);
                }
            }
        }

        /// <summary>
        /// Draws the colorslider control using passed colors.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
        /// <param name="thumbOuterColorPaint">The thumb outer color paint.</param>
        /// <param name="thumbInnerColorPaint">The thumb inner color paint.</param>
        /// <param name="thumbPenColorPaint">The thumb pen color paint.</param>
        /// <param name="barOuterColorPaint">The bar outer color paint.</param>
        /// <param name="barInnerColorPaint">The bar inner color paint.</param>
        /// <param name="barPenColorPaint">The bar pen color paint.</param>
        /// <param name="elapsedOuterColorPaint">The elapsed outer color paint.</param>
        /// <param name="elapsedInnerColorPaint">The elapsed inner color paint.</param>
        private void DrawColorSlider(PaintEventArgs e, Color thumbOuterColorPaint, Color thumbInnerColorPaint,
                                     Color thumbPenColorPaint, Color barOuterColorPaint, Color barInnerColorPaint,
                                     Color barPenColorPaint, Color elapsedOuterColorPaint, Color elapsedInnerColorPaint)
        {
            try
            {
                // set up thumbRect aproprietly
                int TrackX = (((trackerValue - barMinimum) * (ClientRectangle.Width - thumbSize.Width)) / (barMaximum - barMinimum));
                thumbRect = new Rectangle(TrackX, (ClientRectangle.Height - thumbSize.Height) / 2, thumbSize.Width - 1, thumbSize.Height);

                // get thumb shape path 
                GraphicsPath thumbPath;
                if (thumbCustomShape == null)
                    thumbPath = CreateRoundRectPath(thumbRect, thumbRoundRectSize);
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
                if (solidBackgroundColor)
                {
                    var barBrush = new SolidBrush(BarInnerColor);
                    e.Graphics.FillRectangle(barBrush, 0, Convert.ToInt32((7 * ClientRectangle.Height) / 16.0), ClientRectangle.Width, Convert.ToInt32(ClientRectangle.Height / 8.0));
                }
                else
                {
                    // adjust drawing rects
                    barRect = ClientRectangle;

                    barRect.Inflate(-1, -barRect.Height / 3);
                    barHalfRect = barRect;
                    barHalfRect.Height /= 2;
                    elapsedRect = barRect;
                    elapsedRect.Width = thumbRect.Left + thumbSize.Width / 2;

                    using (var lgbBar = new LinearGradientBrush(barHalfRect,
                        barOuterColorPaint, barInnerColorPaint, LinearGradientMode.Vertical))
                    {
                        lgbBar.WrapMode = WrapMode.TileFlipXY;
                        e.Graphics.FillRectangle(lgbBar, barRect);
                        // draw elapsed bar
                        using (var lgbElapsed = new LinearGradientBrush(barHalfRect,
                            elapsedOuterColorPaint, elapsedInnerColorPaint, LinearGradientMode.Vertical))
                        {
                            lgbElapsed.WrapMode = WrapMode.TileFlipXY;
                            if (Capture && drawSemitransparentThumb)
                            {
                                var elapsedReg = new Region(elapsedRect);
                                elapsedReg.Exclude(thumbPath);
                                e.Graphics.FillRegion(lgbElapsed, elapsedReg);
                            }
                            else
                                e.Graphics.FillRectangle(lgbElapsed, elapsedRect);
                        }
                        // draw bar band                    
                        using (var barPen = new Pen(barPenColorPaint, 0.5f))
                        {
                            e.Graphics.DrawRectangle(barPen, barRect);
                        }
                    }
                }

                // draw thumb
                Color newthumbOuterColorPaint = thumbOuterColorPaint, newthumbInnerColorPaint = thumbInnerColorPaint;
                if (Capture && drawSemitransparentThumb)
                {
                    newthumbOuterColorPaint = Color.FromArgb(175, thumbOuterColorPaint);
                    newthumbInnerColorPaint = Color.FromArgb(175, thumbInnerColorPaint);
                }
                using (var lgbThumb = new LinearGradientBrush(thumbHalfRect,
                    newthumbOuterColorPaint, newthumbInnerColorPaint, LinearGradientMode.Vertical))
                {
                    lgbThumb.WrapMode = WrapMode.TileFlipXY;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.FillPath(lgbThumb, thumbPath);
                    // draw thumb band
                    Color newThumbPenColor = thumbPenColorPaint;
                    if (mouseEffects && (Capture || mouseInThumbRegion))
                        newThumbPenColor = ControlPaint.Dark(newThumbPenColor);
                    using (var thumbPen = new Pen(newThumbPenColor))
                    {
                        e.Graphics.DrawPath(thumbPen, thumbPath);
                    }
                }
            }
            catch (Exception Err)
            {
                Console.WriteLine(string.Format("DrawBackground Error in {0}: {1}", Name, Err.Message));
            }
        }

        #endregion

        #region Overided events

        private bool mouseInRegion;
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseInRegion = true;
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInRegion = false;
            mouseInThumbRegion = false;
            Invalidate();
        }

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

        private bool mouseInThumbRegion = false;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
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
            mouseInThumbRegion = IsPointInRect(e.Location, thumbRect);
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

        /*/// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.KeyUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Left:
                    SetProperValue(Value - (int)smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallDecrement, Value));
                    break;
                case Keys.Up:
                case Keys.Right:
                    SetProperValue(Value + (int)smallChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.SmallIncrement, Value));
                    break;
                case Keys.Home:
                    Value = barMinimum;
                    break;
                case Keys.End:
                    Value = barMaximum;
                    break;
                case Keys.PageDown:
                    SetProperValue(Value - (int)largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeDecrement, Value));
                    break;
                case Keys.PageUp:
                    SetProperValue(Value + (int)largeChange);
                    if (Scroll != null) Scroll(this, new ScrollEventArgs(ScrollEventType.LargeIncrement, Value));
                    break;
            }
            if (Scroll != null && Value == barMinimum) Scroll(this, new ScrollEventArgs(ScrollEventType.First, Value));
            if (Scroll != null && Value == barMaximum) Scroll(this, new ScrollEventArgs(ScrollEventType.Last, Value));
            Point pt = PointToClient(Cursor.Position);
            OnMouseMove(new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0));
        }*/

        /*/// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
        /// <returns>
        /// true if the key was processed by the control; otherwise, false.
        /// </returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Tab | ModifierKeys == Keys.Shift)
                return base.ProcessDialogKey(keyData);
            else
            {
                OnKeyDown(new KeyEventArgs(keyData));
                return true;
            }
        }*/

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

        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(int val)
        {
            if (val < barMinimum) Value = barMinimum;
            else if (val > barMaximum) Value = barMaximum;
            else Value = val;
        }

        /// <summary>
        /// Determines whether rectangle contains given point.
        /// </summary>
        /// <param name="pt">The point to test.</param>
        /// <param name="rect">The base rectangle.</param>
        /// <returns>
        /// 	<c>true</c> if rectangle contains given point; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPointInRect(Point pt, Rectangle rect)
        {
            if (pt.X > rect.Left & pt.X < rect.Right & pt.Y > rect.Top & pt.Y < rect.Bottom)
                return true;
            return false;
        }

        #endregion
    }
}
