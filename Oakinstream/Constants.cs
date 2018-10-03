using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oakinstream
{
    public class Constants
    {
        public static string SiteName { get; } = "Oakinstream";

        public const int ProjectItemsPerPage = 10;
        public const int ProjectImageMinWidth = 200;
        public const int ProjectImageMaxWidth = 1000;
        public const int ProjectThumbnailMaxWidth = 500;

        public const int NumberOfProjectFiles = 5;
        public const int NumberOfProjectComments = 5;
        public const string ProjectImagePath = "~/Content/Images/ProjectImages/";
        public const string ProjectThumbnailPath = "~/Content/Images/ProjectImages/Thumbnails/";

        public const int BlogItemsPerPage = 10;
        public const int BlogImageMinWidth = 200;
        public const int BlogImageMaxWidth = 1000;
        public const int BlogThumbnailMaxWidth = 250;

        public const int NumberOfBlogImages = 5;
        public const string BlogImagePath = "~/Content/Images/BlogImages/";
        public const string BlogThumbnailPath = "~/Content/Images/BlogImages/Thumbnails/";

        public static long MegabytesToBytes(int MB)
        {
            return (MB * 1048576);
        }
        public static int BytesToMegabytes(long bytes)
        {
            return (int)(bytes / 1048576);
        }

        public static int MaxFileSizeMB { get; } = 10;
        public static string ProjectFilePath { get; } = "~/Content/Files/ProjectFiles/";

        public static int DescriptionLengthIndex { get; } = 65;

    }
}