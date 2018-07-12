using KasabaKiat.BusinessLogic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.ViewModel.Putyovki
{
    public class RealizasiyaViplMainViewModel
    {
      /// <summary>
      /// Unical Identifier
      /// </summary>
      public long                       id                      {get; set;}

      /// <summary>
      /// Organization link
      /// </summary>
      public long?                      id_org                  {get; set;}

      public string                     organizationName { get; set; }

        /// <summary>
        /// Data postupleniya
        /// </summary>
        public DateTime?                  data_post               {get; set;}

      /// <summary>
      /// Nomer documenta
      /// </summary>
      public string                     num_doc                 {get; set;}

      /// <summary>
      /// Nomer po poryadku
      /// </summary>
      public long?                      id_pok                  {get; set;}

      /// <summary>
      /// Kolichestvo putyovok
      /// </summary>
      public int?                       num_kol                 {get; set;}

      /// <summary>
      /// Summa obshaya
      /// </summary>
      public decimal?                   symma                   {get; set;}

      private string _concat_dov;
      public string                    concat_dov
        {
            get
            {
                if (string.IsNullOrEmpty(name_dov) && string.IsNullOrEmpty(num_dov) && string.IsNullOrEmpty(date_dov))
                {
                    if (string.IsNullOrEmpty(_concat_dov))
                    {
                        return string.Empty;
                    }

                    var strArr = _concat_dov.Split('~');
                    if (strArr.Count() == 3)
                    {
                        name_dov = strArr[0];
                        num_dov  = strArr[1];
                        date_dov = strArr[2];
                    }
                }

                return string.Concat(name_dov, "~", num_dov, "~", date_dov);
            }
            set
            {
                _concat_dov = value;

                if (!string.IsNullOrEmpty(_concat_dov))
                {
                    var strArr = _concat_dov.Split('~');
                    if (strArr.Count() == 3)
                    {
                        name_dov = strArr[0];
                        num_dov = strArr[1];
                        date_dov = strArr[2];
                    }
                }
            }
        }

      /// <summary>
      /// Nazvaniye doverennosti
      /// </summary>
      public string                     name_dov                {get; set;}

        /// <summary>
        /// Nazvaniye doverennosti
        /// </summary>
        public string                   num_dov                 { get; set; }

        /// <summary>
        /// Nazvaniye doverennosti
        /// </summary>
        public string                   date_dov                { get; set; }

        /// <summary>
        /// Id Kvartala
        /// </summary>
        public int?                       id_kv                   {get; set;}

      /// <summary>
      /// Flag vidannosti
      /// </summary>
      public bool                       vid                     {get; set;}

      /// <summary>
      /// God zapolneniya
      /// </summary>
      public int?                       smfil                   {get; set;}

      /// <summary>
      /// Polzovatel
      /// </summary>
      public long?                      userid                  {get; set;}

      /// <summary>
      /// Department
      /// </summary>
      public string                     departmentbase          {get; set;}

      public int NaklDay
      {
            get
            {
                if (data_post.HasValue)
                {
                    return data_post.Value.Day;
                }

                return 0;
            }
      }

        private string[] month = new[] { "", "января", "февраля", "марта", "апреля", "мая", "июня", "июля", "августа", "сентября", "октября", "ноября", "декабря" };

    public string NaklMonth
        {
            get
            {
                if (data_post.HasValue)
                {
                    return month[data_post.Value.Month];
                }

                return "";
            }
        }


        public int NaklYear
        {
            get
            {
                if (data_post.HasValue)
                {
                    return data_post.Value.Year;
                }

                return 0;
            }
        }

        public string SummaPropisyu
        {
            get
            {
                if (symma.HasValue)
                {
                    return RuDateAndMoneyConverter.CurrencyToTxt((double)symma.Value, true);
                }

                return "";
            }
        }
    }
}