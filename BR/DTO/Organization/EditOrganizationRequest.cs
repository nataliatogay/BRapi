﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Organization
{
    public class UpdateOrganizationRequest
    {
        public int OrganizationId { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
