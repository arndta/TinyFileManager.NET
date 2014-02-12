using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TinyFileManager.NET
{
    public class AzureSource
    {
        private CloudBlobClient blobClient;
        private bool boolOnlyImage;
        private bool boolOnlyVideo;
        private ArrayList arrLinks = new ArrayList();
        private string strApply;
        private string strType;

        public AzureSource(string blobStore, string publicUrl, string container, bool onlyImages, bool onlyVideos, string selectFnString, string type)
        {
            boolOnlyImage = onlyImages;
            boolOnlyVideo = onlyVideos;
            strApply = selectFnString;
            strType = type;

            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(blobStore);
            blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(container);

            var blobs = blobContainer.ListBlobs("files/");
            foreach (var blob in blobs)
            {
                
                var fileItem = new TinyFileManager.NET.clsFileItem();
                string fileUrl = blob.Uri.ToString();
                fileItem.strName = Path.GetFileNameWithoutExtension(fileUrl); 
                fileItem.boolIsFolder = false;
                //objFItem.intColNum = this.getNextColNum();
                fileItem.strPath = fileUrl;
                string fileName = Path.GetFileName(fileUrl);
                fileItem.boolIsImage = Helper.isImageFile(fileName);
                fileItem.boolIsVideo = Helper.isVideoFile(fileName);
                fileItem.boolIsMusic = Helper.isMusicFile(fileName);
                fileItem.boolIsMisc = Helper.isMiscFile(fileName);
                // get display class type
                if (fileItem.boolIsImage)
                {
                    fileItem.strClassType = "2";
                }
                else
                {
                    if (fileItem.boolIsMisc)
                    {
                        fileItem.strClassType = "3";
                    }
                    else
                    {
                        if (fileItem.boolIsMusic)
                        {
                            fileItem.strClassType = "5";
                        }
                        else
                        {
                            if (fileItem.boolIsVideo)
                            {
                                fileItem.strClassType = "4";
                            }
                            else
                            {
                                fileItem.strClassType = "1";
                            }
                        }
                    }
                }
                // get delete link
                if (clsConfig.boolAllowDeleteFile)
                {
                    //fileItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfile&file=" + fileItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure to delete this file?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                }
                else
                {
                    fileItem.strDeleteLink = "<a class=\"btn erase-button disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                }
                // get thumbnail image
                if (fileItem.boolIsImage)
                {
                    fileItem.strThumbImage = clsConfig.azureBlobUrl + clsConfig.azureBlobContainer + "/thumbs/" + fileName; //todo:switch to azure vm as strPath is the full url now.
                }
                else
                {
                    //if (File.Exists(Directory.GetParent(physicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(strF).TrimStart('.').ToUpper() + ".png"))
                    //{
                    //    fileItem.strThumbImage = "img/ico/" + Path.GetExtension(fileUrl).TrimStart('.').ToUpper() + ".png";
                    //}
                    //else
                    //{
                    //    fileItem.strThumbImage = "img/ico/Default.png";
                    //}
                }

                fileItem.strDownFormOpen = "<form class=\"download-form\">";
                fileItem.strDownBtn = "<a class=\"btn\" title=\"Download\" href=\"" + fileUrl + "\"><i class=\"icon-download\"></i></a>";
                if (fileItem.boolIsImage)
                {
                    fileItem.strPreviewLink = "<a class=\"btn preview\" title=\"Preview\" data-url=\"" + clsConfig.strUploadURL + "/" + fileItem.strPath.Replace('\\', '/') + "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a>";
                }
                else
                {
                    fileItem.strPreviewLink = "<a class=\"btn preview disabled\" title=\"Preview\"><i class=\"icon-eye-open\"></i></a>";
                }
                fileItem.strLink = "<a href=\"#\" title=\"Select\" onclick=\"" + this.strApply + "('" + clsConfig.strUploadURL + "/" + fileItem.strPath.Replace('\\', '/') + "'," + this.strType + ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" + fileItem.strThumbImage + "\" height=\"100\"><h4>" + fileItem.strName + "</h4></a>";

                // check to see if it's the type of file we are looking at
                if ((this.boolOnlyImage && fileItem.boolIsImage) || (this.boolOnlyVideo && fileItem.boolIsVideo) || (!this.boolOnlyImage && !this.boolOnlyVideo))
                {
                    this.arrLinks.Add(fileItem);
                }
            }
        }

        public ArrayList GetLinks()
        {
            return arrLinks;
        }
    }
}