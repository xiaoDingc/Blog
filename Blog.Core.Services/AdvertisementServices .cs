namespace Blog.Core.Services
{
    using Blog.Core.IRepository;
    using Blog.Core.IServices;
    using Blog.Core.Model;
    using Blog.Core.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Blog.Core.Services.Base;

    /// <summary>
    /// The advertisement services.
    /// </summary>
    public class AdvertisementServices:BaseServices<Advertisement>,IAdvertisementServices
    {
        // IAdvertisementRepository iAdvertisementRepository;
     
    }
}