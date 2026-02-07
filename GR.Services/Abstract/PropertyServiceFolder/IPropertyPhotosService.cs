using GR.Core.Interface;
using GR.Models.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.PropertyServiceFolder
{
    public interface IPropertyPhotosService : IServiceBase<PropertyPhoto,int>
    {
        Task<List<PropertyPhoto>> GetPhotosByPropertyIdAsync(int propertyId);
        Task<int> CurentCounter(int propertyId);
        Task<int> MaxCounter(int propertyId);
        Task<PropertyPhoto>? GetByPropertyIdAndPhotoId(int propertyId, int photoId);
        Task<PropertyPhoto>? GetCurrentCover(int propertyId);
        Task<PropertyPhoto> GetDeleted(int propertyId, int photoId);
        Task<List<PropertyPhoto>> GetCurrentCovers(int page, int pageSize);
    }
}
