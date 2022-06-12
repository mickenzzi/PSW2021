using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pharmacy.Model;
using Pharmacy.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IMedicineRepository _medicineRepository;

        public GreeterService(ILogger<GreeterService> logger, IMedicineRepository medicineRepository)
        {
            _logger = logger;
            _medicineRepository = medicineRepository;
        }

        public override Task<MedicineResponse> ShareMedicine(MedicineRequest request, ServerCallContext context)
        {
            Medicine medicine = GetMedicine(request.Name, request.Quantity);
            if (medicine.Quantity != 0)
            {
                medicine.Quantity = medicine.Quantity - request.Quantity;
                _medicineRepository.Update(medicine);
            }
            return Task.FromResult(new MedicineResponse
            {
                Id = medicine.Id,
                Name = medicine.Name,
                Dose = medicine.Dose,
                Quantity = medicine.Quantity
            }) ;
        }

        private Medicine GetMedicine(string name, int quantity)
        {
            List<Medicine> medicines = _medicineRepository.GetAll();
            Medicine medicine = new Medicine();
            foreach(Medicine m in medicines)
            {
                if (m.Name.Equals(name) && m.Quantity >= quantity)
                {
                    medicine = m;
                }
            }
            return medicine;
        }


    }
}
