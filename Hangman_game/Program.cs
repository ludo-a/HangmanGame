using Hangman_game;

static void AfficherMot(string mot, List<char> lettres)
{
    for(int i = 0; i < mot.Length; i++)
    {
        char lettre = mot[i];
        if (lettres.Contains(lettre))
        {
            Console.Write(lettre + " ");
        }
        else
        {
            Console.Write("_ ");
        }
    } 
}

static bool ToutesLettresDevinees(string mot, List<char> lettres)
{
    foreach(var lettre in lettres)
    {
        mot = mot.Replace(lettre.ToString(), "");
    }
    if(mot.Length == 0)
    {
        return true;
    }
    return false;
}
static char DemanderUneLettre(string message = "Entrer votre caractère : ")
{
    while(true)
    {
        Console.WriteLine();
        Console.Write(message);
        var reponse = Console.ReadLine();
        if (reponse?.Length == 1)
        {
            reponse = reponse.ToUpper();
            return reponse[0];
        }
        Console.WriteLine("ERREUR : Vous devez rentrer un seul caractère");
    }
}

static void DevinerMot(string mot)
{
    List<char> lettres = new List<char>();
    List<char> lettresExclues = new List<char>();
    const int NB_VIES = 6;
    int viesRestantes = NB_VIES;
    while (viesRestantes > 0)
    {
        Console.WriteLine(Ascii.PENDU[NB_VIES-viesRestantes]);

        AfficherMot(mot, lettres);
        char x = DemanderUneLettre();
        Console.Clear();
        
        //test si lettre a déja été tapé
        if (!lettres.Contains(x))
        {
            lettres.Add(x);

            //test si lettre dans le mot
            if (mot.Contains(x))
            {
                Console.WriteLine("Cette lettre est dans le mot");
                if (ToutesLettresDevinees(mot, lettres))
                {
                    break;
                }
            }
            else
            {
                viesRestantes--;
                lettresExclues.Add(x);
                Console.WriteLine("Vies restantes : " + viesRestantes);
            }

            if(lettresExclues.Count > 0)
            {
                Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(", ", lettresExclues));
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("ATTENTION: la lettre a déja été saisie");
            Console.WriteLine("Le mot ne contient pas les lettres : " + String.Join(", ", lettresExclues));
        }
    }
    if(viesRestantes == 0)
    {
        Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
        Console.WriteLine($"Vous avez perdu, le mot était {mot}");
    }
    else
    {
        AfficherMot(mot, lettres);
        Console.WriteLine();
        Console.WriteLine("Gagné");
    }

    //AfficherMot();
    //DemanderUneLettre
}

static string[] ChargerLesMots(string nomFichier)
{
    try
    {
        return File.ReadAllLines(nomFichier);
    }
    catch(Exception ex) 
    {
        Console.WriteLine("Erreur de lecture du fichier : " + nomFichier + " (" + ex.Message);
    }
    return null;
}


static bool Rejouer()
{
    
    char reponse = DemanderUneLettre("Voulez-vous rejouer (o/n) : ");
    if (reponse == 'O')
    {
        return true;
    }
    else if(reponse == 'N')
    {
        return false;
    }
    else
    {
        Console.WriteLine("Erreur : Vous devez répondre avec o ou n");
        return Rejouer();
    }
    
}

var mots = ChargerLesMots("mots.txt");
if((mots ==null) || (mots.Length == 0))
{
    Console.WriteLine("La liste de mots est vide");
}
else
{
    while (true)
    {
        Random rnd = new Random();
        int numeroRnd = rnd.Next(mots.Length);
        string mot = mots[numeroRnd].Trim().ToUpper();
        DevinerMot(mot);
        if (!Rejouer())
        {
            Console.WriteLine("Merci et à bientôt");
            break;
        }
        Console.Clear();
    }
}
