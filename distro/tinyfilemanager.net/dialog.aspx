<%@ Page Title="Tiny File Manager" Language="C#" AutoEventWireup="true" CodeBehind="dialog.aspx.cs" Inherits="TinyFileManager.NET.dialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
        <meta name="robots" content="noindex,nofollow">
        <title>Tiny File Manager</title>
        <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
        <link href="css/bootstrap-lightbox.min.css" rel="stylesheet" type="text/css" />
        <link href="css/style.css" rel="stylesheet" type="text/css" />
		<link href="css/dropzone.css" type="text/css" rel="stylesheet" />
        <script type="text/javascript" src="js/jquery.1.9.1.min.js"></script>
        <script type="text/javascript" src="js/bootstrap.min.js"></script>
        <script type="text/javascript" src="js/bootstrap-lightbox.min.js"></script>
		<script type="text/javascript" src="js/dropzone.min.js"></script>
		<script>
	    	var ext_img=new Array(<% Response.Write(TinyFileManager.NET.clsConfig.strAllowedImageExtensions); %>);
	    	var allowed_ext=new Array(<% Response.Write(TinyFileManager.NET.clsConfig.strAllowedAllExtensions); %>);
            var track = '<% Response.Write(this.strEditor); %>';
            var curr_dir = '<% Response.Write(this.strCurrPath.Replace("\\", "\\\\")); %>';

			//dropzone config
			Dropzone.options.myAwesomeDropzone = {
				dictInvalidFileType: "File extension is not allowed",
				dictFileTooBig: "The upload exceeds the max filesize allowed",
				dictResponseError: "SERVER ERROR",
				paramName: "file", // The name that will be used to transfer the file
				maxFilesize: <% Response.Write(TinyFileManager.NET.clsConfig.intMaxUploadSizeMb); %>, // MB
				accept: function(file, done) {
				    var extension=file.name.split('.').pop();
				    if ($.inArray(extension, allowed_ext) > -1) {
				        done();
				    } else { 
                        done("File extension is not allowed"); 
                    }
				}
			};
	    </script>
		<script type="text/javascript" src="js/include.js"></script>
    </head>
    <body>
		
<!----- uploader div start ------->
        <div class="uploader">            
	        <form action="dialog.aspx?cmd=upload" id="myAwesomeDropzone" class="dropzone">
		        <input type="hidden" name="folder" value="<% Response.Write(this.strCurrPath); %>"/>
		        <div class="fallback">
	    	        <input name="file" type="file" multiple />
	  	        </div>
	        </form>
	        <center><button class="btn btn-large btn-primary close-uploader"><i class="icon-backward icon-white"></i> Return to files list</button></center>
	        <div class="space10"></div><div class="space10"></div>
        </div>
<!----- uploader div end ------->

        <div class="container-fluid">
          
          
<!----- header div start ------->
			<div class="filters">
<% if (TinyFileManager.NET.clsConfig.boolAllowUploadFile) { %>
                <button class="btn btn-primary upload-btn" style="margin-left:5px;"><i class="icon-upload icon-white"></i> Upload a file</button> 
<% } %>
<% if (TinyFileManager.NET.clsConfig.boolAllowCreateFolder)
   { %>
			    <button class="btn new-folder" style="margin-left:5px;"><i class="icon-folder-open"></i> New Folder</button> 
<% } %>
<% if ((Convert.ToInt32(this.strType) != 1) && (Convert.ToInt32(this.strType) < 3)) { // not only image or only video %>
                <div class="pull-right">Filter: &nbsp;&nbsp;
			        <input id="select-type-all" name="radio-sort" type="radio" data-item="ff-item-type-all" class="hide" />
                    <label id="ff-item-type-all" for="select-type-all" class="btn btn-info ff-label-type-all">All</label>
                    &nbsp;
                    <input id="select-type-1" name="radio-sort" type="radio" data-item="ff-item-type-1" checked="checked"  class="hide"  />
                    <label id="ff-item-type-1" for="select-type-1" class="btn ff-label-type-1">Files</label>
                    &nbsp;
                    <input id="select-type-2" name="radio-sort" type="radio" data-item="ff-item-type-2" class="hide"  />
                    <label id="ff-item-type-2" for="select-type-2" class="btn ff-label-type-2">Images</label>
                    &nbsp;
                    <input id="select-type-3" name="radio-sort" type="radio" data-item="ff-item-type-3" class="hide"  />
                    <label id="ff-item-type-3" for="select-type-3" class="btn ff-label-type-3">Archives</label>
                    &nbsp;
                    <input id="select-type-4" name="radio-sort" type="radio" data-item="ff-item-type-4" class="hide"  />
                    <label id="ff-item-type-4" for="select-type-4" class="btn ff-label-type-4">Videos</label>
                    &nbsp;
                    <input id="select-type-5" name="radio-sort" type="radio" data-item="ff-item-type-5" class="hide"  />
                    <label id="ff-item-type-5" for="select-type-5" class="btn ff-label-type-5">Music</label>
                </div>
<% } %>
            </div>
<!----- header div end ------->

<!----- breadcrumb div start ------->
			<div class="row-fluid">
				<ul class="breadcrumb">
                <%= this.getBreadCrumb() %>
				</ul>
			</div>
<!----- breadcrumb div end ------->
            
            <div class="row-fluid ff-container">
                <div class="span12 pull-right">
                    <ul class="thumbnails ff-items">                

                    <%  
                        // loop through folder/file list that we have already created
                        foreach (TinyFileManager.NET.clsFileItem objF in this.arrLinks)
                        {
                            //get start of line html, if necessary
                            Response.Write(this.getStartOfLine(objF.intColNum));

                            // start of item
                            Response.Write("<li class=\"span2 ff-item-type-" + objF.strClassType + "\">");
                            Response.Write("<div class=\"boxes thumbnail\">");
                                    
                            if (objF.boolIsFolder)
                            {
                                // if folder
                                Response.Write(objF.strDeleteLink);
                                Response.Write(objF.strLink);
                            }
                            else
                            {
                                // if file
                                Response.Write(objF.strDownFormOpen);
                                Response.Write("<div class=\"btn-group toolbox\">");
                                Response.Write("<button type=\"submit\" title=\"Download\" class=\"btn\"><i class=\"icon-download\"></i></button>");
                                Response.Write(objF.strPreviewLink);
                                Response.Write(objF.strDeleteLink);
                                Response.Write("</div>");
                                Response.Write("</form>");
                                Response.Write(objF.strLink);
                            }      
                                    
                            // end of item
                            Response.Write("</div>");
                            Response.Write("</li>");
                                    
                            //get end of line html, if necessary
                            Response.Write(this.getEndOfLine(objF.intColNum));                                    
                        }
                    %>

                    </ul>
                </div>
            </div>    
        </div>
        
	    <!----- lightbox div end ------->    
	    <div id="previewLightbox" class="lightbox hide fade"  tabindex="-1" role="dialog" aria-hidden="true">
		    <div class='lightbox-content'>
			    <img id="full-img" src="">
		    </div>    
	    </div>
	    <!----- lightbox div end ------->
    </body>
</html>