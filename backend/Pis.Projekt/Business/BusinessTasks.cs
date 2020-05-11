using Microsoft.Extensions.Logging;

namespace Pis.Projekt.Business
{
    public static class BusinessTasks
    {
        public const string FetchLastAggregates = "Načítanie agregovaných údajov o produktoch";
        public const string WaitingTask = "Čakanie 'n' hodín po začatí procesu";
        public const string SaleEvaluationTask = "Vyhodnotenie predajnosti produktov";
        public const string SaleDecreaseEvaluationTask = "Vyhodnotenie miery zníženia produktov";
        public const string SelectToAdTask = "Zaradenie do reklamných letákov";
        public const string EndStockingTask = "Ukončenie nakupovania produktov";
        public const string UserEvaluationSchedulingTask = "Planovanie pre pouzivatela";
        public const string OptimizationSchedulingTask = "Optimalizacia kazdy tyzden";
        public const string IncreasedSalesBranch = "Vetva pre zvysenie predaja o viac ako 10%";
        public const string DecreasedSalesBranch = "Vetva pre Znizenie predaja o viac ako 20%";
        public const string NotifyMarketing = "Upozornenie Marketingoveho oddelenia";
        public const string CalculateFinalPrice = "Výpočet novej ceny";
        public const string CreateOrder = "Vytvorenie objednávky";
        public const string SendOrder = "Objednanie produktov";
        public const string DecreasedSaleOfAtLeastOneProduct =
            "Znížil sa predaj aspoň jedného produktu?";
        public const string PersistenceTask = "Zmena cien v centrálnom systéme";
        public const string WaitingTaskSeasonStart = "Zaciatok sezony";
    }
}