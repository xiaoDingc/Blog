namespace Blog.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Blog.Core.IRepository;
    using Blog.Core.IRepository.Base;
    using Blog.Core.Model;
    using Blog.Core.Repository.Base;

    using SqlSugar;

    public class AdvertisementRepository : BaseRepository<Advertisement>,IAdvertisementRepository
    {
       
    }
}