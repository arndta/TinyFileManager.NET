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

        
        public static void getProportionalResize(int intOldWidth, int intOldHeight, ref int intNewWidth, ref int intNewHeight)
        {
            int intHDiff = 0;
            int intWDiff = 0;
            decimal decProp = 0;
            int intTargH = 78;
            int intTargW = 156;

            if ((intOldHeight <= intTargH) && (intOldWidth <= intTargW))
            {
                // no resize needed
                intNewHeight = intOldHeight;
                intNewWidth = intOldWidth;
                return;
            }

            //get the differences between desired and current height and width
            intHDiff = intOldHeight - intTargH;
            intWDiff = intOldWidth - intTargW;

            //whichever is the bigger difference is the chosen proportion
            if (intHDiff > intWDiff)
            {
                decProp = (decimal)intTargH / (decimal)intOldHeight;
                intNewHeight = intTargH;
                intNewWidth = Convert.ToInt32(Math.Round(intOldWidth * decProp, 0));
            }
            else
            {
                decProp = (decimal)intTargW / (decimal)intOldWidth;
                intNewWidth = intTargW;
                intNewHeight = Convert.ToInt32(Math.Round(intOldHeight * decProp, 0));
            }
        } // getProportionalResize
    }
}