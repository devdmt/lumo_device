
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Mapster;
using DAL.ViewModels.ClaimDTO;
using DAL.ModelView;
using DAL.ModelView.ClaimDTO;
using DAL.Model.ClaimDTO;


namespace API.Infrastructure.Application.Clients
{
    internal partial class ClientClaims
    {
        public async Task<List<DeviceModels>> GetDeviceModel()
        {
            var response= new List<DeviceModels>();
            try
            {
                string query = "SELECT [Id],REPLACE(REPLACE(Rtrim(Ltrim(trim([Brand]))), CHAR(13), ''), CHAR(10), '') as Brand,REPLACE(REPLACE(Rtrim(Ltrim(trim([Model]))), CHAR(13), ''), CHAR(10), '') as Model,[Active] FROM [dbo].[PhoneModels]";
                var devices = await _db.Connection.QueryAsync(query);
                response=devices.Adapt<List<DeviceModels>>();   
                return response;

            } catch (Exception ex)
            {

            }
            return response;
        }
        public async Task<ResponseDTO<List<ClaimsDTO>>> GetAutoApproved(ApprovalDTO status)
        {
           var response = new ResponseDTO<List<ClaimsDTO>>();
            IEnumerable<dynamic> claims = null;
               int NoOfRecords = 0;
                int TotalNoOfPages = 0;
            var _param = new DynamicParameters();
            try
            {
                 string criteria = "";
                bool autoapproved = false;
                
                autoapproved = status.AutoApproval??false;
            
                _param.Add("Id",status.Id);
                
            _param.Add("Idnumber",status.Idnumber);
            _param.Add("phonenumber",status.phonenumber);
            _param.Add("datefrom",status.datefrom);
            _param.Add("dateto",status.dateto);
            _param.Add("claimtype",status.claimStatus == null? null:(int)status.claimStatus);
            _param.Add("claimref",status.ClaimRefNumber);
            _param.Add("autoapproved", autoapproved);
            _param.Add("manualapproved",!autoapproved);
            _param.Add("Skip",status.Skip);
            _param.Add("Take",status.Take);
                  _param.Add("NoOfRecords",NoOfRecords, direction: ParameterDirection.Output);
            _param.Add("TotalNoOfPages",TotalNoOfPages, direction: ParameterDirection.Output);
             claims= await _db.Connection.QueryAsync("GetClaims",_param,
                 commandType:CommandType.StoredProcedure); 
                //response = await _db.Connection.QueryAsync<List<ClaimsDTO>>("GetClaims",_param,
                //commandType: System.Data.CommandType.StoredProcedure);
              NoOfRecords =  _param.Get<int>("NoOfRecords");
              TotalNoOfPages =  _param.Get<int>("TotalNoOfPages");
           response.Result = claims.Adapt<List<ClaimsDTO>>();
            response.NoOfRecords = NoOfRecords;
                response.TotalNoOfPages = TotalNoOfPages;
                return response;

            } catch (Exception ex)
            {

            }
            return response;    
        }
        public async Task<ResponseDTO<List<DeviceDTO>>> GetDevices(Devicesearch devicesearch)
        {
             var response = new ResponseDTO<List<DeviceDTO>>();
              int NoOfRecords = 0;
                int TotalNoOfPages = 0;
            var _param = new DynamicParameters();
            try
            {
            _param.Add("Idnumber",devicesearch.Idnumber);
            _param.Add("phonenumber",devicesearch.phonenumber);
            _param.Add("phonemodel",devicesearch.phonemodel);
            _param.Add("Imeinumber",devicesearch.Imeinumber);
            _param.Add("customerId",devicesearch.customerId); 
            _param.Add("active",devicesearch.isActive);
            _param.Add("Skip",devicesearch.Skip);
            _param.Add("Take",devicesearch.Take);
            _param.Add("NoOfRecords",NoOfRecords, direction: ParameterDirection.Output);
            _param.Add("TotalNoOfPages",TotalNoOfPages, direction: ParameterDirection.Output);
            var devices= await _db.Connection.QueryAsync("Getdevices",_param,
                 commandType:CommandType.StoredProcedure); 
                
              NoOfRecords =  _param.Get<int>("NoOfRecords");
              TotalNoOfPages =  _param.Get<int>("TotalNoOfPages");
           response.Result= devices.Adapt<List<DeviceDTO>>();
                response.NoOfRecords = NoOfRecords;
                response.TotalNoOfPages = TotalNoOfPages;

            } catch(Exception ex) 
            {
            _settings.LogRequests("GetDevices", ex.Message, Interface.RequestType.Error);
            }
            return response;
        }
       public async Task<ResponseDTO<List<CustomerDTO>>> GetCustomers(Devicesearch customersearch)
        {
            var response = new ResponseDTO<List<CustomerDTO>>();
              int NoOfRecords = 0;
                int TotalNoOfPages = 0;
            var _param = new DynamicParameters();
            try
            {
                 
            _param.Add("Idnumber",customersearch.Idnumber);
            _param.Add("phonenumber",customersearch.phonenumber);
            _param.Add("Skip",customersearch.Skip);
            _param.Add("Take",customersearch.Take);
            _param.Add("NoOfRecords",NoOfRecords, direction: ParameterDirection.Output);
            _param.Add("TotalNoOfPages",TotalNoOfPages, direction: ParameterDirection.Output);
            var customers= await _db.Connection.QueryAsync("GetCustomers",_param,
                 commandType:CommandType.StoredProcedure); 
                
              NoOfRecords =  _param.Get<int>("NoOfRecords");
              TotalNoOfPages =  _param.Get<int>("TotalNoOfPages");
           response.Result= customers.Adapt<List<CustomerDTO>>();
                response.NoOfRecords = NoOfRecords;
                response.TotalNoOfPages = TotalNoOfPages;
            } catch (Exception ex)
            {
                _settings.LogRequests("GetCustomers", ex.Message,  Interface.RequestType.Error);
            }
            return response;
        }
    }
}
