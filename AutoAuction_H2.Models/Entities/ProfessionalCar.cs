namespace AutoAuction_H2.Models.Entities
{
    public class ProfessionalCar : Car
    {
        public bool SafetyBar { get; private set; }
        public double LoadCapacity { get; private set; }

        internal ProfessionalCar(
            string name,
            string regNumber,
            int year,
            decimal purchasePrice,
            double mileage,
            bool towBar,
            double motorSize,
            double fuelEfficiency,
            FuelType fuelType,
            int seats,
            int trunkSizeLitres,
            bool safetyBar,
            double loadCapacity)
            : base(name, regNumber, year, purchasePrice, mileage, towBar, motorSize, fuelEfficiency, fuelType, seats, trunkSizeLitres)
        {
            SafetyBar = safetyBar;
            LoadCapacity = loadCapacity;
            LicenseType = towBar || loadCapacity > 750 ? LicenseType.BE : LicenseType.B;
        }

        private ProfessionalCar() { }

        public override string ToString()
        {
            return base.ToString() + $" | Erhvervsbil: Sikkerhedsbøjle = {(SafetyBar ? "Ja" : "Nej")}, Lasteevne: {LoadCapacity} kg";
        }
    }
}
