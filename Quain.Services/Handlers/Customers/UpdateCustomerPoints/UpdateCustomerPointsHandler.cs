﻿namespace Quain.Services.Handlers.Customers.UpdateCustomerPoints
{
    using global::AutoMapper;
    using MediatR;
    using Quain.Repository;
    using Quain.Repository.Customers;
    using Quain.Repository.Sales;
    using Quain.Services.DTO;

    public class UpdateCustomerPointsHandler : IRequestHandler<UpdateCustomerPointsCommand, UpdateCustomerPointsResponse>
    {
        private readonly ICustomersRepository _customersRepository;
        private readonly IPointsChangeRepository _pointsChangeRepository;
        private readonly ISalesRepository _salesRepository;
        private readonly IMapper _mapper;

        public UpdateCustomerPointsHandler(ICustomersRepository customersRepository, IPointsChangeRepository pointsChangeRepository, ISalesRepository salesRepository, IMapper mapper)
        {
            _customersRepository = customersRepository;
            _pointsChangeRepository = pointsChangeRepository;
            _salesRepository = salesRepository;
            _mapper = mapper;
        }
        public async Task<UpdateCustomerPointsResponse> Handle(UpdateCustomerPointsCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customersRepository.GetCustomer(request.CodClient, cancellationToken);

            if (customer == null) throw new ApplicationException($"El cliente {request.CodClient} no fue encontrado.");

            var billWasUsed = await _pointsChangeRepository.BillNumberWasUsed(request.NComp);

            if (billWasUsed) throw new ApplicationException($"La factura {request.NComp} ya fue utilizada previamente.");

            var sale = await _salesRepository.GetSale(request.NComp);

            customer.UpdatePoints(ConverToInt(sale.Importe), request.UpdatedBy, request.NComp);

            var customerResult = await _customersRepository.Update(customer, cancellationToken);

            var customerModel = _mapper.Map<CustomerDto>(customerResult);

            return UpdateCustomerPointsResponse.With(customerModel);
        }

        private int ConverToInt(decimal value) => decimal.ToInt32(value);
    }
}
