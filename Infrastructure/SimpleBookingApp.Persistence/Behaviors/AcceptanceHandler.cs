using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBookingApp.Persistence.Behaviors
{
    public class AcceptanceHandler<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<AcceptanceHandler<TRequest, TResponse>> _logger;

        public AcceptanceHandler(ILogger<AcceptanceHandler<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Request işlemesi öncesi loglama
            _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);

            // İşlemi başlat
            var response = await next();

            // İşlem sonrası başarılı loglama
            _logger.LogInformation("{Request} handled successfully", typeof(TRequest).Name);

            return response;
        }
    }
}
