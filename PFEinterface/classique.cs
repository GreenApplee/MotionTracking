using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using SampleGrabberNET;
using System.Threading;
using pfe;
namespace BMAclassique
{
    class classique:fctcommunes
    {
        
        public Bitmap im1;
        public Bitmap im2;      
        public int deplacement_fenetre;
        public int t_bloc;
        public static Bitmap m;
        BitmapData bmp1;
        BitmapData bmp2;
        //UnsafeBitmap a, b;
        int w, h;
        public static double sad_moyen =0;
       //int compteur_sad = 0;
       // StreamWriter sady = new StreamWriter(".\\sady.txt");
       // StreamWriter sadt = new StreamWriter(".\\sadt.txt");
            
      //  private File result;
       // StreamWriter fic_ecr = new StreamWriter(".\\res.txt");
       // Form1 fm1 = new Form1();
      //  byte t;

        public classique(Bitmap _im1, Bitmap _im2, int _fenetre, int _bloc, double _seuil):base(_im1,_im2,_bloc,_fenetre,_seuil)
        {

            im1 = _im1;
            im2 = _im2;
            bmp1 = new BitmapData();
            bmp2 = new BitmapData();
            //bmp1 = im1.LockBits(new Rectangle(0, 0, im1.Width, im1.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //bmp2 = im2.LockBits(new Rectangle(0, 0, im2.Width, im2.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
        /*    unsafe
            {
                ptrsource = (byte*)bmp1.Scan0.ToPointer();
                ptrsearch = (byte *)bmp2.Scan0.ToPointer();
            }*/
            m = im2;    
            deplacement_fenetre = _fenetre;
            t_bloc = _bloc;
            w = image1.Width;
            h = image1.Height;
            



        }

        //public struct bloc
        //{
        //    public int x;
        //    public int y;
        //};

        //public struct opt
        //{
        //    public bloc blc;
        //    public double sad;
        //};
        
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
        /// tester si ke bloc condidat appartient à la fenetre de recherche dub bloc référence
        /// </summary>
        /// <param name="Br">Bloc référence</param>
        /// <param name="Bc">Bloc condidat</param>
        /// <returns>b un booleen</returns>
  /*      
        public bool appartient_fen(bloc Br, bloc Bc)
        {
            bool b;
            if ((Bc.x >= Br.x - deplacement_fenetre) && (Bc.x <= Br.x + deplacement_fenetre))
            {
                if ((Bc.y >= Br.y - deplacement_fenetre) && (Bc.y <= Br.y + deplacement_fenetre))
                {
                   if (Bc.x+taille_bloc < w && Bc.y+taille_bloc < h)
                    {
                        if (Bc.x >= 0 && Bc.y >= 0)
                       {
                            b = true;
                        }
                       else b = false;
                   }
                    else b = false;
                }
                else b = false;
            }
            else b = false;
            return b;
        }
*/
        /// <summary>
        /// constuire une liste de blocs selon le modèle LDSP
        /// </summary>
        /// <param name="Br">bloc référence</param>
        /// <param name="B">le bloc central courant</param>
        /// <returns></returns>
        public bloc[] LDSP(bloc Br,bloc B)
        {
           // List<bloc> lis_ldsp = new List<bloc>();
            int cpt = 0;
            bloc[] tab_ldsp = new bloc[9];
            for (int k = 0; k < 9; k++)
            {
                tab_ldsp[k].x = -1;
                tab_ldsp[k].y = -1;
            }
            bloc tmp;
           // lis_ldsp.Add(B);
            tab_ldsp[cpt] = B;
            cpt++;
            tmp.x = B.x + 2;
            tmp.y = B.y;
            if (appartient_fen(Br, tmp) == true)
              //  MessageBox.Show(tmp.x.ToString()+","+tmp.y.ToString());
            { /*lis_ldsp.Add(tmp); */tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x - 2;
            tmp.y = B.y;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++;}
            tmp.x = B.x;
            tmp.y = B.y + 2;
            if (appartient_fen(Br, tmp) == true)
            { /* lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x;
            tmp.y = B.y - 2;
            if (appartient_fen(Br, tmp) == true)
            { /* lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x + 1;
            tmp.y = B.y + 1;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x - 1;
            //if (tmp.x < 0) MessageBox.Show("le bleme est presque ici");
            tmp.y = B.y - 1;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x + 1;
            tmp.y = B.y - 1;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
            tmp.x = B.x - 1;
          //  if (tmp.x < 0) MessageBox.Show("le bleme est presque ici");
            tmp.y = B.y + 1;
            if (appartient_fen(Br, tmp) == true)
            { /* lis_ldsp.Add(tmp);*/ tab_ldsp[cpt] = tmp; cpt++; }
           // return lis_ldsp;
        //    if (cpt == 1) MessageBox.Show("cpt =1");
            return tab_ldsp;
        }

