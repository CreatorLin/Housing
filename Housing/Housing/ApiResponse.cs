public class ApiResponse
{
    public Msg[] Msgs { get; set; }

    public class Msg
    {
        public string ID { get; set; }
        public User User { get; set; }
        public float Time { get; set; }
        public string Body { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public float SKF64 { get; set; }
        public App App { get; set; }
        public string CustomID { get; set; }
    }

    public class User
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string ThirdPartyID { get; set; }
        public string Privacy { get; set; }
    }

    public class App
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string MarketingURI { get; set; }
    }
}
