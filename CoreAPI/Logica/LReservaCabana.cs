using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
namespace Logica
{
    public class LReservaCabana
    {

        ///Fecha : 14/04/2020
        /// Nombre: Jose Luis Soriano 
        /// Resive :id de la cabana
        /// Retorna : Los dias disponibles para esa cabana
        public IEnumerable<DateTime> ObtenerDiasHabilesCabana(int idCabana) {
            try{
                List<DateTime> diasHabiles = diasHabilesS();
                List<UReservaCabana> diasReservados = new DaoReservaCabana().ObtenerTodos().Where(x => x.UCabanaId == idCabana).ToList();
                if (diasReservados.Count != 0){
                    for (int x = 0; x < diasReservados.Count(); x++){
                        bool buscar = diasHabiles.Contains(diasReservados[x].FechaReserva);
                        if (buscar != false){
                            int id = diasHabiles.IndexOf(diasReservados[x].FechaReserva);
                            //
                            diasHabiles.RemoveAt(id);
                        }
                    }
                    return diasHabiles;
                }
                else{
                    return diasHabiles;
                }
            }
            catch (Exception ex) {
                throw ex; 
            }
        }

        public IEnumerable<UReservaCabana> ObtenerTodos()
        {
            return new DaoReservaCabana().ObtenerTodos();
        }

        public IEnumerable<UReservaCabana> ObtenerPorUsuario(int userId)
        {
            return new DaoReservaCabana().ObtenerPorUsuario(userId);
        }

        public UReservaCabana Buscar(int id)
        {
            return new DaoReservaCabana().Buscar(id);
        }

        public bool Agregar(UReservaCabana reserva)
        {
            return new DaoReservaCabana().Agregar(reserva);
        }

        public bool Actualizar(UReservaCabana reserva, int id)
        {
            return new DaoReservaCabana().Actualizar(reserva, id);
        }

        public bool Eliminar(int id)
        {
            return new DaoReservaCabana().Eliminar(id);
        }

        ///Fecha : 13/04/2020
        /// Nombre: Jose Luis Soriano 
        /// Resive :el año para calcular el dia de pascua de este
        /// Retorna : El dia de pascua 

        protected DateTime calcularPascua(int anio)
        {
            int M = 24;
            int N = 5;
            int dia, mes;
            int a, b, c, d, e;
            a = anio % 19;
            b = anio % 4;
            c = anio % 7;
            d = (19 * a + M) % 30;
            e = (2 * b + 4 * c + 6 * d + N) % 7;
            dia = (d + e < 10) ? d + e + 22 : d + e - 9;
            mes = (d + e < 10) ? 3 : 4;
            if (dia == 26 && mes == 4)
            {
                dia = 19;
            }
            if (dia == 25 && mes == 4 && d == 28 && e == 6 && a > 10)
            {
                dia = 18;
            }
            return new DateTime(anio, mes, dia);
        }

        ///Fecha : 13/04/2020
        /// Nombre: Jose Luis Soriano 
        /// Resive :Rango en que necesite los festivos entre esa fecha
        /// Retorna : lista de festivos en esos rango de fecha

