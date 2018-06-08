using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KasabaKIAT.ViewModel.Putyovki;
using KasabaKIAT.ViewModel;
using KasabaKiat.ViewModel.Putyovki;
using KasabaKiat.ViewModel;

namespace KasabaKiat.BusinessLogic.Putyovki.PrixodPutyovok
{
    public class PostupleniyaEnterTemp : IPostupleniyaEnter
    {
        void IPostupleniyaEnter.EnterProcess(IPostupleniyaViewModel model)
        {
            //
            var  mm = (PutPostTempViewModel)model;

            var mainViewModel = new PostupleniyaViewModel();

            mainViewModel.UserId = model.UserId;
            mainViewModel.DepartmentBase = model.DepartmentBase;
            mainViewModel.BeginNumber = mm.BeginNumber;
            mainViewModel.EndNumber = mm.EndNumber;
           
            if (mm.BeginNumber < mm.EndNumber)
            {
                var countOfPutyovka = mm.EndNumber - mm.BeginNumber + 1;
                mainViewModel.Num_Kol = countOfPutyovka;
                mainViewModel.Symma = mm.Price * countOfPutyovka;

                

                //PostMainDataManager.Create(ref mainViewModel, null, out null);

                for (int i = mm.BeginNumber; i <= mm.EndNumber; i++)
                {
                    var detailModel = new PostPutyovkaViewModel();

                    detailModel.DataSan = mm.BeginSanDate;
                    detailModel.DataSanEnd = mm.EndSanDate;
                    detailModel.Num_Put = i.ToString(); //Maybe need formatting
                    detailModel.Symma_Put = mm.Price;
                    detailModel.IdSanat = mm.SanatoryId;
                    detailModel.ID_PolVoz = false;
                    detailModel.UserId = model.UserId;
                    detailModel.Departmentbase = model.DepartmentBase;
                    detailModel.IdPost = mainViewModel.Id; //Master Detail Connection

                    //PostPutDetailDataManager.Create(ref detailModel, null);
                }
            }
            else
            {
                throw new ApplicationException("Invalid numbers");
            }
            ///////////////////////////////////////////////
        }

        void IPostupleniyaEnter.ModifyProcess(IPostupleniyaViewModel model)
        {
            var mm = (PutPostTempViewModel)model;

            var mainViewModel = new PostupleniyaViewModel();

            mainViewModel.Id = mm.Id;
            mainViewModel.UserId = model.UserId;
            mainViewModel.DepartmentBase = model.DepartmentBase;
            mainViewModel.BeginNumber = mm.BeginNumber;
            mainViewModel.EndNumber = mm.EndNumber;

            if (mm.BeginNumber < mm.EndNumber)
            {
                var countOfPutyovka = mm.EndNumber - mm.BeginNumber + 1;
                mainViewModel.Num_Kol = countOfPutyovka;
                mainViewModel.Symma = mm.Price * countOfPutyovka;

                PostMainDataManager.Modify(mainViewModel);

                for (int i = mm.BeginNumber; i <= mm.EndNumber; i++)
                {
                    var detailModel = new PostPutyovkaViewModel();

                    detailModel.DataSan = mm.BeginSanDate;
                    detailModel.DataSanEnd = mm.EndSanDate;
                    detailModel.Num_Put = i.ToString(); //Maybe need formatting
                    detailModel.Symma_Put = mm.Price;
                    detailModel.IdSanat = mm.SanatoryId;
                    detailModel.ID_PolVoz = false;
                    detailModel.IdPost = mainViewModel.Id; //Master Detail Connection

                    await PostPutDetailDataManager.Modify(detailModel);
                }
            }
            else
            {
                throw new ApplicationException("Invalid numbers");
            }
        }

        void IPostupleniyaEnter.DeleteProcess(IPostupleniyaViewModel model)
        {
            var mm = (PutPostTempViewModel)model;

            var mainViewModel = new PostupleniyaViewModel();

            mainViewModel.BeginNumber = mm.BeginNumber;
            mainViewModel.EndNumber = mm.EndNumber;

            var detailModel = new PostPutyovkaViewModel();

            detailModel.IdPost = mainViewModel.Id; //Master Detail Connection

            PostPutDetailDataManager.Remove(detailModel);

            PostMainDataManager.Remove(mainViewModel);
        }

    }
}