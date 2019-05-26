using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundWorkerDemo {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Press any key to start the amazing threading demo!");
            Console.ReadKey(false);
            using (var bw = new BackgroundWorker()) {

                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += bw_DoWork;
                bw.ProgressChanged += bw_ProgressChanged;
                bw.RunWorkerAsync();
                for (var i = 0; i < 10; i++) {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Message from foreground thread: " + i * 10);
                    Console.ForegroundColor = color;
                    Thread.Sleep(300);
                }
            }
            Console.ReadKey(false);
        }

        static void bw_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Message from background worker: " + e.ProgressPercentage);
            Console.ForegroundColor = color;
        }

        static void bw_DoWork(object sender, DoWorkEventArgs e) {
            var worker = sender as BackgroundWorker;

            for (var i = 1; (i <= 10); i++) {
                if ((worker.CancellationPending)) {
                    e.Cancel = true;
                    break;
                }
                Thread.Sleep(500);
                worker.ReportProgress((i * 10));
            }
        }
    }
}
