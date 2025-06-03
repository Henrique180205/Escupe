namespace escupe.Models
{
    public class UsuarioAutenticado
    {
        public object Usuario { get; set; }
        public string TipoUsuario { get; set; } // "C" para candidato, "E" para empresa
    }
}
