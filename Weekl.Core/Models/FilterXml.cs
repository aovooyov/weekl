namespace Weekl.Core.Models
{
    public class FilterXml
    {
        public int[] SourceId { get; set; }

        public static string Empty => "<Filter><Source></Source></Filter>";
    }
}