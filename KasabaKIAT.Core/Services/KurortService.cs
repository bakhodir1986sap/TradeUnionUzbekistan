using KasabaKIAT.Core.Data;
using KasabaKIAT.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.ServiceModel;
using System.Xml.Serialization;

namespace KasabaKIAT.Core.Services;

public class KurortService(KasabaKIATContext db, IConfiguration config) : IKurortService
{
    private KurortSoap.SkuTradeUnionExchgPortTypeClient CreateClient()
    {
        var url = config["Kurort:ServiceUrl"] ?? "http://sku-tradeunion.uz/SkuTradeUnionExchg.asmx";
        var login = config["Kurort:Login"] ?? "SovetFederatsii";
        var password = config["Kurort:Password"] ?? "J8VU9~@dpZ";

        var binding = new BasicHttpBinding
        {
            Security = { Mode = BasicHttpSecurityMode.TransportCredentialOnly },
            MaxReceivedMessageSize = 10_000_000
        };
        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

        var client = new KurortSoap.SkuTradeUnionExchgPortTypeClient(binding, new EndpointAddress(url));
        client.ClientCredentials.UserName.UserName = login;
        client.ClientCredentials.UserName.Password = password;
        return client;
    }

    private async Task<TbkuJournal> EnterJournalAsync(int serviceId, long userId)
    {
        var journal = new TbkuJournal
        {
            ServiceID = serviceId,
            CreateDT = DateTime.Now,
            UserID = userId,
            DepartmentBase = "173",
            State = 0,
            ErrorCode = 0
        };
        db.TbkuJournals.Add(journal);
        await db.SaveChangesAsync();
        return journal;
    }

    private async Task UpdateJournalAsync(TbkuJournal journal)
    {
        db.TbkuJournals.Update(journal);
        await db.SaveChangesAsync();
    }

    public async Task LoadSpiskaProfsoyzovAsync(long userId)
    {
        var journal = await EnterJournalAsync(1, userId);
        var client = CreateClient();

        var response = await client.ZaprosSpiskaProfsoyzovAsync();
        var result = response.@return;

        if (!string.IsNullOrEmpty(result?.KodOshibki) && result.KodOshibki != "0")
        {
            journal.ErrorCode = int.TryParse(result.KodOshibki, out var ec) ? ec : -1;
            journal.ErrorDesc = result.OpisanieOshibki;
            journal.State = 2;
            await UpdateJournalAsync(journal);
            throw new InvalidOperationException(result.OpisanieOshibki);
        }

        journal.State = 1;
        await UpdateJournalAsync(journal);

        if (result?.StrokiProfsoyzov == null) return;

        foreach (var prof in result.StrokiProfsoyzov)
        {
            var exists = await db.TbkuStrokaProfsoyazs.AnyAsync(p => p.ProfsoyzKod == prof.ProfsoyzKod);
            if (exists) continue;
            db.TbkuStrokaProfsoyazs.Add(new TbkuStrokaProfsoyza
            {
                ProfsoyzKod = prof.ProfsoyzKod,
                ProfsoyzNaimenovanie = prof.ProfsoyzNaimenovanie
            });
        }
        await db.SaveChangesAsync();
    }

    public async Task LoadZaezdovSanatorievAsync(int year, string sanatoryKod, long userId)
    {
        var journal = await EnterJournalAsync(2, userId);
        var client = CreateClient();

        var response = await client.ZaprosZaezdovSanatorievAsync(year.ToString(), sanatoryKod ?? "");
        var result = response.@return;

        if (!string.IsNullOrEmpty(result?.KodOshibki) && result.KodOshibki != "0")
        {
            journal.ErrorCode = int.TryParse(result.KodOshibki, out var ec) ? ec : -1;
            journal.ErrorDesc = result.OpisanieOshibki;
            journal.State = 2;
            await UpdateJournalAsync(journal);
            throw new InvalidOperationException(result.OpisanieOshibki);
        }

        var old = await db.TbkuGrafikZaezdovs.Where(g => g.GodZaezdov == year).ToListAsync();
        db.TbkuGrafikZaezdovs.RemoveRange(old);
        await db.SaveChangesAsync();

        journal.State = 1;
        await UpdateJournalAsync(journal);

        if (result?.StrokiGrafikaZaezdovSanatoriev == null) return;

        foreach (var row in result.StrokiGrafikaZaezdovSanatoriev)
        {
            db.TbkuGrafikZaezdovs.Add(new TbkuGrafikZaezdov
            {
                GodZaezdov = year,
                SanatoryKod = row.SanatoryKod,
                SanatoryNaimenovanie = row.SanatoryNaimenovanie,
                SanatoryProfil = row.SanatoryProfil?.Length > 50 ? row.SanatoryProfil[..50] : row.SanatoryProfil,
                SanatoryOblast = row.SanatoryOblast,
                SanatoryKoekVsego = long.TryParse(row.SanatoryKoekVsego, out var kv) ? kv : 0,
                SanatoryKoekLetom = long.TryParse(row.SanatoryKoekLetom, out var kl) ? kl : 0,
                SanatoryKoekZimoy = long.TryParse(row.SanatoryKoekZimoy, out var kz) ? kz : 0,
                SanatoryRazmeshenieKod = row.SanatoryRazmeshenieKod,
                SanatoryRazmeshenieNaimenovanie = row.SanatoryRazmeshenieNaimenovanie,
                Putevok1kv = long.TryParse(row.Putevok1kv, out var p1) ? p1 : 0,
                Putevok2kv = long.TryParse(row.Putevok2kv, out var p2) ? p2 : 0,
                Putevok3kv = long.TryParse(row.Putevok3kv, out var p3) ? p3 : 0,
                Putevok4kv = long.TryParse(row.Putevok4kv, out var p4) ? p4 : 0,
            });
        }
        await db.SaveChangesAsync();
    }

