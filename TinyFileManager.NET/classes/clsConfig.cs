using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TinyFileManager.NET
{
    public static class clsConfig
    {
        #region Settings Properties
        /// <summary>
        ///  Max upload filesize in Mb
        /// </summary>
        public static int intMaxUploadSizeMb
        {
            get
            {
                return Convert.ToInt32(Properties.Settings.Default.MaxUploadSizeMb);
            }
        }

        /// <summary>
        ///  Allowed image file extensions
        /// </summary>
        public static string strAllowedImageExtensions
        {
            get
            {
                return Properties.Settings.Default.AllowedImageExtensions;
            }
        }

        /// <summary>
        ///  Allowed image file extensions as an array
        /// </summary>
        public static string[] arrAllowedImageExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedImageExtensions);
            }
        }

        /// <summary>
        ///  Allowed document file extensions
        /// </summary>
        public static string strAllowedFileExtensions
        {
            get
            {
                return Properties.Settings.Default.AllowedFileExtensions;
            }
        }

        /// <summary>
        ///  Allowed document file extensions as an array
        /// </summary>
        public static string[] arrAllowedFileExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedFileExtensions);
            }
        }

        /// <summary>
        ///  Allowed video file extensions
        /// </summary>
        public static string strAllowedVideoExtensions
        {
            get
            {
                return Properties.Settings.Default.AllowedVideoExtensions;
            }
        }

        /// <summary>
        ///  Allowed video file extensions as an array
        /// </summary>
        public static string[] arrAllowedVideoExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedVideoExtensions);
            }
        }

        /// <summary>
        ///  Allowed music file extensions
        /// </summary>
        public static string strAllowedMusicExtensions
        {
            get
            {
                return Properties.Settings.Default.AllowedMusicExtensions;
            }
        }

        /// <summary>
        ///  Allowed music file extensions as an array
        /// </summary>
        public static string[] arrAllowedMusicExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedMusicExtensions);
            }
        }

        /// <summary>
        ///  Allowed misc file extensions
        /// </summary>
        public static string strAllowedMiscExtensions
        {
            get
            {
                return Properties.Settings.Default.AllowedMiscExtensions;
            }
        }

        /// <summary>
        ///  Allowed misc file extensions as an array
        /// </summary>
        public static string[] arrAllowedMiscExtensions
        {
            get
            {
                return getArrayFromString(clsConfig.strAllowedMiscExtensions);
            }
        }

        /// <summary>
        ///  All allowed file extensions
        /// </summary>
        public static string strAllowedAllExtensions
        {
            get
            {
                string strRet = "";

                if (clsConfig.strAllowedImageExtensions.Length > 0)
                {
                    strRet = clsConfig.strAllowedImageExtensions;
                }
                if (clsConfig.strAllowedFileExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedFileExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedFileExtensions;
                    }
                }
                if (clsConfig.strAllowedVideoExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedVideoExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedVideoExtensions;
                    }
                }
                if (clsConfig.strAllowedMusicExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedMusicExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedMusicExtensions;
                    }
                }
                if (clsConfig.strAllowedMiscExtensions.Length > 0)
                {
                    if (strRet.Length > 0)
                    {
                        strRet += "," + clsConfig.strAllowedMiscExtensions;
                    }
                    else
                    {
                        strRet = clsConfig.strAllowedMiscExtensions;
                    }
                }

                return strRet;
            }
        }

        /// <summary>
        /// Returns document root
        /// </summary>
        public static string strDocRoot
        {
            get
            {
                //return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].TrimEnd('\\');
                return HttpContext.Current.Server.MapPath("/").TrimEnd('\\');
            }
        }

        /// <summary>
        /// Returns the base url of the site
        /// </summary>
        public static string strBaseURL
        {
            get
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority.TrimEnd('/');
            }
        }

        /// <summary>
        /// Returns the full upload drive path
        /// </summary>
        public static string strUploadPath
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + Properties.Settings.Default.UploadPath.TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Returns the full thumb drive path
        /// </summary>
        public static string strThumbPath
        {
            get
            {
                return clsConfig.strDocRoot + "\\" + Properties.Settings.Default.ThumbPath.TrimEnd('\\') + "\\";
            }
        }

        /// <summary>
        /// Returns the full upload url
        /// </summary>
        public static string strUploadURL
        {
            get
            {
                return clsConfig.strBaseURL + "/" + Properties.Settings.Default.UploadPath.Replace('\\', '/');
            }
        }

        /// <summary>
        /// Returns the full thumb url
        /// </summary>
        public static string strThumbURL
        {
            get
            {
                return clsConfig.strBaseURL + "/" + Properties.Settings.Default.ThumbPath.Replace('\\', '/');
            }
        }

        /// <summary>
        /// Returns the setting for allowing upload of file
        /// </summary>
        public static bool boolAllowUploadFile
        {
            get
            {
                return Convert.ToBoolean(Properties.Settings.Default.AllowUploadFile);
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of file
        /// </summary>
        public static bool boolAllowDeleteFile
        {
            get
            {
                return Convert.ToBoolean(Properties.Settings.Default.AllowDeleteFile);
            }
        }

        /// <summary>
        /// Returns the setting for allowing creation of folder
        /// </summary>
        public static bool boolAllowCreateFolder
        {
            get
            {
                return Convert.ToBoolean(Properties.Settings.Default.AllowCreateFolder);
            }
        }

        /// <summary>
        /// Returns the setting for allowing delete of folder
        /// </summary>
        public static bool boolAllowDeleteFolder
        {
            get
            {
                return Convert.ToBoolean(Properties.Settings.Default.AllowDeleteFolder);
            }
        }
        #endregion

        private static string[] getArrayFromString(string strInput)
        {
                string[] arrExt;
                string strTemp;

                //remove lead and trail single quotes so we can SPLIT the hell out of it
                strTemp = strInput.Trim('\'');
                arrExt = strTemp.Split(new string[] {"'",",","'"}, StringSplitOptions.RemoveEmptyEntries);

                return arrExt;
        }   // getArrayFromString

    }   // class

    
}   // namespace