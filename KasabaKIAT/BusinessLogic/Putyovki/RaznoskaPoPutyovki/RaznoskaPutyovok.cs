using KasabaKiat.ViewModel.Putyovki.RaznoskaPutyovok;
using KasabaKIAT.BusinessLogic.Putyovki.RaznoskaPoPutyovki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKiat.BusinessLogic.Putyovki.RaznoskaPoPutyovki
{
    public class RaznoskaPutyovok
    {
        public void SaveChanges(RaznoskaGeneralViewModel model)
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

            if (!listMain.Any())
            {
                FaceDataManager.Create(ref faceMain);
                Face_MedDataManager.Create(ref faceMedDtl);
            }
            else
            {
                FaceDataManager.Update(faceMain);
                Face_MedDataManager.Update(faceMedDtl);
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