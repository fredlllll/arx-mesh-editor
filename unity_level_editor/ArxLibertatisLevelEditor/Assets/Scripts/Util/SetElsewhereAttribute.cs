using System;

namespace Assets.Scripts.Util
{
    /// <summary>
    /// used to mark fields in the structs. means that the value of that field will be modified elsewhere (from a collection count for example) and should not be edited directly by the user
    /// </summary>
    public class SetElsewhereAttribute : Attribute
    {
        public SetElsewhereAttribute(string where = null)
        {
            //could also just be a comment, but this is prettier and probably gets the point accross better
        }
    }
}
