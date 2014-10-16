using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SampleGrabberNET;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AForge;
using pfe;

namespace gen_21fev
{
    class genetique:fctcommunes
    {
        public static List<bloc> liste_bloc1;
        public static List<bloc> liste_bloc2;
       // int deplace_fen;
      //  public int taille_bloc;
        Bitmap ima1;
        Bitmap ima2;
        //public static Bitmap zm;
        //UnsafeBitmap a;
        //UnsafeBitmap b;
        int nbchro;
        int nbgen;
        public bloc[] resultfinal;
        public int indfinal = 0;
        object ob = new object();
        int h, w;
        static public double sad_moyen = 0;
        static public int compteur_sad = 0;
       // StreamWriter fich;

        public genetique(Bitmap _im1, Bitmap _im2, int t_bloc, int d_fenetre, int _nbchro, int _nbgen, double _seuil)
            : base(_im1, _im2, t_bloc, d_fenetre, _seuil)
        {
           // //MessageBox.Show(" cv");
           // fich = new StreamWriter(".\\fich.txt");
            image1 = _im1;
            image2 = _im2;
            ima1 = _im1;
            ima2 = _im2;
            h = image1.Height;
            w = image1.Width;
            a = new UnsafeBitmap(image1);
            b = new UnsafeBitmap(image2);
            //a.LockBitmap();
            //b.LockBitmap();
            taille_bloc = t_bloc;
            deplace_fen = d_fenetre;
            nbchro = _nbchro;
            nbgen = _nbgen;
          //  zm = _im1;
             liste_bloc1 = new List<bloc>();
             liste_bloc2 = new List<bloc>();
        }