        /// <summary>
        /// constuire une liste de blocs selon le modèle LDSP
        /// </summary>
        /// <param name="Br">le bloc reference</param>
        /// <param name="B">le bloc central courant</param>
        /// <returns></returns>
        public bloc[] SDSP(bloc Br, bloc B)
        {
            int cpt = 0;
            bloc[] tab_sdsp = new bloc[5];
            for (int k = 0; k < 5; k++)
            {
                tab_sdsp[k].x = -1;
                tab_sdsp[k].y = -1;
            }

          //  List<bloc> lis_sdsp = new List<bloc>();
            bloc tmp;
            tab_sdsp[cpt] = B;
            cpt++;
           // lis_sdsp.Add(B);
            tmp.x = B.x + 1;
            tmp.y = B.y;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_sdsp.Add(tmp);*/ tab_sdsp[cpt] = tmp; cpt++; }
            tmp.x = B.x - 1;
           // if (tmp.x < 0) MessageBox.Show("le bleme est presque ici");
            tmp.y = B.y;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_sdsp.Add(tmp);*/ tab_sdsp[cpt] = tmp; cpt++; }
            tmp.x = B.x;
            tmp.y = B.y + 1;
            if (appartient_fen(Br, tmp) == true)
            { /*lis_sdsp.Add(tmp);*/ tab_sdsp[cpt] = tmp; cpt++; }
            tmp.x = B.x;
            tmp.y = B.y - 1;
          //  if (tmp.x < 0) MessageBox.Show("le bleme est presque ici");
            if (appartient_fen(Br, tmp) == true)
            { /* lis_sdsp.Add(tmp); */tab_sdsp[cpt] = tmp; cpt++; }
           // return lis_sdsp;
            return tab_sdsp;
 
        }

        public unsafe bloc diamond_search(bloc Br)
        {
            bloc Bcm =  Br;
            bloc B;
          //  bool b=false;
           // List<bloc> liste_ldsp;
           // List<bloc> liste_sdsp;
            bloc[] table_ldsp = new bloc[9];
            bloc[] table_sdsp = new bloc[5];
            bloc[] obj = new bloc[2];
            obj[0] = Br;
            double treshold=10;
            
            List<bloc> deja_visite = new List<bloc>();
            double[] sad_visite = new double[225];
            List<bloc> liste_alea=new List<bloc>();
            Random rndm = new Random(DateTime.Now.Millisecond);
 
            
            int ind;
            B.x = -1;
            B.y = -1;
            double comp=0;
            int cpt = 0;
            double min = int.MaxValue;
            double min1 = int.MaxValue;
           // liste_Bcm.Add(Bcm);
            while (((Bcm.x != B.x) ||(Bcm.y!=B.y)) && cpt<10)
            {
                int i = 0;
                cpt++;
                B = Bcm;
                liste_alea.Clear();
                table_ldsp = LDSP(Br, B);
                if (table_ldsp[0].x>0 )
                {
                    while(i<9 && table_ldsp[i].x>0  )
                    {                                            
                        comp = SAD(table_ldsp[i], Br);
                        if (comp <= treshold) 
                        { 
                            return table_ldsp[i]; 
                        }
                    
                        if (comp < min) 
                        { 
                            Bcm = table_ldsp[i];
                            min = comp;
                        }
                        else
                        {

                            if ((comp == min  && i != 0) ) 
                            { 
                                liste_alea.Add(table_ldsp[i]); min1 = min;
                            }
                            
                        }
                        i++;
                        
                    }
                    if (liste_alea.Count > 0 && min1<=min )
                    {
                            
                            ind = rndm.Next(liste_alea.Count);
                          //  fic_ecr.WriteLine("\nl'indice aléatoirement choisi est:" + ind.ToString());
                                Bcm = liste_alea[ind];
                               // liste_Bcm.Add(Bcm);
                           // fic_ecr.WriteLine("\nle bloc alétoirement choisi qui sera le prochaine centre est" + Bcm.x.ToString() + "," + Bcm.y.ToString());
                             
                        }
                          
                  
                  
                }
                
            }
           // MessageBox.Show(cpt.ToString());
           // comp = SAD(B, Br);
           // MessageBox.Show(comp.ToString());
            //min = 69854631325;
            table_sdsp = SDSP(Br, B);
            int k = 0;
            //MessageBox.Show("bloc minimisant est:" + B.x.ToString() + "," + B.y.ToString()+"mtn calcul des blocs sdsp");
            if (table_sdsp[0].x >0)
            {
                while(k<5 && table_sdsp[k].x>0  )
                {
                    comp=SAD(table_sdsp[k],Br);
                    //MessageBox.Show("blocs" + liste_sdsp[i].x.ToString() + "," + liste_sdsp[i].y.ToString() + "et" + Br.x.ToString() + "," + Br.y.ToString() + "sad=" + comp.ToString());
                    if (comp < min) { Bcm = table_sdsp[k]; min = comp;/* fic_ecr.WriteLine("\ncomp<min dans sdsp, Bcm=" + Bcm.x.ToString() + "," + Bcm.y.ToString());*/ }
                  //  MessageBox.Show("le min ldsp est:" + Bcm.x.ToString() + "," + Bcm.y.ToString());
                    k++;
                }
            }
            
            //MessageBox.Show("!!!");
           // fic_ecr.WriteLine("le bloc résultat final est:" + Bcm.x.ToString() + "," + Bcm.y.ToString());
           
          //  MessageBox.Show("cpt=" + cpt.ToString());
           // bloc v;
           // v.x=-1; v.y=-1;
           // if (min < treshold) return Bcm;
           // else return v;
            return Bcm;
        }




        public List<bloc> analyse(List<bloc> liste_bloc1,List<bloc>liste_bloc2)
        {
           
 

 

            a= new UnsafeBitmap(image1);
            b= new UnsafeBitmap(image2);
            bloc rst= new bloc();
            List<bloc> resultat= new List<bloc>();
         //   List<bloc> liste_bloc1= new List<bloc>();
         //   List<bloc> liste_bloc2= new List<bloc>();
         //   liste_bloc1= decoupage(image1);
          //  liste_bloc2= decoupage(image2);
            DateTime start = DateTime.Now;
          //  MessageBox.Show(liste_bloc1.Count.ToString());
            a.LockBitmap();
            b.LockBitmap();
            object o = new object();
            zmp(ref liste_bloc1,ref liste_bloc2);

         //   MessageBox.Show(liste_bloc1.Count.ToString());
            AForge.Parallel.For(0, liste_bloc1.Count, delegate(int i)
            //   for(int i=0;i<liste_bloc1.Count;i++)
            {
                rst = diamond_search(liste_bloc1[i]);

                if (lst_contain(liste_bloc1, rst) == true)
                    lock (o)
                    {
                        if ((rst.x != liste_bloc1[i].x) || (rst.y != liste_bloc1[i].y)) //MessageBox.Show("kifkif");
                            resultat.Add(rst);
            //            sad_moyen += SAD(liste_bloc1[i], rst);
              //          compteur_sad++;
                        // MessageBox.Show(SAD(liste_bloc1[i],rst).ToString());
                        //sad_moyen.Add(SAD(liste_bloc1[i], rst));
                    }
            });


          //  sad_moyen = sad_moyen / compteur_sad;
            //);
            a.UnlockBitmap();
            b.UnlockBitmap();
            //fic_ecr.Close();
            //sady.Close();
            //sadt.Close();
           // MessageBox.Show(sad_moyen.Average().ToString());
            TimeSpan dur = DateTime.Now - start;
          //  MessageBox.Show(dur.ToString());
           return resultat;
        }
        
    }
}
