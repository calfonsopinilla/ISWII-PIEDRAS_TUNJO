import { Comentario } from './comentario.interface';

export interface ComentarioEvento extends Comentario {
    EventoId?: number;
}
