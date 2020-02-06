using System;

namespace RedditEmblemAPI.Models.Exceptions
{
    public class CoordinateFormattingException : Exception
    {
        public CoordinateFormattingException(string coord)
            : base(string.Format("The coordinate \"{0}\" could not be parsed. All coordinates should be in \"x,y\" format, where x and y are non-zero, positive numbers.", coord)) 
        { }
    }
}
