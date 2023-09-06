namespace wineo.Domain.Entities;

public class WineEvaluation : BaseAuditableEntity
{
    public double Appearance { get; set; } // Criteria 1: Appearance
    public double Aroma { get; set; }     // Criteria 2: Aroma
    public double Taste { get; set; }     // Criteria 3: Taste


    public string Evaluation { get; set; }

    public string AuthorId { get; set; }

    public int WineId { get; set; }

    public Wine Wine { get; set; }

    

    public double CalculateOverallScore()
    {
        double totalScore = Appearance + Aroma + Taste;
        double overallScore = totalScore / 3.0;

        return overallScore;
    }
}