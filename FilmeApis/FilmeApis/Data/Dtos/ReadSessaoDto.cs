namespace FilmeApis.Data.Dtos
{
    public class ReadSessaoDto
    {
        public int Id { get; set; }
        public int FilmeId { get; set; }
        public int CinemaId { get; set; }
        public ReadFilmeDto Filme { get; set; }
        public ReadCinemaDto Cinema { get; set; }


    }
}
