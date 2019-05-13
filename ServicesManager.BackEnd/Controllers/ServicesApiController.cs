using ServicesManager.BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace ServicesManager.BackEnd.Controllers
{
    public class ServicesApiController : UmbracoApiController
    {

        public IHttpActionResult GetAll()
        {
            UmbracoHelper _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            // Get all Jobs from the Umbraco tree 
            var categories = _umbracoHelper.TypedContentAtXPath("//" + "mainCategory");
            // Map the found nodes from IPublishedContent to a strongly typed object of type Job (defined below)
            var mappedJobs = categories.Select(x => new MainCategories
            {
                Id = x.Id,
                CategoryName = x.GetPropertyValue<string>("categoryName"),
                Categorydesc = x.GetPropertyValue<string>("Categorydesc"),
                ImageUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
    HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + Umbraco.Media(Umbraco.Field(x, "categoryIcon").ToString()).Url //Umbraco.TypedMedia(x.GetPropertyValue("categoryIcon")).Url,
            });
            return Ok(mappedJobs);
        }
        public IHttpActionResult GetAllservices()
        {
            UmbracoHelper _umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            // Get all Jobs from the Umbraco tree 
            var categories = _umbracoHelper.TypedContentAtXPath("//" + "service");
            // Map the found nodes from IPublishedContent to a strongly typed object of type Job (defined below)


          
            var mappedJobs = categories.Select(x => new serviceResponseDto()
            {
                Id = x.Id,
                FullName = x.GetPropertyValue<string>("createdBy"),
                Desc = x.GetPropertyValue<string>("description"),
                Name = x.GetPropertyValue<string>("serviceName"),
                phoneNumber = x.GetPropertyValue<string>("phoneNumber"),
                UserId = x.GetPropertyValue<string>("userId"),
                image = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
   HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + Umbraco.Media(Umbraco.Field(x, "image").ToString()).Url
            });
            return Ok(mappedJobs);
        }
        public IHttpActionResult AddService(ServiceDto serviceDto)
        {
            var newNode = Services.ContentService.CreateContent(serviceDto.Name, serviceDto.ParentID, "service");
            var newImage = Services.MediaService.CreateMedia(serviceDto.Name, -1, "Image");
            System.IO.MemoryStream strm = new MemoryStream(serviceDto.image);
            var ext = GeMimeTypeFromImageByteArray(serviceDto.image).Split('/')[1];
            newImage.SetValue("umbracoFile", serviceDto.Name + "." + ext, strm);
            Services.MediaService.Save(newImage);
            newNode.SetValue("image", newImage.GetUdi().ToString());

            newNode.SetValue("serviceName", serviceDto.Name);
            newNode.SetValue("createdBy", serviceDto.FullName);
            newNode.SetValue("description", serviceDto.Desc);
            newNode.SetValue("phoneNumber", serviceDto.phoneNumber);
            newNode.SetValue("userId", serviceDto.UserId);
            
            //************** END Saving user info ********************

            //saving the pubshing into umbraco content 
            Services.ContentService.SaveAndPublishWithStatus(newNode);
            return Ok(serviceDto);
        }
        public static string GeMimeTypeFromImageByteArray(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            using (Image image = Image.FromStream(stream))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == image.RawFormat.Guid).MimeType;

            }
        }
        public IHttpActionResult GetServiceByCategory(int id)
        {

            var serviceresponsedto = new List<serviceResponseDto>();

            var caegory = Umbraco.TypedContent(id);

            foreach (var item in caegory.Children)
            {
                serviceresponsedto.Add(new serviceResponseDto()
                {
                    Id = item.Id,
                    FullName = item.GetPropertyValue<string>("createdBy"),
                    Desc = item.GetPropertyValue<string>("description"),
                    Name = item.GetPropertyValue<string>("serviceName"),
                    phoneNumber = item.GetPropertyValue<string>("phoneNumber"),
                    image = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
    HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/" + Umbraco.Media(Umbraco.Field(item, "image").ToString()).Url
                });
            }

            return Ok(serviceresponsedto);
        }

       
        public IHttpActionResult DeleteService(int id)
        {
            var serviceresponsedto = new List<serviceResponseDto>();

           

            Services.ContentService.Delete(Services.ContentService.GetById(id));


            return Ok(serviceresponsedto);
        }
    }
}
