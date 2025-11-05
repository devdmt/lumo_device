//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using DAL.ViewModels;
//using Dapper;

//namespace API.Infrastructure.Application.Clients
//{
//    internal partial class ClaimManager
//    {
//        public async Task<string> getToken(EndPointType endPointType)
//        {

            
//            string token = null;
//            var authdetails = _db.Connection.QueryFirstOrDefault<AppAuth>("select * from AppAuths where EndPointType=" + (int)endPointType + " and isnull(Active,'0')='1'");

//            token = (string) await _db.Connection.ExecuteScalarAsync("select top 1 isnull(TokenId,'') as token from [aPPTokens]" +
//                " where (expiresIn -20)>DATEDIFF(second,GeneratedOn,GETDATE()) and endPointType =" + (int)endPointType + " order by Id desc");

//            if (string.IsNullOrEmpty(token))
//            {
//                try
//                {
//                    switch (endPointType)
//                    {
//                        case EndPointType.TOKEN:
//                            var authbytes = ASCIIEncoding.UTF8.GetBytes(authdetails.userName + ":" + authdetails.Password);
//                            string encodedAuth = Convert.ToBase64String(authbytes);
//                            //var data = new ESBLogin()
//                            //{
//                            //    authparam = "bWJhbmt1c2VyOlhVM2VCS1BmdDhOeTI5VlVJSHlWVmR6MzllV2JLWTN0cWdQY1pyY1c="
//                            //};

//                            var tokenres =await requestToken<LoginRes>(endPointType, authdetails, encodedAuth);
//                           if(tokenres == null)
//                            {
//                                 tokenres =await requestToken<LoginRes>(endPointType, authdetails, encodedAuth);
//                            }
//                               token = tokenres.ResponseBody.jwt.token;
//                               //token = tokenres.access_token;

//                            if(tokenres != null)
//                            {
//                            //     var dt = DateTime.Now.AddSeconds(Convert.ToInt16(tokenres.ResponseBody.jwt.expiry)).ToString("dd-MMM-yyyy HH:mm:ss");
//                            //string query = "exec CreateToken '" + tokenres.ResponseBody.jwt.token + "'," + Convert.ToInt16(tokenres.ResponseBody.jwt.expiry) 
//                            //        + "," + (int)endPointType + "," +
//                            //    "'" + dt + "'";
//                            //await _db.Connection.ExecuteAsync(query);
//                            }
                           
                         
//                            break;
//                        //case EndPointType.Mpesa:
//                        //    var mpesalogindata = new MpesaLogin()
//                        //    {
//                        //        password = url.Password,
//                        //        username = url.userName,
//                        //        scope = ""
//                        //    };

//                        //    tokenres = requestToken<LoginRes>(endPointType, url, mpesalogindata);

//                        //    query = "exec CreateToken '" + tokenres.token + "'," + Convert.ToInt16(tokenres.expiration) + "," + (int)endPointType + ",'" + tokenres.expiration + "'";
//                        //    await _db.Connection.ExecuteAsync(query);
//                        //    token = tokenres.token;
//                        //    break;
//                        default:
//                            // Do Something
//                            break;


//                    }


//                }
//                catch (Exception x)
//                {
//                    // _settings.LogErrors(x.Message, "getToken");
//                  _settings.LogRequests( "getToken", string.Format("{0}"+x.Message,"getToken"), Log_Type.Error);

//                }


//            }
//            return token;
//        }
//        public async Task<T> requestToken<T>(EndPointType endPointType, AppAuth appAuth, object param)
//        {
//            var token = new object();
//            var response = new HttpResponseMessage();
//            try
//            {
//                StringContent data = null;
//                using var client = new HttpClient();
//                if (endPointType == EndPointType.TOKEN)
//                {
//                    using (var requestMessage =
//               new HttpRequestMessage(HttpMethod.Get, appAuth.Url))
//                    {
//                        requestMessage.Headers.Authorization =
//                            new AuthenticationHeaderValue("Basic", param.ToString());

//                      var Authresult= await  client.SendAsync(requestMessage);
//                         var result1 =await Authresult.Content.ReadAsStringAsync();
//            JsonSerializerOptions options1 = new JsonSerializerOptions();
//            if (result1 == null || result1 == "")
//            {
//                throw new ArgumentNullException();
//            }
//            var res1 = System.Text.Json.JsonSerializer.Deserialize<T>(result1);

//            return res1;
//             }
                    
//                }
//                else
//                {
//                    var json = System.Text.Json.JsonSerializer.Serialize(param);

//                    HttpClientHandler handler = new HttpClientHandler()
//                    {
//                        //  Proxy = new WebProxy(_configs.ProxyIp, Convert.ToInt32(_configs.ProxyPort)),
//                        UseProxy = false,
//                    };
//                    //using var client = new HttpClient();

//                    response = client.PostAsync(appAuth.Url, data).Result;
//                }

//                // _configs.Processurl.Trim(); // "http://127.0.0.1:5000/api/B2BServices";


//            }
//            catch (Exception ex)
//            {
//                _settings.LogRequests("requestToken", string.Format( "{0} : " +ex.Message,"requestToken"),Log_Type.Error);
//                //return null;
//            }
//            var result = response.Content.ReadAsStringAsync().Result;
//            JsonSerializerOptions options = new JsonSerializerOptions();
//            if (result == null || result == "")
//            {
//                throw new ArgumentNullException();
//            }
//            var res = System.Text.Json.JsonSerializer.Deserialize<T>(result);

//            return res;
//        }
//    }
//}
