using GTA.Native;

namespace BillsyLiamGTA.UI.Elements
{
    public class Extensions
    {
        #region Functions

        public static void CallFunction(int handle, string functionName, params object[] args)
        {
            Function.Call(Hash.BEGIN_SCALEFORM_MOVIE_METHOD, handle, functionName);
            foreach (object obj in args)
            {
                if (obj.GetType() == typeof(int))
                {
                    Function.Call<int>(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_INT, (int)obj);
                }
                else if (obj.GetType() == typeof(float))
                {
                    Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)obj);
                }
                else if (obj.GetType() == typeof(double))
                {
                    Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_FLOAT, (float)(double)obj);
                }
                else if (obj.GetType() == typeof(bool))
                {
                    Function.Call(Hash.SCALEFORM_MOVIE_METHOD_ADD_PARAM_BOOL, (bool)obj);
                }
                else if (obj.GetType() == typeof(string))
                {
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, (string)obj);
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                }
                else if (obj.GetType() == typeof(char))
                {
                    Function.Call(Hash.BEGIN_TEXT_COMMAND_SCALEFORM_STRING, "STRING");
                    Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, ((char)obj).ToString());
                    Function.Call(Hash.END_TEXT_COMMAND_SCALEFORM_STRING);
                }
            }
            Function.Call(Hash.END_SCALEFORM_MOVIE_METHOD);
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }

        #endregion
    }
}