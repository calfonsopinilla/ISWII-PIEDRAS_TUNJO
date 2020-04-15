import { Cabana } from './cabana.interface';

export interface ReservaCabana {
    Id?: number;
    FechaReserva?: Date;
    ValorTotal?: number;
    UUsuarioId?: number;
    UCabanaId?: number;
    UCabana?: Cabana;
}
