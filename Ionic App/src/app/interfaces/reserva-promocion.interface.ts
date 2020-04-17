import { Promocion } from './promocion.interface';

export interface ReservaPromocion {
    Id?: number;
    FechaCompra?: Date;
    Precio?: number;
    UsuarioId?: number;
    EstadoId?: number;
    UPromocionId?: number;
    UPromocion?: Promocion;
}
