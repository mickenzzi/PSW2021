﻿using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Repository.Inteface
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        public Medicine GetMedicineById(string id);
    }
}
