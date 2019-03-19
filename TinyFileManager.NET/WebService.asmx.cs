using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace TinyFileManager.NET
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]


    public class JsTreeNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public A_Attr a_attr { get; set; } = new A_Attr();
        public Li_Attr li_attr { get; set; } = new Li_Attr();
        public State state { get; set; } = new State();
        public bool children { get; set; }
    }

    public class State
    {
        public bool opened { get; set; } = false;
        public bool disabled { get; set; } = false;
        public bool selected { get; set; } = false;
    }

    public class A_Attr
    {
        public string href { get; set; }

        [JsonProperty(PropertyName = "class")]
        public string _class { get; set; } = "";
        public string frontendurl { get; set; }
    }

    public class Li_Attr
    {
        [JsonProperty(PropertyName = "class")]
        public string _class { get; set; } = "";
        public string mediaDetailId { get; set; }
    }

    public class WebService : System.Web.Services.WebService
	{
		public WebService():base()
		{
		}

		private string GetBasePath(string relPath = "/")
		{
			return HttpContext.Current.Server.MapPath(relPath);
		}

        [WebMethod(EnableSession = true)]
        public void RenameFileManagerItem(string oldText, string newText, string href)
        {
            var hrefQueryString = System.Web.HttpUtility.ParseQueryString(href);

            var hrefFile =  hrefQueryString["file"];
            var hrefCurrPath = hrefQueryString["currpath"];             
            
            if(!string.IsNullOrEmpty(hrefFile))
            {                                
                var oldFileInfo = new FileInfo(GetBasePath() + hrefFile);

                var newHref = oldFileInfo.Directory.FullName +"\\"+ newText;

                var newFileInfo = new FileInfo(newHref);                

                if(oldFileInfo.Exists && !newFileInfo.Exists)
                {
                    File.Move(oldFileInfo.FullName, newFileInfo.FullName);
                }

            }
            else if(!string.IsNullOrEmpty(hrefCurrPath))
            {
                var absPath = GetBasePath(hrefCurrPath);
                var oldDirectoryInfo = new DirectoryInfo(absPath);
                
                var newHref = oldDirectoryInfo.Parent.FullName +"\\"+ newText;

                var newDirInfo = new DirectoryInfo(newHref);

                if(oldDirectoryInfo.Exists && !newDirInfo.Exists)
                {
                    Directory.Move(oldDirectoryInfo.FullName, newDirInfo.FullName);
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void MoveFileManagerItem(string draggedItem, string droppedOn)
        {
            var draggedItemUriSegments = System.Web.HttpUtility.ParseQueryString(draggedItem);
            var droppedOnUriSegments = System.Web.HttpUtility.ParseQueryString(droppedOn);

            var draggedFile =  draggedItemUriSegments["file"];
            var draggedCurrPath = draggedItemUriSegments["currpath"];
            var droppedCurrPath = droppedOnUriSegments["currpath"];
            
            droppedCurrPath = droppedCurrPath.Replace(GetBasePath(), "~/").Replace("\\", "/").Replace("//", "/").Replace("~/", "/");

            if (!droppedCurrPath.Contains("/"))
            {
                throw new Exception("You cannot move the item to this folder, you can only move it to folders under /media/uploads/");
            }

            var toDirectory = GetBasePath() + droppedCurrPath;                      

            if (draggedFile != "" && draggedFile != null)
            {
                var filePath = GetBasePath() + draggedFile;

                if (File.Exists(filePath))
                {
                    var fileInfo = new FileInfo(filePath);

                    File.Move(fileInfo.FullName, toDirectory +"\\"+ fileInfo.Name);
                }
            }
            else if (draggedCurrPath != "" && draggedCurrPath != null)
            {
                var dirPath = GetBasePath() + draggedCurrPath;

                if (Directory.Exists(dirPath))
                {
                    var dirInfo = new DirectoryInfo(dirPath);

                    Directory.Move(dirInfo.FullName, toDirectory +"\\"+ dirInfo.Name);
                }
            }

        }
    }
}