using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEVTRACKR.API.Entities;

namespace DEVTRACKR.API.Persistence
{
    public class DevTrackRContext
    {
        public DevTrackRContext()
        {
            Packages = new List<Package>(); 
        }
        public List<Package> Packages { get; set;}
    }
}