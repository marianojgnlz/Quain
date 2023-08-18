﻿using Microsoft.EntityFrameworkCore;
using Quain.Domain.Models;

namespace Quain.Repository.Sales
{
    public class SalesRepository : ISalesRepository
    {
        private readonly QuainRadioContext _context;

        public SalesRepository(QuainRadioContext context)
        {

            _context = context;

        }

        public async Task<Sale> GetSale(string nComp, string codClient)
            => await _context.FN_GetBillAmountByNCompAndCodClient(nComp, codClient).FirstOrDefaultAsync() 
                ?? throw new ApplicationException($"La factura {nComp} no fue emitida para el cliente {codClient}.");
    }
}
