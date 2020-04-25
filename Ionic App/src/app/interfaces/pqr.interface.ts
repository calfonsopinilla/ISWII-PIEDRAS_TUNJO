import { Usuario } from './usuario.interface';

export interface Pqr {
    Id?: number;
    FechaPublicacion?: Date;
    Pregunta?: string;
    Respuesta?: string;
    Token?: string;
    LastModification?: Date;
    UEstadoPQRId?: number;
    UEstadoPQR?: UEstadoPQR;
    UUsuarioId?: number;
    UUsuario?: Usuario;
    FechaRespuesta?: Date;
}

export interface UEstadoPQR {
    Id: number;
    Nombre: string;
}
