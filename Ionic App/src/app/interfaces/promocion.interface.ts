export interface Promocion {
    Id?: number;
    Nombre?: string;
    Descripcion?: string;
    Precio?: number;
    FechaInicio?: Date;
    FechaFin?: Date;
    PorcentajeDescuento?: number;
    TicketId?: number;
    Estado?: number;
    LastModification?: Date;
    Token?: string;
}
