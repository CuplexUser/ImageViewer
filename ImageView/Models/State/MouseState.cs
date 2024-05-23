using System.Numerics;

namespace ImageViewer.Models.State
{
    /// <summary>
    /// MouseState class
    /// </summary>
    public class MouseState
    {
        /// <summary>
        /// Gets or sets the mouse position.
        /// </summary>
        /// <value>
        /// The mouse position.
        /// </value>
        public Point MousePosition { get; protected set; } = new();
        /// <summary>
        /// The mouse position previous
        /// </summary>
        private Point mousePositionPrev;
        /// <summary>
        /// Gets the last updated.
        /// </summary>
        /// <value>
        /// The last updated.
        /// </value>
        public DateTime LastUpdated { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [cursor visible].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [cursor visible]; otherwise, <c>false</c>.
        /// </value>
        public bool CursorVisible { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [automatic hide enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [automatic hide enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoHideEnabled { get; protected set; }
        /// <summary>
        /// The hover time
        /// </summary>
        private DateTime hoverTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseState"/> class.
        /// </summary>
        /// <param name="autoHideCursorDelay">The automatic hide cursor delay.</param>
        /// <param name="autoHideCursor">if set to <c>true</c> [automatic hide cursor].</param>
        public MouseState(int autoHideCursorDelay, bool autoHideCursor)
        {
            LastUpdated = DateTime.Now;
            hoverTime = DateTime.Now;
            mousePositionPrev = MousePosition;
            CursorVisible = true;
            AutoHideEnabled = autoHideCursor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseState"/> class.
        /// </summary>
        public MouseState()
        {
            LastUpdated = DateTime.Now;
            hoverTime = DateTime.Now;
            mousePositionPrev = MousePosition;
            CursorVisible = true;
            AutoHideEnabled = false;
        }

        /// <summary>
        /// Updates the state from settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void UpdateStateFromSettings(ApplicationSettingsModel settings)
        {
            AutoHideEnabled = settings.AutoHideCursor;
        }

        /// <summary>
        /// Gets the idle time.
        /// </summary>
        /// <returns></returns>
        public long GetIdleTime()
        {
            double diff = (DateTime.Now - LastUpdated).TotalMilliseconds;
            return Convert.ToInt64(diff);
        }

        /// <summary>
        /// Gets the hover time.
        /// </summary>
        /// <returns></returns>
        public int GetHoverTime()
        {
            if (hoverTime == LastUpdated)
                return 0;

            return Convert.ToInt32((hoverTime - LastUpdated).TotalMilliseconds);

        }

        /// <summary>
        /// Determines whether this instance has moved.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has moved; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMoved()
        {
            return MousePosition.X != mousePositionPrev.X || (MousePosition.X != mousePositionPrev.Y);

        }

        /// <summary>
        /// Updates the hover.
        /// </summary>
        public void UpdateHover()
        {
            hoverTime = DateTime.Now;
        }

        /// <summary>
        /// Updates the location.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void UpdateLocation(int x, int y)
        {
            mousePositionPrev = MousePosition;
            MousePosition = new Point(x, y);
            if (HasMoved())
            {
                LastUpdated = DateTime.Now;
                hoverTime = LastUpdated;
            }
        }
    }
}