using AutomationFramework.Data.Models;

namespace AutomationFramework.Data
{
    public interface IDataProvider
    {
        LoginData GetLoginData();
        ServicingCreateData GetServicingCreateData();
        ServicingUpdateData GetServicingUpdateData();
    }
}