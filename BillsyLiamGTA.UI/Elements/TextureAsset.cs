using System;
using GTA;
using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    /// <summary>
    /// A class for loading and managing textures.
    /// </summary>
    public class TextureAsset
    {
        #region Properties

        /// <summary>
        /// Name of the dictionary.
        /// </summary>
        public string Dictionary { get; set; }
        /// <summary>
        /// Name of the texture.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Returns true if the texture dictionary is loaded.
        /// </summary>
        public bool IsLoaded
        {
            get
            {
                return Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, Dictionary);
            }
        }

        #endregion

        #region Constructors

        public TextureAsset(string dictionary, string name)
        {
            Dictionary = dictionary;
            Name = name;
        }

        public TextureAsset(string dictionary)
        {
            Dictionary = dictionary;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Load's the texture dictionary and times out if it takes too long.
        /// </summary>
        /// <param name="timeout"></param>
        /// <exception cref="TimeoutException"></exception>
        public void Load(int timeout = 5000)
        {
            int start = Game.GameTime;
            Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, Dictionary, false);
            while (!IsLoaded)
            {
                if (Game.GameTime > start + timeout)
                {
                    throw new TimeoutException($"ERROR: Loading texture dictionary '{Dictionary}' timed out after {timeout}ms.");
                }
                Script.Wait(0);
            }
        }
        /// <summary>
        /// Sets the texture dictionary as no longer needed, if it is loaded.
        /// </summary>
        public void Dispose()
        {
            if (IsLoaded)
            {
                Function.Call(Hash.SET_STREAMED_TEXTURE_DICT_AS_NO_LONGER_NEEDED, Dictionary);
            }
        }

        #endregion
    }
}