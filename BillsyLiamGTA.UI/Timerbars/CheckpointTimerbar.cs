using GTA.Native;
using System.Drawing;
using System.Collections.Generic;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class CheckpointTimerbar : BaseTimerbar
    {
        /// <summary>
        /// A enum containing the possible checkpoint states.
        /// </summary>
        public enum eState
        { 
            Default = 0,
            InProgress = 1,
            Failed = 2
        }
        /// <summary>
        /// A struct containing essential data for the checkpoint.
        /// </summary>
        public struct Checkpoint
        {
            public Color Colour { get; set; }

            public Color ProgressColour { get; set; }

            public Color FailedColour { get; set; }

            public eState State { get; set; }

            public Checkpoint(Color colour, Color progressColour, Color failedColour, eState state)
            {
                Colour = colour;
                ProgressColour = progressColour;
                FailedColour = failedColour;
                State = state;
            }

            public void Draw(float x, float y)
            {
                Color suitableColor = State == eState.Default ? Colour : (State == eState.InProgress ? ProgressColour : FailedColour);
                Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints", x, y, checkpointWidth, checkpointHeight, 0, suitableColor.R, suitableColor.G, suitableColor.B, 255, false, 0);
            }
        }

        #region Properties

        public List<Checkpoint> Checkpoints { get; set; }

        #endregion

        #region Constructors

        public CheckpointTimerbar(string text, List<Checkpoint> checkpoints) : base(text, true)
        {
            Checkpoints = checkpoints;
        }

        #endregion

        #region Functions

        public override void Draw(float y)
        {
            base.Draw(y);
            y += checkpointOffsetY;
            float cpX = checkpointBaseX;
            if (Checkpoints?.Count > 0)
            {
                for (int i = 0; i < Checkpoints.Count; i++)
                {
                    Checkpoints[i].Draw(cpX, y);
                    cpX -= checkpointOffsetX;
                }
            }
        }

        #endregion
    }
}