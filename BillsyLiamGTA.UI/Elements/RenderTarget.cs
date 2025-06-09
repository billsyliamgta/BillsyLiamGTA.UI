using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    public class RenderTarget
    {
        #region Fields
        public int Handle { get; set; }
        
        public string Name { get; set; }
        
        public int Model { get; set; }

        #endregion

        public RenderTarget(string name, int model)
        {
            Name = name;
            Model = model;
            if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, name))
            {
                Function.Call(Hash.REGISTER_NAMED_RENDERTARGET, name, 0);
            }
            if (!Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_LINKED, model))
            {
                Function.Call(Hash.LINK_NAMED_RENDERTARGET, model);
            }
            if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, name))
            {
                Handle = Function.Call<int>(Hash.GET_NAMED_RENDERTARGET_RENDER_ID, name);
            }
        }
        
        public void Release()
        {
            if (Function.Call<bool>(Hash.IS_NAMED_RENDERTARGET_REGISTERED, Name))
            {
                Function.Call(Hash.RELEASE_NAMED_RENDERTARGET, Name);
            }
        }
    }
}