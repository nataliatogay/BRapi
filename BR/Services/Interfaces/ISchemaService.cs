using BR.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface ISchemaService
    {
        Task AddNewSchema(NewSchemaRequest newScheme, string clientIdentityId);
        Task UpdateSchema(UpdateSchemaRequest updateSchemeRequest);
    }
}
