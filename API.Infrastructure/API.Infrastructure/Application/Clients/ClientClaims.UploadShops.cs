using DAL.ViewModels;

using System.Text.RegularExpressions;

using Dapper;
using System.Security.Claims;
using DAL.Common;

using System.Security.Policy;
using Mapster;
using Newtonsoft.Json;
using DAL.ModelView.ClaimDTO;
namespace API.Infrastructure.Application.Clients
{
    internal partial class ClientClaims
    {
        public async Task<PaginationResponse<ShopDTO>> getShops(ShopFilters shopFilters)
        {
            var shops = new List<ShopDTO>();
            try
            {
                    int maxPagSize = Int32.MaxValue;
                    shopFilters.PageSize = (shopFilters.PageSize > 0 && shopFilters.PageSize <= maxPagSize) ? shopFilters.PageSize : maxPagSize;

                    int skip = (shopFilters.PageNumber - 1) * shopFilters.PageSize;
                    int take = shopFilters.PageSize;
                string filtercriteria = "";
                if (!string.IsNullOrEmpty(shopFilters.Shopname))
                {
                    filtercriteria = " and ShopName like '%" + shopFilters.Shopname +"%'";
                }

                   if (!string.IsNullOrEmpty(shopFilters.Shoplocation))
                {
                    filtercriteria = " and ShopLocation like '%" + shopFilters.Shoplocation +"%'";
                }
                if (!string.IsNullOrEmpty(shopFilters.Phonenumber))
                {
                      filtercriteria = " and right(Phonenumber,9) = right('" + shopFilters.Phonenumber +"',9)";
                }
                  if (!string.IsNullOrEmpty(shopFilters.ContactPerson))
                {
                      filtercriteria = " and ContactName like '%" + shopFilters.ContactPerson +"%'";
                }
                string query = "SELECT [Id],[ShopName],[Phonenumber],[ShopLocation],[County],[Subcountry],[Ward],[Town],[Address],[SecondaryPhone],[Email],[ContactName],[ShopOwner],[iSActive],[CreatedBy]" +
                    ",[CreatedName],[UpdatedBy],[UpdatedName],[CreatedDate],[UpdatedDate],[IsDeleted],[DeletedOn],[DeletedById],[DeletedByUser],[loginType],[shopType]  FROM [dbo].[Shops]" +
                    " where isnull(IsDeleted,'0')<>'1' "+ filtercriteria +"   ORDER BY ShopName asc    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
                var results = await _db.Connection.QueryAsync<ShopDTO>(query,new
                    {
                        Skip = skip,
                        Take = take,
                    });
                shops= results.ToList();
            }
            catch (Exception ex) { 

                 // _settin.LogRequests("getShops", ex.Message, Log_Type.Error);
            }
         return    new PaginationResponse<ShopDTO>(shops,shops.Count, shopFilters.PageNumber, shopFilters.PageSize); 

        }
       
        public async void sendShopDetails(SafDetailsShop safDetails)
        {
            try
            {
              //  await _Jsonrequest.SendJson(
                

            }
            catch (Exception ex) { 
            
            }

        }
    }
}
