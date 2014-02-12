using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TinyFileManager.NET
{
    public static class Helper
    {
        public static bool isImageFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedImageExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of image file extensions
        } // isImageFile

        public static bool isVideoFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedVideoExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of video file extensions
        } // isVideoFile

        public static bool isMusicFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedMusicExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of music file extensions
        } // isMusicFile

        public static bool isMiscFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedMiscExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of misc file extensions
        } // isMiscFile
    }
}