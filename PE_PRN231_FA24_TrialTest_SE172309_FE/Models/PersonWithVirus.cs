namespace PE_PRN231_FA24_TrialTest_SE172309_FE.Models
{
    public class PersonWithVirus
    {
        public int PersonId { get; set; }

        public string Fullname { get; set; } = null!;

        public DateOnly BirthDay { get; set; }

        public string Phone { get; set; } = null!;
        public List<PersonVirusDTO> Viruses { get; set; }
    }

    public class PersonVirusDTO
    {
        public string VirusName { get; set; } = null!;
        public double? ResistanceRate { get; set; }
    }
}
