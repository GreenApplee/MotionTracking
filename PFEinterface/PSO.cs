using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pfe;
using SampleGrabberNET;
using System.Drawing;
using System.Windows.Forms;
using AForge;
using System.Threading.Tasks;
namespace PSO_final
{
    class PSO:fctcommunes
    {
        Bitmap im1;
        Bitmap im2;
        int fenetre, nbgen, nbpar,h,w;
      // UnsafeBitmap a, b;
        struct particule
        {
            public bloc position;
            public double qualite;
        };
        particule gbest;
        public static double sad_moyen = 0;
      //  List<particule> pbest;
       
        private double c1,c_max;
        public PSO(Bitmap _im1, Bitmap _im2, int _tBloc, int _fen, int _nbrgen, int _nbpar, double _seuil):base(_im1,_im2,_tBloc,_fen,_seuil)
        {
            im1 = _im1;
            im2 = _im2;
            taille_bloc = _tBloc;
            fenetre = _fen*2;
            nbgen = _nbrgen;
            nbpar = _nbpar;
            a = new UnsafeBitmap(im1);
            b = new UnsafeBitmap(im2);
            
            h = im1.Height-1;
            w = im1.Width-1;
            c1 = calcul_c1();
            c_max = calcule_cmax(c1);
        }


         private double calcul_c1()
        {
            Random x = new Random();
            return x.NextDouble();
        }
        private double calcule_cmax(double c1)
        {
            return (2 / 0.97725) * c1;
        }
        public bool lst_contain(List<bloc> list, bloc b)
        {

            int v = 6;
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


        /* dans cette fonction on initialise une liste de particule par rapport à un bloc donné
         * et on calcule le critere SAD entre ce bloc et le bloc généré aleatoirement*/
        private List<particule> initialiser(bloc BR)
        {
            Random x = new Random();
            List<particule> liste = new List<particule>();
            particule tmp = new particule();
            int cpt =0;
            int arret = 0;
            while(cpt<nbpar)
            {
                tmp.position.x = Math.Max(0, Math.Min(x.Next(BR.x - fenetre / 2, BR.x + fenetre / 2), w));
                tmp.position.y = Math.Max(0, Math.Min(x.Next(BR.y - fenetre / 2, BR.y + fenetre / 2), h));
                
                while (liste.Contains(tmp))
                {
                    tmp.position.x = Math.Max(0, Math.Min(x.Next(BR.x - fenetre / 2, BR.x + fenetre / 2), w));
                    tmp.position.y = Math.Max(0, Math.Min(x.Next(BR.y - fenetre / 2, BR.y + fenetre / 2), h));
                    arret++;
                    if (arret > 200) break;
                }
                tmp.qualite = SAD(tmp.position, BR);
               // MessageBox.Show(tmp.position.x.ToString() + "," + tmp.position.y.ToString());

                liste.Add(tmp);
                cpt++;
            }
            //MessageBox.Show(cpt.ToString());
            return liste;
        }
        private void initialiser_gbset_pbest(ref List<particule> pbest)
        {
            particule tmp = new particule();
            tmp.qualite = int.MaxValue;
            
            for (int i = 0; i < nbpar; i++)
            {
                pbest.Add(tmp);
            }
           gbest = tmp;
        }



        private void mise_a_jour_pbset_gbset(List<particule> liste,ref List<particule> pbest)
        {
           
            for (int i = 0; i <liste.Count; i++)
            {
              
                if (pbest[i].qualite > liste[i].qualite)
                    pbest[i] = liste[i];
                  
           
                if (gbest.qualite > liste[i].qualite)
                        gbest = liste[i];
                    
            }

        }

        /*cette fonction calcule les nouvelles positions a partir du gbest et le pbest
         * et mis à jour les SAD de ces nlles positions*/
        private void calcule_nouvelle_position(ref List<particule> l, bloc br, List<particule> pbest)
        {
            //Random x = new Random();
            particule tmp = new particule();
            bloc bl = new bloc();
            int deplacement,nouvelle_pos;
            double s;
            for (int i = 0; i < l.Count; i++)
          //  AForge.Parallel.For(0, l.Count, delegate(int i)
            {
                 bl = l[i].position;
                // deplacement =(int)(2 * x.NextDouble() * (pbest[i].position.x - l[i].position.x) + 2 * x.NextDouble() * (pbest[i].position.x * gbest.position.x - l[i].position.x));
                     deplacement = (int)(c1 * bl.x + c_max * (pbest[i].position.x - bl.x - 1) + c_max * (gbest.position.x - bl.x - 1));

                //bl.x= bl.x + deplacement;
                nouvelle_pos = Math.Min(l[i].position.x + fenetre / 2, bl.x + deplacement);
                nouvelle_pos = Math.Max(nouvelle_pos, l[i].position.x - fenetre / 2);
                nouvelle_pos = Math.Max(0, Math.Min(nouvelle_pos, w));
                bl.x = nouvelle_pos;
                //deplacement = (int)(2 * x.NextDouble() * (pbest[i].position.y - l[i].position.y) + 2 * x.NextDouble() * (pbest[i].position.y * gbest.position.y - l[i].position.y));
                
                        deplacement = (int)(c1 * bl.y + c_max * (pbest[i].position.y - bl.y - 1) + c_max * (gbest.position.y - bl.y - 1));
                
                //bl.y = bl.y + deplacement;
                //bl.y=Math.Max(0, Math.Min(bl.y + deplacement, h));
                nouvelle_pos = Math.Min(l[i].position.y + fenetre / 2, bl.y + deplacement);
                nouvelle_pos = Math.Max(nouvelle_pos, l[i].position.y - fenetre / 2);
                nouvelle_pos = Math.Max(0, Math.Min(nouvelle_pos, h));
                bl.y = nouvelle_pos;
                //   MessageBox.Show(bl.x.ToString()+","+bl.y.ToString());
                s = SAD(bl, br);
                tmp.position = bl;
                tmp.qualite = s;
                l[i] = tmp;
                //  MessageBox.Show(bl.x.ToString()+","+bl.y.ToString());
            }
            //);
        }

        private bloc opt_pso(bloc b, List<particule> pbest)
        {
            List<particule> li_par = new List<particule>();
            li_par = initialiser(b);
            System.Threading.Tasks.Parallel.For(0, nbgen, delegate(int i)
            //for (int i = 0; i < nbgen; i++)
            {
                mise_a_jour_pbset_gbset(li_par, ref pbest);

                calcule_nouvelle_position(ref li_par, b, pbest);
            }
            );
            mise_a_jour_pbset_gbset(li_par, ref pbest);
            return gbest.position;
        }


        public List<bloc> analyser(List<bloc> li,List<bloc> li2)
        {
           // int compteur_sad = 0;
            List<bloc> res = new List<bloc>();
            bloc tmp = new bloc();
            object ob = new object();
            a.LockBitmap();
            b.LockBitmap();
            zmp(ref li,ref li2);
            //MessageBox.Show(li.Count.ToString());
           // for (int i = 0; i < li.Count; i++)
            AForge.Parallel.For(0, li.Count, delegate(int i)
            //for(int i=0;i<li.Count;i++)
            {
               // pbest.Clear();
                List<particule> pbest = new List<particule>();
               initialiser_gbset_pbest(ref pbest);
                // MessageBox.Show(pbest.Count.ToString());
                tmp = opt_pso(li[i],pbest);
                // MessageBox.Show(tmp.x.ToString() + "," + tmp.y.ToString());
                if (!lst_contain(li, tmp))
                    lock (ob)
                    {
                        res.Add(tmp);
                        //sad_moyen += SAD(li[i],tmp);
                        //compteur_sad++;
                    }
            }
            );


          //  sad_moyen = sad_moyen / compteur_sad;
            b.UnlockBitmap();
            a.UnlockBitmap();
            
            return res;
        }


    }
}