    public async Task SendRaspredeleniyaAsync(int year, long userId)
    {
        var list = await db.TbkuSpisokRaspredeleniyas.Where(r => r.GodZaezdov == year).ToListAsync();
        if (!list.Any()) throw new InvalidOperationException("No raspredeleniya found for year " + year);

        var journal = await EnterJournalAsync(3, userId);
        var client = CreateClient();

        var xmlStr = SerializeRaspredeleniya(year, list);
        var response = await client.ZagruzkaRaspredeleniyaZaezdovAsync(xmlStr);
        var resultStr = response.@return;

        if (!string.IsNullOrEmpty(resultStr))
        {
            journal.ErrorCode = -1;
            journal.ErrorDesc = resultStr;
            journal.State = 2;
            await UpdateJournalAsync(journal);
            throw new InvalidOperationException(resultStr);
        }

        journal.State = 1;
        await UpdateJournalAsync(journal);
    }

    public async Task LoadSchetFakturaAsync(DateTime periodNachalo, DateTime periodOkonchanie, string profsoyzKod, long userId)
    {
        if (periodNachalo > periodOkonchanie)
            throw new ArgumentException("periodNachalo must be before periodOkonchanie");

        var journal = await EnterJournalAsync(4, userId);
        var client = CreateClient();

        var response = await client.ZaprosSpiskaSchetFakturAsync(periodNachalo, periodOkonchanie, profsoyzKod ?? "", "");
        var result = response.@return;

        if (!string.IsNullOrEmpty(result?.KodOshibki) && result.KodOshibki != "0")
        {
            journal.ErrorCode = int.TryParse(result.KodOshibki, out var ec) ? ec : -1;
            journal.ErrorDesc = result.OpisanieOshibki;
            journal.State = 2;
            await UpdateJournalAsync(journal);
            throw new InvalidOperationException(result.OpisanieOshibki);
        }

        journal.State = 1;
        await UpdateJournalAsync(journal);

        if (result?.StrokiDocumentov == null) return;

        foreach (var doc in result.StrokiDocumentov)
        {
            var existing = await db.TbkuDocumentSchetFakturas
                .Where(d => d.DocNomer == doc.DocNomer && d.DocData == doc.DocData && d.DocTypeRealVozv == doc.DocTypeRealVozv)
                .ToListAsync();
            foreach (var e in existing)
            {
                var lines = db.TbkuStrokaSchetFakturis.Where(s => s.SchetFactura_ID == e.ID);
                db.TbkuStrokaSchetFakturis.RemoveRange(lines);
                db.TbkuDocumentSchetFakturas.Remove(e);
            }
            await db.SaveChangesAsync();

            var header = new TbkuDocumentSchetFaktura
            {
                DocNomer = doc.DocNomer,
                DocData = doc.DocData,
                DocTypeRealVozv = doc.DocTypeRealVozv,
                ProfsoyzKod = doc.ProfsoyzKod,
                ProfsoyzDogovor = doc.ProfsoyzDogovor,
                ProfsoyzDoverennost = doc.ProfsoyzDoverennost,
                ProfsoyzDoverennostData = doc.ProfsoyzDoverennostData,
                ProfsoyzDoverennostMOL = doc.ProfsoyzDoverennostMOL,
                SummaDoc = doc.SummaDoc,
                KolichestvoPutevok = (int)doc.KolichestvoPutevok
            };
            db.TbkuDocumentSchetFakturas.Add(header);
            await db.SaveChangesAsync();

            if (doc.StrokiSchetaFakturi == null) continue;

            foreach (var line in doc.StrokiSchetaFakturi)
            {
                db.TbkuStrokaSchetFakturis.Add(new TbkuStrokaSchetFakturi
                {
                    SchetFactura_ID = header.ID,
                    SanatoryKod = line.SanatoryKod,
                    SanatoryNaimenovanie = line.SanatoryNaimenovanie,
                    Blank1 = line.Blank1,
                    Blank2 = line.Blank2,
                    DataZaezda = line.DataZaezda,
                    Cena = line.Cena
                });
            }
            await db.SaveChangesAsync();
        }
    }

