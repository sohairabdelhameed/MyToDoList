namespace MyToDoList.Models
{
    public class Filter
    {
        public Filter(string subject, string due, string status)
        {
            SubjectId = string.IsNullOrWhiteSpace(subject) ? "all" : subject;
            Due = string.IsNullOrWhiteSpace(due) ? "all" : due;
            StatusId = string.IsNullOrWhiteSpace(status) ? "all" : status;
        }

        public string SubjectId { get; private set; }
        public string Due { get; private set; }
        public string StatusId { get; private set; }

        public bool HasSubject => SubjectId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";

        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string>
            {
                { "future", "Future" },
                { "past", "Past" },
                { "today", "Today" }
            };

        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";
    }
}
