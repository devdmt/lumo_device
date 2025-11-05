using API.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;

namespace API.Infrastructure.Application;

    public  class Setting:Isettings
    {
    public void LogRequests(string errMsg, string module, RequestType requestType, string request = "")
    {
        try
        {
            //HERE I LOG THE ERRORS BECAUSE I WAS TAUGHT WELL
            //I WILL ALSO CREATE A CLASS FOR MAILING THEM - BECAUSE AM EXEMPLARY
            DateTime currtime = DateTime.Now;
            errMsg = "module: " + module + "  " + requestType.ToString() + ":  " + errMsg + "  " + currtime;
            if(!string.IsNullOrEmpty(request))
            {
                 errMsg = errMsg + "\nrerrorMsg:  " + request;
            }
           
            //string appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string appPath = System.IO.Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Logs";// _config.configs.Logpath;
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }

                appPath = appPath + Path.DirectorySeparatorChar + requestType.ToString() + "_" + String.Format("{0:yyyy-MM-dd}", DateTime.Now).ToString() + "log.msg";
                using (StreamWriter sw = File.AppendText(appPath))
                {
                    sw.WriteLine(errMsg);
                }
            


        }
        catch (Exception ex)
        {

        }
    }
}

