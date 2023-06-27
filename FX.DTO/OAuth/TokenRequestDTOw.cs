using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.DTO.OAuth
{
    public record TokenRequestDTOw
    {
        public string RefreshToken { get; set; }
    }
}
