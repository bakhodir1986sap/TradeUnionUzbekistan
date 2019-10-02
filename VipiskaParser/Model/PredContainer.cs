using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipiskaParser.Model
{
    public class PredContainer
    {
        public List<PredModel> listPred { get; set; }

        public PredContainer()
        {
            listPred = new List<PredModel>();

            var model1 = new PredModel(); model1.Naimenovaniye = "Ўзбекистон касаба уюшмалари Федерацияси"; model1.Inn = "201685882"; model1.RaschetniySchet1 = "20212000100431324011"; model1.RaschetniySchet2 = "20212000100431324011";listPred.Add(model1);
            var model2 = new PredModel(); model2.Naimenovaniye = "Андижан";                                 model2.Inn = "200242707"; model2.RaschetniySchet1 = "20212000000184956018"; model2.RaschetniySchet2 = "20212000900184956019";listPred.Add(model2);
            var model3 = new PredModel(); model3.Naimenovaniye = "Бухара";                                  model3.Inn = "201512163"; model3.RaschetniySchet1 = "20212000900487320011"; model3.RaschetniySchet2 = "20212000600487320009";listPred.Add(model3);
            var model4 = new PredModel(); model4.Naimenovaniye = "Джиззах";                                 model4.Inn = "201672298"; model4.RaschetniySchet1 = "20212000900400852010"; model4.RaschetniySchet2 = "20212000200400852012";listPred.Add(model4);
            var model5 = new PredModel(); model5.Naimenovaniye = "Кашкадарья";                              model5.Inn = "201998549"; model5.RaschetniySchet1 = "20212000900305583007"; model5.RaschetniySchet2 = "20212000100305583002";listPred.Add(model5);
            var model6 = new PredModel(); model6.Naimenovaniye = "Навоий";                                  model6.Inn = "200007159"; model6.RaschetniySchet1 = "20212000700506482008"; model6.RaschetniySchet2 = "20212000400506482001";listPred.Add(model6);
            var model7 = new PredModel(); model7.Naimenovaniye = "Наманган";                                model7.Inn = "200056304"; model7.RaschetniySchet1 = "20212000000467922008"; model7.RaschetniySchet2 = "20212000900467922002";listPred.Add(model7);
            var model8 = new PredModel(); model8.Naimenovaniye = "Самарканд";                               model8.Inn = "202128290"; model8.RaschetniySchet1 = "20212000000510123016"; model8.RaschetniySchet2 = "20212000900510123017";listPred.Add(model8);
            var model9 = new PredModel(); model9.Naimenovaniye = "Сурхандарья";                             model9.Inn = "200479309"; model9.RaschetniySchet1 = "20212000700361799006"; model9.RaschetniySchet2 = "20212000500361799005";listPred.Add(model9);
            var modela = new PredModel(); modela.Naimenovaniye = "Сирдарья";                                modela.Inn = "200324798"; modela.RaschetniySchet1 = "20212000900134927013"; modela.RaschetniySchet2 = "20212000400134927009";listPred.Add(modela);
            var modelb = new PredModel(); modelb.Naimenovaniye = "Ташкент город";                           modelb.Inn = "201273304"; modelb.RaschetniySchet1 = "20212000500136154006"; modelb.RaschetniySchet2 = "20212000800136154002";listPred.Add(modelb);
            var modelc = new PredModel(); modelc.Naimenovaniye = "Ташкент область";                         modelc.Inn = "201757318"; modelc.RaschetniySchet1 = "20212000600431318006"; modelc.RaschetniySchet2 = "20212000200431318004";listPred.Add(modelc);
            var modeld = new PredModel(); modeld.Naimenovaniye = "Фергана";                                 modeld.Inn = "200152691"; modeld.RaschetniySchet1 = "20212000101947929017"; modeld.RaschetniySchet2 = "20212000001947929008";listPred.Add(modeld);
            var modele = new PredModel(); modele.Naimenovaniye = "Хорезм";                                  modele.Inn = "201943220"; modele.RaschetniySchet1 = "20212000600431876015"; modele.RaschetniySchet2 = "20212000200431876011";listPred.Add(modele);
            var modelf = new PredModel(); modelf.Naimenovaniye = "Каракалпакский Респ";                     modelf.Inn = "200357737"; modelf.RaschetniySchet1 = "20212000300538352006"; modelf.RaschetniySchet2 = "20212000700538352008";listPred.Add(modelf);
            var modelg = new PredModel(); modelg.Naimenovaniye = "Профсоюз работников НГМК";                modelg.Inn = "200007142"; modelg.RaschetniySchet1 = "20212000200501905001"; modelg.RaschetniySchet2 = "20212000400501905002";listPred.Add(modelg);
            var modelh = new PredModel(); modelh.Naimenovaniye = "Олмалик КМК";                             modelh.Inn = "200604358"; modelh.RaschetniySchet1 = "20212000400131184003"; modelh.RaschetniySchet2 = "20212000600131184004";listPred.Add(modelh);
            var modelk = new PredModel(); modelk.Naimenovaniye = "РС Профсоюза ЖЕЛЕЗНОДОРОЖНИКОВ";          modelk.Inn = "201534714"; modelk.RaschetniySchet1 = "20212000200179870004"; modelk.RaschetniySchet2 = "20212000400179870005";listPred.Add(modelk);

        }
    }
}
