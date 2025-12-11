using EduTrack.Service.DTOs.Payments;
using EduTrack.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EduTrack.Service.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<PaymentResultDto> AddAsync(PaymentCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PaymentResultDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResultDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentResultDto> UpdateAsync(int id, PaymentUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
