using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Sedmice
{
    public partial class Form1 : Form
    {
        public List<string> znakovi = new List<string>();
        public List<string> brojevi = new List<string>();
        public List<Karta> karte = new List<Karta>();
        public List<bool> kartaUIgri = new List<bool>();

        public static string selektovana = "";

        //public static Dictionary<Karta, bool> kartee = new Dictionary<Karta, bool>();

        public static List<Karta> igrac1 = new List<Karta>();
        public static List<Karta> igrac2 = new List<Karta>();
        public static List<Karta> igrac1Odneo = new List<Karta>();
        public static List<Karta> igrac2Odneo = new List<Karta>();
        public static List<Karta> prosleKarte = new List<Karta>();
        public static List<Karta> talonKarte = new List<Karta>();

        public string bacio2;

        public static int stanje = 1;           // 1 -> igrac 1 igra prvi, 2 -> igrac 2 igra prvi, 3 -> igrac 2 odgova na kartu

        //POKU

        bool igrac1IgraPrvi = true;

        bool odgovaraNaKartu = false;
        bool nastavlja = false;

        int brojKarataURuci1 = 4;
        int brojKarataURuci2 = 4;
        int x = 1;

        //~POKU

        public Form1()
        {
            InitializeComponent();
            popuniListe();
        }

        private void popuniListe()
        {
            znakovi.Add("herc");
            znakovi.Add("pik");
            znakovi.Add("karo");
            znakovi.Add("tref");

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
                    karte.Add(new Karta(tempZnak, tempBroj));
                    kartaUIgri.Add(true);
                    //kartee.Add(new Karta(tempZnak, tempBroj), true);
                }
            }
        }

        private void startGame_Click(object sender, EventArgs e)
        {
            podeliKarte();

            dodajUFormu();
        }

        public void dodajUFormu()
        {
            brojKarata1.Text = igrac1.Count.ToString();
            brojKarata2.Text = igrac2.Count.ToString();

            this.mojeKarte.Items.Clear();

            for (int i = 0; i < igrac1.Count; i++)
            {
                this.mojeKarte.Items.Add(igrac1[i].znak + " " + igrac1[i].broj);
            }
        }

        private void baciKartu_Click(object sender, EventArgs e)
        {
            selektovana = (string)this.mojeKarte.SelectedItem;
            
            // STAVITI DA NISTA NIJE SELEKTOVANO!

            if (selektovana == "" || selektovana == null)
                return;

            brojKarataURuci1--;
            string bacio1 = selektovana;    // iz liste uzeti, nakon klika na dugme Baci
            for (int i = 0; i < igrac1.Count; i++)
            {
                if (bacio1.Contains(igrac1[i].broj) && bacio1.Contains(igrac1[i].znak))
                {
                    talonKarte.Add(igrac1[i]);
                    prosleKarte.Add(igrac1[i]);
                    //karte.Remove(igrac1[i]);
                    igrac1.Remove(igrac1[i]);
                    this.mojeKarte.SelectedItem = "";
                    dodajUFormu();
                }
            }

            //dodajUFormu();

            if (stanje == 1) { 
                // baca igrac 2

                brojKarataURuci2--;

                //dddd
                if (karte.Count == 0)
                {
                    if (igrac1.Count == 0 && igrac2.Count == 0)
                    {
                        label1.Text = "ZAVRSENA IGRA!";
                        label2.Text = "Igrac:";
                        label3.Text = "Kompjuter:";
                        label4.Text = poeniIgrac(igrac1Odneo).ToString();
                        label5.Text = poeniKomp(igrac2Odneo).ToString();
                        dodajUFormu();
                        return;
                    }
                }
                //~dddd

                bacio2 = izracunajNajboljuJaPrvi(igrac2, prosleKarte);
                kartaTalon.Text = bacio2;

                for (int i = 0; i < igrac2.Count; i++)
                {
                    if (bacio2.Contains(igrac2[i].broj) && bacio2.Contains(igrac2[i].znak))
                    {
                        talonKarte.Add(igrac2[i]);
                        prosleKarte.Add(igrac2[i]);
                        //karte.Remove(igrac2[i]);
                        igrac2.Remove(igrac2[i]);
                        
                    }
                }

                odgovaraNaKartu = proveraOdgovaranja(bacio1, bacio2);

                if (odgovaraNaKartu)        // igrac 2 odlucuje da "ubije" igraca 1
                {
                    stanje = 3;
                    odustani.Enabled = true;
                }
                else
                {                      // igrac 2 odustaje, igrac 1 nosi i vuce prvi.
                    igrac1IgraPrvi = true;
                    odustani.Enabled = false;
                    stanje = 1;

                    //u listu igracOdneo1 ubaciti karte sa talona T
                    igrac1Odneo.AddRange(talonKarte);
                    talonKarte.Clear();

                    deliKarte();
                    brojKarataURuci1 += x;
                    brojKarataURuci2 += x;
                }
            }
            else if (stanje == 2) { 
                // proveri odgovaranje na kartu
                odgovaraNaKartu = proveraOdgovaranja(bacio2, bacio1);

                if (odgovaraNaKartu)
                {
                    // da li komp nastavlja?
                    int tempNastavlja = -1;
                    try
                    {
                        int[] tempVerovatnoca = {0, 0, 0, 0};
                        for (int xx = 0; xx < igrac2.Count; xx++ )
                            
                            if (bacio1.Contains(igrac2[xx].broj) || igrac2[xx].broj.Equals("7"))
                            {
                                List<string> tempTalon = new List<string>();
                                for (int z = 0; z < talonKarte.Count; z++) {
                                    tempTalon.Add(talonKarte[z].broj);
                                }
                                
                                tempVerovatnoca[xx] = verovatnocaNastavljanjaInt(igrac2[xx].broj, bacio1, bacio2, tempTalon);
                            }
                        if (tempVerovatnoca.Max() >= 5) {
                            for (int v = 0; v < tempVerovatnoca.Length; v++) {
                                if (tempVerovatnoca.Max().Equals(tempVerovatnoca[v]))
                                {
                                    tempNastavlja = v;
                                }
                            }
                        }
                    }
                    catch (Exception eee)
                    {

                    }


                    if (tempNastavlja != -1)      // ako nastavlja komp! IZRACUNATI!
                    {
                        // baca igrac 2
                        x++;
                        brojKarataURuci2--;

                        //dddd
                        if (karte.Count == 0)               // OVO JE VISAK VRV
                        {
                            if (igrac1.Count == 0 && igrac2.Count == 0)
                            {
                                label1.Text = "ZAVRSENA IGRA!";
                                label2.Text = "Igrac:";
                                label3.Text = "Kompjuter:";
                                label4.Text = poeniIgrac(igrac1Odneo).ToString();
                                label5.Text = poeniKomp(igrac2Odneo).ToString();
                                dodajUFormu();
                                return;
                            }
                        }
                        //~dddd

                        bacio2 = igrac2[tempNastavlja].znak + " " + igrac2[tempNastavlja].broj;
                        kartaTalon.Text = bacio2;

                        for (int i = 0; i < igrac2.Count; i++)
                        {
                            if (bacio2.Contains(igrac2[i].broj) && bacio2.Contains(igrac2[i].znak))
                            {
                                talonKarte.Add(igrac2[i]);
                                prosleKarte.Add(igrac2[i]);
                                //karte.Remove(igrac2[i]);
                                igrac2.Remove(igrac2[i]);

                            }
                        }
                    }
                    else {
                        stanje = 1;
                        //u listu igracOdneo1 ubaciti karte sa talona T
                        igrac1Odneo.AddRange(talonKarte);
                        talonKarte.Clear();

                        deliKarte();
                        brojKarataURuci1 += x;
                        brojKarataURuci2 += x;
                    }
                }
                else
                {
                    igrac2Odneo.AddRange(talonKarte);
                    talonKarte.Clear();

                    deliKarte();
                    brojKarataURuci2 += x;
                    brojKarataURuci1 += x;

                    brojKarataURuci2--;

                    //dddd
                    if (karte.Count == 0)
                    {
                        if (igrac1.Count == 0 && igrac2.Count == 0)
                        {
                            label1.Text = "ZAVRSENA IGRA!";
                            label2.Text = "Igrac:";
                            label3.Text = "Kompjuter:";
                            label4.Text = poeniIgrac(igrac1Odneo).ToString();
                            label5.Text = poeniKomp(igrac2Odneo).ToString();
                            dodajUFormu();
                            return;
                        }
                    }
                    //~dddd

                    bacio2 = izracunajNajbolju(igrac2, prosleKarte);
                    kartaTalon.Text = bacio2;

                    for (int i = 0; i < igrac2.Count; i++)
                    {
                        if (bacio2.Contains(igrac2[i].broj) && bacio2.Contains(igrac2[i].znak))
                        {
                            talonKarte.Add(igrac2[i]);
                            prosleKarte.Add(igrac2[i]);
                            //karte.Remove(igrac2[i]);
                            igrac2.Remove(igrac2[i]);
                            
                        }
                    }
                }
            }
            else if (stanje == 3)
            {
                // baci opet ili odustani

                igrac1IgraPrvi = true;
                x++;

                brojKarataURuci2--;

                //dddd
                if (karte.Count == 0)
                {
                    if (igrac1.Count == 0 && igrac2.Count == 0)
                    {
                        label1.Text = "ZAVRSENA IGRA!";
                        label2.Text = "Igrac:";
                        label3.Text = "Kompjuter:";
                        label4.Text = poeniIgrac(igrac1Odneo).ToString();
                        label5.Text = poeniKomp(igrac2Odneo).ToString();
                        dodajUFormu();
                        return;
                    }
                }
                //~dddd

                bacio2 = izracunajNajboljuJaPrvi(igrac2, prosleKarte);
                kartaTalon.Text = bacio2;

                for (int i = 0; i < igrac2.Count; i++)
                {
                    if (bacio2.Contains(igrac2[i].broj) && bacio2.Contains(igrac2[i].znak))
                    {
                        talonKarte.Add(igrac2[i]);
                        prosleKarte.Add(igrac2[i]);
                        //karte.Remove(igrac2[i]);
                        igrac2.Remove(igrac2[i]);
                    }
                }


                odgovaraNaKartu = proveraOdgovaranja(bacio1, bacio2);

                if (odgovaraNaKartu)        // igrac 2 odlucuje da "ubije" igraca 1
                {
                    stanje = 3;
                    odustani.Enabled = true;
                }
                else
                {                      // igrac 2 odustaje, igrac 1 nosi i vuce prvi.
                    igrac1IgraPrvi = true;
                    odustani.Enabled = false;
                    stanje = 1;

                    //u listu igracOdneo1 ubaciti karte sa talona T
                    igrac1Odneo.AddRange(talonKarte);
                    talonKarte.Clear();

                    deliKarte();
                    brojKarataURuci1 += x;
                    brojKarataURuci2 += x;
                }

            }
        }

        private void deliKarte()
        {
            int temp1 = karte.Count / 2;
            Console.WriteLine("DELI KARTE temp1: " + temp1);

            if (karte.Count == 0)
            {
                x = 0;
                dodajUFormu();
                if (igrac1.Count == 0 && igrac2.Count == 0)
                {
                    label1.Text = "ZAVRSENA IGRA!";
                    label2.Text = "Igrac:";
                    label3.Text = "Kompjuter:";
                    label4.Text = poeniIgrac(igrac1Odneo).ToString();
                    label5.Text = poeniKomp(igrac2Odneo).ToString();
                    dodajUFormu();
                    return;
                }
                else
                {
                    dodajUFormu();
                    return;
                }
            }        

            if ((4 - brojKarataURuci1) <= temp1)
            {
                while (x != 0)
                {
                    while (true)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, karte.Count);

                        Karta k = karte[randomNumber];
                        //if (kartaUIgri[randomNumber] == true)
                        //{
                            Console.WriteLine("DELI KARTE ja: " + k.broj);
                            igrac1.Add(k);
                           // kartaUIgri[randomNumber] = false;
                            //kartee[k] = false;
                            karte.Remove(k);
                            break;
                       // }
                    }

                    while (true)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, karte.Count);

                        Karta k = karte[randomNumber];
                        //if (kartaUIgri[randomNumber] == true)
                        //{
                            Console.WriteLine("DELI KARTE komp: " + k.broj);
                            igrac2.Add(k);
                            //kartaUIgri[randomNumber] = false;
                            //kartee[k] = false;
                            karte.Remove(k);
                            break;
                        //}
                    }
                    dodajUFormu();
                    --x;
                }
                x = 1;
            }
            else
            {
                while (temp1 != 0)
                {
                    while (true)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, karte.Count);

                        Karta k = karte[randomNumber];
                        //if (kartaUIgri[randomNumber] == true)
                        //{
                            Console.WriteLine("DELI KARTE ja1: " + k.broj);
                            igrac1.Add(k);
                           // kartaUIgri[randomNumber] = false;
                            //kartee[k] = false;
                            karte.Remove(k);
                            break;
                        //}
                    }

                    while (true)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, karte.Count);

                        Karta k = karte[randomNumber];
                        //if (kartaUIgri[randomNumber] == true)
                        //{
                            Console.WriteLine("DELI KARTE komp1: " + k.broj);
                            igrac2.Add(k);
                            //kartaUIgri[randomNumber] = false;
                            //kartee[k] = false;
                            karte.Remove(k);
                            break;
                        //}
                    }
                    dodajUFormu();
                    --temp1;
                }
                x = 1;
            }
        }


        private void odustani_Click(object sender, EventArgs e)
        {
            odustani.Enabled = false;
            stanje = 2;
            igrac1IgraPrvi = false;

            igrac2Odneo.AddRange(talonKarte);   //u listu igracOdneo2 ubaciti karte sa talona
            talonKarte.Clear();

            brojKarataURuci2 += x;
            //iz liste "spil" dati igracu 2 x karata    NE

            brojKarataURuci1 += x;
            //iz liste "spil" dati igracu 1 x karata    NE
            deliKarte();
            x = 1;

            // IGRA KOMP PRVI!

            brojKarataURuci2--;

            //dddd
            if (karte.Count == 0)
            {
                if (igrac1.Count == 0 && igrac2.Count == 0)
                {
                    label1.Text = "ZAVRSENA IGRA!";
                    label2.Text = "Igrac:";
                    label3.Text = "Kompjuter:";
                    label4.Text = poeniIgrac(igrac1Odneo).ToString();
                    label5.Text = poeniKomp(igrac2Odneo).ToString();
                    dodajUFormu();
                    return;
                }
            }
            //~dddd

            bacio2 = izracunajNajbolju(igrac2, prosleKarte);
            kartaTalon.Text = bacio2;

            for (int i = 0; i < igrac2.Count; i++)
            {
                if (bacio2.Contains(igrac2[i].broj) && bacio2.Contains(igrac2[i].znak))
                {
                    talonKarte.Add(igrac2[i]);
                    prosleKarte.Add(igrac2[i]);
                    karte.Remove(igrac2[i]);
                    igrac2.Remove(igrac2[i]);
                }
            }
        }


        public void podeliKarte()
        {
            Random random = new Random();

            while (true)
            {
                int randomNumber = random.Next(0, karte.Count);

                Karta k = karte[randomNumber];
                if (kartaUIgri[randomNumber] == true)
                {

                    if (!igrac1.Count.Equals(4))
                    {
                        igrac1.Add(k);
                        kartaUIgri[randomNumber] = false;
                        //kartee[k] = false;
                        karte.Remove(k);
                    }
                    else if (!igrac2.Count.Equals(4))
                    {
                        igrac2.Add(k);
                        kartaUIgri[randomNumber] = false;
                        //kartee[k] = false;
                        karte.Remove(k);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        // NAJBITNIJE, OVDE RAZVIJAM STABLO!
        public string izracunajNajbolju(List<Karta> karteURuci, List<Karta> prosleKarte)
        {
            string temp = null;
            List<List<string>> mogucaStanja = new List<List<string>>();
            int[] brojStanjaZaSvakuKartu = { 0, 0, 0, 0 };

            int redniBrojKarte = 0;         // redom karte u ruci

            //Console.WriteLine("KARTE U RUCI:   " + karteURuci[0].broj + karteURuci[1].broj + karteURuci[2].broj + karteURuci[3].broj);

            while (redniBrojKarte != (karteURuci.Count))
            {                                               // I nivo
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
                {         // II nivo
                    tempProsleKarte.Add(karteKojesuOstale[i]);
                    List<string> karteKojesuOstale2 = pretraziKarteKojeSuOstaleString(tempProsleKarte);

                    if (karteKojesuOstale2.Count == 0)
                    {
                        List<string> jednoStanje = new List<string>();

                        for (int t = 0; t < talonKarte.Count; t++)
                        {
                            jednoStanje.Add(talonKarte[t].broj);
                        }

                        jednoStanje.Add(karteURuci[redniBrojKarte].broj);                   
                        jednoStanje.Add(karteKojesuOstale[i]);

                        try
                        {
                            brojStanjaZaSvakuKartu[redniBrojKarte]++;
                        }
                        catch (Exception e) { }

                        mogucaStanja.Add(jednoStanje);
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

                            jednoStanje.Add(karteURuci[redniBrojKarte].broj);
                            jednoStanje.Add(karteKojesuOstale[i]);
                            jednoStanje.Add(karteKojesuOstale[j]);

                            try
                            {
                                brojStanjaZaSvakuKartu[redniBrojKarte]++;
                            }
                            catch (Exception e) { }

                            mogucaStanja.Add(jednoStanje);
                        }


                        for (int k = 0; k < karteKojesuOstale3.Count; k++)
                        {   // IV nivo

                            List<string> jednoStanje = new List<string>();

                            //PROBA TALON:
                            for (int t = 0; t < talonKarte.Count; t++)
                            {
                                jednoStanje.Add(talonKarte[t].broj);
                            }
                            
                            jednoStanje.Add(karteURuci[redniBrojKarte].broj);                   // 10 7 10 K
                            jednoStanje.Add(karteKojesuOstale[i]);
                            jednoStanje.Add(karteKojesuOstale2[j]);
                            jednoStanje.Add(karteKojesuOstale3[k]);


                            try
                            {
                                brojStanjaZaSvakuKartu[redniBrojKarte]++;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("BAG BAG BAG!");
                            }
                            mogucaStanja.Add(jednoStanje);
                        }
                        tempProsleKarte.Remove(karteKojesuOstale2[j]);
                    }
                    tempProsleKarte.Remove(karteKojesuOstale[i]);
                }
                redniBrojKarte++;
            }           // UNELI SMO SVE MOGUCE KOMBINACIJE BACANJA KARATA u 4 bacanja (2 ruke)


            // IGRAMO PARTIJU SA SVAKOM KOMBINACIJOM I RACUNAMO KO JE POBEDIO!
            Console.WriteLine("MOGUCA STANJA:  " + mogucaStanja.Count);
            int r = 0;
            int[] uspesnost = { -15000, -15000, -15000, -15000 };
            for (int x = 0; x < karteURuci.Count; x++)
                uspesnost[x] = 0;

            int tempBrojStanja = 0;

            for (int brojStanjaBrojac = 0; brojStanjaBrojac < mogucaStanja.Count; brojStanjaBrojac++)
            {
                List<string> jednoStanje = new List<string>();
                jednoStanje = mogucaStanja[brojStanjaBrojac];             // ovde sad imamo 4 karte za 4 bacanja
                //Console.WriteLine(jednoStanje.Count + " ddd:  " + jednoStanje[0] + jednoStanje [1] + jednoStanje[2] + jednoStanje[3]);
                // PARTIJA
                bool igrac1IgraPrvi = false;            // prvo igra igrac 2 kada racunamo

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
                        string bacio1 = jednoStanje[brojPartije * 2 + 1];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2];
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
                            catch (Exception e) { 
                                
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
                        string bacio1 = jednoStanje[brojPartije * 2 + 1];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2];
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
                            catch (Exception e)
                            {
                            }


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

                
                if (karteURuci[r].broj.Equals(jednoStanje[0]) && tempBrojStanja < brojStanjaZaSvakuKartu[r])          // bice izuzetak ako komp dobije dve iste karte jednu za drugom!
                {
                    tempBrojStanja++;
                    uspesnost[r] += izracunajUspehStanja(tempOdneo1, tempOdneo2, tempBacio1, tempBacio2, igrac1IgraPrvi);
                }
                else
                {
                    ++r;
                    tempBrojStanja = 0;
                    uspesnost[r] += izracunajUspehStanja(tempOdneo1, tempOdneo2, tempBacio1, tempBacio2, igrac1IgraPrvi);
                }
            }

            // UPOREDI uspesnost i vrati onaj sa najvecom sansom!

            int tempBroj = 0;

            for (int i = 0; i < karteURuci.Count - 1; i++)
            {
                for (int j = i + 1; j < karteURuci.Count; j++)
                {
                    if (karteURuci[i].broj.Equals(karteURuci[j].broj) && karteURuci[i].broj != "7")
                    {
                        uspesnost[i] += 200;
                    }
                }
            }

            for (int i = 0; i < uspesnost.Length; i++)
            {
                if (uspesnost.Max().Equals(uspesnost[i]))
                {
                    tempBroj = i;
                    break;
                }
            }

            try
            {
                Console.WriteLine("KARTA:  " + karteURuci[0].broj + "  USPESNOST:  " + uspesnost[0]);
                Console.WriteLine("KARTA:  " + karteURuci[1].broj + "  USPESNOST:  " + uspesnost[1]);
                Console.WriteLine("KARTA:  " + karteURuci[2].broj + "  USPESNOST:  " + uspesnost[2]);
                Console.WriteLine("KARTA:  " + karteURuci[3].broj + "  USPESNOST:  " + uspesnost[3]);
            }
            catch (Exception e) { }

            Console.WriteLine("TEMP MAX:  " + uspesnost.Max() + "  TEMP BROJ:  " + tempBroj);

            temp = karteURuci[tempBroj].znak + " " + karteURuci[tempBroj].broj;

            return temp;
        }

            // STABLO KADA JA IGRAM PRVI (POSTOJI KARTA NA TALONU!)

       public string izracunajNajboljuJaPrvi(List<Karta> karteURuci, List<Karta> prosleKarte)
       {
            string temp = null;
            List<List<string>> mogucaStanja = new List<List<string>>();
            int[] brojStanjaZaSvakuKartu = { 0, 0, 0, 0 };
            
            int redniBrojKarte = 0;         // redom karte u ruci

            //Console.WriteLine("KARTE U RUCI:   " + karteURuci[0].broj + karteURuci[1].broj + karteURuci[2].broj + karteURuci[3].broj);

            Console.WriteLine("BROJ KARATA NA TALONU: " + talonKarte.Count);

            while (redniBrojKarte != (karteURuci.Count)){                                               // I nivo
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

                for (int i = 0; i < karteKojesuOstale.Count; i++) {         // II nivo
                    tempProsleKarte.Add(karteKojesuOstale[i]);
                    List<string> karteKojesuOstale2 = pretraziKarteKojeSuOstaleString(tempProsleKarte);

                    if (karteKojesuOstale2.Count == 0)
                    {
                        List<string> jednoStanje = new List<string>();

                        for (int t = 0; t < talonKarte.Count; t++)
                        {
                            jednoStanje.Add(talonKarte[t].broj);
                        }

                        jednoStanje.Add(karteURuci[redniBrojKarte].broj);
                        jednoStanje.Add(karteKojesuOstale[i]);

                        try
                        {
                            brojStanjaZaSvakuKartu[redniBrojKarte]++;
                        }
                        catch (Exception e) { }

                        mogucaStanja.Add(jednoStanje);
                    }

                    for (int j = 0; j < karteKojesuOstale2.Count; j++) {   // III nivo
                        tempProsleKarte.Add(karteKojesuOstale2[j]);
                        List<string> karteKojesuOstale3 = pretraziKarteKojeSuOstaleString(tempProsleKarte);


                        if (karteKojesuOstale3.Count == 0)
                        {
                            List<string> jednoStanje = new List<string>();

                            for (int t = 0; t < talonKarte.Count; t++)
                            {
                                jednoStanje.Add(talonKarte[t].broj);
                            }

                            jednoStanje.Add(karteURuci[redniBrojKarte].broj);
                            jednoStanje.Add(karteKojesuOstale[i]);
                            jednoStanje.Add(karteKojesuOstale[j]);

                            try
                            {
                                brojStanjaZaSvakuKartu[redniBrojKarte]++;
                            }
                            catch (Exception e) { }

                            mogucaStanja.Add(jednoStanje);
                        }


                        for (int k = 0; k < karteKojesuOstale3.Count; k++){   // IV nivo
                            // PROBA NOVI NIVO!
                           // tempProsleKarte.Add(karteKojesuOstale3[k]);
                            //List<string> karteKojesuOstale4 = pretraziKarteKojeSuOstaleString(tempProsleKarte);

                           // for (int kk = 0; kk < karteKojesuOstale4.Count; kk++)   // V nivo
                            //{   
                                List<string> jednoStanje = new List<string>();

                                //PROBA TALON:
                                for (int t = 0; t < talonKarte.Count; t++)
                                {
                                    jednoStanje.Add(talonKarte[t].broj);
                                }

                                jednoStanje.Add(karteURuci[redniBrojKarte].broj);                   // 10 7 10 K
                                jednoStanje.Add(karteKojesuOstale[i]);
                                jednoStanje.Add(karteKojesuOstale2[j]);
                                jednoStanje.Add(karteKojesuOstale3[k]);
                                //jednoStanje.Add(karteKojesuOstale4[kk]);

                                brojStanjaZaSvakuKartu[redniBrojKarte]++;

                                mogucaStanja.Add(jednoStanje);
                           // }
                           // tempProsleKarte.Remove(karteKojesuOstale3[k]);
                        }
                        tempProsleKarte.Remove(karteKojesuOstale2[j]);
                    }
                    tempProsleKarte.Remove(karteKojesuOstale[i]);
                }
                redniBrojKarte++;
            }           // UNELI SMO SVE MOGUCE KOMBINACIJE BACANJA KARATA u 4 bacanja (2 ruke)


            // IGRAMO PARTIJU SA SVAKOM KOMBINACIJOM I RACUNAMO KO JE POBEDIO!
            Console.WriteLine("MOGUCA STANJA JA PRVI:  " + mogucaStanja.Count);
            int r = 0;
            int[] uspesnost = { -15000, -15000, -15000, - 15000 };
            for (int x = 0; x < karteURuci.Count; x++)
                uspesnost[x] = 0;

            int tempBrojStanja = 0;
            for (int brojStanjaBrojac = 0; brojStanjaBrojac < mogucaStanja.Count; brojStanjaBrojac++)
            {
                List<string> jednoStanje = new List<string>();
                jednoStanje = mogucaStanja[brojStanjaBrojac];             // ovde sad imamo 4 karte za 4 bacanja
                //Console.WriteLine(jednoStanje.Count + " ddd:  " + jednoStanje[0] + jednoStanje [1] + jednoStanje[2] + jednoStanje[3]);
                
                // PARTIJA
                bool igrac1IgraPrvi = true;

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
                        string bacio1 = jednoStanje[brojPartije * 2];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2 + 1];
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
                            catch (Exception e) { 
                            
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
                        string bacio1 = jednoStanje[brojPartije * 2];
                        tempBacio1.Add(bacio1);
                        string bacio2 = jednoStanje[brojPartije * 2 + 1];
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

                
                if (karteURuci[r].broj.Equals(jednoStanje[talonKarte.Count]) && tempBrojStanja < brojStanjaZaSvakuKartu[r])          // bice izuzetak ako komp dobije dve iste karte jednu za drugom!
                {
                    tempBrojStanja++;
                    uspesnost[r] += izracunajUspehStanja(tempOdneo1, tempOdneo2, tempBacio1, tempBacio2, igrac1IgraPrvi);
                }
                else
                {
                    ++r;
                    tempBrojStanja = 0;
                    uspesnost[r] += izracunajUspehStanja(tempOdneo1, tempOdneo2, tempBacio1, tempBacio2, igrac1IgraPrvi);
                }
            }
            // UPOREDI uspesnost i vrati onaj sa najvecom sansom!

            int tempBroj = 0;

            for (int i = 0; i < uspesnost.Length; i++) {
                if (uspesnost.Max().Equals(uspesnost[i])) {
                    tempBroj = i;
                    break;
                }
            }

            try
            {
                Console.WriteLine("KARTA:  " + karteURuci[0].broj + "  USPESNOST:  " + uspesnost[0]);
                Console.WriteLine("KARTA:  " + karteURuci[1].broj + "  USPESNOST:  " + uspesnost[1]);
                Console.WriteLine("KARTA:  " + karteURuci[2].broj + "  USPESNOST:  " + uspesnost[2]);
                Console.WriteLine("KARTA:  " + karteURuci[3].broj + "  USPESNOST:  " + uspesnost[3]);
            }
            catch (Exception e) { }

            Console.WriteLine("TEMP MAX JA PRVI!:  " + uspesnost.Max() + "  TEMP BROJ:  " + tempBroj);

            temp = karteURuci[tempBroj].znak + " " + karteURuci[tempBroj].broj;

            return temp;
        }

       public int izracunajUspehStanja(List<string> tempOdneo1, List<string> tempOdneo2, List<string> tempBacio1, List<string> tempBacio2, bool igraPrviIgrac1)
       {
            int koeficijentOdneo = 0;
            int koeficijentBacio = 0;
            int koeficijentBacioPunu = 0;
            int prviIgrac1 = 0;

            for (int i = 0; i < tempOdneo1.Count; i++) {
                if (tempOdneo1[i] == "A" || tempOdneo1[i] == "10") {
                    koeficijentOdneo -= 1;
                }
            }

            for (int i = 0; i < tempOdneo2.Count; i++) {
                if (tempOdneo2[i] == "A" || tempOdneo2[i] == "10")
                {
                    koeficijentOdneo += 1;
                }
            }

            for (int i = 0; i < tempBacio1.Count; i++)
            {
                if (tempBacio1[i] == "7")
                {
                    koeficijentBacio += 1;
                }
            }

            for (int i = 0; i < tempBacio2.Count; i++)
            {
                if (tempBacio2[i] == "7")
                {
                    koeficijentBacio -= 1;
                }
            }

           for (int i = 0; i < tempBacio2.Count; i++)
            {
                if (tempBacio2[i] == "10" || tempBacio2[i] == "A")
                {
                    koeficijentBacioPunu -= 1;
                }
            }

           
            if (!igrac1IgraPrvi)
                prviIgrac1 = 1;

            return koeficijentBacioPunu + 2 * koeficijentOdneo + 5 * koeficijentBacio + prviIgrac1 * 20;
        }

        public bool verovatnocaNastavljanja(string trenutno, string bacio1, string bacio2, List<string> tempTalon){
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
            else {
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

        public int verovatnocaNastavljanjaInt(string trenutno, string bacio1, string bacio2, List<string> tempTalon)
        {
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

            for (int i = 0; i < tempTalon.Count; i++) {
                if (tempTalon[i].Contains("A") || tempTalon[i].Contains("10")) {
                    uspeh += 5;
                }
            }

            if (bacio1.Contains("7"))
                uspeh += 20;

            return uspeh;
        }


        public int poeniIgrac(List<Karta> odneo1) {
            int temp = 0;

            for (int i = 0; i < odneo1.Count; i++) {
                if (odneo1[i].broj.Equals("10") || odneo1[i].broj.Equals("A"))
                    temp++;
            }
            
            return temp*10;
        }

        public int poeniKomp(List<Karta> odneo2)
        {
            int temp = 0;

            for (int i = 0; i < odneo2.Count; i++)
            {
                if (odneo2[i].broj.Equals("10") || odneo2[i].broj.Equals("A"))
                    temp++;
            }

            return temp * 10;
        }


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
                }else if (tempProsleKarte[i] == "J")
                {
                    broj[5]--;
                }else if (tempProsleKarte[i] == "Q")
                {
                    broj[6]--;
                }else if (tempProsleKarte[i] == "K")
                {
                    broj[7]--;
                }else if (Int32.Parse(tempProsleKarte[i]) == 7)
                {
                    broj[0]--;
                }else if (Int32.Parse(tempProsleKarte[i]) == 8)
                {
                    broj[1]--;
                }else if (Int32.Parse(tempProsleKarte[i]) == 9)
                {
                    broj[2]--;
                }else if (Int32.Parse(tempProsleKarte[i]) == 10)
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

            //Console.WriteLine("TEMP COUNT:  " + temp.Count);
            return temp;
        }

        public bool proveraOdgovaranja(string k1, string k2)
        {

            Console.WriteLine("PROVERA ODG:   " + "~" + k1 + "~  !" + k2 + "!");

            string k1Temp = k1.Split(' ')[1];
            string k2Temp = k2.Split(' ')[1];

            if (k1Temp.Equals(k2Temp))
            {
                return true;
            }
            else if (k2Temp == "7")
            {
                return true;
            }
            else {
                return false;
            }
        }

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
    }
}
