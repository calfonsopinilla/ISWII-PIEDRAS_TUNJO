
import {ComentarioNoticia} from '../interfaces/comentario-noticia';

export interface Noticias {

    id :  number ;
    titulo : string;
    descripcion :string;
    fechaPublicacion : Date;
    imagenesUrl: string;
    listaImagenes  : string[];
    comentariosId : string ; 
    listaComentariosNoticia  :ComentarioNoticia[];
    calificacion : number;

}
