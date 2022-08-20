namespace Commerce.Models
{
    public class NewebPayReturn
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public NewebPayPeriodResult Result { get; set; }
    }

    public class NewebPayPeriodResult
    {
        public string MerchantID { get; set; }
        public string MerchantOrderNo { get; set; }
        public string PeriodType { get; set; }
        public int AuthTimes { get; set; }
        public string AuthTime { get; set; }
        public string DateArray { get; set; }
        public string TradeNo { get; set; }
        public string CardNo { get; set; }
        public decimal PeriodAmt { get; set; }
        public string AuthCode { get; set; }
        public string RespondCode { get; set; }
        public string EscrowBank { get; set; }
        public string AuthBank { get; set; }
        public string PaymentMethod { get; set; }
        public string PeriodNo { get; set; }
        public string Extday { get; set; }

    }
}
