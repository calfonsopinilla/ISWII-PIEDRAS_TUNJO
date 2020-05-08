
export interface Noticias {
    id :  number ;
    titulo : string;
    descripcion :string;
    fechaPublicacion : Date;
    imagenesUrl: string;
    listaImagenes  : string[];
    comentariosId : string ; 
    calificacion : number;
}
