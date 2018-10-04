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
        public const string ProjectImagePath = "~/Content/Images/ProjectImages/";
        public const string ProjectThumbnailPath = "~/Content/Images/ProjectImages/Thumbnails/";
        public static string ProjectFilePath { get; } = "~/Content/Files/ProjectFiles/";

        public const int BlogItemsPerPage = 10;
        public const int BlogImageMinWidth = 200;
        public const int BlogImageMaxWidth = 1000;
        public const int BlogThumbnailMaxWidth = 250;

        public const int NumberOfBlogImages = 5;
        public const string BlogImagePath = "~/Content/Images/BlogImages/";
        public const string BlogThumbnailPath = "~/Content/Images/BlogImages/Thumbnails/";


        public const int AboutItemsPerPage = 10;
        public const int AboutImageMinWidth = 200;
        public const int AboutImageMaxWidth = 1000;
        public const int AboutThumbnailMaxWidth = 500;

        public const int NumberOfAboutFiles = 5;
        public const string AboutImagePath = "~/Content/Images/AboutImages/";
        public const string AboutThumbnailPath = "~/Content/Images/AboutImages/Thumbnails/";
        public static string AboutFilePath { get; } = "~/Content/Files/AboutFiles/";

        public static long MegabytesToBytes(int MB)
        {
            return (MB * 1048576);
        }
        public static int BytesToMegabytes(long bytes)
        {
            return (int)(bytes / 1048576);
        }

        public static int MaxFileSizeMB { get; } = 10;
        public static int DescriptionLengthIndex { get; } = 65;

    }
}