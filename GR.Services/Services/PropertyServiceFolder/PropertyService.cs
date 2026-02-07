using GR.Core.Interface;
using GR.Models.DTOs;
using GR.Models.Entities.Property;
using GR.Models.Enums;
using GR.Models.ViewModels.PropertyViewModelFolder;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.PropertyServiceFolder
{
    public class PropertyService : ServiceBase<Property, int>, IPropertyService 
    {
        private readonly IUnitOfWork _unitOfWork;
        public PropertyService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Property>> GetPropertiesByUserId(string userId)
        {
            return (await _unitOfWork.GetRepository<Property, int>()
                .GetAllAsync(filter: p => p.OwnerId == userId)).ToList();
        }

        public async Task<PagedResult<PropertyListItemDTO>> GetPageAsync(int page, int pageSize, string? ownerId = null)
        {
            var q = new PropertyListQuery
            {
                Page = page <= 0 ? 1 : page,
                PageSize = pageSize <= 0 ? 12 : pageSize,
                OwnerId = ownerId,
                // Varsayılanlar: liste ilk açılış davranışınla uyumlu olacak şekilde set edebilirsin
                IsActive = null,
                IsSold = null,
                SortBy = PropertySortBy.CreatedAt,
                SortDir = SortDir.Desc
            };

            return await GetPageAsync(q);
        }

        public async Task<PagedResult<PropertyListItemDTO>> GetPageAsync(PropertyListQuery q)
        {
            var repo = _unitOfWork.GetRepository<Property, int>();
            var page = q.Page <= 0 ? 1 : q.Page;
            var pageSize = q.PageSize <= 0 ? 12 : q.PageSize;
            var skip = (page - 1) * pageSize;

            // --- FILTER ---
            // EF.Functions.Like ile case-insensitive arama (SQL Collation'a göre değişebilir)
            string? search = string.IsNullOrWhiteSpace(q.Search) ? null : $"%{q.Search.Trim()}%";

            Expression<Func<Property, bool>> filter = p =>
                (string.IsNullOrEmpty(q.OwnerId) || p.OwnerId == q.OwnerId) &&
                (!q.IsActive.HasValue || p.IsActive == q.IsActive.Value) &&
                (!q.IsSold.HasValue || p.IsSold == q.IsSold.Value) &&
                (!q.MinPrice.HasValue || (p.Price ?? 0m) >= q.MinPrice.Value) &&
                (!q.MaxPrice.HasValue || (p.Price ?? 0m) <= q.MaxPrice.Value) &&
                (!q.CityId.HasValue || p.CityId == q.CityId.Value) &&
                (!q.DistrictId.HasValue || p.DistrictId == q.DistrictId.Value) &&
                (!q.NeighborhoodId.HasValue || p.NeighborhoodId == q.NeighborhoodId.Value) &&
                (!q.CategoryId.HasValue || p.CategoryId == q.CategoryId.Value) &&
                (!q.TransactionTypeId.HasValue || p.TransactionTypeId == q.TransactionTypeId.Value) &&
                (search == null ||
                  EF.Functions.Like(p.Title ?? "", search) ||
                  EF.Functions.Like(p.Description ?? "", search) ||
                  EF.Functions.Like(p.AddressLine ?? "", search));

            // --- ORDER BY ---
            Func<IQueryable<Property>, IOrderedQueryable<Property>> orderBy = q.SortBy switch
            {
                PropertySortBy.Price => q.SortDir == SortDir.Asc
                    ? qry => qry.OrderBy(p => p.Price ?? 0m)
                    : qry => qry.OrderByDescending(p => p.Price ?? 0m),

                PropertySortBy.Title => q.SortDir == SortDir.Asc
                    ? qry => qry.OrderBy(p => p.Title)
                    : qry => qry.OrderByDescending(p => p.Title),

                _ => q.SortDir == SortDir.Asc
                    ? qry => qry.OrderBy(p => p.CreatedAt)
                    : qry => qry.OrderByDescending(p => p.CreatedAt),
            };

            // --- SELECT (projeksiyon) ---
            Expression<Func<Property, PropertyListItemDTO>> selector = p => new PropertyListItemDTO
            {
                Id = p.Id,
                Title = p.Title,
                AddressLine = p.AddressLine,
                CityName = p.City != null ? p.City.Name : "",
                DistrictName = p.District != null ? p.District.Name : "",
                NeighborhoodName = p.Neighborhood != null ? p.Neighborhood.Name : "",
                Price = p.Price ?? 0m,
                Currency = p.Currency ?? "TRY",
                TransactionTypeName = p.TransactionType != null ? p.TransactionType.Name : "",
                CreatedAt = p.CreatedAt,
                HomePageImagePath = string.IsNullOrWhiteSpace(p.homePageImagePath) ? "assets/img/properties/properties-1.png" : p.homePageImagePath,
                IsActive = p.IsActive,
                ownerFullName = p.Owner != null ? ((p.Owner.Name ?? "") + " " + (p.Owner.Surname ?? "")).Trim() : "",
                IsSold = p.IsSold,
                GrossM2 = p.GrossM2,
            };

            // --- PAGING ---
            var items = (await repo.GetAllProjectedAsync(
                filter: filter,
                selector: selector,
                orderBy: orderBy,
                skip: skip,
                take: pageSize
            )).ToList();

            var total = await repo.CountAsync(filter);

            return new PagedResult<PropertyListItemDTO>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<Property?> GetDetailAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Property, int>();

            // Tüm gerekli navigation’ları tek çağrıda yüklüyoruz:
            var p = await repo.GetAsync(
                filter: x => x.Id == id,
                orderBy: null, // tek kayıt, sıralama gereksiz
                includes: new Expression<Func<Property, object>>[] {
                x => x.TransactionType,
                x => x.Category,
                x => x.Subtype!,
                x => x.City,
                x => x.District,
                x => x.Neighborhood!,
                x => x.Owner,
                x => x.PropertyPhotos
                }
            );

            return p;
        }

    }
}
