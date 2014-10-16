using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pfe;
using SampleGrabberNET;
using System.Drawing;
using System.Windows.Forms;
using AForge;

namespace abeilles
{
    class class_abeilles:fctcommunes
    {
        public struct abeille
        {
            public int direction;
            public bloc distance;
            public double qualite;
        };
        private int fenetre;
        private int n;
        private int m;
        private int e;
        private int nbe;
        private int nba;
        private int nbgen;
        private int h, w;
       // public static List<bloc> verif;
       // public static Bitmap zm;
        object proteger_abeille;
      //  object proteger_inc;
        //public static double sad_moyen = 0;
        //public static Binding s;
        //public static List<double> li_sad = new List<double>();

        public class_abeilles(int _m, int _e, int _nbe, int _nba, int _nbgen, int _n, Bitmap im1, Bitmap im2, int _fenetre, int _taille_bloc, double _seuil)
            : base(im1, im2, _taille_bloc, _fenetre, _seuil)
        {
            image1 = im1;
            image2 = im2;
            h = image1.Height;
            w = image1.Width;
            m = _m;
            n = _n;
            e = _e;
            nbe = _nbe;
            nba = _nba;
            nbgen = _nbgen;
            fenetre = _fenetre*2;
            taille_bloc = _taille_bloc;
            a = new UnsafeBitmap(im1);
            b = new UnsafeBitmap(im2);
           // seuil = 255;
            //verif = new List<bloc>();
           // zm = im1;
            proteger_abeille = new object();
          //  proteger_inc = new object();
           // s = new Binding("Text", sad_moyen, "abeilles_vacances.class_abeilles.sad_moyen");
        }

        private List<abeille> disperser_abeilles(bloc br, int nbr, int fen)
        {
            List<abeille> tmp = new List<abeille>();
            Random x = new Random(DateTime.Now.Millisecond);
            abeille ab = new abeille();
            int stop=0;
            lock(proteger_abeille)
            for (int i = 0; i < nbr; i++)
            {
                ab.distance.x =Math.Max(0,Math.Min(w, x.Next(br.x - fen , br.x + fen )));
                ab.distance.y =Math.Max(0,Math.Min(h, x.Next(br.y - fen , br.y + fen )));
                while (tmp.Contains(ab))
                {
                    ab.distance.x =Math.Max(0,Math.Min(w, x.Next(br.x - fen , br.x + fen )));
                    ab.distance.y = Math.Max(0, Math.Min(h, x.Next(br.y - fen , br.y + fen )));
                    //MessageBox.Show(ab.distance.x.ToString() + "," + br.x.ToString());
                
                        stop++;
                        if (stop >= 20)
                        {
                            break;
                        }
                    }
                stop = 0;
                
                tmp.Add(ab);
            }
            return tmp;
        }

        private List<abeille> recuperer_abeilles(int d, int nbr, List<abeille> l)
        {
            List<abeille> tmp = new List<abeille>();
            for (int i = d; i < nbr + d; i++)
            {
                tmp.Add(l[i]);
            }
                return tmp;

        }

        private void tamiser(ref List<abeille> arbre, int noeud, int n)
        {
            //List<int> tmp = new List<int>();
            int k = noeud;
            int j = 2 * k;
            while (j <= n)
            {
                if ((j < n) && (arbre[j].qualite < arbre[j + 1].qualite))
                {
                    j++;
                }
                if (arbre[k].qualite < arbre[j].qualite)
                {
                    abeille permutt = arbre[k];
                    arbre[k] = arbre[j];
                    arbre[j] = permutt;
                    k = j;
                    j = 2 * k;
                }
                else
                {
                    break;
                }
            }
        }


        private void trier_selon_qualite(ref List<abeille> li, int taille)
        {
            if (taille > 1)
            {
                for (int i = taille / 2; i >= 0; i--)
                {
                    tamiser(ref li, i, taille);
                }

                for (int i = taille; i >= 1; i--)
                {
                    abeille permutt = li[i];
                    li[i] = li[0];
                    li[0] = permutt;
                    tamiser(ref li, 0, i - 1);
                }
            }
        }