          public bool lst_contain(List<bloc> list, bloc b)
          {
              int v = 2;
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

    
        /// <summary>
        /// supprimer les blocs qui ne contiennent pas de mouvement
        /// </summary>
        /// <param name="li1">liste des blocs de l'image 1</param>
        /// <param name="li2">liste des blocs de l'image 2</param>
     /*   public void zmp(ref List<bloc> li1, ref List<bloc> li2)
        {
            double zm;
            List<bloc> intermed1 = new List<bloc>();
           // List<bloc> intermed2 = new List<bloc>();
            int k = 0;
            while (k < li1.Count())
            {

                zm = SAD(li2[k], li1[k]);


                if (zm > 2500) //là faut faire le seuil
                {
                    intermed1.Add(li1[k]);
                   // intermed2.Add(li2[k]);
                }

                k++;

            }
            li1 = intermed1;
           // li2 = intermed2;
        }

        */
     /*   public double SAD(bloc bc, bloc br)
        {
            double sad = 0;
            PixelData t1= new PixelData();
            PixelData t2= new PixelData();

            // PixelData t3 = new PixelData();
            //accés au pixel
            double val1, val2;
            //System.Threading.Tasks.Parallel.For(0, taille_bloc, delegate(int i)
            for (int i = 0; i < taille_bloc; i++)
            {
                for (int j = 0; j < taille_bloc; j++)
                {
                    // //MessageBox.Show("fait");
                    t1 = a.GetPixel((br.x + i), (br.y + j));
                    t2 = b.GetPixel((bc.x + j), (bc.y + i));
                    val1 = t1.blue * 0.1140 + t1.green * 0.5870 + t1.red * 0.2989;
                    val2 = t2.blue * 0.1140 + t2.green * 0.5870 + t2.red * 0.2989;

                    if (val2 - val1 < 0)
                    {
                        sad -= (val2 - val1);
                    }
                    else
                    {
                        sad += (val2 - val1);
                    }
                }

            }
            return sad;
        }
        */
        //public bool appartient_fen(bloc Br, bloc Bc)
        //{
        //    bool b;
        //    if ((Bc.x >= Br.x - deplace_fen) && (Bc.x <= Br.x + deplace_fen))
        //    {
        //        if ((Bc.y >= Br.y - deplace_fen) && (Bc.y <= Br.y + deplace_fen))
        //        {
                        
                        
        //                if (Bc.x + taille_bloc < w && Bc.y + taille_bloc < h)
                        
        //                {
        //                    if (Bc.x >= 0 && Bc.y >= 0)
        //                    {
        //                        b = true;
        //                    }
        //                    else b = false;
        //                }
        //                else b = false;
                    
                    
        //        }
        //        else b = false;
        //    }
        //    else b = false;
        //    return b;
        //}

        public int trouver_ind(double[] tab,double var)
        {
            int i = 0;
            while (tab[i] != var) i++;
            return i;
        }
        public List<bloc> initialisation(bloc Br)
        {
            List<bloc> li_chro = new List<bloc>();
            bloc chro = new bloc();
            Random rn = new Random(DateTime.Now.Millisecond);
            while (li_chro.Count < nbchro)
            {    
                //generer aléatoirement des blocs   
                chro.x = rn.Next(Br.x-deplace_fen,Br.x+deplace_fen);
                chro.y = rn.Next(Br.y-deplace_fen,Br.y+deplace_fen);
                if (appartient_fen(Br, chro)) { li_chro.Add(chro);  }
                
            }
           
            
            ////MessageBox.Show(li_chro.Count.ToString());
            return li_chro;
        }

        public bloc mutation(bloc Br)
        {
            Random rd = new Random();
            bloc alea = new bloc();
            alea.x = -1;
            alea.y = -1;
            while (!appartient_fen(Br, alea))
            {
                alea.x = rd.Next(Br.x - deplace_fen, Br.x + deplace_fen);
                alea.y = rd.Next(Br.y - deplace_fen, Br.y + deplace_fen);
            }
            return alea;
        }

        public bloc mutation(bloc Br,bloc B)
        {
            bloc chro_mute;
            chro_mute.x = -1;
            chro_mute.y = -1;
            string tmp1, tmp2;
            tmp1 = Convert.ToString(B.x, 2);
            tmp2 = Convert.ToString(B.y, 2);
            string sauv1 = tmp1;
            string sauv2 = tmp2;
            //intervertir un bit de tmp1 et un bit de tmp2
            StringBuilder str1 = new StringBuilder(tmp1);
            StringBuilder str2 = new StringBuilder(tmp2);
            char c;
            Random rd = new Random(DateTime.Now.Millisecond);
            while (!appartient_fen(Br, chro_mute))
            {
                str1 = new StringBuilder(sauv1);
                str2 = new StringBuilder(sauv2);
                int position = rd.Next(0, str1.Length - 1);
                if (str1[position] == '0') c = '1';
                else c = '0';
                str1[position] = c;
                position = rd.Next(0, str2.Length - 1);
                if (str2[position] == '0') c = '1';
                else c = '0';
                str2[position] = c;
                tmp1 = str1.ToString();
                tmp2 = str2.ToString();
                //reconversion en entier
                chro_mute.x = Convert.ToInt32(tmp1, 2);
                chro_mute.y = Convert.ToInt32(tmp2, 2);
            }
            return chro_mute;

        }

        public double[] next_gen(bloc Br, ref List<bloc> li_chro)
        {
            List<bloc> temp = new List<bloc>();
            temp= li_chro.ToList();
            double[] tab_sad = new double[li_chro.Count];
            double sadm = 0;
            for(int i=0;i<li_chro.Count;i++)
           // System.Threading.Tasks.Parallel.For(0, temp.Count, delegate(int i)
              {

                  tab_sad[i] = SAD(li_chro[i], Br);

              }
            sadm = tab_sad.Average();
            //appliquer le parrallelisme ici
          //for(int i=0;i<li_chro.Count;i++)
            System.Threading.Tasks.Parallel.For(0, temp.Count, delegate(int i)
              {
                  if (tab_sad[i] >= sadm)
                  {
                      ////MessageBox.Show(i.ToString());
                      temp[i] = mutation(Br);

                  }
              });
           li_chro = temp;
            return tab_sad;
            
        }

        public bloc algo_gen(bloc Br)
        {
            //bloc Br = new bloc();
           // bloc Br = (bloc)Bro;
            double[] tab_sad = new double[nbchro];
            List<bloc> li_chro = new List<bloc>();
            bloc result = new bloc();
            int ind = 0;
            //double sad = 0;
           
                li_chro = initialisation(Br);
                
                     for (int j = 0; j < nbgen; j++)
                    {
                        tab_sad = next_gen(Br, ref li_chro);
                    }

                ind = trouver_ind(tab_sad, tab_sad.Min());
                result = li_chro[ind];
               
                    //resultfinal[indfinal] = result;
                    //indfinal++;
                
            return result;
            
        }

        public List<bloc> analyse(List<bloc> liste_bloc1, List<bloc> liste_bloc2)
        {

            bloc rst = new bloc();
            List<bloc> resultat = new List<bloc>();
            DateTime start = DateTime.Now;
            //  //MessageBox.Show(liste_bloc1.Count.ToString());
            a.LockBitmap();
            b.LockBitmap();
            
            zmp(ref liste_bloc1, ref liste_bloc2);
            resultfinal = new bloc[liste_bloc1.Count];
            object ob = new object();
                AForge.Parallel.For(0, liste_bloc1.Count, delegate(int i)
            //for (int i = 0;i< liste_bloc1.Count; i++) 
             {
                 //tabth[i].Start(liste_bloc1[i]);
                 rst = algo_gen(liste_bloc1[i]);
                 if (lst_contain(liste_bloc1, rst) == true)
                     lock (ob)
                     {
                         if ((rst.x != liste_bloc1[i].x) || (rst.y != liste_bloc1[i].y))
                             resultat.Add(rst);
                     }
                 
             }
            );
            a.UnlockBitmap();
            b.UnlockBitmap();           
            return resultat;
        }
        
    }
}
