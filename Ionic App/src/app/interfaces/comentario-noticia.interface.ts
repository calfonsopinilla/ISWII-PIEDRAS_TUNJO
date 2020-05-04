import { Comentario } from './comentario.interface';

export interface ComentarioNoticia extends Comentario {
    NoticiaId?: number;
}