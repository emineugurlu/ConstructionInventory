using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionInventory.Domain.Entities
{
    public class ConstructionSite
    {
        public int Id { get; set; } 
        public string SiteName { get; set; } // Şantiye adı 
        public string Address { get; set; } // Şantiye adresi
        public string ResponsiblePerson { get; set;  } // Şantiyede ki şef
    }
}
