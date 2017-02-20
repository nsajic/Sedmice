using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedmice
{
    public class SedmiceRacunanje
    {

        public static List<Karta> sveKarte = new List<Karta>();         // Ceo spil karata (32) ~~ NE KORISTI SE!!!
        public static List<Karta> prosleKarte = new List<Karta>();      // Karte koje su (bile) na talonu + karte u ruci --> "vidljive karte"

        // Konstruktor
        public SedmiceRacunanje()
        {
            popuniSpil();                                               // inicijalno popunjavamu listu svih karata
        }

        // Metoda za popunjavanje liste svih karata     ~~ NE KORISTI SE!!!
        private void popuniSpil()                                       
        {
            List<string> znakovi = new List<string>();
            List<string> brojevi = new List<string>();
            znakovi.Add("H");
            znakovi.Add("P");
            znakovi.Add("K");
            znakovi.Add("T");

            brojevi.Add("A");
            brojevi.Add("7");
            brojevi.Add("8");
            brojevi.Add("9");
            brojevi.Add("10");
            brojevi.Add("J");
            brojevi.Add("Q");
            brojevi.Add("K");

            foreach (string tempZnak in znakovi)
            {
                foreach (string tempBroj in brojevi)
                {
                    sveKarte.Add(new Karta(tempZnak, tempBroj));
                }
            }
        }

        // return: Dictionary<Karta, int> --> Karta - procenat kombinacija za svaku kartu iz ruke!
        public Dictionary<Karta, float> procenaPoteza(List<Karta> talonKarte, List<Karta> rukaKarte)
        {
            Dictionary<Karta, float> kartaSaProcentom = new Dictionary<Karta, float>();

            kartaSaProcentom = izracunajProcente(talonKarte, rukaKarte);

            // Ovde mozda proveriti da li se u prosleKarte vec nalazi neka karta iz talonKarte ili rukaKarte
            bool tempNeki = true;
            foreach (Karta k in talonKarte)
            {
                foreach (Karta pk in prosleKarte)
                {
                    if (pk.broj.Equals(k.broj) && pk.znak.Equals(k.znak))
                    {
                        tempNeki = false;
                        break;
                    }
                }
                if (tempNeki)
                {
                    prosleKarte.Add(k);
                    //ThreadHelperClass.SetText(this, textBox15, textBox15.Text + Environment.NewLine + k.broj + "  -  " + k.znak);
                    //ServerSide.ProsleKarte.Items.Add(k.broj + "  -  " + k.znak);
                    tempNeki = true;
                }
            }

            tempNeki = true;
            foreach (Karta k in rukaKarte)
            {
                foreach (Karta pk in prosleKarte)
                {
                    if (pk.broj.Equals(k.broj) && pk.znak.Equals(k.znak))
                    {
                        tempNeki = false;
                    }
                }
                if (tempNeki)
                {
                    prosleKarte.Add(k);
                    //ThreadHelperClass.SetText(this, textBox15, textBox15.Text + Environment.NewLine + k.broj + "  -  " + k.znak);
                    //ServerSide.ProsleKarte.Items.Add(k.broj + "  -  " + k.znak);
                    tempNeki = true;
                }
            }

            //prosleKarte.AddRange(talonKarte);
            //prosleKarte.AddRange(rukaKarte);

            return kartaSaProcentom;
        }

        // Moguca stanja... Razraditi metodu da duplikat karte imaju veci znacaj.
        public List<List<string>> mogucaStanja(List<Karta> talonKarte, List<Karta> karteURuci)
        {
            List<List<string>> stanja = new List<List<string>>();       // Sva moguca (buduca) stanja za svaku od karata u ruci
            int redniBrojKarte = 0;                                     // redom karte u ruci

            List<Karta> karteZaProveru = izbaciDuplikate(karteURuci);

            while (redniBrojKarte != (karteZaProveru.Count))
            {                                                           // I nivo
                List<string> tempProsleKarte = new List<string>();
                for (int i = 0; i < karteURuci.Count; i++)
                {
                    tempProsleKarte.Add(karteURuci[i].broj);
                }
                for (int i = 0; i < prosleKarte.Count; i++)
                {
                    tempProsleKarte.Add(prosleKarte[i].broj);
                }

                List<string> karteKojesuOstale = pretraziKarteKojeSuOstaleString(tempProsleKarte);

                for (int i = 0; i < karteKojesuOstale.Count; i++)
                {                                                   // II nivo
                    tempProsleKarte.Add(karteKojesuOstale[i]);
                    List<string> karteKojesuOstale2 = pretraziKarteKojeSuOstaleString(tempProsleKarte);

                    if (karteKojesuOstale2.Count == 0)
                    {
                        List<string> jednoStanje = new List<string>();

                        for (int t = 0; t < talonKarte.Count; t++)
                        {
                            jednoStanje.Add(talonKarte[t].broj);
                        }

                        jednoStanje.Add(karteZaProveru[redniBrojKarte].broj);
                        jednoStanje.Add(karteKojesuOstale[i]);

                        stanja.Add(jednoStanje);
                    }

                    for (int j = 0; j < karteKojesuOstale2.Count; j++)
                    {   // III nivo
                        tempProsleKarte.Add(karteKojesuOstale2[j]);
                        List<string> karteKojesuOstale3 = pretraziKarteKojeSuOstaleString(tempProsleKarte);


                        if (karteKojesuOstale3.Count == 0)
                        {
                            List<string> jednoStanje = new List<string>();

                            for (int t = 0; t < talonKarte.Count; t++)
                            {
                                jednoStanje.Add(talonKarte[t].broj);
                            }

                            jednoStanje.Add(karteZaProveru[redniBrojKarte].broj);
                            jednoStanje.Add(karteKojesuOstale[i]);
                            jednoStanje.Add(karteKojesuOstale[j]);

                        }


                        for (int k = 0; k < karteKojesuOstale3.Count; k++)
                        {   // IV nivo

                            List<string> jednoStanje = new List<string>();

                            //PROBA TALON:
                            for (int t = 0; t < talonKarte.Count; t++)
                            {
                                jednoStanje.Add(talonKarte[t].broj);
                            }

                            jednoStanje.Add(karteZaProveru[redniBrojKarte].broj);                   // 10 7 10 K
                            jednoStanje.Add(karteKojesuOstale[i]);
                            jednoStanje.Add(karteKojesuOstale2[j]);
                            jednoStanje.Add(karteKojesuOstale3[k]);

                            stanja.Add(jednoStanje);
                        }
                        tempProsleKarte.Remove(karteKojesuOstale2[j]);
                    }
                    tempProsleKarte.Remove(karteKojesuOstale[i]);
                }
                redniBrojKarte++;
            }
            return stanja;
        }

        // Pretraga karata koje su nepoznate (u spilu ili kod protivnika).
        public List<string> pretraziKarteKojeSuOstaleString(List<string> tempProsleKarte)
        {
            List<string> temp = new List<string>();
            List<int> broj = new List<int>();
            broj.Add(4);    // 7
            broj.Add(4);    // 8
            broj.Add(4);    // 9
            broj.Add(4);    // 10
            broj.Add(4);    // A
            broj.Add(4);    // J
            broj.Add(4);    // Q
            broj.Add(4);    // K

            for (int i = 0; i < tempProsleKarte.Count; i++)
            {
                if (tempProsleKarte[i] == "A")
                {
                    broj[4]--;
                }
                else if (tempProsleKarte[i] == "J")
                {
                    broj[5]--;
                }
                else if (tempProsleKarte[i] == "Q")
                {
                    broj[6]--;
                }
                else if (tempProsleKarte[i] == "K")
                {
                    broj[7]--;
                }
                else if (Int32.Parse(tempProsleKarte[i]) == 7)
                {
                    broj[0]--;
                }
                else if (Int32.Parse(tempProsleKarte[i]) == 8)
                {
                    broj[1]--;
                }
                else if (Int32.Parse(tempProsleKarte[i]) == 9)
                {
                    broj[2]--;
                }
                else if (Int32.Parse(tempProsleKarte[i]) == 10)
                {
                    broj[3]--;
                }
            }

            if (broj[0] != 0)
            {
                temp.Add("7");
            }
            if (broj[1] != 0)
            {
                temp.Add("8");
            }
            if (broj[2] != 0)
            {
                temp.Add("9");
            }
            if (broj[3] != 0)
            {
                temp.Add("10");
            }
            if (broj[4] != 0)
            {
                temp.Add("A");
            }
            if (broj[5] != 0)
            {
                temp.Add("J");
            }
            if (broj[6] != 0)
            {
                temp.Add("Q");
            }
            if (broj[7] != 0)
            {
                temp.Add("K");
            }

            return temp;
        }

        // Izbacuje duplikate (za: 7, 7, 8, 9 vraca: 7, 8, 9).
        public List<Karta> izbaciDuplikate(List<Karta> karteURuci)
        {
            List<Karta> bezDuplikata = new List<Karta>();

            for(int i=0; i<karteURuci.Count; i++)
            {
                bool temp = true;
                for(int j=i+1; j<karteURuci.Count; j++)
                {
                    if (karteURuci[i].broj.Equals(karteURuci[j].broj))
                    {
                        temp = false;
                        break;
                    }
                }
                if (temp)
                {
                    bezDuplikata.Add(karteURuci[i]);
                }
            }

            return bezDuplikata;
        }

        // Korisnik igra prvi.
        public Dictionary<Karta, float> izracunajProcente(List<Karta> talonKarte, List<Karta> rukaKarte)
        {
            Dictionary<Karta, float> kartaSaProcentom = new Dictionary<Karta, float>();

            List<List<string>> stanja = mogucaStanja(talonKarte, rukaKarte);

            string promenaKarte = "";
            Dictionary<string, int> uspesnostKarte = new Dictionary<string, int>();

            
            // TODO: Postoji mogucnost ne dodeljivanje odnetih karata, ukolo se 3 puta nastavi (primer: I: 8, P: 8, I: 8, P: 8)...
            foreach (List<string> jednoStanje in  stanja)       // ovde sad imamo 4 karte za 4 bacanja
            {
                if (!promenaKarte.Equals(jednoStanje[talonKarte.Count]))
                {
                    promenaKarte = jednoStanje[talonKarte.Count];       // jednoStanje[talonKarte.Count]  -->>  talonKarte.Count = 3 -> karta u nasoj ruci je cetvrta (sa indexom 3)
                    uspesnostKarte.Add(promenaKarte, 0);
                }

                // PARTIJA
                bool igrac1IgraPrvi;
                if (talonKarte.Count%2 == 0)
                    igrac1IgraPrvi = true;
                else
                    igrac1IgraPrvi = false;

                bool odgovaraNaKartu = false;
                bool nastavlja = false;

                List<string> tempOdneo1 = new List<string>();
                List<string> tempOdneo2 = new List<string>();
                List<string> tempBacio1 = new List<string>();
                List<string> tempBacio2 = new List<string>();
                List<string> tempTalon = new List<string>();

                for (int brojPartije = 0; brojPartije < jednoStanje.Count / 2; brojPartije++)
                {
                    if (igrac1IgraPrvi)
                    {
                        string bacio1 = jednoStanje[brojPartije * 2 + talonKarte.Count % 2];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2 - talonKarte.Count % 2 + 1];       // RAZLICITOO!!!!!
                        tempBacio2.Add(bacio2);

                        odgovaraNaKartu = proveraOdgovaranjaK(bacio1, bacio2);

                        if (odgovaraNaKartu)        // igrac 2 odlucuje da "ubije" igraca 1
                        {
                            nastavlja = false;
                            try
                            {
                                if (jednoStanje[brojPartije * 2 + 2].Equals(bacio1) || jednoStanje[brojPartije * 2 + 2].Equals("7"))
                                    nastavlja = verovatnocaNastavljanja(jednoStanje[brojPartije * 2 + 2], bacio1, bacio2, tempTalon);
                            }
                            catch (Exception e)
                            {

                            }

                            if (nastavlja)          // igrac 1 odlucuje da nastavi
                            {
                                igrac1IgraPrvi = true;
                                tempTalon.Add(bacio1);
                                tempTalon.Add(bacio2);
                            }
                            else
                            {
                                igrac1IgraPrvi = false;
                                tempOdneo2.Add(bacio1);
                                tempOdneo2.Add(bacio2);
                                tempOdneo2.AddRange(tempTalon);
                            }
                        }
                        else
                        {                      // igrac 2 odustaje, igrac 1 nosi i vuce prvi.
                            igrac1IgraPrvi = true;
                            tempOdneo1.Add(bacio1);
                            tempOdneo1.Add(bacio2);
                            tempOdneo1.AddRange(tempTalon);
                        }
                    }
                    else
                    {
                        string bacio1 = jednoStanje[brojPartije * 2 + talonKarte.Count % 2];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2 - talonKarte.Count % 2 + 1];
                        tempBacio2.Add(bacio2);
                        odgovaraNaKartu = proveraOdgovaranjaK(bacio2, bacio1);

                        if (odgovaraNaKartu)        // igrac 1 odlucuje da "ubije" igraca 2
                        {
                            nastavlja = false;

                            try
                            {
                                if (jednoStanje[brojPartije * 2 + 2].Equals(bacio1) || jednoStanje[brojPartije * 2 + 2].Equals("7"))
                                    nastavlja = verovatnocaNastavljanja(jednoStanje[brojPartije * 2 + 2], bacio1, bacio2, tempTalon);
                            }
                            catch (Exception e) { }

                            if (nastavlja)          // igrac 2 odlucuje da nastavi
                            {
                                igrac1IgraPrvi = false;
                                tempTalon.Add(bacio1);
                                tempTalon.Add(bacio2);
                            }
                            else
                            {
                                igrac1IgraPrvi = true;
                                tempOdneo1.Add(bacio1);
                                tempOdneo1.Add(bacio2);
                                tempOdneo1.AddRange(tempTalon);
                            }
                        }
                        else
                        {                      // igrac 1 odustaje, igrac 2 nosi i vuce prvi.
                            igrac1IgraPrvi = false;
                            tempOdneo2.Add(bacio1);
                            tempOdneo2.Add(bacio2);
                            tempOdneo2.AddRange(tempTalon);
                        }

                        // KRAJ PARTIJE
                    }
                }

                uspesnostKarte[promenaKarte] += izracunajUspehStanja(tempOdneo1, tempOdneo2, tempBacio1, tempBacio2, igrac1IgraPrvi);

            }

            Dictionary<Karta, int> kartaUspesnost = new Dictionary<Karta, int>();
            foreach(Karta k in rukaKarte)
            {
                if(uspesnostKarte[k.broj] > 0)                              // Zbog negativnih brojeva...
                    kartaUspesnost.Add(k, uspesnostKarte[k.broj]);
                else
                    kartaUspesnost.Add(k, 0);
            }

            // TODO: DODAJ NA KARTE KOJE SE PONAVLJAJU (procetan, duplo, fixni broj,...)
            List<Karta> bezDuplikata = izbaciDuplikate(rukaKarte);
            foreach(Karta karta in rukaKarte)
            {
                kartaSaProcentom.Add(karta, (float)Math.Round(((float)kartaUspesnost[karta] * 100) / kartaUspesnost.Values.Sum(), 2));
            }

            return kartaSaProcentom;
        }

        // Provera da li bacena karta odgovara onoj na talonu.
        public bool proveraOdgovaranjaK(string k1, string k2)
        {
            string k1Temp = k1;
            string k2Temp = k2;

            if (k1Temp.Equals(k2Temp))
            {
                return true;
            }
            else if (k2Temp == "7")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Racuna mogucnost da se nastavi na kartu (ako uopste imamo kartu za nastavljanje u ruci).
        public bool verovatnocaNastavljanja(string trenutno, string bacio1, string bacio2, List<string> tempTalon)
        {
            bool temp = false;
            int uspeh = 0;

            if (trenutno.Equals("10") || trenutno.Equals("A"))
            {
                if (bacio1.Equals("10") || bacio1.Equals("A"))
                    uspeh += 10;
            }
            else if (trenutno.Equals("7"))
            {
                uspeh -= 5;
            }
            else
            {
                uspeh += 5;
            }

            if (bacio2.Equals("10") || bacio2.Equals("A"))
                uspeh += 10;

            if (bacio2.Equals("7"))
                uspeh += 20;

            if (uspeh >= 5)
                temp = true;

            return temp;
        }

        // Racuna uspeh za svaku kartu u ruci (bez duplikata).
        public int izracunajUspehStanja(List<string> tempOdneo1, List<string> tempOdneo2, List<string> tempBacio1, List<string> tempBacio2, bool igraPrviIgrac1)
        {
            int koeficijentOdneo = 0;
            int koeficijentBacio = 0;
            int koeficijentBacioPunu = 0;
            int prviIgrac1 = 0;

            for (int i = 0; i < tempOdneo1.Count; i++)
            {
                if (tempOdneo1[i] == "A" || tempOdneo1[i] == "10")
                {
                    koeficijentOdneo += 1;
                }
            }

            for (int i = 0; i < tempOdneo2.Count; i++)
            {
                if (tempOdneo2[i] == "A" || tempOdneo2[i] == "10")
                {
                    koeficijentOdneo -= 1;
                }
            }

            for (int i = 0; i < tempBacio1.Count; i++)
            {
                if (tempBacio1[i] == "7")
                {
                    koeficijentBacio -= 3;
                }
            }

            for (int i = 0; i < tempBacio2.Count; i++)
            {
                if (tempBacio2[i] == "7")
                {
                    koeficijentBacio += 1;
                }
            }

            for (int i = 0; i < tempBacio1.Count; i++)
            {
                if (tempBacio2[i] == "10" || tempBacio2[i] == "A")
                {
                    koeficijentBacioPunu -= 1;
                }
            }


            if (igraPrviIgrac1)        // Proveriti....
                prviIgrac1 = 1;

            return koeficijentBacioPunu + 2 * koeficijentOdneo + 5 * koeficijentBacio + prviIgrac1 * 20;
        }
    }
}