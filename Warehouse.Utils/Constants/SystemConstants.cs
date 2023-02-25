using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Utils.Constants
{
    public static class SystemConstants
    {
        public const string SuccessMessage = "SuccessMessage";
        public const string WarningMessage = "WarningMessage";
        public const string InfoMessage = "InfoMessage";
        public const string ErrorMessage = "ErrorMessage";

        public const string ImageResizerBlogImageSettings = "maxwidth=1600;maxheight=1600;quality=90;autorotate=false";
        public const string ImageResizerBlogThumbImageSettings = "maxwidth=100;maxheight=100;autorotate=false";
        public const string ImageResizerServiceImageSettings = "maxwidth=1600;maxheight=1600;quality=90;autorotate=false";
        public const string ImageResizerServiceThumbImageSettings = "maxwidth=100;maxheight=100;autorotate=false";
        public static string BlogServiceImagePath = ConfigurationManager.AppSettings["BlogService.ImagePath"];
        public static string BlogServiceImageThumbPath = ConfigurationManager.AppSettings["BlogService.ImagePath"] + "thumbs\\";
        public static string BlogServiceTempImagePath = ConfigurationManager.AppSettings["BlogService.TempImagePath"];
        public static string BlogServiceTempImageThumbPath = ConfigurationManager.AppSettings["BlogService.TempImagePath"] + "thumbs\\";
        public static string BlogImagePath = ConfigurationManager.AppSettings["Blog.ImagePath"];
        public static string BlogImageThumbPath = ConfigurationManager.AppSettings["Blog.ImagePath"] + "thumbs/";
        public static string BlogTempImagePath = ConfigurationManager.AppSettings["Blog.TempImagePath"];
        public static string BlogTempImageThumbPath = ConfigurationManager.AppSettings["Blog.TempImagePath"] + "thumbs/";

        public static string ServiceServiceImagePath = ConfigurationManager.AppSettings["ServiceService.ImagePath"];
        public static string ServiceServiceImageThumbPath = ConfigurationManager.AppSettings["ServiceService.ImagePath"] + "thumbs\\";
        public static string ServiceServiceTempImagePath = ConfigurationManager.AppSettings["ServiceService.TempImagePath"];
        public static string ServiceServiceTempImageThumbPath = ConfigurationManager.AppSettings["ServiceService.TempImagePath"] + "thumbs\\";
        public static string ServiceImagePath = ConfigurationManager.AppSettings["Service.ImagePath"];
        public static string ServiceImageThumbPath = ConfigurationManager.AppSettings["Service.ImagePath"] + "thumbs/";
        public static string ServiceTempImagePath = ConfigurationManager.AppSettings["Service.TempImagePath"];
        public static string ServiceTempImageThumbPath = ConfigurationManager.AppSettings["Service.TempImagePath"] + "thumbs/";



        public static string SliderServiceImagePath = ConfigurationManager.AppSettings["SliderService.ImagePath"];
        public static string SliderServiceImageThumbPath = ConfigurationManager.AppSettings["SliderService.ImagePath"] + "thumbs\\";
        public static string SliderImagePath = ConfigurationManager.AppSettings["Slider.ImagePath"];
        public static string SliderImageThumbPath = ConfigurationManager.AppSettings["Slider.ImagePath"] + "thumbs/";
        public const int DefaultBlogPageSize = 10;
        public const int DefaultServicePageSize = 10;
        public const int DefaultPropertyPageSize = 10;
        public const int DefaultKeywordPageSize = 10;
        public const int DefaultCountryPageSize = 10;
        public const int DefaultOrderPageSize = 5;

        public static string PropertyServiceImagePath = ConfigurationManager.AppSettings["PropertyService.ImagePath"];
        public static string PropertyServiceImageThumbPath = ConfigurationManager.AppSettings["PropertyService.ImagePath"] + "thumbs\\";
        public static string PropertyServiceTempImagePath = ConfigurationManager.AppSettings["PropertyService.TempImagePath"];
        public static string PropertyServiceTempImageThumbPath = ConfigurationManager.AppSettings["PropertyService.TempImagePath"] + "thumbs\\";
        public static string PropertyImagePath = ConfigurationManager.AppSettings["Property.ImagePath"];
        public static string PropertyImageThumbPath = ConfigurationManager.AppSettings["Property.ImagePath"] + "thumbs/";
        public static string PropertyTempImagePath = ConfigurationManager.AppSettings["Property.TempImagePath"];
        public static string PropertyTempImageThumbPath = ConfigurationManager.AppSettings["Property.TempImagePath"] + "thumbs/";

        public static string ReferenceServiceImagePath = ConfigurationManager.AppSettings["ReferenceService.ImagePath"];
        public static string ReferenceServiceImageThumbPath = ConfigurationManager.AppSettings["ReferenceService.ImagePath"] + "thumbs\\";
        public static string ReferenceServiceTempImagePath = ConfigurationManager.AppSettings["ReferenceService.TempImagePath"];
        public static string ReferenceServiceTempImageThumbPath = ConfigurationManager.AppSettings["ReferenceService.TempImagePath"] + "thumbs\\";
        public static string ReferenceImagePath = ConfigurationManager.AppSettings["Reference.ImagePath"];
        public static string ReferenceImageThumbPath = ConfigurationManager.AppSettings["Reference.ImagePath"] + "thumbs/";
        public static string ReferenceTempImagePath = ConfigurationManager.AppSettings["Reference.TempImagePath"];
        public static string ReferenceTempImageThumbPath = ConfigurationManager.AppSettings["Reference.TempImagePath"] + "thumbs/";

        public static string SettingServiceImagePath = ConfigurationManager.AppSettings["SettingService.ImagePath"];
        public static string SettingServiceImageThumbPath = ConfigurationManager.AppSettings["SettingService.ImagePath"] + "thumbs\\";
        public static string SettingServiceTempImagePath = ConfigurationManager.AppSettings["SettingService.TempImagePath"];
        public static string SettingServiceTempImageThumbPath = ConfigurationManager.AppSettings["SettingService.TempImagePath"] + "thumbs\\";
        public static string SettingImagePath = ConfigurationManager.AppSettings["Setting.ImagePath"];
        public static string SettingImageThumbPath = ConfigurationManager.AppSettings["Setting.ImagePath"] + "thumbs/";
        public static string SettingTempImagePath = ConfigurationManager.AppSettings["Setting.TempImagePath"];
        public static string SettingTempImageThumbPath = ConfigurationManager.AppSettings["Setting.TempImagePath"] + "thumbs/";

        public static string GalleryServiceImagePath = ConfigurationManager.AppSettings["GalleryService.ImagePath"];
        public static string GalleryServiceImageThumbPath = ConfigurationManager.AppSettings["GalleryService.ImagePath"] + "thumbs\\";
        public static string GalleryServiceTempImagePath = ConfigurationManager.AppSettings["GalleryService.TempImagePath"];
        public static string GalleryServiceTempImageThumbPath = ConfigurationManager.AppSettings["GalleryService.TempImagePath"] + "thumbs\\";
        public static string GalleryImagePath = ConfigurationManager.AppSettings["Reference.ImagePath"];
        public static string GalleryImageThumbPath = ConfigurationManager.AppSettings["Gallery.ImagePath"] + "thumbs/";
        public static string GalleryTempImagePath = ConfigurationManager.AppSettings["Gallery.TempImagePath"];
        public static string GalleryTempImageThumbPath = ConfigurationManager.AppSettings["Gallery.TempImagePath"] + "thumbs/";

        public static string CustomerCommentServiceImagePath = ConfigurationManager.AppSettings["CustomerCommentService.ImagePath"];
        public static string CustomerCommentServiceImageThumbPath = ConfigurationManager.AppSettings["CustomerCommentService.ImagePath"] + "thumbs\\";
        public static string CustomerCommentServiceTempImagePath = ConfigurationManager.AppSettings["CustomerCommentService.TempImagePath"];
        public static string CustomerCommentServiceTempImageThumbPath = ConfigurationManager.AppSettings["CustomerCommentService.TempImagePath"] + "thumbs\\";
        public static string CustomerCommentImagePath = ConfigurationManager.AppSettings["CustomerComment.ImagePath"];
        public static string CustomerCommentImageThumbPath = ConfigurationManager.AppSettings["CustomerComment.ImagePath"] + "thumbs/";
        public static string CustomerCommentTempImagePath = ConfigurationManager.AppSettings["CustomerComment.TempImagePath"];
        public static string CustomerCommentTempImageThumbPath = ConfigurationManager.AppSettings["CustomerComment.TempImagePath"] + "thumbs/";


    }
}
