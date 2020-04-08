export interface Usuario {
    Id?: number;
    Nombre?: string;
    Apellido?: string;
    TipoDocumento?: string;
    NumeroDocumento?: string;
    LugarExpedicion?: string;
    CorreoElectronico?: string;
    Clave?: string;
    Icono_url?: string;
    VerificacionCuenta?: boolean;
    EstadoCuenta?: boolean;
    RolId?: number;
    Imagen_documento?: string;
    Token?: string;
}