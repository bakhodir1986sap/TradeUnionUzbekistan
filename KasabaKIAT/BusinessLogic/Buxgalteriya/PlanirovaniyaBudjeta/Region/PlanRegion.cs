using KasabaKiat.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region;
using KasabaKIAT.ViewModel.Buxgalteriya;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KasabaKIAT.BusinessLogic.Buxgalteriya.PlanirovaniyaBudjeta.Region
{
    public class Item
    {
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public decimal  ValueYearly         { get; set; }

        public bool     IsModValueYearly    { get; set; }

        public decimal  Value1Quater        { get; set; }

        public bool     IsModValue1Quater   { get; set; }

        public decimal  Value1Half          { get; set; }

        public bool     IsModValue1Half     { get; set; }

        public decimal  Value9Month         { get; set; }

        public bool     IsModValue9Month    { get; set; }
    }

    public class ItemController
    {
        private List<Item> itemsList = null;

        public void InitializeItems()
        {
            itemsList = new List<Item>();
            //Daromadlar
            var it1 = new Item();
            it1.ItemCode = "01";
            it1.ItemDescription = "Худудий КУТБ Кенгаши ижтимоий суғурта бадали";
            it1.ValueYearly = 0;
            it1.IsModValueYearly = false;
            it1.Value1Quater = 0;
            it1.IsModValue1Quater = false;
            it1.Value1Half = 0;
            it1.IsModValue1Half = false;
            it1.Value9Month = 0;
            it1.IsModValue9Month = false;

            itemsList.Add(it1);

            var it2 = new Item();
            it2.ItemCode = "02";
            it2.ItemDescription = "НКМК ижтимоий суғурта бадали";
            it2.ValueYearly = 0;
            it2.IsModValueYearly = false;
            it2.Value1Quater = 0;
            it2.IsModValue1Quater = false;
            it2.Value1Half = 0;
            it2.IsModValue1Half = false;
            it2.Value9Month = 0;
            it2.IsModValue9Month = false;

            itemsList.Add(it2);

            var it3 = new Item();
            it3.ItemCode = "03";
            it3.ItemDescription = "Темирйўлчилар ва транспорт қурувчилари ижтимоий суғурта бадали";
            it3.ValueYearly = 0;
            it3.IsModValueYearly = false;
            it3.Value1Quater = 0;
            it3.IsModValue1Quater = false;
            it3.Value1Half = 0;
            it3.IsModValue1Half = false;
            it3.Value9Month = 0;
            it3.IsModValue9Month = false;

            itemsList.Add(it3);

            var it4 = new Item();
            it4.ItemCode = "04";
            it4.ItemDescription = "ОКМК ижтимоий суғурта бадали";
            it4.ValueYearly = 0;
            it4.IsModValueYearly = false;
            it4.Value1Quater = 0;
            it4.IsModValue1Quater = false;
            it4.Value1Half = 0;
            it4.IsModValue1Half = false;
            it4.Value9Month = 0;
            it4.IsModValue9Month = false;

            itemsList.Add(it4);

            var it5 = new Item();
            it5.ItemCode = "05";
            it5.ItemDescription = "Жами ижтимоий суғурта бадали";
            it5.ValueYearly = 0;
            it5.IsModValueYearly = false;
            it5.Value1Quater = 0;
            it5.IsModValue1Quater = false;
            it5.Value1Half = 0;
            it5.IsModValue1Half = false;
            it5.Value9Month = 0;
            it5.IsModValue9Month = false;

            itemsList.Add(it5);

            var it6 = new Item();
            it6.ItemCode = "06";
            it6.ItemDescription = "Йўлланмалар учун қисман тўлов";
            it6.ValueYearly = 0;
            it6.IsModValueYearly = true;
            it6.Value1Quater = 0;
            it6.IsModValue1Quater = true;
            it6.Value1Half = 0;
            it6.IsModValue1Half = true;
            it6.Value9Month = 0;
            it6.IsModValue9Month = true;

            itemsList.Add(it6);

            var it7 = new Item();
            it7.ItemCode = "07";
            it7.ItemDescription = "Бошқа даромадлар";
            it7.ValueYearly = 0;
            it7.IsModValueYearly = true;
            it7.Value1Quater = 0;
            it7.IsModValue1Quater = false;
            it7.Value1Half = 0;
            it7.IsModValue1Half = false;
            it7.Value9Month = 0;
            it7.IsModValue9Month = false;

            itemsList.Add(it7);

            var it8 = new Item();
            it8.ItemCode = "08";
            it8.ItemDescription = "Жами даромадлар";
            it8.ValueYearly = 0;
            it8.IsModValueYearly = false;
            it8.Value1Quater = 0;
            it8.IsModValue1Quater = false;
            it8.Value1Half = 0;
            it8.IsModValue1Half = false;
            it8.Value9Month = 0;
            it8.IsModValue9Month = false;

            itemsList.Add(it8);

            var it9 = new Item();
            it9.ItemCode = "09";
            it9.ItemDescription = "КУТБК даромади";
            it9.ValueYearly = 0;
            it9.IsModValueYearly = false;
            it9.Value1Quater = 0;
            it9.IsModValue1Quater = false;
            it9.Value1Half = 0;
            it9.IsModValue1Half = false;
            it9.Value9Month = 0;
            it9.IsModValue9Month = false;

            itemsList.Add(it9);

            //Xarajatlar
            var it10 = new Item();
            it10.ItemCode = "10";
            it10.ItemDescription = "Санаторийларда соғломлаштириш";
            it10.ValueYearly = 0;
            it10.IsModValueYearly = true;
            it10.Value1Quater = 0;
            it10.IsModValue1Quater = true;
            it10.Value1Half = 0;
            it10.IsModValue1Half = true;
            it10.Value9Month = 0;
            it10.IsModValue9Month = true;

            itemsList.Add(it10);

            var it11 = new Item();
            it11.ItemCode = "11";
            it11.ItemDescription = "КУТБК даромадига нисбатан фоиз ҳисобида";
            it11.ValueYearly = 0;
            it11.IsModValueYearly = false;
            it11.Value1Quater = 0;
            it11.IsModValue1Quater = false;
            it11.Value1Half = 0;
            it11.IsModValue1Half = false;
            it11.Value9Month = 0;
            it11.IsModValue9Month = false;

            itemsList.Add(it11);

            var it12 = new Item();
            it12.ItemCode = "12";
            it12.ItemDescription = "Болаларни оромгоҳларида соғломлаштириш ";
            it12.ValueYearly = 0;
            it12.IsModValueYearly = true;
            it12.Value1Quater = 0;
            it12.IsModValue1Quater = false;
            it12.Value1Half = 0;
            it12.IsModValue1Half = false;
            it12.Value9Month = 0;
            it12.IsModValue9Month = false;

            itemsList.Add(it12);

            var it13 = new Item();
            it13.ItemCode = "13";
            it13.ItemDescription = "КУТБК даромадига нисбатан фоиз ҳисобида";
            it13.ValueYearly = 0;
            it13.IsModValueYearly = false;
            it13.Value1Quater = 0;
            it13.IsModValue1Quater = false;
            it13.Value1Half = 0;
            it13.IsModValue1Half = false;
            it13.Value9Month = 0;
            it13.IsModValue9Month = false;

            itemsList.Add(it13);

            var it14 = new Item();
            it14.ItemCode = "14";
            it14.ItemDescription = "Ижтимоий суғурта бўлими ходимларини сақлаш";
            it14.ValueYearly = 0;
            it14.IsModValueYearly = false;
            it14.Value1Quater = 0;
            it14.IsModValue1Quater = false;
            it14.Value1Half = 0;
            it14.IsModValue1Half = false;
            it14.Value9Month = 0;
            it14.IsModValue9Month = false;

            itemsList.Add(it14);

            var it15 = new Item();
            it15.ItemCode = "15";
            it15.ItemDescription = "КУТБК даромадига нисбатан фоиз ҳисобида";
            it15.ValueYearly = 0;
            it15.IsModValueYearly = false;
            it15.Value1Quater = 0;
            it15.IsModValue1Quater = false;
            it15.Value1Half = 0;
            it15.IsModValue1Half = false;
            it15.Value9Month = 0;
            it15.IsModValue9Month = false;

            itemsList.Add(it15);

            var it16 = new Item();
            it16.ItemCode = "16";
            it16.ItemDescription = "шу жумладан: маъмурий бошқарув харажатлари";
            it16.ValueYearly = 0;
            it16.IsModValueYearly = true;
            it16.Value1Quater = 0;
            it16.IsModValue1Quater = false;
            it16.Value1Half = 0;
            it16.IsModValue1Half = false;
            it16.Value9Month = 0;
            it16.IsModValue9Month = false;

            itemsList.Add(it16);

            var it17 = new Item();
            it17.ItemCode = "17";
            it17.ItemDescription = "Кадрларни тайёрлаш";
            it17.ValueYearly = 0;
            it17.IsModValueYearly = true;
            it17.Value1Quater = 0;
            it17.IsModValue1Quater = false;
            it17.Value1Half = 0;
            it17.IsModValue1Half = false;
            it17.Value9Month = 0;
            it17.IsModValue9Month = false;

            itemsList.Add(it17);

            var it18 = new Item();
            it18.ItemCode = "18";
            it18.ItemDescription = "Касаба уюшма фаолларини рағбатлантириш";
            it18.ValueYearly = 0;
            it18.IsModValueYearly = true;
            it18.Value1Quater = 0;
            it18.IsModValue1Quater = false;
            it18.Value1Half = 0;
            it18.IsModValue1Half = false;
            it18.Value9Month = 0;
            it18.IsModValue9Month = false;

            itemsList.Add(it18);

            var it19 = new Item();
            it19.ItemCode = "19";
            it19.ItemDescription = "Бошқа харажатлар";
            it19.ValueYearly = 0;
            it19.IsModValueYearly = true;
            it19.Value1Quater = 0;
            it19.IsModValue1Quater = false;
            it19.Value1Half = 0;
            it19.IsModValue1Half = false;
            it19.Value9Month = 0;
            it19.IsModValue9Month = false;

            itemsList.Add(it19);

            var it20 = new Item();
            it20.ItemCode = "20";
            it20.ItemDescription = "Капитал қурилиш";
            it20.ValueYearly = 0;
            it20.IsModValueYearly = true;
            it20.Value1Quater = 0;
            it20.IsModValue1Quater = true;
            it20.Value1Half = 0;
            it20.IsModValue1Half = true;
            it20.Value9Month = 0;
            it20.IsModValue9Month = true;

            itemsList.Add(it20);

            var it21 = new Item();
            it21.ItemCode = "21";
            it21.ItemDescription = "Жами харажатлар";
            it21.ValueYearly = 0;
            it21.IsModValueYearly = false;
            it21.Value1Quater = 0;
            it21.IsModValue1Quater = false;
            it21.Value1Half = 0;
            it21.IsModValue1Half = false;
            it21.Value9Month = 0;
            it21.IsModValue9Month = false;

            itemsList.Add(it21);

            var it22 = new Item();
            it22.ItemCode = "22";
            it22.ItemDescription = "КУТБК даромадига нисбатан фоиз ҳисобида";
            it22.ValueYearly = 0;
            it22.IsModValueYearly = false;
            it22.Value1Quater = 0;
            it22.IsModValue1Quater = false;
            it22.Value1Half = 0;
            it22.IsModValue1Half = false;
            it22.Value9Month = 0;
            it22.IsModValue9Month = false;

            itemsList.Add(it22);

            var it23 = new Item();
            it23.ItemCode = "23";
            it23.ItemDescription = "ЎзКУФКга 30 % ажратма -жами";
            it23.ValueYearly = 0;
            it23.IsModValueYearly = false;
            it23.Value1Quater = 0;
            it23.IsModValue1Quater = false;
            it23.Value1Half = 0;
            it23.IsModValue1Half = false;
            it23.Value9Month = 0;
            it23.IsModValue9Month = false;

            itemsList.Add(it23);

            var it24 = new Item();
            it24.ItemCode = "24";
            it24.ItemDescription = "КУТБдан ЎзКУФКга 30 % ажратма ";
            it24.ValueYearly = 0;
            it24.IsModValueYearly = false;
            it24.Value1Quater = 0;
            it24.IsModValue1Quater = false;
            it24.Value1Half = 0;
            it24.IsModValue1Half = false;
            it24.Value9Month = 0;
            it24.IsModValue9Month = false;

            itemsList.Add(it24);

            var it25 = new Item();
            it25.ItemCode = "25";
            it25.ItemDescription = "НКМКдан ЎзКУФКга 30 % ажратма ";
            it25.ValueYearly = 0;
            it25.IsModValueYearly = false;
            it25.Value1Quater = 0;
            it25.IsModValue1Quater = false;
            it25.Value1Half = 0;
            it25.IsModValue1Half = false;
            it25.Value9Month = 0;
            it25.IsModValue9Month = false;

            itemsList.Add(it25);

            var it26 = new Item();
            it26.ItemCode = "26";
            it26.ItemDescription = "Темирйўлчилардан ЎзКУФКга 30 % ажратма  ";
            it26.ValueYearly = 0;
            it26.IsModValueYearly = false;
            it26.Value1Quater = 0;
            it26.IsModValue1Quater = false;
            it26.Value1Half = 0;
            it26.IsModValue1Half = false;
            it26.Value9Month = 0;
            it26.IsModValue9Month = false;

            itemsList.Add(it26);

            var it27 = new Item();
            it27.ItemCode = "27";
            it27.ItemDescription = "ОКМК дан ЎзКУФКга 30 % ажратма  ";
            it27.ValueYearly = 0;
            it27.IsModValueYearly = false;
            it27.Value1Quater = 0;
            it27.IsModValue1Quater = false;
            it27.Value1Half = 0;
            it27.IsModValue1Half = false;
            it27.Value9Month = 0;
            it27.IsModValue9Month = false;

            itemsList.Add(it27);

            var it28 = new Item();
            it28.ItemCode = "28";
            it28.ItemDescription = "Жами харажатлар";
            it28.ValueYearly = 0;
            it28.IsModValueYearly = false;
            it28.Value1Quater = 0;
            it28.IsModValue1Quater = false;
            it28.Value1Half = 0;
            it28.IsModValue1Half = false;
            it28.Value9Month = 0;
            it28.IsModValue9Month = false;

            itemsList.Add(it28);

            var it29 = new Item();
            it29.ItemCode = "29";
            it29.ItemDescription = "даромаднинг 70% қайтарилиши";
            it29.ValueYearly = 0;
            it29.IsModValueYearly = false;
            it29.Value1Quater = 0;
            it29.IsModValue1Quater = false;
            it29.Value1Half = 0;
            it29.IsModValue1Half = false;
            it29.Value9Month = 0;
            it29.IsModValue9Month = false;

            itemsList.Add(it29);

            var it30 = new Item();
            it30.ItemCode = "30";
            it30.ItemDescription = "Маблағ қолдиғи (+), (-)";
            it30.ValueYearly = 0;
            it30.IsModValueYearly = false;
            it30.Value1Quater = 0;
            it30.IsModValue1Quater = false;
            it30.Value1Half = 0;
            it30.IsModValue1Half = false;
            it30.Value9Month = 0;
            it30.IsModValue9Month = false;

            itemsList.Add(it30);

            var it31 = new Item();
            it31.ItemCode = "31";
            it31.ItemDescription = "КУТБ иш ҳақи фонди (млн.сўм)";
            it31.ValueYearly = 0;
            it31.IsModValueYearly = true;
            it31.Value1Quater = 0;
            it31.IsModValue1Quater = false;
            it31.Value1Half = 0;
            it31.IsModValue1Half = false;
            it31.Value9Month = 0;
            it31.IsModValue9Month = false;

            itemsList.Add(it31);

            var it32 = new Item();
            it32.ItemCode = "32";
            it32.ItemDescription = "НКМК иш ҳақи фонди (млн.сўм)";
            it32.ValueYearly = 0;
            it32.IsModValueYearly = true;
            it32.Value1Quater = 0;
            it32.IsModValue1Quater = false;
            it32.Value1Half = 0;
            it32.IsModValue1Half = false;
            it32.Value9Month = 0;
            it32.IsModValue9Month = false;

            itemsList.Add(it32);

            var it33 = new Item();
            it33.ItemCode = "33";
            it33.ItemDescription = "Темирйўлчилар иш ҳақи фонди (млн.сўм)";
            it33.ValueYearly = 0;
            it33.IsModValueYearly = true;
            it33.Value1Quater = 0;
            it33.IsModValue1Quater = false;
            it33.Value1Half = 0;
            it33.IsModValue1Half = false;
            it33.Value9Month = 0;
            it33.IsModValue9Month = false;

            itemsList.Add(it33);

            var it34 = new Item();
            it34.ItemCode = "34";
            it34.ItemDescription = "ОКМК иш хаки фонди (млн.сўм)";
            it34.ValueYearly = 0;
            it34.IsModValueYearly = true;
            it34.Value1Quater = 0;
            it34.IsModValue1Quater = false;
            it34.Value1Half = 0;
            it34.IsModValue1Half = false;
            it34.Value9Month = 0;
            it34.IsModValue9Month = false;

            itemsList.Add(it34);

            var it35 = new Item();
            it35.ItemCode = "35";
            it35.ItemDescription = "Жами иш ҳақи фонди";
            it35.ValueYearly = 0;
            it35.IsModValueYearly = false;
            it35.Value1Quater = 0;
            it35.IsModValue1Quater = false;
            it35.Value1Half = 0;
            it35.IsModValue1Half = false;
            it35.Value9Month = 0;
            it35.IsModValue9Month = false;

            itemsList.Add(it35);

            var it36 = new Item();
            it36.ItemCode = "36";
            it36.ItemDescription = "йўлланмалар сони санаторийларга";
            it36.ValueYearly = 0;
            it36.IsModValueYearly = true;
            it36.Value1Quater = 0;
            it36.IsModValue1Quater = true;
            it36.Value1Half = 0;
            it36.IsModValue1Half = true;
            it36.Value9Month = 0;
            it36.IsModValue9Month = true;

            itemsList.Add(it36);

            var it37 = new Item();
            it37.ItemCode = "37";
            it37.ItemDescription = "йўлланмалар сони болалар соғломлаштириш оромгоҳларига-жами";
            it37.ValueYearly = 0;
            it37.IsModValueYearly = true;
            it37.Value1Quater = 0;
            it37.IsModValue1Quater = true;
            it37.Value1Half = 0;
            it37.IsModValue1Half = false;
            it37.Value9Month = 0;
            it37.IsModValue9Month = true;

            itemsList.Add(it37);

        }

        public List<Item> Items
        {
            get
            {
                return itemsList;
            }
        }

        public void CalcFormulas(List<PlanRegionViewModel> plan_list)
        {
            if (plan_list == null) return;

            foreach(var plan in plan_list)
            {
                switch(plan.item_code)
                {
                    case "01":
                        break;

                    case "02":
                        break;

                    case "03":
                        break;

                    case "04":
                        break;

                    case "05":
                        break;

                    case "06":
                        break;

                    case "07":
                        break;

                    case "08":
                        break;

                    case "09":
                        break;

                    case "10":
                        break;

                    case "11":
                        break;

                    case "12":
                        break;

                    case "13":
                        break;

                    case "14":
                        break;

                    case "15":
                        break;

                    case "16":
                        break;

                    case "17":
                        break;

                    case "18":
                        break;

                    case "19":
                        break;

                    case "20":
                        break;

                    case "21":
                        break;

                    case "22":
                        break;

                    case "23":
                        break;

                    case "24":
                        break;

                    case "25":
                        break;

                    case "26":
                        break;

                    case "27":
                        break;

                    case "28":
                        break;

                    case "29":
                        break;

                    case "30":
                        break;

                    case "31":
                        break;

                    case "32":
                        break;

                    case "33":
                        break;
                    case "34":
                        break;

                    case "35":
                        break;

                    case "36":
                        break;

                    case "37":
                        break;
                }

            }
        }

        #region FuncList

        public void Func01(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "31");
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "01");

            if ((plan1 != null) && (plan != null))
            {
                plan.godovoy = plan1.godovoy;
                plan.quarter1 = plan1.quarter1; 
                plan.halfyear = plan1.halfyear; 
                plan.month9 = plan1.month9; 
            }
        }

        public void Func02(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "32");
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "02");

            if ((plan1 != null) && (plan != null))
            {
                plan.godovoy = plan1.godovoy;
                plan.quarter1 = plan1.quarter1;
                plan.halfyear = plan1.halfyear;
                plan.month9 = plan1.month9;
            }
        }

        public void Func03(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "33");
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "03");

            if ((plan1 != null) && (plan != null))
            {
                plan.godovoy = plan1.godovoy;
                plan.quarter1 = plan1.quarter1;
                plan.halfyear = plan1.halfyear;
                plan.month9 = plan1.month9;
            }
        }

        public void Func04(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "34");
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "04");

            if ((plan1 != null) && (plan != null))
            {
                plan.godovoy = plan1.godovoy;
                plan.quarter1 = plan1.quarter1;
                plan.halfyear = plan1.halfyear;
                plan.month9 = plan1.month9;
            }
        }

        public void Func05(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "01");
            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "02");
            var plan3 = plan_lis.FirstOrDefault(z => z.item_code == "03");
            var plan4 = plan_lis.FirstOrDefault(z => z.item_code == "04");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "05");

            if ((plan1 != null) && (plan2 != null) && (plan3 != null) && (plan4 != null))
            {
                plan.godovoy = plan1.godovoy + plan2.godovoy + plan3.godovoy + plan4.godovoy;
                plan.quarter1 = plan1.quarter1 + plan2.quarter1 + plan3.quarter1 + plan4.quarter1;
                plan.halfyear = plan1.halfyear + plan2.halfyear + plan3.halfyear + plan4.halfyear;
                plan.month9 = plan1.month9 + plan2.month9 + plan3.month9 + plan4.month9;
            }
        }

        public void Func06(ref List<PlanRegionViewModel> plan_lis)
        {
            //Vvodniye danniye
        }

        public void Func07(ref List<PlanRegionViewModel> plan_lis)
        {
            //Vvodniye danniye
        }

        public void Func08(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "05");
            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "06");
            var plan3 = plan_lis.FirstOrDefault(z => z.item_code == "07");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "08");

            if ((plan1 != null) && (plan2 != null) && (plan3 != null))
            {
                plan.godovoy = plan1.godovoy + plan2.godovoy + plan3.godovoy ;
                plan.quarter1 = plan1.quarter1 + plan2.quarter1 + plan3.quarter1 ;
                plan.halfyear = plan1.halfyear + plan2.halfyear + plan3.halfyear ;
                plan.month9 = plan1.month9 + plan2.month9 + plan3.month9 ;
            }
        }

        public void Func09(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "01");
            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "06");
            var plan3 = plan_lis.FirstOrDefault(z => z.item_code == "07");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "09");

            if ((plan1 != null) && (plan2 != null) && (plan3 != null))
            {
                plan.godovoy = plan1.godovoy + plan2.godovoy + plan3.godovoy;
                plan.quarter1 = plan1.quarter1 + plan2.quarter1 + plan3.quarter1;
                plan.halfyear = plan1.halfyear + plan2.halfyear + plan3.halfyear;
                plan.month9 = plan1.month9 + plan2.month9 + plan3.month9;
            }
        }

        public void Func10(ref List<PlanRegionViewModel> plan_lis)
        {
            //Vvod ruchkami
        }

        public void Func11(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "10");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "11");

            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "08");

            if ((plan1 != null) && (plan!=null) && (plan2 != null))
            {
                if (plan2.godovoy != 0)
                {
                    plan.godovoy = (plan1.godovoy / plan2.godovoy) * 100;
                }

                if (plan2.quarter1 != 0)
                {
                    plan.quarter1 = (plan1.quarter1 / plan2.quarter1) * 100;
                }

                if (plan2.halfyear != 0)
                {
                    plan.halfyear = (plan1.halfyear / plan2.halfyear) * 100;
                }

                if (plan2.month9 != 0)
                {
                    plan.month9 = (plan1.month9 / plan2.month9) * 100;
                }
            }

        }

        public void Func12(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "11");

            if (plan != null)
            {
                plan.halfyear = plan.godovoy * 40 / 100;
                plan.month9 = plan.godovoy;
            }
        }

        public void Func13(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "12");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "13");

            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "08");

            if ((plan1 != null) && (plan != null) && (plan2 != null))
            {
                if (plan2.godovoy != 0)
                {
                    plan.godovoy = (plan1.godovoy / plan2.godovoy) * 100;
                }

                if (plan2.quarter1 != 0)
                {
                    plan.quarter1 = (plan1.quarter1 / plan2.quarter1) * 100;
                }

                if (plan2.halfyear != 0)
                {
                    plan.halfyear = (plan1.halfyear / plan2.halfyear) * 100;
                }

                if (plan2.month9 != 0)
                {
                    plan.month9 = (plan1.month9 / plan2.month9) * 100;
                }
            }
        }

        public void Func14(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func15(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func16(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func17(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func18(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func19(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func20(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func21(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func22(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func23(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func24(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func25(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func26(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func27(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func28(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func29(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func30(ref List<PlanRegionViewModel> plan_lis)
        {

        }

        public void Func31(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "31");

            if (plan != null)
            {
                plan.quarter1 = (decimal)(plan.godovoy * 25/100);
                plan.halfyear = (decimal)(plan.godovoy * 50 / 100);
                plan.month9   = (decimal)(plan.godovoy * 75 / 100);
            }
        }

        public void Func32(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "32");

            if (plan != null)
            {
                plan.quarter1 = (decimal)(plan.godovoy * 25 / 100);
                plan.halfyear = (decimal)(plan.godovoy * 50 / 100);
                plan.month9 = (decimal)(plan.godovoy * 75 / 100);
            }
        }

        public void Func33(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "33");

            if (plan != null)
            {
                plan.quarter1 = (decimal)(plan.godovoy * 25 / 100);
                plan.halfyear = (decimal)(plan.godovoy * 50 / 100);
                plan.month9 = (decimal)(plan.godovoy * 75 / 100);
            }
        }

        public void Func34(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "34");

            if (plan != null)
            {
                plan.quarter1 = (decimal)(plan.godovoy * 25 / 100);
                plan.halfyear = (decimal)(plan.godovoy * 50 / 100);
                plan.month9 = (decimal)(plan.godovoy * 75 / 100);
            }
        }

        public void Func35(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan1 = plan_lis.FirstOrDefault(z => z.item_code == "31");
            var plan2 = plan_lis.FirstOrDefault(z => z.item_code == "32");
            var plan3 = plan_lis.FirstOrDefault(z => z.item_code == "33");
            var plan4 = plan_lis.FirstOrDefault(z => z.item_code == "34");

            var plan = plan_lis.FirstOrDefault(z => z.item_code == "35");

            if ((plan1 != null) && (plan2 != null) && (plan3 != null) && (plan4 != null))
            {
                plan.godovoy = plan1.godovoy + plan2.godovoy + plan3.godovoy + plan4.godovoy;
                plan.quarter1 = plan1.quarter1 + plan2.quarter1 + plan3.quarter1 + plan4.quarter1;
                plan.halfyear = plan1.halfyear + plan2.halfyear + plan3.halfyear + plan4.halfyear;
                plan.month9 = plan1.month9 + plan2.month9 + plan3.month9 + plan4.month9;
            }
        }

        public void Func36(ref List<PlanRegionViewModel> plan_lis)
        {
            //Vvodniye denniye
        }

        public void Func37(ref List<PlanRegionViewModel> plan_lis)
        {
            var plan = plan_lis.FirstOrDefault(z => z.item_code == "37");

            if (plan != null)
            {
                plan.quarter1 = 0;
                plan.halfyear = (decimal)(plan.godovoy * 40 / 100);
                plan.month9 = plan.godovoy;
            }
        }





        #endregion

    }

    public class PlanRegion
    {
        /// <summary>
        /// Step 1
        /// </summary>
        /// <returns></returns>
        public List<int> YearsList()
        {
           return PlanRegionDataManager.GetYears();
        }
        
        /// <summary>
        /// Step 2
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public  List<YearAndRegion> GetRegionListByYear(int year)
        {
            return PlanRegionDataManager.GetRegionByYears(year);
        }

        /// <summary>
        /// Step 3
        /// </summary>
        /// <param name="soato_code"></param>
        /// <param name="year"></param>
        /// <param name="department_code"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<PlanRegionViewModel> GetRegionPlan(string soato_code, int year , string department_code, long UserId)
        {
            var checkList = PlanRegionDataManager.Get(soato_code, year);

            if (!checkList.Any())
            {
                var itemCont = new ItemController();
                itemCont.InitializeItems();

                List<PlanRegionViewModel> extra = new List<PlanRegionViewModel>();

                foreach (var item in itemCont.Items)
                {
                    var plan = new PlanRegionViewModel();

                    plan.year = year;
                    plan.soato_code = soato_code;
                    plan.department_base = department_code;
                    plan.userid = UserId;

                    plan.item_code = item.ItemCode;
                    plan.item_name = item.ItemDescription;
                    plan.quarter1 = item.Value1Quater;
                    plan.halfyear = item.Value1Half;
                    plan.month9 = item.Value9Month;
                    plan.godovoy = item.ValueYearly;

                    PlanRegionDataManager.Create(ref plan); 

                    extra.Add(plan);
                }

                return extra;
            }

            return checkList; 
        }

        public void SaveChanges(List<PlanRegionViewModel> savedList)
        {

        }

    }

    public class YearAndRegion
    {
        public int Year { get; set; }

        public string SoatoCode { get; set; }

        public string RegionName { get; set; }
    }


}