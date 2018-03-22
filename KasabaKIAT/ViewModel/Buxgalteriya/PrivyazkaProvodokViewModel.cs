using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.ViewModel.Buxgalteriya
{
    public class PrivyazkaProvodokViewModel
    {
        public long id                        { get; set;}
        public long? jo_ref                    { get; set;}
        public string owner_departmentbase      { get; set;}
        public int? provodok_id               { get; set;}
        public long? user_id                   { get; set;}
    }
}