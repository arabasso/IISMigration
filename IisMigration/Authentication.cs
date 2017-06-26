namespace IisMigration
{
    public class Authentication
    {
        public AnonymousAuthentication Anonymous { get; set; }

        public Authentication()
        {
            
        }

        public Authentication(
            string site,
            AppCmd appCmd)
        {
            Anonymous = new AnonymousAuthentication(site, appCmd);
        }
    }
}
