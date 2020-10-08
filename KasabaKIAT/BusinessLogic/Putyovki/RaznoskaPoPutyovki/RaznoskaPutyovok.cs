using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.RaznoskaPoPutyovki;
using KasabaKIAT.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    public class RaznoskaPutyovok
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public async Task SaveChanges(RaznoskaGeneralViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("model is null");
            }

            if ((model.faceMain == null) || (model.faceMedDtl == null))
            {
                throw new NullReferenceException("child models is null");
            }

            model.faceMain.departmentbase = model.DepartmentBase;
            model.faceMain.userid = model.UsrId;

            model.faceMedDtl.departmentbase = model.DepartmentBase;
            model.faceMedDtl.userid = model.UsrId;

            var faceMain = new FaceViewModel();

            faceMain = model.faceMain;
            faceMain.id_put = model.viplPutId;

            var faceMedDtl = new Face_MedViewModel();

            faceMedDtl = model.faceMedDtl;
            faceMedDtl.id_face = faceMain.id;

            var listMain = FaceDataManager.Get(faceMain);

            using (KasabaKIATEntities db = new KasabaKIATEntities())
            {
                if (!listMain.Any())
                {
                    tblFase face = null;
                    FaceDataManager.Create(ref faceMain, db, out face);
                    faceMedDtl.id_face = faceMain.id;
                    Face_MedDataManager.Create(ref faceMedDtl, db, face);
                }
                else
                {
                    FaceDataManager.Update(faceMain, db);
                    faceMedDtl.id_face = faceMain.id;
                    Face_MedDataManager.Update(faceMedDtl, db);
                }

                await db.SaveChangesAsync();
            }
        }

        public RaznoskaGeneralViewModel FillById(long idPut)
        {
            var model = new RaznoskaGeneralViewModel();

            var listMain = FaceDataManager.Get(new FaceViewModel()
            {
                id_put = idPut
            });

            if (listMain.Any())
            {
                model.faceMain = listMain.First();
            }

            var listMed = Face_MedDataManager.Get(new Face_MedViewModel()
            {
                id_face = model.faceMain.id
            });

            if (listMed.Any())
            {
                model.faceMedDtl = listMed.First();
            }

            return model;
        }
    }
}