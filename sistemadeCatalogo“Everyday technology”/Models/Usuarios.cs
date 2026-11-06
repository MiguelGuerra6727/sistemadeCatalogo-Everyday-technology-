using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace sistemadeCatalogo_Everyday_technology_.Models
{
    //Le puse usuario porque estaba cansado y e confundi (fue a las 2:00 AM )
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Nombre { get; set; }

        //Este lo que hace es que no permitira que los dos administrador no tengan el mismo correo
        [Required, StringLength(150)]
        [Index("Ix_Usuarios_Correo", IsUnique =true)]
        public string Correo { get; set; }
        [Required, StringLength(300)]
        public string PasswordHash { get; set; }
        [Required, StringLength(150)]
        public string  Salt { get; set; }
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public virtual Rol Rol{ get; set; }

        public bool Activo { get; set; } = true;

    }
}