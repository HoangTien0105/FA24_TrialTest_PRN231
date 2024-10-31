using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PersonVirusRepository : GenericRepository<PersonVirus>, IPersonVirusRepository
    {
    }
}
