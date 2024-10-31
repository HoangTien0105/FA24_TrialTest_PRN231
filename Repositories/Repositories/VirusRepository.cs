﻿using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class VirusRepository : GenericRepository<Virus>, IVirusRepository
    {
    }
}
