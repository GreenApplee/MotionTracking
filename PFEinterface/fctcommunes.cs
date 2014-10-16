using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SampleGrabberNET;

namespace pfe
{
    class fctcommunes
    {
        //variables de classe
       protected Bitmap image1;
       protected Bitmap image2;
       protected int deplace_fen;
       protected static int taille_bloc;
       protected UnsafeBitmap a, b;
      protected static int he, wi;
      protected double seuil;
     
        public struct bloc
        {
            public int x;
            public int y;
        };


        //constructeur
        public fctcommunes(Bitmap _im1, Bitmap _im2, int t_bloc, int d_fenetre, double _seuil)
        {
            image1 = _im1;
            image2 = _im2;
            a = new UnsafeBitmap(_im1);
            b = new UnsafeBitmap(_im2);
            taille_bloc = t_bloc;
            deplace_fen = d_fenetre;
            he = image1.Height;
            wi = image1.Width;
            seuil = _seuil;
        }

        /// <summary>
        /// découper l'image en blocs de taille fixe
        /// </summary>
        /// <param name="im">l'image</param>
        /// <returns>une liste de coordonnées de blocs</returns>
        public static List<bloc> decoupage(Bitmap im)
        {
            List<bloc> liste = new List<bloc>();
            bloc tmp = new bloc();
            for (int i = 0; i < wi; i++)
            {
                for (int j = 0; j < he; j++)
                {
                    tmp = new bloc();
                    tmp.x = i;
                    tmp.y = j;
                    if (i + taille_bloc < wi && j + taille_bloc < he)
                    {

                        liste.Add(tmp);
                    }
                    j = j + taille_bloc - 1;
                }
                i = i + taille_bloc - 1;
            }
            return liste;
        }

        /// <summary>
        /// supprimer les blocs qui ne contiennent pas de mouvement
        /// </summary>
        /// <param name="li1">liste des blocs de l'image 1</param>
        /// <param name="li2">liste des blocs de l'image 2</param>
        public void zmp(ref List<bloc> li1, ref List<bloc> li2)
        {
            double zm;
            List<bloc> intermed1 = new List<bloc>();
            List<bloc> intermed2 = new List<bloc>();
            int k = 0;
            while (k < li1.Count())
            {

                zm = SAD(li2[k], li1[k]);


                if (zm > seuil)
                {
                    intermed1.Add(li1[k]);
                    intermed2.Add(li2[k]);
                }

                k++;

            }
            li1 = intermed1;
            li2 = intermed2;
        }

        /// <summary>
        /// critère de comparaison entre deux blocs
        /// </summary>
        /// <param name="bc">bloc candidat</param>
        /// <param name="br">bloc référence</param>
        /// <returns>l'erreur entre les deux blocs</returns>
        public double SAD(bloc bc, bloc br)
        {
            double sad = 0;
            PixelData t1;
            PixelData t2;
            int x1, x2, y1, y2;
            // PixelData t3 = new PixelData();
            //accés au pixel
            double val1, val2;

            for (int i = 0; i < taille_bloc; i++)
            {
                for (int j = 0; j < taille_bloc; j++)
                {
                     x1 = Math.Max(0, Math.Min(wi-1, (br.x + i)));
                     y1 = Math.Max(0, Math.Min(he-1, (br.y + j)));
                     x2 = Math.Max(0, Math.Min(wi-1, (bc.x + i)));
                     y2 = Math.Max(0, Math.Min(he-1, (bc.y + j)));
                    t1 = a.GetPixel(x1, y1);
                    t2 = b.GetPixel(x2, y2);
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

        /// <summary>
        /// tester si un bloc appartient à la fenetre de recherche
        /// </summary>
        /// <param name="Br">bloc référence</param>
        /// <param name="Bc">bloc candidat</param>
        /// <returns>booleen qui indique si Bc appartient à la fenetre de Br</returns>
        public bool appartient_fen(bloc Br, bloc Bc)
        {
            bool b;
            if ((Bc.x >= Br.x - deplace_fen) && (Bc.x <= Br.x + deplace_fen))
            {
                if ((Bc.y >= Br.y - deplace_fen) && (Bc.y <= Br.y + deplace_fen))
                {
                    if (Bc.x + taille_bloc < wi && Bc.y + taille_bloc < he)
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
    }
}
