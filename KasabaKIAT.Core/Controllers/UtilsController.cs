using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KasabaKIAT.Core.Controllers;

/// <summary>
/// Utility endpoints — amountInWords, Latin2Cyrillic transliteration.
/// Ported from BusinessLogic/Utils/amountInWords.cs and Latin2Cyrillic.cs
/// </summary>
[ApiController]
[Route("api/utils")]
[Authorize]
public class UtilsController : ControllerBase
{
    // ── Amount in words (Uzbek) ───────────────────────────────────────────
    [HttpGet("amount-in-words")]
    public IActionResult AmountInWords([FromQuery] decimal amount, [FromQuery] string lang = "uz")
    {
        var words = lang.ToLower() == "ru"
            ? AmountToWordsRu(amount)
            : AmountToWordsUz(amount);
        return Ok(new { amount, words });
    }

    // ── Latin ↔ Cyrillic transliteration ─────────────────────────────────
    [HttpGet("latin-to-cyrillic")]
    public IActionResult LatinToCyrillic([FromQuery] string text)
        => Ok(new { original = text, converted = ConvertLatinToCyrillic(text) });

    [HttpGet("cyrillic-to-latin")]
    public IActionResult CyrillicToLatin([FromQuery] string text)
        => Ok(new { original = text, converted = ConvertCyrillicToLatin(text) });

    // ─────────────────────────────────────────────────────────────────────

    private static string AmountToWordsUz(decimal amount)
    {
        var intPart = (long)Math.Floor(amount);
        var decPart = (int)Math.Round((amount - intPart) * 100);

        return $"{NumberToWordsUz(intPart)} so'm {decPart:D2} tiyn";
    }

    private static string AmountToWordsRu(decimal amount)
    {
        var intPart = (long)Math.Floor(amount);
        var decPart = (int)Math.Round((amount - intPart) * 100);

        return $"{NumberToWordsRu(intPart)} сум {decPart:D2} тийин";
    }

    private static string NumberToWordsUz(long n)
    {
        if (n == 0) return "nol";
        if (n < 0) return "minus " + NumberToWordsUz(-n);

        string[] units = ["", "bir", "ikki", "uch", "to'rt", "besh", "olti", "yetti", "sakkiz", "to'qqiz"];
        string[] teens = ["o'n", "o'n bir", "o'n ikki", "o'n uch", "o'n to'rt",
                          "o'n besh", "o'n olti", "o'n yetti", "o'n sakkiz", "o'n to'qqiz"];
        string[] tens  = ["", "o'n", "yigirma", "o'ttiz", "qirq", "ellik",
                          "oltmish", "yetmish", "sakson", "to'qson"];

        if (n < 10) return units[n];
        if (n < 20) return teens[n - 10];
        if (n < 100) return tens[n / 10] + (n % 10 > 0 ? " " + units[n % 10] : "");
        if (n < 1000) return units[n / 100] + " yuz" + (n % 100 > 0 ? " " + NumberToWordsUz(n % 100) : "");
        if (n < 1_000_000) return NumberToWordsUz(n / 1000) + " ming" + (n % 1000 > 0 ? " " + NumberToWordsUz(n % 1000) : "");
        if (n < 1_000_000_000) return NumberToWordsUz(n / 1_000_000) + " million" + (n % 1_000_000 > 0 ? " " + NumberToWordsUz(n % 1_000_000) : "");
        return NumberToWordsUz(n / 1_000_000_000) + " milliard" + (n % 1_000_000_000 > 0 ? " " + NumberToWordsUz(n % 1_000_000_000) : "");
    }

