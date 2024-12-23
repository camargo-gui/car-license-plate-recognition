﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ProjEncontraPlaca
{
    class Filtros
    {
        private static void segmenta8(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, Point ini, List<Point> listaPini, List<Point> listaPfim, Color cor_pintar)
        {
            Point menor = new Point(), maior = new Point(), patual = new Point();
            Queue<Point> fila = new Queue<Point>();
            menor.X = maior.X = ini.X;
            menor.Y = maior.Y = ini.Y;
            fila.Enqueue(ini);
            while (fila.Count != 0)
            {
                patual = fila.Dequeue();
                imageBitmapSrc.SetPixel(patual.X, patual.Y, Color.FromArgb(255, 0, 0));
                imageBitmapDest.SetPixel(patual.X, patual.Y, cor_pintar);

                if (patual.X < menor.X)
                    menor.X = patual.X;
                if (patual.X > maior.X)
                    maior.X = patual.X;
                if (patual.Y < menor.Y)
                    menor.Y = patual.Y;
                if (patual.Y > maior.Y)
                    maior.Y = patual.Y;

                if (patual.X > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X - 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X - 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y > 0)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y - 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y - 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y - 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X < imageBitmapSrc.Width - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y - 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y - 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y - 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.X < imageBitmapSrc.Width - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X + 1, patual.Y));
                        imageBitmapSrc.SetPixel(patual.X + 1, patual.Y, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.Y < imageBitmapSrc.Height - 1)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X + 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X + 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X + 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }
                if (patual.Y < imageBitmapSrc.Height - 1)
                {
                    Color cor = imageBitmapSrc.GetPixel(patual.X, patual.Y + 1);
                    if (cor.R == 0)
                    {
                        fila.Enqueue(new Point(patual.X, patual.Y + 1));
                        imageBitmapSrc.SetPixel(patual.X, patual.Y + 1, Color.FromArgb(255, 0, 0));
                    }
                    if (patual.X > 0)
                    {
                        cor = imageBitmapSrc.GetPixel(patual.X - 1, patual.Y + 1);
                        if (cor.R == 0)
                        {
                            fila.Enqueue(new Point(patual.X - 1, patual.Y + 1));
                            imageBitmapSrc.SetPixel(patual.X - 1, patual.Y + 1, Color.FromArgb(255, 0, 0));
                        }
                    }
                }

            }

            if (menor.X > 0)
                menor.X--;
            if (maior.X < imageBitmapSrc.Width - 1)
                maior.X++;
            if (menor.Y > 0)
                menor.Y--;
            if (maior.Y < imageBitmapSrc.Height - 1)
                maior.Y++;

            // Removemos a linha que desenha retângulos vermelhos durante a segmentação inicial
            // desenhaRetangulo(imageBitmapDest, menor, maior, Color.FromArgb(255, 0, 0));

            listaPini.Add(menor);
            listaPfim.Add(maior);
        }

        public static void segmentar8conectado(Bitmap imageBitmapSrc, Bitmap imageBitmapDest, List<Point> listaPini, List<Point> listaPfim)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;

                    if (r == 0)
                        segmenta8(imageBitmapSrc, imageBitmapDest, new Point(x, y), listaPini, listaPfim, Color.FromArgb(100, 100, 100));
                }
            }
        }

        private static void desenhaRetangulo(Bitmap imageBitmapDest, Point menor, Point maior, Color cor)
        {
            for (int x = menor.X; x <= maior.X; x++)
            {
                imageBitmapDest.SetPixel(x, menor.Y, cor);
                imageBitmapDest.SetPixel(x, maior.Y, cor);
            }
            for (int y = menor.Y; y <= maior.Y; y++)
            {
                imageBitmapDest.SetPixel(menor.X, y, cor);
                imageBitmapDest.SetPixel(maior.X, y, cor);
            }
        }

        public static Bitmap segmentaRoI(Bitmap imageBitmap, int x, int y, int w, int h)
        {
            Bitmap img_dig = new Bitmap(w, h);
            int cor;
            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                {
                    cor = imageBitmap.GetPixel(x + j, y + i).R;
                    img_dig.SetPixel(j, i, Color.FromArgb(cor, cor, cor));
                }
            return img_dig;
        }

        public static Bitmap Dilation(Bitmap binaryImage)
        {
            Bitmap dilatedImage = new Bitmap(binaryImage.Width, binaryImage.Height);

            // Copiar a imagem original para a dilatada
            for (int y = 0; y < binaryImage.Height; y++)
            {
                for (int x = 0; x < binaryImage.Width; x++)
                {
                    dilatedImage.SetPixel(x, y, binaryImage.GetPixel(x, y));
                }
            }

            // Iterar sobre cada pixel (excluindo as bordas)
            for (int y = 1; y < binaryImage.Height - 1; y++)
            {
                for (int x = 1; x < binaryImage.Width - 1; x++)
                {
                    // Se o pixel atual é branco, preencher seus vizinhos
                    if (binaryImage.GetPixel(x, y).R == 255)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            for (int dx = -1; dx <= 1; dx++)
                            {
                                dilatedImage.SetPixel(x + dx, y + dy, Color.FromArgb(255, 255, 255));
                            }
                        }
                    }
                }
            }

            return dilatedImage;
        }

        public static void encontra_placa(Bitmap imageBitmapSrc, ref Bitmap imageBitmapDest, ref String placa)
        {
            try
            {
                List<Point> listaPini = new List<Point>();
                List<Point> listaPfim = new List<Point>();

                Otsu otsu = new Otsu();

                // Clonar imagem original
                Bitmap imageBitmapDestTemp = (Bitmap)imageBitmapSrc.Clone();
                otsu.Convert2GrayScaleFast(imageBitmapDestTemp);

                int otsuThreshold = otsu.getOtsuThreshold(imageBitmapDestTemp);
                otsu.threshold(imageBitmapDestTemp, otsuThreshold);

                // Segmentação 8-conectado
                Filtros.segmentar8conectado(imageBitmapDestTemp, imageBitmapDestTemp, listaPini, listaPfim);

                int altura, largura;
                List<Retangulo> retangulos = new List<Retangulo>();

                // Coletar os retângulos detectados (possíveis caracteres)
                for (int i = 0; i < listaPini.Count; i++)
                {
                    altura = listaPfim[i].Y - listaPini[i].Y;
                    largura = listaPfim[i].X - listaPini[i].X;

                    if (altura > 3 && altura < 100 && largura > 2 && largura < 50)
                    {
                        retangulos.Add(new Retangulo
                        {
                            Pini = listaPini[i],
                            Pfim = listaPfim[i],
                            CentroX = (listaPini[i].X + listaPfim[i].X) / 2.0f,
                            CentroY = (listaPini[i].Y + listaPfim[i].Y) / 2.0f
                        });
                    }
                }

                // Desenhar retângulos para debug
                for (int i = 0; i < retangulos.Count; i++)
                {
                    desenhaRetangulo(imageBitmapDestTemp, retangulos[i].Pini, retangulos[i].Pfim, Color.FromArgb(0, 255, 0));
                }
                imageBitmapDestTemp.Save("imagemretangulos.jpg", ImageFormat.Jpeg);

                Console.WriteLine("Número de retângulos detectados: " + retangulos.Count);

                if (retangulos.Count == 0)
                    return; // Nenhum caractere detectado

                // Ordenar retângulos por CentroX
                retangulos.Sort((a, b) => a.CentroX.CompareTo(b.CentroX));

                // Agrupar retângulos
                List<List<Retangulo>> grupos = new List<List<Retangulo>>();
                float distanciaMaximaHorizontal = 100;
                float distanciaMaximaVertical = 45;

                float tamanhoAltura = imageBitmapSrc.Height / 3;
                float tamanhoLargura = imageBitmapSrc.Width / 3;

                foreach (var ret in retangulos)
                {
                    bool adicionadoAoGrupo = false;
                    foreach (var grupo in grupos)
                    {
                        var ultimoRetanguloDoGrupo = grupo[grupo.Count - 1];

                        float distanciaHorizontal = Math.Abs(ret.CentroX - ultimoRetanguloDoGrupo.CentroX);
                        float distanciaVertical = Math.Abs(ret.CentroY - ultimoRetanguloDoGrupo.CentroY);

                        if (distanciaHorizontal <= distanciaMaximaHorizontal && distanciaVertical <= distanciaMaximaVertical)
                        {
                            if (!(ret.Pfim.X < tamanhoLargura && ret.Pfim.Y < tamanhoAltura || ret.Pfim.X < tamanhoLargura &&
                                ret.Pfim.Y > imageBitmapSrc.Height - tamanhoAltura ||
                                ret.Pfim.X > imageBitmapSrc.Width - tamanhoLargura && ret.Pfim.Y < tamanhoAltura ||
                                ret.Pfim.X > imageBitmapSrc.Width - tamanhoLargura && ret.Pfim.Y > imageBitmapSrc.Height - tamanhoAltura))
                            {
                                grupo.Add(ret);
                                adicionadoAoGrupo = true;
                                break;
                            }
                        }
                    }
                    if (!adicionadoAoGrupo)
                    {
                        grupos.Add(new List<Retangulo> { ret });
                    }
                }

                Console.WriteLine("Número de grupos formados: " + grupos.Count);

                var grupoPlaca = grupos.OrderByDescending(g => g.Count).First();
                Console.WriteLine("Tamanho do maior grupo (grupo da placa): " + grupoPlaca.Count);

                int minX = grupoPlaca.Min(r => r.Pini.X);
                int minY = grupoPlaca.Min(r => r.Pini.Y);
                int maxX = grupoPlaca.Max(r => r.Pfim.X);
                int maxY = grupoPlaca.Max(r => r.Pfim.Y);

                minX = Math.Max(minX - 60, 0);
                minY = Math.Max(minY - 15, 0);
                maxX = Math.Min(maxX + 5, imageBitmapSrc.Width - 1);
                maxY = Math.Min(maxY + 15, imageBitmapSrc.Height - 1);

                Rectangle plateRect = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                Bitmap plateImageOriginal = imageBitmapSrc.Clone(plateRect, imageBitmapSrc.PixelFormat);

                // Primeiro processamento na placa
                Bitmap dilatedPlateImage = ProcessarPlaca(plateImageOriginal, otsu);

                // Segmentar e reconhecer caracteres
                List<Caracter> caracteres = SegmentarEReconhecerCaracteres(dilatedPlateImage);

                Console.WriteLine("Número de caracteres detectados na placa: " + caracteres.Count);

                // Se apenas 6 caracteres foram reconhecidos, tente o recorte e reprocessamento
                if (caracteres.Count == 5 || caracteres.Count == 6)
                {
                    // Encontrar linha inferior máxima dos caracteres
                    int maxYBottom = caracteres.Max(c => c.Pfim.Y);
                    // Recortar a placa original até maxYBottom+margem
                    int novoMaxY = Math.Min(maxYBottom, plateImageOriginal.Height - 1);
                    Rectangle novoRecorte = new Rectangle(0, 0, plateImageOriginal.Width, novoMaxY);
                    Bitmap plateImageRecortada = plateImageOriginal.Clone(novoRecorte, plateImageOriginal.PixelFormat);

                    // Reprocessar com a imagem recortada
                    Bitmap dilatedPlateImageRecortada = ProcessarPlaca(plateImageRecortada, otsu);
                    caracteres = SegmentarEReconhecerCaracteres(dilatedPlateImageRecortada);
                    Console.WriteLine("Número de caracteres após reprocessamento: " + caracteres.Count);

                    // Atualiza dilatedPlateImage caso tenha tido sucesso
                    dilatedPlateImage = dilatedPlateImageRecortada;
                }

                // Ajuste da contagem de caracteres (se necessário)
                if (caracteres.Count > 7)
                {
                    caracteres.RemoveRange(0, caracteres.Count - 7);
                }

                StringBuilder placaBuilder = new StringBuilder();
                for (int i = 0; i < caracteres.Count; i++)
                {
                    desenhaRetangulo(dilatedPlateImage, caracteres[i].Pini, caracteres[i].Pfim, Color.FromArgb(0, 255, 0));
                    ClassificacaoCaracteres classificaChar = new ClassificacaoCaracteres(
                        caracteres[i].Imagem.Height,
                        caracteres[i].Imagem.Width,
                        i > 2 ? 1 : 2, 'S');
                    String transicao = classificaChar.retornaTransicaoHorizontal(caracteres[i].Imagem);
                    char caractere = classificaChar.reconheceCaractereTransicao_2pixels(transicao);
                    placaBuilder.Append(caractere);
                }

                placa = placaBuilder.ToString();

                // Atualizar imageBitmapDest com a imagem da placa processada
                imageBitmapDest.Dispose();
                imageBitmapDest = (Bitmap)dilatedPlateImage.Clone();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro durante o processamento: " + ex.Message);
            }
        }

        private static Bitmap ProcessarPlaca(Bitmap plateImage, Otsu otsu)
        {
            // Aplicar Otsu
            otsu.Convert2GrayScaleFast(plateImage);
            int plateThreshold = otsu.getOtsuThreshold(plateImage);
            otsu.threshold(plateImage, plateThreshold);

            // Aplicar Dilatação Condicional
            Bitmap dilatedPlateImage = ConditionalDilation(plateImage, whitePixelDensityThreshold: 0.15);
            return dilatedPlateImage;
        }

        private static List<Caracter> SegmentarEReconhecerCaracteres(Bitmap dilatedPlateImage)
        {
            List<Point> plateListaPini = new List<Point>();
            List<Point> plateListaPfim = new List<Point>();

            Bitmap plateImageClone = (Bitmap)dilatedPlateImage.Clone();
            Filtros.segmentar8conectado(plateImageClone, dilatedPlateImage, plateListaPini, plateListaPfim);

            List<Caracter> caracteres = new List<Caracter>();
            for (int i = 0; i < plateListaPini.Count; i++)
            {
                int charAltura = plateListaPfim[i].Y - plateListaPini[i].Y;
                int charLargura = plateListaPfim[i].X - plateListaPini[i].X;

                if (charAltura > 10 && charAltura < 100 && charLargura > 2 && charLargura < 50)
                {
                    Caracter c = new Caracter
                    {
                        Pini = plateListaPini[i],
                        Pfim = plateListaPfim[i],
                        CentroX = (plateListaPini[i].X + plateListaPfim[i].X) / 2.0f,
                        Imagem = Filtros.segmentaRoI(dilatedPlateImage, plateListaPini[i].X, plateListaPini[i].Y, charLargura, charAltura)
                    };
                    caracteres.Add(c);
                }
            }

            // Ordenar caracteres pela posição X
            caracteres.Sort((a, b) => a.CentroX.CompareTo(b.CentroX));
            return caracteres;
        }
        public static Bitmap Dilation(Bitmap binaryImage, int iterations = 1)
        {
            Bitmap dilatedImage = (Bitmap)binaryImage.Clone();

            for (int i = 0; i < iterations; i++)
            {
                Bitmap tempImage = (Bitmap)dilatedImage.Clone();

                for (int y = 1; y < dilatedImage.Height - 1; y++)
                {
                    for (int x = 1; x < dilatedImage.Width - 1; x++)
                    {
                        // Se o pixel atual é branco, preencher seus vizinhos
                        if (binaryImage.GetPixel(x, y).R == 255)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                for (int dx = -1; dx <= 1; dx++)
                                {
                                    tempImage.SetPixel(x + dx, y + dy, Color.FromArgb(255, 255, 255));
                                }
                            }
                        }
                    }
                }

                dilatedImage.Dispose();
                dilatedImage = tempImage;
            }

            return dilatedImage;
        }


        public static Bitmap ConditionalDilation(Bitmap binaryImage, double whitePixelDensityThreshold = 0.1)
        {
            // Calcular a densidade de pixels brancos
            int whitePixels = 0;
            int totalPixels = binaryImage.Width * binaryImage.Height;

            for (int y = 0; y < binaryImage.Height; y++)
            {
                for (int x = 0; x < binaryImage.Width; x++)
                {
                    if (binaryImage.GetPixel(x, y).R == 255)
                        whitePixels++;
                }
            }

            double density = (double)whitePixels / totalPixels;

            // Decidir se aplica a dilatação
            if (density < whitePixelDensityThreshold)
            {
                // Aplicar dilatação uma vez
                return Dilation(binaryImage, 1);
            }
            else
            {
                // Não aplicar dilatação
                return (Bitmap)binaryImage.Clone();
            }
        }

        // Método threshold (opcional, se necessário)
        public static void threshold(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.1140 + g * 0.5870 + b * 0.2990);
                    if (gs > 127)
                        gs = 255;
                    else
                        gs = 0;

                    // Nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        public static void brancoPreto(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    if (gs > 220)
                        gs = 255;
                    else
                        gs = 0;

                    // Nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);
                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }
    }
}
