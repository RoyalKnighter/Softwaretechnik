namespace SingletonPattern
{
    public class Singleton
    {    
        private static string aMessage = string.Empty;


        
        private static Singleton singleton;
        public static Singleton CreateSingleton(string message) {
            if (singleton == null)
                singleton = new Singleton(message);

            return singleton;
        }



        private Singleton(string message) => aMessage = message;

        // Gibt den Text zurück, statt ihn wie in der Konsolenversion
        // mit Console.WriteLine auszugeben: Die Anzeige übernimmt das Fenster.
        public string sayHi()
        {
            return "Singleton Message " + aMessage;
        }
    }
}
