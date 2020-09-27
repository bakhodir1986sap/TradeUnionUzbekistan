using KasabaKIAT.ViewModel.Buxgalteriya;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.Platejki
{
    public class Platejki
    {
        public void Enter(PlatejkiViewModel model)
        {
            PlatejkiDataManager.Create(ref model);
        }
    }
}