using System.Drawing;
using System.IO;
using System.Threading;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Iniciando redimensionador \n\n ~~");

        Thread thread = new Thread(Redimensionar);
        thread.Start();



        Console.Read();
    }

    static void Redimensionar()
    {
        #region "Diretorios"
        string dirEntrada = "arquivosEntrada";
        string dirRedi = "arquivoRedimensionado";
        string dirSaida = "arquivosFinalizados";

        if (!Directory.Exists(dirEntrada)) { Directory.CreateDirectory(dirEntrada); }
        if (!Directory.Exists(dirRedi)) { Directory.CreateDirectory(dirRedi); }
        if (!Directory.Exists(dirSaida)) { Directory.CreateDirectory(dirSaida); }
        #endregion

        FileStream Fs;
        FileInfo Fi;

        while (true) 
        {
            var arquivosEntrada = Directory.EnumerateFiles(dirEntrada);
            int Altura = 200;

            foreach(var arquivo in  arquivosEntrada)
            {
                Fs = new FileStream(arquivo,FileMode.Open,FileAccess.ReadWrite,FileShare.ReadWrite);
                Fi = new FileInfo(arquivo);

                String caminho = Environment.CurrentDirectory + @"\" + dirRedi + @"\" + DateTime.Now.Millisecond.ToString() +"_"+ Fi.Name;

                Redimensionador(Image.FromStream(Fs),Altura, caminho);

                Fs.Close();

                string caminho_saida = Environment.CurrentDirectory + @"\" + dirSaida+ @"\" + Fi.Name;
                Fi.MoveTo(caminho_saida);
            }

            
            Thread.Sleep(new TimeSpan(0,0,5));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imagem">Imagem a ser redimensionada</param>
    /// <param name="altura">Altura que queremos</param>
    /// <param name="caminho">Caminho de gravação do arquivo redimensionado</param>
    /// <returns></returns>

    static void Redimensionador(Image imagem, int altura, string caminho)
    {
        double ration = (double)altura / (double)imagem.Height;
        int novaLargura = (int)(imagem.Width * ration);
        int novaAltura = (int)(imagem.Height * ration);

        Bitmap novaImagem = new Bitmap(novaLargura, novaAltura);

        using (Graphics g = Graphics.FromImage(novaImagem))
        {
            g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
        }

        novaImagem.Save(caminho);
        imagem.Dispose();
    }
}