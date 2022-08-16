namespace BHub.Infra.Environments
{
    public static class Constants
    {
        public static readonly string HOST = Env.Get("DATABASE_HOST");
        public static readonly string DATABASE = Env.Get("DATABASE_NAME");
        public static readonly string USER = Env.Get("DATABASE_USER");
        public static readonly string PASS = Env.Get("DATABASE_PASS");
        public static readonly string ELASTIC_APM_SERVER_URL = Env.Get("ELASTIC_APM_SERVER_URL");
        public static readonly string ELASTIC_APM_TOKEN = Env.Get("ELASTIC_APM_TOKEN");
        public static readonly string ConnectionString = $@"
            Server = {HOST};
            Database = {DATABASE}; 
            User Id  =  {USER}; 
            Password = {PASS};";

        public static readonly string SMTP_USERNAME = Env.Get("SMTP_USERNAME");
        public static readonly string SMTP_USER_PASSWORD = Env.Get("SMTP_USER_PASSWORD");
        public static readonly string SMTP_SERVER_ADDRESS = Env.Get("SMTP_SERVER_ADDRESS");
        public static readonly string HOST_RABBITMQ = Env.Get("HOST_RABBITMQ");
        public static readonly string USERNAME_RABBITMQ = Env.Get("USERNAME_RABBITMQ");
        public static readonly string PASSWORD_RABBITMQ = Env.Get("PASSWORD_RABBITMQ");
        public static readonly string PORT_RABBITMQ = Env.Get("PORT_RABBITMQ");
        public static readonly string NAME_QUEUE_RABBITMQ = Env.Get("NAME_QUEUE_RABBITMQ");

        public static readonly string APPLICATION_NAME = "worker-gerarboletim";
    }
}
