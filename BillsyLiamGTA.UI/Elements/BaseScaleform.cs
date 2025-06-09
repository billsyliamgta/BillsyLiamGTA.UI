using System;
using System.Drawing;
using System.Threading.Tasks;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    public abstract class BaseScaleform
    {
        #region Fields

        public int Handle { get; private set; }

        public string MovieName { get; private set; }

        public bool IsLoaded
        {
            get
            {
                return Function.Call<bool>(Hash.HAS_SCALEFORM_MOVIE_LOADED, Handle);
            }
        }

        #endregion

        public BaseScaleform(string movieName)
        {
            MovieName = movieName;
        }

        #region Methods

        public void Load(int timeout = 5000)
        {
            int start = Game.GameTime;
            Handle = Function.Call<int>(Hash.REQUEST_SCALEFORM_MOVIE, MovieName);
            while (!IsLoaded)
            {
                if (Game.GameTime > start + timeout)
                {
                    throw new TimeoutException($"ERROR: Timed out while loading scaleform movie '{MovieName}'.");
                }
                Script.Wait(0);
            }
        }

        /// <summary>
        /// Set the scaleform movie to be no longer needed.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsLoaded)
            {
                unsafe
                {
                    int handle = Handle;
                    Function.Call(Hash.SET_SCALEFORM_MOVIE_AS_NO_LONGER_NEEDED, &handle);
                    Handle = 0;
                }
            }
        }

        public virtual void Draw()
        {
            if (IsLoaded)
            {
                Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN, Handle, 255, 255, 255, 255, 0);
            }
        }

        public virtual void DrawMasked(BaseScaleform secondScaleform)
        {
            if (IsLoaded)
            {
                Function.Call(Hash.DRAW_SCALEFORM_MOVIE_FULLSCREEN_MASKED, Handle, secondScaleform.Handle, 255, 255, 255, 255);
            }
        }

        public virtual void DrawAt(PointF position, SizeF size)
        {
            if (IsLoaded)
            {
                Function.Call(Hash.DRAW_SCALEFORM_MOVIE, Handle, position.X, position.Y, size.Width, size.Height, 255, 255, 255, 255, 0);
            }
        }

        /// <summary>
        /// Pushes scaleform movie functions onto the stack.
        /// </summary>
        /// <param name="functionName"></param>
        /// <param name="args"></param>
        public void CallFunction(string functionName, params object[] args) => Extensions.CallFunction(Handle, functionName, args);
        #endregion
    }
}