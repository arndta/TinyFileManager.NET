using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TinyFileManager.NET
{
    public class AzureSource : Source
    {
        private CloudBlobClient blobClient;
        private CloudBlobContainer blobContainer;
        
        public AzureSource(string currentPath, string currentLink, bool onlyImages, bool onlyVideos, string physicalPath, string selectFnString, string type)
            :base (currentPath, currentLink, onlyImages, onlyVideos, physicalPath, selectFnString, type)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(clsConfig.azureBlobStore);
            blobClient = storageAccount.CreateCloudBlobClient();
            blobContainer = blobClient.GetContainerReference(clsConfig.azureBlobContainer);
            
        }

        internal override ArrayList GetLinks()
        {
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
                    fileItem.strDeleteLink = "<a href=\"" + this.strCurrLink + "&cmd=delfile&file=" + fileItem.strPath + "&currpath=" + this.strCurrPath + "\" class=\"btn erase-button\" onclick=\"return confirm('Are you sure to delete this file?');\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                }
                else
                {
                    fileItem.strDeleteLink = "<a class=\"btn erase-button disabled\" title=\"Erase\"><i class=\"icon-trash\"></i></a>";
                }
                // get thumbnail image
                if (fileItem.boolIsImage)
                {
                    fileItem.strThumbImage = clsConfig.azureBlobUrl + clsConfig.azureBlobContainer + "/thumbs/" + fileName;
                }
                else
                {
                    if (File.Exists(Directory.GetParent(physicalPath).FullName + "\\img\\ico\\" + Path.GetExtension(fileUrl).TrimStart('.').ToUpper() + ".png"))
                    {
                        fileItem.strThumbImage = "img/ico/" + Path.GetExtension(fileUrl).TrimStart('.').ToUpper() + ".png";
                    }
                    else
                    {
                        fileItem.strThumbImage = "img/ico/Default.png";
                    }
                }

                fileItem.strDownFormOpen = "<form class=\"download-form\">";
                fileItem.strDownBtn = "<a class=\"btn\" title=\"Download\" href=\"" + fileUrl + "\"><i class=\"icon-download\"></i></a>";
                if (fileItem.boolIsImage)
                {
                    fileItem.strPreviewLink = "<a class=\"btn preview\" title=\"Preview\" data-url=\"" + fileItem.strPath + "\" data-toggle=\"lightbox\" href=\"#previewLightbox\"><i class=\"icon-eye-open\"></i></a>";
                }
                else
                {
                    fileItem.strPreviewLink = "<a class=\"btn preview disabled\" title=\"Preview\"><i class=\"icon-eye-open\"></i></a>";
                }
                fileItem.strLink = "<a href=\"#\" title=\"Select\" onclick=\"" + this.strApply + "('" + fileItem.strPath + "'," + this.strType + ")\";\"><img data-src=\"holder.js/140x100\" alt=\"140x100\" src=\"" + fileItem.strThumbImage + "\" height=\"100\"><h4>" + fileItem.strName + "</h4></a>";

                // check to see if it's the type of file we are looking at
                if ((this.boolOnlyImage && fileItem.boolIsImage) || (this.boolOnlyVideo && fileItem.boolIsVideo) || (!this.boolOnlyImage && !this.boolOnlyVideo))
                {
                    this.arrLinks.Add(fileItem);
                }
            }
            return arrLinks;
        }

        internal override void UploadFile(HttpPostedFile fileUpload, string folderName)
        {
            //check file was submitted
            if ((fileUpload != null) && (fileUpload.ContentLength > 0))
            {
                var blobName = "files/" + fileUpload.FileName;
                var blob = blobContainer.GetBlockBlobReference(blobName);

                //make a copy of the file stream so we can use it twice.
                var stream = new MemoryStream();
                fileUpload.InputStream.CopyTo(stream);
                stream.Position = 0;

                blob.UploadFromStream(stream);

                if (Helper.isImageFile(fileUpload.FileName))
                {
                    stream.Position = 0;
                    this.createThumbnail(stream, fileUpload.FileName);
                }
            }
        }
        private void createThumbnail(Stream stream, string fileName)
        {
            // open image and get dimensions in pixels
            var image = System.Drawing.Image.FromStream(stream);
            var objUnits = System.Drawing.GraphicsUnit.Pixel;
            var objRect = image.GetBounds(ref objUnits);

            int intHeight = 0;
            int intWidth = 0;
            // what are we going to resize to, to fit inside 156x78
            Helper.getProportionalResize(Convert.ToInt32(objRect.Width), Convert.ToInt32(objRect.Height), ref intWidth, ref intHeight);

            // create thumbnail
            var objCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
            var thumb = image.GetThumbnailImage(intWidth, intHeight, objCallback, IntPtr.Zero);

            // finish up

            var blobName = "thumbs/" + fileName;
            var blob = blobContainer.GetBlockBlobReference(blobName);
            using (var thumbStream = new MemoryStream())
            {
                thumb.Save(thumbStream, image.RawFormat);
                thumbStream.Position = 0;
                blob.UploadFromStream(thumbStream);
            }

            image.Dispose();
            thumb.Dispose();

        } // createThumbnail
        private bool ThumbnailCallback()
        {
            return false;
        } // ThumbnailCallback
        
        internal override void DeleteFile(string fileUrl)
        {
            try
            {
                var fileName = Path.GetFileName(fileUrl);

                //delete the main file
                var blobName = "files/" +fileName;
                var blob = blobContainer.GetBlockBlobReference(blobName);
                blob.DeleteIfExists();

                //delete the thumbnail
                blobName = "thumbs/" + fileName;
                blob = blobContainer.GetBlockBlobReference(blobName);
                blob.DeleteIfExists();
            }
            catch
            {
                //TODO: set error
            }
        }
    }
}