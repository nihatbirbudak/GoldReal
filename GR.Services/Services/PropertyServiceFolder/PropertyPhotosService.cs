using Azure;
using GR.Core.Interface;
using GR.Models.Entities.Property;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.PropertyServiceFolder
{
    public class PropertyPhotosService : ServiceBase<PropertyPhoto, int>, IPropertyPhotosService
    {
        private readonly IUnitOfWork unitOfWork;
        public PropertyPhotosService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<PropertyPhoto>> GetPhotosByPropertyIdAsync(int propertyId)
        {
            return (await unitOfWork.GetRepository<PropertyPhoto, int>().GetAllAsync(filter: x => x.PropertyId == propertyId)).ToList();
        }

        public async Task<int> CurentCounter(int propertyId)
        {
            var c = await unitOfWork.GetRepository<PropertyPhoto, int>().CountAsync(x => x.PropertyId == propertyId);
            return c;
        }

        public async Task<int> MaxCounter(int propertyId)
        {
            var c = await unitOfWork.GetRepository<PropertyPhoto, int>().GetAsync(
                                                        filter: x => x.PropertyId == propertyId,
                                                        orderBy: q => q.OrderByDescending(x => x.SortOrder));
            return c?.SortOrder ?? 0;
        }

        public async Task<PropertyPhoto>? GetByPropertyIdAndPhotoId(int propertyId, int photoId)
        {
            var p = await unitOfWork.GetRepository<PropertyPhoto, int>().GetAsync(x => x.PropertyId == propertyId && x.Id == photoId);
            return p!;
        }

        public async Task<PropertyPhoto>? GetCurrentCover(int propertyId)
        {
            var p = await unitOfWork.GetRepository<PropertyPhoto, int>().GetAsync(x => x.PropertyId == propertyId && x.IsCover == true);
            return p!;
        }

        public async Task<PropertyPhoto> GetDeleted(int propertyId, int photoId)
        {
            var p = await unitOfWork.GetRepository<PropertyPhoto, int>().GetAsync(x => x.PropertyId == propertyId && x.Id == photoId);
            return p!;

        }

        public async Task<List<PropertyPhoto>> GetCurrentCovers(int page, int pageSize)
        {
            return (await unitOfWork.GetRepository<PropertyPhoto, int>().GetAllProjectedAsync(c => c.IsCover == true,
                selector: c => new PropertyPhoto
                {
                    Id = c.Id,
                    PropertyId = c.PropertyId,
                    Url = c.Url,
                    IsCover = c.IsCover,
                    SortOrder = c.SortOrder
                },
                orderBy: q => q.OrderByDescending(c => c.Id),
                skip: Math.Max(0, (page - 1) * pageSize),
                take: pageSize > 0 ? pageSize : 10)).ToList();
        }
    }
}