        private void mise_a_jour(ref List<abeille> l, bloc Br)
        {
            abeille ab = new abeille();
            for (int i = 0; i < l.Count; i++)
            {
                
                ab.distance = l[i].distance;
                ab.direction = l[i].direction;
                ab.qualite = SAD(l[i].distance, Br);
                l[i] = ab;
            }

        }
/*
       public int selection_meilleur_abeille(List<abeille> l)
        {
            int pos=0;
            for (int i = 0; i < l.Count-1; i++)
            {
                if (l[i].qualite != l[i + 1].qualite)
                {
                    pos = i;
                    break;
                }
            }
         
            return pos;
        }
        */
        private abeille operation_abeille(bloc br)
        {
            abeille ab = new abeille();
            //int cpt = 0;
            int d=0;
            int voisinage = fenetre / 2;
            List<abeille> list_n = new List<abeille>();
            List<abeille> list_m = new List<abeille>();
            List<abeille> list_e1 = new List<abeille>();
            List<abeille> list_e2 = new List<abeille>();
            List<abeille> bcm = new List<abeille>();
            list_n = disperser_abeilles(br, n, fenetre);
            //if (list_n.Count == 0) MessageBox.Show("vide");
            //else MessageBox.Show("non vide");
            mise_a_jour(ref list_n, br);
            trier_selon_qualite(ref list_n, list_n.Count - 1);
            list_m = recuperer_abeilles(0, m, list_n);

         //   do
            for(int cpt =0;cpt < nbgen;cpt++)
          //  System.Threading.Tasks.Parallel.For(0, nbgen, delegate(int cpt)
            {
                //bcm.Clear();
                list_e1.Clear();
                list_e2.Clear();
                if (list_m.Count == 0)
                {
                  //  System.Windows.Forms.MessageBox.Show("list_m = 0");
                    break;
                }
                else
                {
                    object o1 = new object();
                    object o2 = new object();
                    System.Threading.Tasks.Parallel.For(0, e, delegate(int i)
                  //  for (int i = 0; i < e; i++)
                    {
                        //if (list_m.Count == 0) MessageBox.Show("azeakzekazejazeazeazezae");
                        lock (o1)
                            list_e1 = disperser_abeilles(list_m[i].distance, nbe, voisinage);
                        mise_a_jour(ref list_e1, br);
                         trier_selon_qualite(ref list_e1, list_e1.Count - 1);
                        //     d = selection_meilleur_abeille(list_e1);
                        // if(d!=0)  MessageBox.Show(d.ToString());
                        lock (o2)
                            bcm.Add(list_e1[d]);
                    }
                     );

                    System.Threading.Tasks.Parallel.For(e, m, delegate(int i)
                    // for (int i = e; i < m; i++)
                    {
                        lock (o2)
                            list_e2 = disperser_abeilles(list_m[i].distance, nba, voisinage);
                        mise_a_jour(ref list_e2, br);
                        trier_selon_qualite(ref list_e2, list_e2.Count - 1);
                        //   d = selection_meilleur_abeille(list_e2);
                        //       if (d != 0) MessageBox.Show(d.ToString());
                        lock (o1)
                            bcm.Add(list_e2[d]);
                    }
                    );
      //              
                    list_m = bcm.ToList();
                    // mise_a_jour(ref list_m, br);
                    //      cpt++;
                }

            } //while (cpt <= nbgen);
           //);
           // mise_a_jour(ref bcm,br);
            trier_selon_qualite(ref bcm, bcm.Count- 1);
            //d = selection_meilleur_abeille(bcm);
       //     if (d != 0) MessageBox.Show(d.ToString());
            ab = bcm[0];



            return ab;
        }

        public bool lst_contain(List<bloc> list, bloc b)
        {
            
            int v = 5;
            bool bo = false;
            int i = 0;
            while (i < list.Count && bo == false)
            {
                if (list[i].x == b.x && list[i].y == b.y) bo = true;
                else
                {
                    if (b.x < list[i].x + v && b.x > list[i].x - v)
                    {
                        if (b.y < list[i].y + v && b.y > list[i].y - v) bo = true;
                        else bo = false;
                    }
                    else bo = false;
                }
                i++;
            }
            return bo;
        }

        public List<bloc> analyse(List<bloc> elts_candidats1, List<bloc> elts_candidats2)
        {
            List<bloc> temp = new List<bloc>();
            bloc rst = new bloc();   
            abeille bctmp = new abeille();
            a.LockBitmap();
            b.LockBitmap();
           
            zmp(ref elts_candidats1, ref elts_candidats2);
           // MessageBox.Show("après zmp: " + elts_candidats1.Count.ToString());
           // verif = elts_candidats1;
          //  for (int i = 0; i < elts_candidats1.Count ; i++)
            //{
            object o = new object();
           AForge.Parallel.For(0, elts_candidats1.Count, delegate(int i)
         //   for(int i=0; i< elts_candidats1.Count;i++)
            {
                //zm.SetPixel(elts_candidats1[i].x, elts_candidats1[i].y, Color.Red);
                bctmp = operation_abeille(elts_candidats1[i]);
                rst = bctmp.distance;
                
                if (lst_contain(elts_candidats1, bctmp.distance))
                {
                    lock (o)
                    {
                        if (!((rst.x == elts_candidats1[i].x) && (rst.y == elts_candidats1[i].y)))
                        {
                            temp.Add(bctmp.distance);
                            //sad_moyen += bctmp.qualite;
                            //compteur_sad++;
                            //li_sad.Add(bctmp.qualite);
                                
                        }
                        
                        
                    }
                    

                   // MessageBox.Show(bctmp.qualite.ToString());
                }
            }
          );
       //     sad_moyen = sad_moyen / compteur_sad;
            //s.ReadValue(); 
           
         //   MessageBox.Show(compteur_sad.ToString());
            //}
            a.UnlockBitmap();
            b.UnlockBitmap();
            
            return temp;
        }


    }
}
