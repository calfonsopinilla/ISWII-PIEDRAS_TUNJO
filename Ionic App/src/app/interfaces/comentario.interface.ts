import { Usuario } from './usuario.interface';

export interface Comentario {

    Id?: number;
    FechaPublicacion?: Date;
    Descripcion?: string;
    Calificacion?: number;
    LastModification?: Date;
    Token?: string;
    UsuarioId?: number;
    Usuario?: Usuario;        
}