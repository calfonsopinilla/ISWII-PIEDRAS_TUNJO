import { Usuario } from './usuario.interface';

export interface ReservaTicket {
    Id?: number;
    FechaCompra?: Date;
    FechaIngreso?: Date;
    Precio?: number;
    Cantidad?: number;
    Qr?: string;
    Token?: string;
    LastModification?: Date;
    EstadoId?: number;
    UUsuarioId?: number;
    UUsuario?: Usuario;
    idTicket?: number;
}
