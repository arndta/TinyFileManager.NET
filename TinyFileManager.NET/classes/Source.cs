using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TinyFileManager.NET
{
    public abstract class Source
    {
        internal string strCurrPath;
        internal string strCurrLink;
        internal bool boolOnlyImage;
        internal bool boolOnlyVideo;
        internal ArrayList arrLinks = new ArrayList();
        internal string strApply;
        internal string strType;
        internal string physicalPath;

        public Source(string currentPath, string currentLink, bool onlyImages, bool onlyVideos, string physicalPath, string selectFnString, string type)
        {
            strCurrPath = currentPath;
            strCurrLink = currentLink;
            boolOnlyImage = onlyImages;
            boolOnlyVideo = onlyVideos;
            this.physicalPath = physicalPath;
            strApply = selectFnString;
            strType = type;
        }
        internal abstract ArrayList GetLinks();
        internal abstract void UploadFile(HttpPostedFile filUpload, string folderName);
        internal abstract void DeleteFile(string fileName);
    }
}
