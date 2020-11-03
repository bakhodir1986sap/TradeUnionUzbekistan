namespace KasabaKIAT.BusinessLogic.StartProcess.LoaderViewModels
{
    public class BalanceSaldoViewModel
    {
        public string   department_base     { get; set;}
        public int      cur_year            { get; set;}
        public int      cur_kv              { get; set;}
        public long     user_id             { get; set;}
        public string   account_code        { get; set;}
        public decimal  debit_bal           { get; set;}
        public decimal  credit_bal          { get; set;}
        public decimal? debit_count         { get; set;}
        public decimal? credit_count        { get; set;}
        public decimal? debit_trans         { get; set;}
        public decimal? credit_trans        { get; set;}
    }
}