    private static string NumberToWordsRu(long n)
    {
        if (n == 0) return "ноль";
        if (n < 0) return "минус " + NumberToWordsRu(-n);

        string[] units = ["", "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять"];
        string[] teens = ["десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать",
                          "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"];
        string[] tens  = ["", "десять", "двадцать", "тридцать", "сорок", "пятьдесят",
                          "шестьдесят", "семьдесят", "восемьдесят", "девяносто"];
        string[] hundreds = ["", "сто", "двести", "триста", "четыреста", "пятьсот",
                              "шестьсот", "семьсот", "восемьсот", "девятьсот"];

        if (n < 10) return units[n];
        if (n < 20) return teens[n - 10];
        if (n < 100) return tens[n / 10] + (n % 10 > 0 ? " " + units[n % 10] : "");
        if (n < 1000) return hundreds[n / 100] + (n % 100 > 0 ? " " + NumberToWordsRu(n % 100) : "");
        if (n < 1_000_000) return NumberToWordsRu(n / 1000) + " тысяч" + (n % 1000 > 0 ? " " + NumberToWordsRu(n % 1000) : "");
        if (n < 1_000_000_000) return NumberToWordsRu(n / 1_000_000) + " миллион" + (n % 1_000_000 > 0 ? " " + NumberToWordsRu(n % 1_000_000) : "");
        return NumberToWordsRu(n / 1_000_000_000) + " миллиард" + (n % 1_000_000_000 > 0 ? " " + NumberToWordsRu(n % 1_000_000_000) : "");
    }

    private static string ConvertLatinToCyrillic(string text)
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            {"sh","ш"},{"ch","ч"},{"ng","нг"},{"o'","ў"},{"O'","Ў"},{"g'","ғ"},{"G'","Ғ"},
            {"a","а"},{"b","б"},{"d","д"},{"e","е"},{"f","ф"},{"g","г"},{"h","ҳ"},{"i","и"},
            {"j","ж"},{"k","к"},{"l","л"},{"m","м"},{"n","н"},{"o","о"},{"p","п"},{"q","қ"},
            {"r","р"},{"s","с"},{"t","т"},{"u","у"},{"v","в"},{"x","х"},{"y","й"},{"z","з"},
            {"A","А"},{"B","Б"},{"D","Д"},{"E","Е"},{"F","Ф"},{"G","Г"},{"H","Ҳ"},{"I","И"},
            {"J","Ж"},{"K","К"},{"L","Л"},{"M","М"},{"N","Н"},{"O","О"},{"P","П"},{"Q","Қ"},
            {"R","Р"},{"S","С"},{"T","Т"},{"U","У"},{"V","В"},{"X","Х"},{"Y","Й"},{"Z","З"}
        };

        var result = text;
        foreach (var (k, v) in map.OrderByDescending(x => x.Key.Length))
            result = result.Replace(k, v);
        return result;
    }

    private static string ConvertCyrillicToLatin(string text)
    {
        var map = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            {"ш","sh"},{"ч","ch"},{"нг","ng"},{"ў","o'"},{"Ў","O'"},{"ғ","g'"},{"Ғ","G'"},
            {"а","a"},{"б","b"},{"д","d"},{"е","e"},{"ф","f"},{"г","g"},{"ҳ","h"},{"и","i"},
            {"ж","j"},{"к","k"},{"л","l"},{"м","m"},{"н","n"},{"о","o"},{"п","p"},{"қ","q"},
            {"р","r"},{"с","s"},{"т","t"},{"у","u"},{"в","v"},{"х","x"},{"й","y"},{"з","z"},
            {"А","A"},{"Б","B"},{"Д","D"},{"Е","E"},{"Ф","F"},{"Г","G"},{"Ҳ","H"},{"И","I"},
            {"Ж","J"},{"К","K"},{"Л","L"},{"М","M"},{"Н","N"},{"О","O"},{"П","P"},{"Қ","Q"},
            {"Р","R"},{"С","S"},{"Т","T"},{"У","U"},{"В","V"},{"Х","X"},{"Й","Y"},{"З","Z"}
        };

        var result = text;
        foreach (var (k, v) in map.OrderByDescending(x => x.Key.Length))
            result = result.Replace(k, v);
        return result;
    }
}
