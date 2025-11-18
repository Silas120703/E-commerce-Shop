using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Interfaces.Services;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using System.Threading.Tasks;

namespace VTT_SHOP_CORE.Services
{
    public class PaymentService : IService
    {
        private readonly VnPayService _vnPayService;
        private readonly OrderRepository _orderRepository;
        private readonly PaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public PaymentService(
            VnPayService vnPayService,
            OrderRepository orderRepository,
            PaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _vnPayService = vnPayService;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public string ProcessVnPayReturn(IQueryCollection query)
        {
            bool isValidSignature = _vnPayService.ValidateSignature(query, out string RspCode, out _);

            var frontendReturnUrl = _config["VnPaySettings:ReturnUrl"]!;

            string statusQuery;
            if (isValidSignature)
            {
                statusQuery = RspCode == "00" ? "success" : "failed";
            }
            else
            {
                statusQuery = "invalid_signature";
            }

            return $"{frontendReturnUrl}?status={statusQuery}&orderId={query["vnp_TxnRef"]}";
        }


        public async Task<VnPayIpnResponse> ProcessVnPayIpnAsync(IQueryCollection query)
        {
            bool isValidSignature = _vnPayService.ValidateSignature(query, out string RspCode, out _);

            if (!isValidSignature)
            {
                return new VnPayIpnResponse { RspCode = "97", Message = "Invalid Checksum" };
            }

            try
            {
                long orderId = Convert.ToInt64(query["vnp_TxnRef"]);
                var order = await _orderRepository.GetByIdAsync(orderId);
                var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

                if (order == null || payment == null)
                {
                    return new VnPayIpnResponse { RspCode = "01", Message = "Order not found" };
                }

                if (payment.Status == "success")
                {
                    return new VnPayIpnResponse { RspCode = "02", Message = "Order already confirmed" };
                }

                long vnpAmount = Convert.ToInt64(query["vnp_Amount"]) / 100;
                if (vnpAmount != (long)order.FinalAmount)
                {
                    return new VnPayIpnResponse { RspCode = "04", Message = "Invalid amount" };
                }

                await _unitOfWork.BeginTransactionAsync();

                if (RspCode == "00")
                {
                    order.Status = "Processing";
                    payment.Status = "success";
                    payment.TransactionDate = DateTime.UtcNow;
                }
                else
                {
                    order.Status = "Failed";
                    payment.Status = "failed";
                }

                _orderRepository.Update(order);
                _paymentRepository.Update(payment);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return new VnPayIpnResponse { RspCode = "00", Message = "Confirm Success" };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new VnPayIpnResponse { RspCode = "99", Message = "Internal Server Error: " + ex.Message };
            }
        }
    }
}