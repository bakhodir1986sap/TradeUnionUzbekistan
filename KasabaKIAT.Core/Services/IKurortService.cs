namespace KasabaKIAT.Core.Services;

public interface IKurortService
{
    Task LoadSpiskaProfsoyzovAsync(long userId);
    Task LoadZaezdovSanatorievAsync(int year, string sanatoryKod, long userId);
    Task SendRaspredeleniyaAsync(int year, long userId);
    Task LoadSchetFakturaAsync(DateTime periodNachalo, DateTime periodOkonchanie, string profsoyzKod, long userId);
    Task LoadZaezdyClientovAsync(string sanatoryKod, DateTime periodNachalo, DateTime periodOkonchanie, long userId);
}
