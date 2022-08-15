namespace BHub.Api.Environments
{
    public static class Constants
    {
        public static readonly string HOST = Env.Get("DATABASE_HOST");
        public static readonly string DATABASE = Env.Get("DATABASE_NAME");
        public static readonly string USER = Env.Get("DATABASE_USER");
        public static readonly string PASS = Env.Get("DATABASE_PASS");
        public static readonly string APP_URL = Env.Get("APP_URL");
        public static readonly string ConnectionString = $@"
            Server = {HOST};
            Database = {DATABASE}; 
            User Id  =  {USER}; 
            Password = {PASS};";

        public static readonly string HOST_RABBITMQ = Env.Get("HOST_RABBITMQ");
        public static readonly string USERNAME_RABBITMQ = Env.Get("USERNAME_RABBITMQ");
        public static readonly string PASSWORD_RABBITMQ = Env.Get("PASSWORD_RABBITMQ");
        public static readonly string PORT_RABBITMQ = Env.Get("PORT_RABBITMQ");
        public static readonly string NAME_EXCHANGE_RABBITMQ = Env.Get("NAME_EXCHANGE_RABBITMQ");

        public static readonly string APPLICATION_NAME = "api-pricemonitoring";
    }
}
