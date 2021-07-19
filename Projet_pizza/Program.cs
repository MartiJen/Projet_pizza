using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Projet_pizza
{
    class PizzaPersonnalisee : Pizza
    {
        static int nbPizzasPersonnalisee = 0;

        public PizzaPersonnalisee() : base("Personnalisee", 5, false, null)
        {
            nbPizzasPersonnalisee++;
            nom = "Personnalisée " + nbPizzasPersonnalisee;

            ingredients = new List<string>();
            

            while (true)
            {
                Console.Write("Rentrez un ingredient pour la pizza personnalisée " + nbPizzasPersonnalisee + " (ENTER pour terminer) : ");
                var ingredient = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(ingredient))
                {
                    break;
                }
                if (ingredients.Contains(ingredient))
                {
                    Console.WriteLine("Erreur : Cet ingrédient est déjà présent dans la pizza.");
                }
                else
                {
                    ingredients.Add(ingredient);
                    Console.WriteLine(string.Join(", ", ingredients));
                }
                Console.WriteLine();
            }

            prix = 5 + ingredients.Count * 1.5f;
        }
    }
    class Pizza
    {
        public string nom { get; protected set; }
        public float prix { get; protected set; }
        bool vegetarienne;
        public List<string> ingredients { get; protected set; }

        public Pizza(string nom, float prix, bool vegetarienne, List<string> ingredients)
        {
            this.nom = nom;
            this.prix = prix;
            this.vegetarienne = vegetarienne;
            this.ingredients = ingredients;
        }

        public void Afficher()
        {
            /*string badgeVegetarienne = " (V)";
            if(!vegetarienne)
            {
                badgeVegetarienne = "";
            }*/

            string badgeVegetarienne = vegetarienne ? " (V)" : "";

            string nomAfficher = FormatPremièreLettreMajuscules(nom);

            /*var ingredientsAfficher = new List<string>();
            foreach(var ingredient in ingredients)
            {
                ingredientsAfficher.Add(FormatPremièreLettreMajuscules(ingredient));
            }*/

            var ingredientsAfficher = ingredients.Select(i => FormatPremièreLettreMajuscules(i)).ToList();

            Console.WriteLine(nomAfficher + badgeVegetarienne + " - " + prix + "€");
            Console.WriteLine(string.Join(",", ingredients));
            Console.WriteLine();

        }
       
        private static string FormatPremièreLettreMajuscules(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            string minuscules = s.ToLower();
            string majuscules = s.ToUpper();

            string resultat = majuscules[0] + minuscules[1..];

            return resultat;
        }

        public bool ContientIngredient(string ingredient)
        {
            return ingredients.Where(i => i.ToLower().Contains(ingredient)).ToList().Count > 0;
        }
    }
    class Program
    {
        // List<Pizza> GetPizzaFromCode()
        // List<Pizza> GetPizzasFromFile(filename)
        // GenerateJsonFile(pizzas, filename)

        static List<Pizza> GetPizzasFromCode()
        {
            List<Pizza> listPizza = new List<Pizza>() { new Pizza("4 fromages", 11.5f, true, new List<string>() {"chèvre",  "cantal", "Mozarella", "gruyère"}),
                                                    new Pizza("indienne", 11.5f, false, new List<string>() {"curry",  "poulet", "Mozarella", "poivron", "oignons","coriandre"}),
                                                    new Pizza("margherita", 9.0f, true, new List<string>() {"sauce tomates",  "Mozarella", "basilic"}),
                                                    new Pizza("mexicaine", 13f, false, new List<string>() {"boeuf",  "maïs", "Mozarella", "tomates", "oignons", "coriandre"}),
                                                    new Pizza("chicken Barbecue", 12.0f, false, new List<string>() {"Sauce Barbecue",  "filet de poulet roti", "Mozarella", "champignons frais", "oignons rouges frais", "poivrons verts frais"}),
                                                    new Pizza("calzone", 12f, false, new List<string>() {"tomate",  "jambon", "persil", "oignons"}),
                                                    new Pizza("complète", 9.5f, false, new List<string>() {"jambom",  "oeuf", "fromage"}),
                                                    new Pizza("spicy hot one", 13.0f, false, new List<string>() {"sauce tomates à l'origan", "merguez", "Mozarella", "fillet de poulet roti", "oignons rouges frais", "sauce samouraï"}),
                                                    //new PizzaPersonnalisee()
                                                    };
            return listPizza;
        }

        static List<Pizza> GetPizzasFromFile(string filename)
        {
            string json = null;
            try
            {
                json = File.ReadAllText(filename);
            }
            catch
            {
                Console.WriteLine("Erreur de lecture du fichier : " + filename);
                return null;
            }
            //Console.WriteLine(json)
            List<Pizza> listPizza = null;
            try
            {
                listPizza = JsonConvert.DeserializeObject<List<Pizza>>(json);
            }
            catch
            {
                Console.WriteLine("Erreur les données json ne sont pas valides");
                return null;
            }

            return listPizza;
        }

        static void GenerateJsonFile(List<Pizza> listPizza, string filename)
        {
            string json = JsonConvert.SerializeObject(listPizza);
            //Console.WriteLine(json);
            File.WriteAllText(filename, json);
        }

        static List<Pizza> GetPizzasFromUrl(string url)
        {
            var webclient = new WebClient();
            string json = webclient.DownloadString(url);

            List<Pizza> listPizza = null;
            try
            {
                listPizza = JsonConvert.DeserializeObject<List<Pizza>>(json);
            }
            catch
            {
                Console.WriteLine("Erreur les données json ne sont pas valides");
                return null;
            }

            return listPizza;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var filename = "pizzas.json";

            //var pizzas = GetPizzasFromCode();
            //GenerateJsonFile(pizzas, filename);
            //var pizzas = GetPizzasFromFile(filename);

            // https://codeavecjonathan.com/res/pizzas2.json

            // GetPizzasFromURL("https://codeavecjonathan.com/res/pizzas2.json");
            // Webclient -> DownloadString() -> string
            // Deserialiser -> Pizzas

            var pizzas = GetPizzasFromUrl("https://codeavecjonathan.com/res/pizzas2.json");

            if (pizzas != null)
            {
                foreach (var pizza in pizzas)
                {
                    pizza.Afficher();
                }
            }
            

            //listPizza = listPizza.OrderBy(p => p.prix).ToList();
            /*Pizza pizzaPrixMin = listPizza[0];
            Pizza pizzaPrixMax = listPizza[0];



            foreach(var pizza in listPizza)
            {
                if(pizza.prix < pizzaPrixMin.prix)
                {
                    pizzaPrixMin = pizza;
                }
                if (pizza.prix > pizzaPrixMax.prix) 
                {
                    pizzaPrixMax = pizza;
                }
            }*/

            //listPizza = listPizza.Where(p => p.vegetarienne).ToList();

            //listPizza = listPizza.Where(p => p.ContientIngredient("tomate")).ToList();


            /*Console.WriteLine();
            Console.WriteLine("La pizza la moins chere est :");
            pizzaPrixMin.Afficher();
            Console.WriteLine();
            Console.WriteLine("La pizza la plus chere est :");
            pizzaPrixMax.Afficher();*/
        }
    }
}
