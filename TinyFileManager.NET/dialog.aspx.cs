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
                    Response.Write("<b>AllowCreateFolder:</b> " + clsConfig.boolAllowCreateFolder + "<br>");
                    Response.Write("<b>AllowDeleteFile:</b> " + clsConfig.boolAllowDeleteFile + "<br>");
                    Response.Write("<b>AllowDeleteFolder:</b> " + clsConfig.boolAllowDeleteFolder + "<br>");
                    Response.Write("<b>AllowUploadFile:</b> " + clsConfig.boolAllowUploadFile + "<br>");
                    Response.Write("<b>MaxUploadSizeMb:</b> " + clsConfig.intMaxUploadSizeMb + "<br>");
                    Response.Write("<b>AllowedAllExtensions:</b> " + clsConfig.strAllowedAllExtensions + "<br>");
                    Response.Write("<b>AllowedFileExtensions:</b> " + clsConfig.strAllowedFileExtensions + "<br>");
                    Response.Write("<b>AllowedImageExtensions:</b> " + clsConfig.strAllowedImageExtensions + "<br>");
                    Response.Write("<b>AllowedMiscExtensions:</b> " + clsConfig.strAllowedMiscExtensions + "<br>");
                    Response.Write("<b>AllowedMusicExtensions:</b> " + clsConfig.strAllowedMusicExtensions + "<br>");
                    Response.Write("<b>AllowedVideoExtensions:</b> " + clsConfig.strAllowedVideoExtensions + "<br>");
                    Response.Write("<b>BaseURL:</b> " + clsConfig.strBaseURL + "<br>");
                    Response.Write("<b>DocRoot:</b> " + clsConfig.strDocRoot + "<br>");
                    Response.Write("<b>ThumbPath:</b> " + clsConfig.strThumbPath + "<br>");
                    Response.Write("<b>ThumbURL:</b> " + clsConfig.strThumbURL + "<br>");
                    Response.Write("<b>UploadPath:</b> " + clsConfig.strUploadPath + "<br>");
                    Response.Write("<b>UploadURL:</b> " + clsConfig.strUploadURL + "<br>");
                    Response.Write("<b>AzureBlobStore:</b> " + clsConfig.azureBlobStore + "<br>");
                    Response.Write("<b>AzureBlobContainer:</b> " + clsConfig.azureBlobContainer + "<br>");
                    Response.Write("<b>AzureBlobUrl:</b> " + clsConfig.azureBlobUrl + "<br>");
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

                        if (Helper.isImageFile(strTargetFile))
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

                    //var source = new DirectorySource(strCurrPath, strCurrLink, boolOnlyImage, boolOnlyVideo, Request.PhysicalPath, strApply, strType);
                    var source = new AzureSource(clsConfig.azureBlobStore, clsConfig.azureBlobUrl, clsConfig.azureBlobContainer, boolOnlyImage, boolOnlyVideo, strApply, strType);
                    this.arrLinks.AddRange(source.GetLinks());

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

