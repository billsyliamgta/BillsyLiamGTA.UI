using GTA;
using GTA.Native;
using BillsyLiamGTA.UI.Elements;

namespace BillsyLiamGTA.UI.Scaleform
{
    public class MidsizedMessage : BaseScaleform
    {
        #region Fields

        public string Title { get; set; }   

        public string Description { get; set; }

        public int Color { get; set; } = 2;

        public int ScreenTime { get; set; } = 8000;

        public int AnimOutColor { get; set; } = 0;

        public float AnimOutSpeed { get; set; } = 0.2f;

        public bool Condensed { get; set; } = true;

        #endregion

        #region Constructors

        public MidsizedMessage(string title, string description) : base("MIDSIZED_MESSAGE")
        {
            Title = title;
            Description = description;
        }

        public MidsizedMessage(string title, string description, int color) : base("MIDSIZED_MESSAGE")
        {
            Title = title;
            Description = description;
            Color = color;
        }

        #endregion

        #region Methods

        public void Show()
        {
            Load();
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Shard_Appear", "GTAO_Boss_Goons_FM_Shard_Sounds", false);
            CallFunction("SHOW_SHARD_MIDSIZED_MESSAGE", Title, Description, Color, false, Condensed);
            int start = Game.GameTime;
            int time = ScreenTime;
            while (Game.GameTime < start + time)
            {
                Draw();
                Script.Wait(0);
            }
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, "Shard_Disappear", "GTAO_Boss_Goons_FM_Shard_Sounds", false);
            CallFunction("SHARD_ANIM_OUT", Color, AnimOutSpeed);
            start = Game.GameTime;
            time = (int)(AnimOutSpeed * 10000);
            while (Game.GameTime < start + time)
            {
                Draw();
                Script.Wait(0);
            }
            Dispose();
        }

        #endregion
    }
}