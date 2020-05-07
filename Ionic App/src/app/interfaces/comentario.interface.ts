import { Usuario } from './usuario.interface';

export interface Comentario {
    id?: number;
    fechaPublicacion?: Date;
    descripcion?: string;
    calificacion?: number;
    lastModification?: Date;
    token?: string;
    usuarioId?: number;
    usuario?: Usuario; 
    reportado: boolean;           
}