        public List<DateTime> festivos(DateTime inicio, DateTime fin)
        {
            //festivos fijos
            List<DateTime> festivos = new List<DateTime>();
            int añosCalcular = fin.Year - inicio.Year;
            int anio = inicio.Year;
            if (inicio.Year != fin.Year)
            {

                if (añosCalcular != 0)
                {
                    for (int x = 0; x <= añosCalcular; x++)
                    {
                        DateTime pascua = calcularPascua(anio);
                        festivos.Add(new DateTime(anio, 1, 1));
                        festivos.Add(new DateTime(anio, 5, 1));
                        festivos.Add(new DateTime(anio, 7, 20));
                        festivos.Add(new DateTime(anio, 8, 7));
                        festivos.Add(new DateTime(anio, 12, 8));
                        festivos.Add(new DateTime(anio, 12, 25));
                        //festivos segun pascua 
                        festivos.Add(pascua.AddDays(-3));
                        festivos.Add(pascua.AddDays(-2));
                        festivos.Add(pascua.AddDays(43));
                        festivos.Add(pascua.AddDays(64));
                        festivos.Add(pascua.AddDays(71));
                        //festivos de fecha trasladable 
                        List<DateTime> festivosTrasladables = new List<DateTime>();
                        festivosTrasladables.Add(new DateTime(anio, 1, 6));
                        festivosTrasladables.Add(new DateTime(anio, 3, 19));
                        festivosTrasladables.Add(new DateTime(anio, 6, 29));
                        festivosTrasladables.Add(new DateTime(anio, 8, 15));
                        festivosTrasladables.Add(new DateTime(anio, 12, 10));
                        festivosTrasladables.Add(new DateTime(anio, 11, 1));
                        festivosTrasladables.Add(new DateTime(anio, 11, 11));
                        for (int i = 0; i < festivosTrasladables.Count; i++)
                        {
                            if ((int)festivosTrasladables[i].DayOfWeek == 1)
                            {
                                festivos.Add(festivosTrasladables[i]);
                            }
                            else
                            {
                                while (true)
                                {
                                    festivosTrasladables[i] = festivosTrasladables[i].AddDays(1);
                                    if ((int)festivosTrasladables[i].DayOfWeek == 1)
                                    {
                                        festivos.Add(festivosTrasladables[i]);
                                        break;
                                    }
                                }
                            }
                        }
                        anio++;
                    }
                }
            }
            else
            {
                DateTime pascua = calcularPascua(anio);
                festivos.Add(new DateTime(anio, 1, 1));
                festivos.Add(new DateTime(anio, 5, 1));
                festivos.Add(new DateTime(anio, 7, 20));
                festivos.Add(new DateTime(anio, 8, 7));
                festivos.Add(new DateTime(anio, 12, 8));
                festivos.Add(new DateTime(anio, 12, 25));
                //festivos segun pascua 
                festivos.Add(pascua.AddDays(-3));
                festivos.Add(pascua.AddDays(-2));
                festivos.Add(pascua.AddDays(43));
                festivos.Add(pascua.AddDays(64));
                festivos.Add(pascua.AddDays(71));
                //festivos de fecha trasladable 
                List<DateTime> festivosTrasladables = new List<DateTime>();
                festivosTrasladables.Add(new DateTime(anio, 1, 6));
                festivosTrasladables.Add(new DateTime(anio, 3, 19));
                festivosTrasladables.Add(new DateTime(anio, 6, 29));
                festivosTrasladables.Add(new DateTime(anio, 8, 15));
                festivosTrasladables.Add(new DateTime(anio, 12, 10));
                festivosTrasladables.Add(new DateTime(anio, 11, 1));
                festivosTrasladables.Add(new DateTime(anio, 11, 11));

                for (int i = 0; i < festivosTrasladables.Count; i++)
                {
                    if ((int)festivosTrasladables[i].DayOfWeek == 1)
                    {
                        festivos.Add(festivosTrasladables[i]);
                    }
                    else
                    {
                        while (true)
                        {
                            festivosTrasladables[i] = festivosTrasladables[i].AddDays(1);
                            if ((int)festivosTrasladables[i].DayOfWeek == 1)
                            {
                                festivos.Add(festivosTrasladables[i]);
                                break;
                            }
                        }
                    }
                }

            }
            return festivos.OrderBy(x => x.Year).ToList();
        }

        ///Fecha : 13/04/2020
        /// Nombre: Jose Luis Soriano 
        /// Resive :nada
        /// Retorna : lista de dias habiles que estara abierto el parque en un mes

        public List<DateTime> diasHabilesS()
        {
            List<DateTime> diashabiles = new List<DateTime>();

            List<DateTime> diasFestivos = festivos(DateTime.Now, DateTime.Now.AddMonths(2)).OrderBy(x => x.Month).ToList();
            DateTime diaAnterior;
            DateTime diaBusqueda;
            for (DateTime i = DateTime.Now.AddDays(1); i <= DateTime.Now.AddMonths(1);)
            {
                if ((int)i.DayOfWeek != 1 && ((int)i.DayOfWeek != 2))
                {
                    diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                }
                else if ((int)i.DayOfWeek == 1)
                {
                    try
                    {
                        DateTime festivo = diasFestivos.Where(x => x.Equals(i.Date)).First();
                        diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else if ((int)i.DayOfWeek == 2)
                {
                    diaAnterior = i.AddDays(-1).Date;

                    diaBusqueda = i;
                    try
                    {
                        DateTime festivo = diasFestivos.Where(x => x.Equals(diaAnterior)).First();
                    }
                    catch (Exception ex)
                    {
                        diashabiles.Add(new DateTime(i.Year, i.Month, i.Day));
                    }

                }

                i = i.AddDays(1);
            }


            return diashabiles;

        }
    }




}





