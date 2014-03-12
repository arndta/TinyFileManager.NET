using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace TinyFileManager.NET
{

    public partial class dialog : System.Web.UI.Page
    {
        public string strType;
        public string strApply;
        public string strCmd;
        public string strFolder;
        public string strFile;
        public string strLang;
        public string strEditor;
        public string strCurrPath;
        public string strCurrLink;      // dialog.aspx?editor=.... for simplicity
        public ArrayList arrLinks = new ArrayList();

        private int intColNum;
        private string[] arrFolders;
        private string[] arrFiles;
        private TinyFileManager.NET.clsFileItem objFItem;
        private bool boolOnlyImage;
        private bool boolOnlyVideo;

        protected void Page_Load(object sender, EventArgs e)
        {

            strCmd = Request.QueryString["cmd"] + "";
            strType = Request.QueryString["type"] + "";
            strFolder = Request.QueryString["folder"] + "";
            strFile = Request.QueryString["file"] + "";
            strLang = Request.QueryString["lang"] + "";      //not used right now, but grab it
            strEditor = Request.QueryString["editor"] + "";
            strCurrPath = Request.QueryString["currpath"] + "";

            //check inputs
            if (this.strCurrPath.Length > 0)
            {
                this.strCurrPath = this.strCurrPath.TrimEnd('\\') + "\\";
            }

            //set the apply string, based on the passed type
            if (this.strType == "")
            {
                this.strType = "0";
            }
            switch (this.strType)
            {
                case "1":
                    this.strApply = "apply_img";
                    this.boolOnlyImage = true;
                    break;
                case "2":
                    this.strApply = "apply_link";
                    break;
                default:
                    if (Convert.ToInt32(this.strType) >= 3)
                    {
                        this.strApply = "apply_video";
                        this.boolOnlyVideo = true;
                    }
                    else
                    {
                        this.strApply = "apply";
                    }
                    break;
            }

            //setup current link
            strCurrLink = "dialog.aspx?type=" + this.strType + "&editor=" + this.strEditor + "&lang=" + this.strLang;

            switch (strCmd)
            {
                case "debugsettings":
                    Response.Write("<style>");
                    Response.Write("body {font-family: Verdana; font-size: 10pt;}");
                    Response.Write(".table {display: table; border-collapse: collapse; margin: 20px; background-color: #e7e5e5;}");
                    Response.Write(".tcaption {display: table-caption; padding: 5px; font-size: 14pt; font-weight: bold; background-color: #9fcff7;}");
                    Response.Write(".tr {display: table-row;}");
                    Response.Write(".tr:hover {background-color: #f0f2f3;}");
                    Response.Write(".td {display: table-cell; padding: 5px; border: 1px solid #a19e9e;}");
                    Response.Write("</style>");

                    Response.Write("<div class=\"table\">");   //start table

                    Response.Write("<div class=\"tcaption\">Operating Settings</div>");  //caption

                    Response.Write("<div class=\"tbody\">");  //start body

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowCreateFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.boolAllowCreateFolder + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFile:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.boolAllowDeleteFile + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowDeleteFolder:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.boolAllowDeleteFolder + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowUploadFile:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.boolAllowUploadFile + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>MaxUploadSizeMb:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.intMaxUploadSizeMb + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedAllExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedAllExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedFileExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedFileExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedImageExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedImageExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedMiscExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedMiscExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedMusicExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedMusicExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>AllowedVideoExtensions:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strAllowedVideoExtensions + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>BaseURL:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strBaseURL + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>DocRoot:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strDocRoot + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>ThumbPath:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strThumbPath + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>ThumbURL:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strThumbURL + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>UploadPath:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strUploadPath + "</div>");
                    Response.Write("</div>");   //end row
                    
                    Response.Write("<div class=\"tr\">");   // start row
                    Response.Write("<div class=\"td\"><b>UploadURL:</b></div>");
                    Response.Write("<div class=\"td\">" + clsConfig.strUploadURL + "</div>");
                    Response.Write("</div>");   //end row

                    Response.Write("</div>");   //end body
                    Response.Write("</div>");   //end table
                    
                    

                    Response.End();
                    break;
                case "createfolder":
                    try
                    {
                        strFolder = Request.Form["folder"] + "";
                        //forge ahead without checking for existence
                        //catch will save us
                        Directory.CreateDirectory(clsConfig.strUploadPath + "\\" + strFolder);
                        Directory.CreateDirectory(clsConfig.strThumbPath + "\\" + strFolder);

                        // end response, since it's an ajax call
                        Response.End();
                    }
                    catch
                    {
                        //TODO: write error
                    }
                    break;

                case "upload":
                    strFolder = Request.Form["folder"] + "";
                    HttpPostedFile filUpload = Request.Files["file"];
                    string strTargetFile;
                    string strThumbFile;

                    //check file was submitted
                    if ((filUpload != null) && (filUpload.ContentLength > 0))
                    {
                        strTargetFile = clsConfig.strUploadPath + this.strFolder + filUpload.FileName;
                        strThumbFile = clsConfig.strThumbPath + this.strFolder + filUpload.FileName;
                        filUpload.SaveAs(strTargetFile);

                        if (this.isImageFile(strTargetFile))
                        {
                            this.createThumbnail(strTargetFile, strThumbFile);
                        }
                    }

                    // end response
                    if (Request.Form["fback"] == "true")
                    {
                        Response.Redirect(this.strCurrLink);
                    }
                    else
                    {
                        Response.End();
                    }
                    
                    break;

                case "download":
                    FileInfo objFile = new FileInfo(clsConfig.strUploadPath + "\\" + this.strFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Pragma", "private");
                    Response.AddHeader("Cache-control", "private, must-revalidate");
                    Response.AddHeader("Content-Type", "application/octet-stream");
                    Response.AddHeader("Content-Length", objFile.Length.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(this.strFile));
                    Response.WriteFile(clsConfig.strUploadPath + "\\" + this.strFile);
                    break;

                case "delfile":
                    try
                    {
                        File.Delete(clsConfig.strUploadPath + "\\" + this.strFile);
                        if (File.Exists(clsConfig.strThumbPath + "\\" + this.strFile))
                        {
                            File.Delete(clsConfig.strThumbPath + "\\" + this.strFile);
                        }
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;

                case "delfolder":
                    try
                    {
                        Directory.Delete(clsConfig.strUploadPath + "\\" + strFolder,true);
                        Directory.Delete(clsConfig.strThumbPath + "\\" + strFolder, true);
                    }
                    catch
                    {
                        //TODO: set error
                    }
                    goto default;

                default:    //just a regular page load
                    if (this.strCurrPath != "")
                    {
                        // add "up one" folder
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = "..";
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.boolIsFolderUp = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.getUpOneDir(this.strCurrPath);
                        this.objFItem.strClassType = "dir";
                        this.objFItem.strDeleteLink = "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        this.objFItem.strThumbImage = "img/ico/folder_return.png";
                        this.objFItem.strLink = "<a title=\"Open\" href=\"" + this.strCurrLink + "&currpath=" + this.objFItem.strPath + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>..</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    //load folders
                    arrFolders = Directory.GetDirectories(clsConfig.strUploadPath + this.strCurrPath);
                    foreach (string strF in arrFolders)
                    {
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = Path.GetFileName(strF);
                        this.objFItem.boolIsFolder = true;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.strCurrPath + Path.GetFileName(strF);
                        this.objFItem.strClassType = "dir";
                        if (clsConfig.boolAllowDeleteFolder)
                        {
                            this.objFItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfolder&folder=" + this.objFItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button top-right\" onclick=\"return confirm('Are you sure to delete the folder and all the objects in it?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strDeleteLink = "<a class=\"btn erase-button top-right disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        this.objFItem.strThumbImage = "img/ico/folder.png";
                        this.objFItem.strLink = "<a title=\"Open\" href=\"" + this.strCurrLink + "&currpath=" + this.objFItem.strPath + "\"><img class=\"directory-img\" src=\"" + this.objFItem.strThumbImage + "\" alt=\"folder\" /><h3>" + this.objFItem.strName + "</h3></a>";
                        this.arrLinks.Add(objFItem);
                    }

                    // load files
                    arrFiles = Directory.GetFiles(clsConfig.strUploadPath + this.strCurrPath);
                    foreach (string strF in arrFiles)
                    {
                        this.objFItem = new TinyFileManager.NET.clsFileItem();
                        this.objFItem.strName = Path.GetFileNameWithoutExtension(strF);
                        this.objFItem.boolIsFolder = false;
                        this.objFItem.intColNum = this.getNextColNum();
                        this.objFItem.strPath = this.strCurrPath + Path.GetFileName(strF);
                        this.objFItem.boolIsImage = this.isImageFile(Path.GetFileName(strF));
                        this.objFItem.boolIsVideo = this.isVideoFile(Path.GetFileName(strF));
                        this.objFItem.boolIsMusic = this.isMusicFile(Path.GetFileName(strF));
                        this.objFItem.boolIsMisc = this.isMiscFile(Path.GetFileName(strF));
                        // get display class type
                        if (this.objFItem.boolIsImage)
                        {
                            this.objFItem.strClassType = "2";
                        }
                        else
                        {
                            if (this.objFItem.boolIsMisc)
                            {
                                this.objFItem.strClassType = "3";
                            }
                            else
                            {
                                if (this.objFItem.boolIsMusic)
                                {
                                    this.objFItem.strClassType = "5";
                                }
                                else
                                {
                                    if (this.objFItem.boolIsVideo)
                                    {
                                        this.objFItem.strClassType = "4";
                                    }
                                    else
                                    {
                                        this.objFItem.strClassType = "1";
                                    }
                                }
                            }
                        }
                        // get delete link
                        if (clsConfig.boolAllowDeleteFile)
                        {
                            this.objFItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfile&file=" + this.objFItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure to delete this file?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strDeleteLink = "<a class=\"btn erase-button disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                        }
                        // get thumbnail image
                        if (this.objFItem.boolIsImage)
                        {
                            // first check to see if thumb exists
                            if (!File.Exists(clsConfig.strThumbPath + this.objFItem.strPath))
                            {
                                // thumb doesn't exist, create it
                                strTargetFile = clsConfig.strUploadPath + this.objFItem.strPath;
                                strThumbFile = clsConfig.strThumbPath + this.objFItem.strPath;
                                this.createThumbnail(strTargetFile, strThumbFile);
                            }
                            this.objFItem.strThumbImage = clsConfig.strThumbURL + "/" + this.objFItem.strPath.Replace('\\', '/');
                        }
                        else
                        {
                            if (File.Exists(Directory.GetParent(Request.PhysicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png"))
                            {
                                this.objFItem.strThumbImage = "img/ico/" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png";
                            }
                            else
                            {
                                this.objFItem.strThumbImage = "img/ico/Default.png";
                            }
                        }
                        this.objFItem.strDownFormOpen = "<form action=\"dialog.aspx?cmd=download&file=" + this.objFItem.strPath + "\" method=\"post\" class=\"download-form\">";
                        if (this.objFItem.boolIsImage)
                        {
                            this.objFItem.strPreviewLink = "<a class=\"btn preview\" title=\"Preview\" data-url=\"" + clsConfig.strUploadURL + "/" + this.objFItem.strPath.Replace('\\', '/') + "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a>";
                        }
                        else
                        {
                            this.objFItem.strPreviewLink = "<a class=\"btn preview disabled\" title=\"Preview\"><i class=\"icon-eye-open\"></i></a>";
                        }
                        this.objFItem.strLink = "<a href=\"#\" title=\"Select\" onclick=\"" + this.strApply + "('" + clsConfig.strUploadURL + "/" + this.objFItem.strPath.Replace('\\', '/') + "'," + this.strType + ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" + this.objFItem.strThumbImage + "\" height=\"100\"><h4>" + this.objFItem.strName + "</h4></a>";

                        // check to see if it's the type of file we are looking at
                        if ((this.boolOnlyImage && this.objFItem.boolIsImage) || (this.boolOnlyVideo && this.objFItem.boolIsVideo) || (!this.boolOnlyImage && !this.boolOnlyVideo)) 
                        {
                            this.arrLinks.Add(objFItem);
                        }
                    } // foreach

                    break;
            }   // switch

        }   // page load

        public string getBreadCrumb()
        {
            string strRet;
            string[] arrFolders;
            string strTempPath = "";
            int intCount = 0;

            strRet = "<li><a href=\"" + this.strCurrLink + "&currpath=\"><i class=\"icon-home\"></i></a>";
            arrFolders = this.strCurrPath.Split('\\');

            foreach (string strFolder in arrFolders) 
            {
                if (strFolder != "")
                {
                    strTempPath += strFolder + "\\";
                    intCount++;

                    if (intCount == (arrFolders.Length - 1))
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li class=\"active\">" + strFolder + "</li>";
                    }
                    else
                    {
                        strRet += " <span class=\"divider\">/</span></li> <li><a href=\"" + this.strCurrLink + "&currpath=" + strTempPath + "\">" + strFolder + "</a>";
                    }
                }
            }   // foreach

            return strRet;
        }   // getBreadCrumb 

        private bool isImageFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedImageExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of image file extensions
        } // isImageFile

        private bool isVideoFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedVideoExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of video file extensions
        } // isVideoFile

        private bool isMusicFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedMusicExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of music file extensions
        } // isMusicFile

        private bool isMiscFile(string strFilename)
        {
            int intPosition;

            intPosition = Array.IndexOf(clsConfig.arrAllowedMiscExtensions, Path.GetExtension(strFilename).TrimStart('.'));
            return (intPosition > -1);  // if > -1, then it was found in the list of misc file extensions
        } // isMiscFile

        private void createThumbnail(string strFilename, string strThumbFilename)
        {
            System.Drawing.Image.GetThumbnailImageAbort objCallback;
            System.Drawing.Image objFSImage;
            System.Drawing.Image objTNImage;
            System.Drawing.RectangleF objRect;
            System.Drawing.GraphicsUnit objUnits = System.Drawing.GraphicsUnit.Pixel;
            int intHeight = 0;
            int intWidth = 0;

            // open image and get dimensions in pixels
            objFSImage = System.Drawing.Image.FromFile(strFilename);
            objRect = objFSImage.GetBounds(ref objUnits);

            // what are we going to resize to, to fit inside 156x78
            getProportionalResize(Convert.ToInt32(objRect.Width), Convert.ToInt32(objRect.Height), ref intWidth, ref intHeight);

            // create thumbnail
            objCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            objTNImage = objFSImage.GetThumbnailImage(intWidth, intHeight, objCallback, IntPtr.Zero);

            // finish up
            objFSImage.Dispose();
            objTNImage.Save(strThumbFilename);
            objTNImage.Dispose();

        } // createThumbnail

        private void getProportionalResize(int intOldWidth, int intOldHeight, ref int intNewWidth, ref int intNewHeight)
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

        private bool ThumbnailCallback()
        {
            return false;
        } // ThumbnailCallback

        public string getEndOfLine(int intColNum) 
        {
            if (intColNum == 6)
            {
                return "</div><div class=\"space10\"></div>";
            }
            else
            {
                return "";
            }
        } // getEndOfLine

        public string getStartOfLine(int intColNum) 
        {
            if (intColNum == 1)
            {
                return "<div class=\"row-fluid\">";
            }
            else
            {
                return "";
            }
        } // getStartOfLine

        private int getNextColNum()
        {
            this.intColNum++;
            if (this.intColNum > 6)
            {
                this.intColNum = 1;
            }
            return this.intColNum;
        } // getNextColNum

        private string getUpOneDir(string strInput)
        {
            string[] arrTemp;

            arrTemp = strInput.TrimEnd('\\').Split('\\');
            arrTemp[arrTemp.Length - 1] = "";
            return String.Join("\\", arrTemp);
        }

    }   // class

}   // namespace

