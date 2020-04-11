using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Organization
{
    public class OrganizationInfoResponse
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string LogoPath { get; set; }
    }
}
