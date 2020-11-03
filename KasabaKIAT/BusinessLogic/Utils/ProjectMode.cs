namespace KasabaKIAT.BusinessLogic.Utils
{
    public enum ProjectTypes
    {
        DEBUG,
        PRODUCTION
    }

    public class ProjectMode
    {
        public static ProjectTypes projectMode = ProjectTypes.PRODUCTION;
    }
}