    public async Task LoadZaezdyClientovAsync(string sanatoryKod, DateTime periodNachalo, DateTime periodOkonchanie, long userId)
    {
        if (string.IsNullOrEmpty(sanatoryKod)) throw new ArgumentException("sanatoryKod is required");
        if (periodNachalo > periodOkonchanie) throw new ArgumentException("periodNachalo must be before periodOkonchanie");

        var journal = await EnterJournalAsync(5, userId);
        var client = CreateClient();

        var response = await client.ZaprosZaezdovClientovAsync(sanatoryKod, periodNachalo, periodOkonchanie, "S");
        var result = response.@return;

        if (!string.IsNullOrEmpty(result?.KodOshibki) && result.KodOshibki != "0")
        {
            journal.ErrorCode = int.TryParse(result.KodOshibki, out var ec) ? ec : -1;
            journal.ErrorDesc = result.OpisanieOshibki;
            journal.State = 2;
            await UpdateJournalAsync(journal);
            throw new InvalidOperationException(result.OpisanieOshibki);
        }

        journal.State = 1;
        await UpdateJournalAsync(journal);

        if (result?.StrokiZaezdovClientov == null) return;

        foreach (var c in result.StrokiZaezdovClientov)
        {
            var old = await db.TbkuZaezdyClientovs
                .Where(z => z.SanatoryKod == c.SanatoryKod && z.NomerPutevki == c.NomerPutevki)
                .ToListAsync();
            db.TbkuZaezdyClientovs.RemoveRange(old);

            db.TbkuZaezdyClientovs.Add(new TbkuZaezdyClientov
            {
                SanatoryKod = c.SanatoryKod,
                SanatoryNaimenovanie = c.SanatoryNaimenovanie,
                NomerPutevki = c.NomerPutevki,
                DataZaezdaPlan = c.DataZaezdaPlan,
                DataZaezdaFakt = c.DataZaezdaFakt,
                StoimostPutevki = c.StoimostPutevki,
                OplachenaStoimost = c.OplachenaStoimost,
                ClientFIO = c.ClientFIO,
                ClientPasport = c.ClientPasport
            });
        }
        await db.SaveChangesAsync();
    }

    private static string SerializeRaspredeleniya(int year, List<TbkuSpisokRaspredeleniya> rows)
    {
        var root = new RaspredelenieXml
        {
            GodZaezdov = year.ToString(),
            Stroki = rows.Select(r => new StrokaRaspredelenieXml
            {
                SanatoryKod = r.SanatoryKod ?? "",
                ProfsoyzKod = r.ProfsoyzKod ?? "",
                SanatoryRazmeshenieKod = r.SanatoryRazmeshenieKod ?? "",
                Putevok1kv = r.Putevok1kv?.ToString() ?? "0",
                Putevok2kv = r.Putevok2kv?.ToString() ?? "0",
                Putevok3kv = r.Putevok3kv?.ToString() ?? "0",
                Putevok4kv = r.Putevok4kv?.ToString() ?? "0"
            }).ToList()
        };

        var ns = new XmlSerializerNamespaces();
        ns.Add("", "http://www.sku-tradeunion.uz");
        var serializer = new XmlSerializer(typeof(RaspredelenieXml));
        using var writer = new StringWriter();
        serializer.Serialize(writer, root, ns);
        return writer.ToString();
    }
}

[XmlRoot("typeRaspredelenieZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
public class RaspredelenieXml
{
    [XmlElement("GodZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
    public string GodZaezdov { get; set; } = "";

    [XmlElement("StrokiRaspredelenyaZaezdov", Namespace = "http://www.sku-tradeunion.uz")]
    public List<StrokaRaspredelenieXml> Stroki { get; set; } = new();
}

public class StrokaRaspredelenieXml
{
    [XmlElement("SanatoryKod", Namespace = "http://www.sku-tradeunion.uz")]
    public string SanatoryKod { get; set; } = "";
    [XmlElement("ProfsoyzKod", Namespace = "http://www.sku-tradeunion.uz")]
    public string ProfsoyzKod { get; set; } = "";
    [XmlElement("SanatoryRazmeshenieKod", Namespace = "http://www.sku-tradeunion.uz")]
    public string SanatoryRazmeshenieKod { get; set; } = "";
    [XmlElement("Putevok1kv", Namespace = "http://www.sku-tradeunion.uz")]
    public string Putevok1kv { get; set; } = "0";
    [XmlElement("Putevok2kv", Namespace = "http://www.sku-tradeunion.uz")]
    public string Putevok2kv { get; set; } = "0";
    [XmlElement("Putevok3kv", Namespace = "http://www.sku-tradeunion.uz")]
    public string Putevok3kv { get; set; } = "0";
    [XmlElement("Putevok4kv", Namespace = "http://www.sku-tradeunion.uz")]
    public string Putevok4kv { get; set; } = "0";
}
