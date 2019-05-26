using System;

namespace Housework {
    public class WashingMachine {
        private readonly HouseworkSimulator sim;

        public WashingMachine(HouseworkSimulator sim) {
            this.sim = sim;
        }

        public void Wash(Laundry laundry) {
            sim.DoWork(TimeSpan.FromHours(2.5));
            laundry.State = LaundryState.Wet;
            sim.Report("(washing machine has finished)");
        }
    }
}