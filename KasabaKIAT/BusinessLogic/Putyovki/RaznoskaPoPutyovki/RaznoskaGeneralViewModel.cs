using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;

namespace KasabaKIAT.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    public class RaznoskaGeneralViewModel
    {
        public long viplPutId { get; set; }

        public long? UsrId { get; set; }

        public string DepartmentBase { get; set; }

        public FaceViewModel faceMain { get; set; }

        public Face_MedViewModel faceMedDtl { get; set; }

        public RaznoskaGeneralViewModel()
        {
            faceMain = new FaceViewModel();

            faceMedDtl = new Face_MedViewModel();
        }
    }
}