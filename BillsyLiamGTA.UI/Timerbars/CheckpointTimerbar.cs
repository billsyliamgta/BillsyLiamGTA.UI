using GTA.Native;
using System.Collections.Generic;
using System.Drawing;
using static BillsyLiamGTA.UI.Timerbars.TimerbarHelpers;

namespace BillsyLiamGTA.UI.Timerbars
{
    public class CheckpointTimerbar : BaseTimerbar
    {
        /// <summary>
        /// A enum containing the possible checkpoint states.
        /// </summary>
        public enum CheckpointState
        { 
            Default,
            InProgress,
            Failed
        }
        /// <summary>
        /// A struct containing essential data for the checkpoint.
        /// </summary>
        public struct Checkpoint
        {
            public Color Color { get; set; }

            public Color ProgressColor { get; set; }

            public Color FailedColor { get; set; }

            public CheckpointState State { get; set; }

            public Checkpoint(Color color, Color progressColor, Color failedColor, CheckpointState state)
            {
                Color = color;
                ProgressColor = progressColor;
                FailedColor = failedColor;
                State = state;
            }

            public void Draw(float x, float y)
            {
                Color suitableColor = State == CheckpointState.Default ? Color : (State == CheckpointState.InProgress ? ProgressColor : FailedColor);
                Function.Call(Hash.DRAW_SPRITE, "timerbars", "circle_checkpoints", x, y, checkpointWidth, checkpointHeight, 0, suitableColor.R, suitableColor.G, suitableColor.B, 255, false, 0);
            }
        }

        #region Fields

        public List<Checkpoint> Checkpoints { get; set; }

        #endregion

        public CheckpointTimerbar(string text, List<Checkpoint> checkpoints) : base(text, true)
        {
            Checkpoints = checkpoints;
        }

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
    }
}