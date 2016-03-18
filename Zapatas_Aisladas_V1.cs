using System;
//para exportar hay que añadir estas tres:
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Geotecnia_3
{
	public class ZapatasAisladas
	{
		//Se escriben en esta parte las variables para usarlas dentro de todo el programa
		//precios del APU de Zapata
		//			OJO!!!!!!			se deben actualizar
		static double cexc=22292, cacero=12952, ccon=82171, czap=55419, con3000=260200, A420=2800, Hmenor=500, mezcladora=8500, vibrador=5000;
		//variables:
		static int opcionPos=0, opcionz=0, i, opcionc=0, opcionTM=0, opciontipo=0, opcionlado=0;
		static string nombrezapata="", eleccion="", ubicacion="";
		
		static double PD=0, PL=0, P1=0, P1u=0, T, alfa, FC, a=0.25, b=0.25;
		static double diametrobz, Areabz, Masabz, Ddoblamientoz;
		static double diametrobc, Areabc, Masabc, Ddoblamientoc;
		static double fc=21, fy=420, TM, df, rsuelo, EsfuerzoAdmisible;
		static double r, rprima=0.075, Hmin, ldc, ldc1, ldc2, d1=0.15, d2, d3, d, H;
		static double rconcreto=24, Sobrecarga, EsfuerzoNeto, areareq, B=1, L=1, areafinal;
		static double V1, V2, rel1, rel2, voladizomax, relmax;
		//aqui
		static double Esfuerzo1u, AtL, V1u, ficortante=0.75, fiVc, landa=1;
		static double AtB, V2u, fiVc2, bo, betac, fiVc1p, fiVc2p, fiVc3p, fiVcpmin, Atp, V1up;
		
		static double c4, c3, c2, c1, c0;
		
		static double kL, mL, MuL, cuantiadL, AtLf, brazoL;
		static double AsrL, AstL, AssL, NbL, SccL, SlbL, Sminb;
		static double AssrL, cuantiaLsum, lddL, cc1, cc2, cc, relccdbz, factorAsL, ldrL, lcorteL;
		static double AtBf, brazoB, MuB, kB, mB, cuantiadB, AsrB, AstB, AssB, NbB, SccB, SlbB, AssrB, cuantiaBsum, lddB, factorAsB, ldrB, lcorteB;
		
		static double vconcreto, vexcavacion, acerototal, costo=0;
		
		
		//stringbuilder definido aqui para poder añadir codigo desde cualquier parte del programa
		static StringBuilder constructor= new StringBuilder();
		
		
		public static void Main()
		{
			//daremos un nombre a la zapata
			Console.WriteLine("Ingrese un nombre para identificar la zapata");
			nombrezapata=Console.ReadLine();
				constructor.AppendLine("---INGENIERIA CIVIL---\n");
				constructor.AppendLine("\nZAPATA: "+nombrezapata+" \n");
				constructor.AppendLine("\n---ZAPATA AISLADA--- \n");
				constructor.AppendLine("\n---MEMORIA DE CALCULOS--- \n");
			
			ZapataAislada();
			
			Console.WriteLine("¿Desea exportar los calculos?\nDigite: si ó no");
				eleccion=Console.ReadLine();
				if (eleccion=="si")
					{
						//desde aqui se exporta
						//			EXPORTAR DATOS DESDE AQUI				//
						
						//desde aqui exportA
									
						constructor.AppendLine("Fin");
						//constructor.AppendLine("Factor T = "+T+"");
						//constructor.AppendLine("Factor alfa = "+alfa+"");
			
						//exportar filestream
						String cadena = "";
						cadena=constructor.ToString();
						FileStream fs = new FileStream("/home/juan/Desktop/Zapata:"+nombrezapata+".txt", FileMode.Append);
						fs.Write(ASCIIEncoding.ASCII.GetBytes(cadena), 0, cadena.Length);
						fs.Close();
						Console.WriteLine("Exportacion correcta!");
			
					}
					
			Console.WriteLine("Está saliendo del programa");
			Console.ReadKey();
			
		}
		
		//metodos personalizados
		//metodo de la zapata aislada
		static void ZapataAislada()
		{
			Console.WriteLine("\t\t\t INGENIERIA CIVIL \n");
			Console.WriteLine("\t\t\t 2.Zapata Aislada \n");
			
			constructor.AppendLine("\n");
			
			//calculo de las cargas
			Console.WriteLine("Ingrese Carga Muerta en KN:");
				double.TryParse(Console.ReadLine(),out PD);
			Console.WriteLine("Ingrese Carga Viva en KN:");
				double.TryParse(Console.ReadLine(),out PL);
				P1=checked(PD+PL);
				P1u=checked(1.2*PD+1.6*PL);
			Console.WriteLine("\t\t\t Calculo de las cargas de diseño. \n");
			Console.WriteLine("Carga Muerta = "+PD+"KN");
			Console.WriteLine("Carga Viva = "+PL+"KN");
			Console.WriteLine("Carga de Servicio = PD + PL");
			Console.WriteLine("Carga de Servicio = {0}+{1}",PD,PL);
			Console.WriteLine("Carga de Servicio = "+P1+"KN");
			Console.WriteLine("Carga Mayorada = 1.2*PD + 1.6*PL");
			Console.WriteLine("Carga Mayorada = 1,2*{0}+1,6*{1}",PD,PL);
			Console.WriteLine("Carga Mayorada = "+P1u+"KN \n");
			
				constructor.AppendLine("Calculo de las cargas de diseno. \n");
				constructor.AppendLine("La carga muerta es: PD = "+PD+"KN");
				constructor.AppendLine("La carga viva es: PL = "+PL+"KN \n");
				constructor.AppendLine("La carga de servicio es: \nP1 = PD+PL");
				constructor.AppendLine("P1 = "+PD+"KN+"+PL+"KN");
				constructor.AppendLine("P1 = "+P1+"KN \n");
				constructor.AppendLine("La carga mayorada es: \nP1u = 1.2*PD+1.6*PL");
				constructor.AppendLine("P1u = 1.2*"+PD+"KN+1.6*"+PL+"KN");
				constructor.AppendLine("P1u = "+P1u+"KN \n");
			
			//detalles constructivos y tipo de concreto
			Console.WriteLine("Ingrese f'c en MPa:");
				double.TryParse(Console.ReadLine(),out fc);
			
				constructor.AppendLine("Propiedades de los Materiales:\n");
			
			Console.WriteLine("Elija el tamaño maximo del agregado: \n");
			for(i=4;i<=11;i++)
				{
					Console.WriteLine("{0} ==> Para #{1}",i,i);
				}
			int.TryParse(Console.ReadLine(), out opcionTM);
			switch (opcionTM)
				{
					
					case 4:
						TM=12.7/1000;
						break;
					case 5:
						TM=15.9/1000;
						break;
					case 6:
						TM=19.1/1000;
						break;
					case 7:
						TM=22.2/1000;
						break;
					case 8:
						TM=25.4/1000;
						break;
					case 9:
						TM=28.7/1000;
						break;
					case 10:
						TM=32.5/1000;
						break;
					case 11:
						TM=35.8/1000;
						break;
					default:
						TM=25.4/1000;
						Console.WriteLine("No seleccionó una opción válida, se tomará tamaño maximo"+
						" del agregado N8 = 1");
						break;	
				}
			
				constructor.AppendLine("Diametro del tamaño maximo del agregado: N"+opcionTM+"\n");
				
			Console.WriteLine("¿Colocará estribos N4@10cm para el acero de la columna?");
			Console.WriteLine("Digite: si ó no");
				eleccion=Console.ReadLine();
					if (eleccion=="si")
						{
							FC=0.75;
							constructor.AppendLine("Tomando factor: FC = "+FC+" empleando estribos N4\n");
						}
						
					if (eleccion=="no")
						{
							FC=1;
							constructor.AppendLine("Tomando factor: FC = "+FC+" sin estribos N4\n");
						}
						
			Console.WriteLine("FC={0} \n",FC);
			
			//escogemos las dimensiones de la columna
			Console.WriteLine("Ingrese dimension 'a' paralela a 'L' de la columna en metros:");
			double.TryParse(Console.ReadLine(), out a);
			Console.WriteLine("Ingrese dimension 'b' paralela a 'B' de la columna en metros:");
			double.TryParse(Console.ReadLine(), out b);
			
			constructor.AppendLine("Dimensiones de la columna: a = "+a+" m; b = "+b+" m.\n");
			
			//escogemos la posicion de la columna
			Console.WriteLine("Elija la posición de la columna:");	
			Console.WriteLine(" 1 ==> Para columna interior");
			Console.WriteLine(" 2 ==> Para columna de lindero o medianera");
			Console.WriteLine(" 3 ==> Para columna de esquina \n");
			int.TryParse(Console.ReadLine(), out opcionPos);
			switch (opcionPos)
				{
					case 1:
						ubicacion="Interior";
						T=2000;
						alfa=40;
						Console.WriteLine("T = {0}",T);
						Console.WriteLine("alfa = {0}",alfa);
						break;
						
					case 2:
						ubicacion="Lindero";
						T=1250;
						alfa=30;
						Console.WriteLine("T = {0}",T);
						Console.WriteLine("alfa = {0}",alfa);
						break;
				
					case 3:
						ubicacion="Esquina";
						T=750;
						alfa=20;
						Console.WriteLine("T = {0}",T);
						Console.WriteLine("alfa = {0}",alfa);
						break;
			
					default:
						Console.WriteLine("No escogio ninguna opción válida, se toma de interior");
						ubicacion="Interior";
						T=2000;
						alfa=40;
						Console.WriteLine("T = {0}",T);
						Console.WriteLine("alfa = {0}",alfa);
						break;
				}
			
			constructor.AppendLine("Posicion de la columna: "+ubicacion+". T = "+T+" ; alfa = "+alfa+".\n");
			
			//escogemos el diametro de las barras
			//bloque de la barra de zapata
			Console.WriteLine("Elija el diametro de refuerzo a usar para la zapata:");
			for(i=2;i<=11;i++)
				{
					Console.WriteLine("{0} ==> Para #{1}",i,i);
				}
			int.TryParse(Console.ReadLine(), out opcionz);
			switch (opcionz)
				{
					case 2:
						diametrobz=6.4/1000;
						Areabz=0.000032;
						Masabz=0.25;
						Ddoblamientoz=6*diametrobz;
						break;
					case 3:
						diametrobz=9.5/1000;
						Areabz=0.000071;
						Masabz=0.56;
						Ddoblamientoz=6*diametrobz;
						break;
					case 4:
						diametrobz=12.7/1000;
						Areabz=0.000129;
						Masabz=0.994;
						Ddoblamientoz=6*diametrobz;
						break;
					case 5:
						diametrobz=15.9/1000;
						Areabz=0.000199;
						Masabz=1.552;
						Ddoblamientoz=6*diametrobz;
						break;
					case 6:
						diametrobz=19.1/1000;
						Areabz=0.000284;
						Masabz=2.235;
						Ddoblamientoz=6*diametrobz;
						break;
					case 7:
						diametrobz=22.2/1000;
						Areabz=0.000387;
						Masabz=3.042;
						Ddoblamientoz=6*diametrobz;
						break;
					case 8:
						diametrobz=25.4/1000;
						Areabz=0.000510;
						Masabz=3.973;
						Ddoblamientoz=6*diametrobz;
						break;
					case 9:
						diametrobz=28.7/1000;
						Areabz=0.000645;
						Masabz=5.06;
						Ddoblamientoz=8*diametrobz;
						break;
					case 10:
						diametrobz=32.5/1000;
						Areabz=0.000819;
						Masabz=6.404;
						Ddoblamientoz=8*diametrobz;
						break;
					case 11:
						diametrobz=35.8/1000;
						Areabz=0.001006;
						Masabz=7.907;
						Ddoblamientoz=8*diametrobz;
						break;
					default:
						diametrobz=12.7/1000;
						Areabz=0.000129;
						Masabz=0.994;
						Ddoblamientoz=6*diametrobz;
						opcionz=4;
						Console.WriteLine("No seleccionó una opción válida, se tomará la #4 :P");
						Console.ReadKey();
						break;	
				}
			
					constructor.AppendLine("Propiedades segun NSR - 10 de los Refuerzos escogidos :\n");
					constructor.AppendLine("Refuerzo para zapata: N"+opcionz+"");
					constructor.AppendLine("Diametro: "+diametrobz+" m");
					constructor.AppendLine("Area: "+Areabz+" m2");
					constructor.AppendLine("Masa: "+Masabz+" Kg/mL");
					constructor.AppendLine("Diametro de Doblamiento: "+Ddoblamientoz+" m\n");
			
					
			
			//bloque de la barra de columna
			Console.WriteLine("Elija el diametro de refuerzo a usar para la columna:");
			for(i=2;i<=11;i++)
				{
					Console.WriteLine("{0} ==> Para #{1}",i,i);
				}
			int.TryParse(Console.ReadLine(), out opcionc);
			switch (opcionc)
				{
					case 2:
						diametrobc=6.4/1000;
						Areabc=0.000032;
						Masabc=0.25;
						Ddoblamientoc=6*diametrobc;
						break;
					case 3:
						diametrobc=9.5/1000;
						Areabc=0.000071;
						Masabc=0.56;
						Ddoblamientoc=6*diametrobc;
						break;
					case 4:
						diametrobc=12.7/1000;
						Areabc=0.000129;
						Masabc=0.994;
						Ddoblamientoc=6*diametrobc;
						break;
					case 5:
						diametrobc=15.9/1000;
						Areabc=0.000199;
						Masabc=1.552;
						Ddoblamientoc=6*diametrobc;
						break;
					case 6:
						diametrobc=19.1/1000;
						Areabc=0.000284;
						Masabc=2.235;
						Ddoblamientoc=6*diametrobc;
						break;
					case 7:
						diametrobc=22.2/1000;
						Areabc=0.000387;
						Masabc=3.042;
						Ddoblamientoc=6*diametrobc;
						break;
					case 8:
						diametrobc=25.4/1000;
						Areabc=0.000510;
						Masabc=3.973;
						Ddoblamientoc=6*diametrobc;
						break;
					case 9:
						diametrobc=28.7/1000;
						Areabc=0.000645;
						Masabc=5.06;
						Ddoblamientoc=8*diametrobc;
						break;
					case 10:
						diametrobc=32.5/1000;
						Areabc=0.000819;
						Masabc=6.404;
						Ddoblamientoc=8*diametrobc;
						break;
					case 11:
						diametrobc=35.8/1000;
						Areabc=0.001006;
						Masabc=7.907;
						Ddoblamientoc=8*diametrobc;
						break;
					default:
						diametrobc=12.7/1000;
						Areabc=0.000129;
						Masabc=0.994;
						Ddoblamientoc=6*diametrobc;
						opcionc=4;
						Console.WriteLine("No seleccionó una opción válida, se tomará la #4 :P");
						Console.ReadKey();
						break;	
				}
			
				constructor.AppendLine("Refuerzo para columna: N"+opcionc+"");
				constructor.AppendLine("Diametro: "+diametrobc+" m");
				constructor.AppendLine("Area: "+Areabc+" m2");
				constructor.AppendLine("Masa: "+Masabc+" Kg/mL");
				constructor.AppendLine("Diametro de Doblamiento: "+Ddoblamientoc+" m\n");
			
			//mostramos las propiedades segun NSR10 de la barra escogida para la zapata:
			Console.WriteLine("Refuerzo para zapata: N°{0}",opcionz);
			Console.WriteLine("Diámetro: {0} m",diametrobz);
			Console.WriteLine("Área: {0} m2",Areabz);
			Console.WriteLine("Masa: {0} Kg/mL",Masabz);
			Console.WriteLine("Diámetro de Doblamiento: {0} m \n",Ddoblamientoz);
			//mostramos las propiedades segun NSR10 de la barra escogida para la columna:
			Console.WriteLine("Refuerzo para columna: N°{0}",opcionc);
			Console.WriteLine("Diámetro: {0} m",diametrobc);
			Console.WriteLine("Área: {0} m2",Areabc);
			Console.WriteLine("Masa: {0} Kg/mL",Masabc);
			Console.WriteLine("Diámetro de Doblamiento: {0} m\n",Ddoblamientoc);
			
			//pedimos las propiedades del suelo de excavacion
			Console.WriteLine("Ingrese la profundidad de desplante en metros:");
				double.TryParse(Console.ReadLine(),out df);
			Console.WriteLine("Ingrese el Peso Especifico del suelo de relleno en KN/m3:");
				double.TryParse(Console.ReadLine(),out rsuelo);
			Console.WriteLine("Ingrese el Esfuerzo Admisible del suelo en KN/m2:");
				double.TryParse(Console.ReadLine(),out EsfuerzoAdmisible);
			
				constructor.AppendLine("Propiedades del suelo de fundacion:\n");
				constructor.AppendLine("Profundidad de desplante df = "+df+" m");
				constructor.AppendLine("Peso especifico del material de relleno: rs = "+rsuelo+" KN/m3");
				constructor.AppendLine("Esfuerzo admisible del suelo: Eadm = "+EsfuerzoAdmisible+" KN/m2\n");
			
			//calculamos el espesor del cimiento
				r=checked(rprima+diametrobz);
				Hmin=checked(d1+r);
				ldc=maximo2(0.24*fy*diametrobc*FC/Math.Pow(fc, 0.5),0.043*diametrobc*fy*FC);
				d2=checked(ldc+Ddoblamientoc/2+diametrobc+diametrobz);
				d3=checked(Math.Pow(P1u/(T*Math.Pow(fc, 0.5)), 0.5));
				d=maximo2(maximo2(d1,d2),d3);
				H=Math.Round((d+r),2,MidpointRounding.AwayFromZero);
				
			
			
			
			Console.WriteLine("\t\t\t Calculo del Espesor y Area del cimiento. \n");
			Console.WriteLine("El recubrimiento: \nr = r' + dbz");
			Console.WriteLine("r = {0}+{1}",rprima,diametrobz);
			Console.WriteLine("r = "+r+"m \n");
		
				constructor.AppendLine("Calculo del Espesor y Area del cimiento.\n");
				constructor.AppendLine("El recubrimiento: \nr = r' + dbz");
				constructor.AppendLine("r = "+rprima+" + "+diametrobz+"");
				constructor.AppendLine("r = "+r+" m.\n");
				
		
			Console.WriteLine("El espesor H minimo por norma es: \nHmin = d1 + r");
			Console.WriteLine("Hmin = {0} + {1}",d1,r);
			Console.WriteLine("Hmin = {0}m \n",Hmin);
			Console.WriteLine("La longitud de desarrollo a compresion \nldc = max(0.24*fy*dbc*FC/raiz(f'c) ó 0.043*dbc*fy*FC)");
				
				constructor.AppendLine("El espesor H minimo por norma es: \nHmin = d1 + r");
				constructor.AppendLine("Hmin = "+d1+" + "+r+"");
				constructor.AppendLine("Hmin = "+Hmin+" m\n");
				
				ldc1=0.24*fy*diametrobc*FC/Math.Pow(fc,0.5);
				ldc2=0.043*diametrobc*fy*FC;
			Console.WriteLine("ldc = max(0.24*{0}*{1}*{2}/raiz({3}) ó 0.043*{4}*{5}*{6})",fy,diametrobc,FC,fc,diametrobc,fy,FC);
			Console.WriteLine("ldc = max({0} ; {1})",ldc1, ldc2);
			Console.WriteLine("ldc = {0} m \n",ldc);

				constructor.AppendLine("ldc = max( ldc1 o ldc2 )");
				constructor.AppendLine("ldc = max( 0.24*"+fy+"*"+diametrobc+"*"+FC+"/raiz("+fc+") o 0.043*"+diametrobc+"*"+fy+"*"+FC+")");
				constructor.AppendLine("ldc = max( "+ldc1+" o "+ldc2+" )");
				constructor.AppendLine("ldc = "+ldc+" m\n");
				

			Console.WriteLine("El diámetro de doblamiento para la barra de columna es: \nDd = {0}m \n",Ddoblamientoc);
			Console.WriteLine("El peralte: \nd2 = ldc + Ddoblamiento/2 + dbc + dbz");
			Console.WriteLine("d2 = {0} + {1}/2 + {2} + {3}",ldc,Ddoblamientoc,diametrobc,diametrobz);
			Console.WriteLine("d2 = {0}m \n",d2);
			
				constructor.AppendLine("El peralte: \nd2 = ldc + Ddoblamientoc/2 + dbc + dbz");
				constructor.AppendLine("d2 = "+ldc+" + "+Ddoblamientoc+"/2 + "+diametrobc+" + "+diametrobz+"");
				constructor.AppendLine("d2 = "+d2+" m\n");
			
			Console.WriteLine("El peralte: \nd3 = raiz(P1u / (T*raiz(f'c)))");
			Console.WriteLine("d3 = raiz({0} / ({1}*raiz({2})))",P1u,T,fc);
			Console.WriteLine("d3 = {0}m \n",d3);
			
				constructor.AppendLine("El peralte: \nd3 = raiz(P1u / (T*raiz(f'c)))");
				constructor.AppendLine("d3 = raiz("+P1u+" / ("+T+"*raiz("+fc+")))");
				constructor.AppendLine("d3 = "+d3+" m\n");
				
			
			Console.WriteLine("El peralte minimo es: \nd = max(d1,d2,d3)");
			Console.WriteLine("d = max({0} ; {1} ; {2})",d1,d2,d3);
			Console.WriteLine("d = "+d+"m \n");
			Console.WriteLine("El Espesor minimo es: \nH = max(Hmin ; d + r)");
			Console.WriteLine("H = max({0} ; {1})",Hmin,H);
			Console.WriteLine("H = {0}m \n",H);
			
				constructor.AppendLine("El peralte minimo es: \nd = max(d1,d2,d3)");
				constructor.AppendLine("d = max("+d1+" ; "+d2+" ; "+d3+")");
				constructor.AppendLine("d = "+d+"m \n");
				
				constructor.AppendLine("El Espesor minimo es: \nH = max(Hmin ; d + r)");
				constructor.AppendLine("H = max("+Hmin+" ; "+d+" + "+r+")");
				constructor.AppendLine("H = max("+Hmin+" ; "+H+")");
				constructor.AppendLine("H = "+H+" m\n");
				
			
			calculos_H:
			
			d = H - r;
			Console.WriteLine("Recalculamos: d = H-r");
			Console.WriteLine("d = {0}-{1}",H,r);
			Console.WriteLine("d = {0} m\n",d);
			
				constructor.AppendLine("Recalculamos: d = H-r");
				constructor.AppendLine("d = "+H+" - "+r+"");
				constructor.AppendLine("d = "+d+" m.\n");
				
			//calculamos el esfuerzo neto con el H encontrado y el area requerida:
				Sobrecarga=checked(rsuelo*(df-H)+rconcreto*H);
				EsfuerzoNeto=checked(EsfuerzoAdmisible-Sobrecarga);
				areareq=checked(P1/EsfuerzoNeto);
			
			Console.WriteLine("La sobrecarga es: \nqw = rsuelo*(df - H) + rconcreto*H");
			Console.WriteLine("qw = {0}*({1} - {2}) + {3}*{4}",rsuelo,df,H,rconcreto,H);
			Console.WriteLine("qw = {0}KN/m2 \n",Sobrecarga);
			Console.WriteLine("El Esfuerzo neto es: \nEsfNeto = EsfuerzoAdmisible - Sobrecarga");
			Console.WriteLine("EsfNeto = {0} - {1}",EsfuerzoAdmisible,Sobrecarga);
			Console.WriteLine("EsfNeto = {0}KN/m2 \n",EsfuerzoNeto);
			Console.WriteLine("El Area minima de la zapata es: \nA = P1 / EsfuerzoNeto");
			Console.WriteLine("A = {0} / {1}",P1,EsfuerzoNeto);
			Console.WriteLine("A = {0}m2 \n",areareq);
			
				constructor.AppendLine("La sobrecarga es: \nqw = rsuelo*(df - H) + rconcreto*H");
				constructor.AppendLine("qw = "+rsuelo+"*("+df+" - "+H+") + "+rconcreto+"*"+H+"");
				constructor.AppendLine("qw = "+Sobrecarga+" KN/m2.\n");
				constructor.AppendLine("El Esfuerzo neto es: \nEsfNeto = EsfuerzoAdmisible - Sobrecarga");
				constructor.AppendLine("EsfNeto = "+EsfuerzoAdmisible+" - "+Sobrecarga+"");
				constructor.AppendLine("EsfNeto = "+EsfuerzoNeto+" KN/m2.\n");
				
				constructor.AppendLine("El Area minima de la zapata es: \nA = P1 / EsfuerzoNeto");
				constructor.AppendLine("A = "+P1+" / "+EsfuerzoNeto+"");
				constructor.AppendLine("A = "+areareq+" m2\n");
							
			//calculamos las dimensiones
			Console.WriteLine("Elija [1] si desea calcular la zapata cuadrada ó\n"+
			"Elija [2] si desea calcular la zapata rectangular \n");
			int.TryParse(Console.ReadLine(), out opciontipo);
			switch (opciontipo)
				{
					case 1:
						ZapataCuadrada();
					break;
					case 2:
						ZapataRectangular();
					break;
					default:
						Console.WriteLine("Se escogerá zapata aislada cuadrada \n");
						ZapataCuadrada();
					break;
				}
			
			
			//calculos de rigidez
			//calculamos los voladizos:
				V1=(L-a)/2;
				V2=(B-b)/2;
			//calculamos las relaciones:
				rel1=V1/H;
				rel2=V2/H;
			Console.WriteLine("\t\t\t Chequeo de Rigidez. \n");
				
			Console.WriteLine("El voladizo 'V1' es la distancia paralela a 'L'");
			Console.WriteLine("V1 = (L - a)/2");
			Console.WriteLine("V1 = ({0} - {1})/2",L,a);
			Console.WriteLine("V1 = {0}m \n",V1);
			Console.WriteLine("Relación entre el voladizo y el espesor H: \nrelacion = V1/H");
			Console.WriteLine("V1/H = {0} / {1}",V1,H);
			Console.WriteLine("V1/H = {0}m \n",rel1);
				
				constructor.AppendLine("Chequeos de Rigidez:\n\n");
				constructor.AppendLine("El voladizo 'V1' es la distancia paralela a 'L'");
				constructor.AppendLine("V1 = (L - a)/2");
				constructor.AppendLine("V1 = ("+L+" - "+a+")/2");
				constructor.AppendLine("V1 = "+V1+" m\n");
				constructor.AppendLine("Relación entre el voladizo y el espesor H: \nrelacion = V1/H");
				constructor.AppendLine("V1/H = "+V1+" / "+H+"");
				constructor.AppendLine("V1/H = "+rel1+"\n");
				
				
			Console.WriteLine("El voladizo 'V2' es la distancia paralela a 'B'");
			Console.WriteLine("V2 = (B - b)/2");
			Console.WriteLine("V2 = ({0} - {1})/2",B,b);
			Console.WriteLine("V2 = {0}m \n",V2);
			Console.WriteLine("Relación entre el voladizo y el espesor H: \nrelacion = V2/H");
			Console.WriteLine("V2/H = {0} / {1}",V2,H);
			Console.WriteLine("V2/H = {0}m \n",rel2);
			
			
				constructor.AppendLine("El voladizo 'V2' es la distancia paralela a 'B'");
				constructor.AppendLine("V2 = (B - b)/2");
				constructor.AppendLine("V2 = ("+B+" - "+b+")/2");
				constructor.AppendLine("V2 = "+V2+" m\n");
				constructor.AppendLine("Relación entre el voladizo y el espesor H: \nrelacion = V2/H");
				constructor.AppendLine("V2/H = "+V2+" / "+H+"");
				constructor.AppendLine("V2/H = "+rel2+"\n");
				
			
			//los condicionales para rediseñar:
			voladizomax=maximo2(V1,V2);
			relmax=voladizomax/H;
				if (relmax>3)
					{
						Console.WriteLine("!!Cuidado no cumple Rigidez!! Se Volverá a calcular el espesor H");
						Console.WriteLine("Tambien se calculará el esfuerzo neto, el area y las dimensiones\n");
							H=checked(Math.Round(voladizomax/2.8,2,MidpointRounding.AwayFromZero));
						Console.WriteLine("H para rigidez = voladizomaximo/2.8");
						Console.WriteLine("H = {0}/2.8",voladizomax);
						Console.WriteLine("H = {0}m \n",H);
						
							constructor.AppendLine("No se cumple la Rigidez!! Se Volverá a calcular el espesor H");
							constructor.AppendLine("Tambien se calculará el esfuerzo neto, el area y las dimensiones\n");
							constructor.AppendLine("H para rigidez = voladizomaximo/2.8");
							constructor.AppendLine("H = "+voladizomax+" / 2.8");
							constructor.AppendLine("H = "+H+" m\n");
							
						
							goto calculos_H;
					}
			
			Console.WriteLine("Cimiento rigido en condicion aceptable\n");
				constructor.AppendLine("Cimiento rigido en condicion aceptable\n");
			//calculamos la presion de contacto mayorada
			Console.WriteLine("\t\t\t Presion de contacto neta mayorada. \n");
				constructor.AppendLine("Presion de contacto neta mayorada:\n");
				Esfuerzo1u=checked(P1u/areafinal);
			Console.WriteLine("El Esfuerzo (1u) = P1u/areafinal");
			Console.WriteLine("Esf(1u) = {0}/{1}",P1u,areafinal);
			Console.WriteLine("Esf(1u) = {0}KN/m2 \n",Esfuerzo1u);
				constructor.AppendLine("El Esfuerzo (1u) = P1u/areafinal");
				constructor.AppendLine("Esf(1u) = "+P1u+" / "+areafinal+"");
				constructor.AppendLine("Esf(1u) = "+Esfuerzo1u+" KN/m2\n");
				constructor.AppendLine("");
			//accion como viga
			Console.WriteLine("\t\t\t Acción como viga. \n");
				constructor.AppendLine("Accion como viga:\n");
			Console.WriteLine("\t\t\t Diseño y revision lado paralelo a 'L' con el 'V1': \n");
				constructor.AppendLine("Diseno y revision lado paralelo a 'L' con el 'V1':");
				
				
				AtL=checked(B*(V1-d));
				V1u=checked(Esfuerzo1u*AtL);
				fiVc=checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*B*d*1000);
			
			Console.WriteLine("El Area tributaria es: \nAtrib = B*(V1 - d)");
			Console.WriteLine("Atrib = {0}*({1} - {2})",B,V1,d);
			Console.WriteLine("Atrib = {0}m2 \n",AtL);
			Console.WriteLine("El Cortante actuante es: \nV1u = Esfuerzo1u*Atributaria");
			Console.WriteLine("V1u = {0}*{1}",Esfuerzo1u,AtL);
			Console.WriteLine("V1u = {0}KN \n",V1u);
			Console.WriteLine("El Cortante resistente es: \nfiVc = ficortante*0.17*landa*raiz(fc)*B*d*1000");
			Console.WriteLine("fiVc = {0}*0.17*{1}*raiz({2})*{3}*{4}*1000",ficortante,landa,fc,B,d);
			Console.WriteLine("fiVc = {0}KN \n",fiVc);
			
				constructor.AppendLine("El Area tributaria es: \nAtrib = B*(V1 - d)");
				constructor.AppendLine("Atrib = "+B+"*("+V1+" - "+d+")");
				constructor.AppendLine("Atrib = "+AtL+" m2\n");
				constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u*Atributaria");
				constructor.AppendLine("V1u = "+Esfuerzo1u+" * "+AtL+"");
				constructor.AppendLine("V1u = "+V1u+" KN\n");
				constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante*0.17*landa*raiz(fc)*B*d*1000");
				constructor.AppendLine("fiVc = "+ficortante+"*0.17*"+landa+"*raiz("+fc+")*"+B+"*"+d+"*1000");
				constructor.AppendLine("fiVc = "+fiVc+" KN\n");
				
			
				if (fiVc>=V1u)
					{
						Console.WriteLine("fiVc > V1u ==>!Cumple!\n{0} > {1}",fiVc,V1u);
						Console.WriteLine("=>Con el peralte d={0}m el concreto resiste los esfuerzos cortantes \n",d);
							constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVc+" > "+V1u+"");
							constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto resiste los esfuerzos cortantes \n");
					}
						
				if (fiVc<V1u)
					{
						Console.WriteLine("fiVc < V1u ==>!No Cumple!\n{0} < {1}",fiVc,V1u);
						Console.WriteLine("!!El concreto no resiste las solicitaciones cortantes!!");
						Console.WriteLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
						"cortante actuante y resistente, despejando a d.\n");
						d = checked(Esfuerzo1u*V1 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + Esfuerzo1u));
						Console.WriteLine("'d' para accion como viga paralelo a 'L': \nd = Esfuerzo1u*V1 / ((ficortante*0.17*landa*raiz(fc)*1000) + Esfuerzo1u)");
						Console.WriteLine("d = {0}*{1} / (({2}*0.17*{3}*raiz({4})*1000) + {5})",Esfuerzo1u,V1,ficortante,landa,fc,Esfuerzo1u);
						Console.WriteLine("d = {0} m\n",d);
						H=checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
						Console.WriteLine("H para accion como viga paralelo a 'L': \nH = d + r");
						Console.WriteLine("H = {0}+{1}",d,r);
						Console.WriteLine("H = {0}m \n",H);
						
							constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVc+" < "+V1u+"");
							constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
							constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
						"cortante actuante y resistente, despejando a d.\n");
							constructor.AppendLine("'d' para accion como viga paralelo a 'L': \nd = Esfuerzo1u*V1 / ((ficortante*0.17*landa*raiz(fc)*1000) + Esfuerzo1u)");
							constructor.AppendLine("d = "+Esfuerzo1u+"*"+V1+" / (("+ficortante+"*0.17*"+landa+"*raiz("+fc+")*1000) + "+Esfuerzo1u+")");
							constructor.AppendLine("d = "+d+" m\n");
							constructor.AppendLine("H para accion como viga paralelo a 'L': \nH = d + r");
							constructor.AppendLine("H = "+d+" + "+r+"");
							constructor.AppendLine("H = "+H+" m\n");
							
						goto calculos_H;
					}
						
			//accion como viga en el otro sentido	
			Console.WriteLine("\t\t\t Diseño y revision lado paralelo a 'B' con el 'V2': \n");
				
					constructor.AppendLine("Diseno y revision lado paralelo a 'B' con el 'V2':");
					constructor.AppendLine("");
						
				AtB=checked(L*(V2-d));
				V2u=checked(Esfuerzo1u*AtB);
				fiVc2=checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*L*d*1000);
			Console.WriteLine("El Area tributaria es: \nAtrib = L*(V2 - d)");
			Console.WriteLine("Atrib = {0}*({1} - {2})",L,V2,d);
			Console.WriteLine("Atrib = {0}m2 \n",AtB);
			Console.WriteLine("El Cortante actuante es: \nV2u = Esfuerzo1u*Atributaria");
			Console.WriteLine("V2u = {0}*{1}",Esfuerzo1u,AtB);
			Console.WriteLine("V2u = {0}KN \n",V2u);
			Console.WriteLine("El Cortante resistente es: \nfiVc = ficortante*0.17*landa*raiz(fc)*L*d*1000");
			Console.WriteLine("fiVc = {0}*0.17*{1}*raiz({2})*{3}*{4}*1000",ficortante,landa,fc,L,d);
			Console.WriteLine("fiVc = {0}KN \n",fiVc2);
			
				constructor.AppendLine("El Area tributaria es: \nAtrib = L*(V2 - d)");
				constructor.AppendLine("Atrib = "+L+"*("+V2+" - "+d+")");
				constructor.AppendLine("Atrib = "+AtB+" m2\n");
				constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u*Atributaria");
				constructor.AppendLine("V1u = "+Esfuerzo1u+" * "+AtB+"");
				constructor.AppendLine("V1u = "+V2u+" KN\n");
				constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante*0.17*landa*raiz(fc)*L*d*1000");
				constructor.AppendLine("fiVc = "+ficortante+"*0.17*"+landa+"*raiz("+fc+")*"+L+"*"+d+"*1000");
				constructor.AppendLine("fiVc = "+fiVc2+" KN\n");
			
				if (fiVc2>=V2u)
					{
						Console.WriteLine("fiVc > V2u ==>!Cumple!\n==> {0} > {1}",fiVc2,V2u);
						Console.WriteLine("=>Con el peralte d={0}m el concreto resiste los esfuerzos cortantes \n",d);
							constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVc2+" > "+V2u+"");
							constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto resiste los esfuerzos cortantes \n");
					}
						
				if (fiVc2<V2u)
					{
						Console.WriteLine("fiVc < V2u ==>!No Cumple!\n==> {0} < {1}",fiVc2,V2u);
						Console.WriteLine("!!El concreto no resiste las solicitaciones cortantes!!");
						Console.WriteLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
						"cortante actuante y resistente, despejando a d.\n");
						d = checked(Esfuerzo1u*V2 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + Esfuerzo1u));
						Console.WriteLine("'d' para accion como viga paralelo a 'L': \nd = Esfuerzo1u*V2 / ((ficortante*0.17*landa*raiz(fc)*1000) + Esfuerzo1u)");
						Console.WriteLine("d = {0}*{1} / (({2}*0.17*{3}*raiz({4})*1000) + {5})",Esfuerzo1u,V2,ficortante,landa,fc,Esfuerzo1u);
						Console.WriteLine("d = {0} m\n",d);
						H=checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
						Console.WriteLine("H para accion como viga paralelo a 'B': \nH = d + r");
						Console.WriteLine("H = {0}+{1}",d,r);
						Console.WriteLine("H = {0}m \n",H);
						//REVISAR
							constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVc2+" < "+V2u+"");
							constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
							constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
						"cortante actuante y resistente, despejando a d.\n");
							constructor.AppendLine("'d' para accion como viga paralelo a 'L': \nd = Esfuerzo1u*V1 / ((ficortante*0.17*landa*raiz(fc)*1000) + Esfuerzo1u)");
							constructor.AppendLine("d = "+Esfuerzo1u+"*"+V2+" / (("+ficortante+"*0.17*"+landa+"*raiz("+fc+")*1000) + "+Esfuerzo1u+")");
							constructor.AppendLine("d = "+d+" m\n");
							constructor.AppendLine("H para accion como viga paralelo a 'L': \nH = d + r");
							constructor.AppendLine("H = "+d+" + "+r+"");
							constructor.AppendLine("H = "+H+" m\n");
						
						goto calculos_H;
					}	
			
			//accion punzonamiento
			Console.WriteLine("\t\t\t Acción Punzonamiento. \n");
			
			Console.WriteLine("\t\t Diseño y revision para punzonamiento: \n");
			//lo que resiste:
			//perimetro critico zapata aislada cargada concentricamente sin excentricidad
				bo = checked( 2*(a + b) + 4*d );
				betac = maximo2(a,b)/minimo2(a,b);
			//fiVc1p
				fiVc1p = ficortante*0.17*Math.Pow(fc,0.5)*(1 + 2/betac)*bo*d*1000;
			//fiVc2p
				fiVc2p = ficortante*0.17*Math.Pow(fc,0.5)*(1 + alfa*d/(2 * bo))*bo*d*1000;
			//fiVc3p
				fiVc3p = ficortante*0.33*Math.Pow(fc,0.5)*bo*d*1000;
			//fiVcp minimo:
				fiVcpmin = minimo2(fiVc3p,minimo2(fiVc1p,fiVc2p));
			//lo que actua:
			//area tributaria
				Atp = (a + d)*(b + d);
				V1up = P1u - Esfuerzo1u*Atp;
			
			Console.WriteLine("Calculo del perimetro critico: \nbo = 2*(a + b) + 4*d");
			Console.WriteLine("bo = 2*({0} + {1}) + 4*{2}",a,b,d);
			Console.WriteLine("bo = {0} m\n",bo);
			Console.WriteLine("Calculo del factor betac: \nbetac = Llargo Columna / Lcorto Columna");
			Console.WriteLine("betac = {0} / {1} ",maximo2(a,b),minimo2(a,b));
			Console.WriteLine("betac = {0} \n",betac);
			
			Console.WriteLine("Calculo del primer fiVc:");
			Console.WriteLine("fiVc1 = ficortante*0.17*raiz(fc)*(1 + 2/betac)*bo*d*1000");
			Console.WriteLine("fiVc1 = {0}*0.17*raiz({1})*(1 + 2/{2})*{3}*{4}*1000",ficortante,fc,betac,bo,d);
			Console.WriteLine("fiVc1 = {0} KN\n",fiVc1p);
			Console.WriteLine("Calculo del segundo fiVc:");
			Console.WriteLine("fiVc2 = ficortante*0.17*raiz(fc)*(1 + alfa*d/(2 * bo))*bo*d*1000");
			Console.WriteLine("fiVc2 = {0}*0.17*raiz({1})*(1 + {2}*{3}/(2 * {4}))*{5}*{6}*1000",ficortante,fc,alfa,d,bo,bo,d);
			Console.WriteLine("fiVc2 = {0} KN\n",fiVc2p);
			Console.WriteLine("Calculo del tercer fiVc:");
			Console.WriteLine("fiVc3 = ficortante*0.33*raiz(fc)*bo*d*1000");
			Console.WriteLine("fiVc3 = {0}*0.33*raiz({1})*{2}*{3}*1000",ficortante,fc,bo,d);
			Console.WriteLine("fiVc3 = {0} KN\n",fiVc3p);
			Console.WriteLine("Se escoge el menor de los tres fiVc: {0} KN \n",fiVcpmin);
			
			Console.WriteLine("Calculo del area tributaria: \nAtrib = (a + d)*(b + d)");
			Console.WriteLine("Atrib = ({0} + {1})*({2} + {3})",a,d,b,d);
			Console.WriteLine("Atrib = {0} m2\n",Atp);
			Console.WriteLine("Calculo del cortante actuante: \nV1u = P1u - Esfuerzo1u*Atp");
			Console.WriteLine("V1u = {0} - {1}*{2}",P1u,Esfuerzo1u,Atp);
			Console.WriteLine("V1u = {0} KN \n",V1up);
			
				if (fiVcpmin>=V1up)
					{
						Console.WriteLine("fiVc > V1u ==>!Cumple para punzonamiento!\n==> {0} > {1}",fiVcpmin,V1up);
						Console.WriteLine("=>Con el peralte d={0}m el concreto resiste los esfuerzos cortantes de punzonamiento \n",d);
					}
			
				if (fiVcpmin<V1up)
					{
						Console.WriteLine("fiVc < V1u ==>!No Cumple!\n==> {0} < {1}",fiVcpmin,V1up);
						Console.WriteLine("!!El concreto no resiste las solicitaciones cortantes!!");
						Console.WriteLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
						"cortante actuante y resistente, despejando a d.\n");
						//aqui meter un metodo para hallar las raices:
							c2 = ficortante*0.33*Math.Pow(fc, 0.5)*4000 + Esfuerzo1u;
							c1 = ficortante*0.33*Math.Pow(fc, 0.5)*2000*(a + b) + Esfuerzo1u*(a + b);
							c0 = Esfuerzo1u*a*b - P1u;
							d = checked(biseccion4(0, 1, 0, 0, c2, c1, c0));
						Console.WriteLine("'d' para punzonamiento se calcula con cualquier metodo para hallar raices de polinomios. "+
						"En este programa se emplea el metodo de la Biseccion, con un error permisible de: 0.0001");
						
						Console.WriteLine("d = {0} m\n",d);
						H=checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
						Console.WriteLine("H para punzonamiento: \nH = d + r");
						Console.WriteLine("H = {0}+{1}",d,r);
						Console.WriteLine("H = {0}m \n",H);
						goto calculos_H;
					}
			
			//Diseño a flexion
			Console.WriteLine("\t\t\t Diseño a Flexion. \n");
			//refuerzo paralelo a L:
			Console.WriteLine("\t\t Acero de refuerzo paralelo a 'L'. \n");
				AtLf = B*V1;
				brazoL = V1/2;
				MuL = Esfuerzo1u*AtLf*brazoL;
				kL = MuL / (B*d*d);
				mL = fy / (0.85*fc);
				cuantiadL = cuantiad(fc, fy, MuL, B, d);
			Console.WriteLine("Area tributaria: \nAt = B*V1");
			Console.WriteLine("At = {0}*{1}",B,V1);
			Console.WriteLine("At = {0} m2\n",AtLf);
			Console.WriteLine("Brazo de momento: \nx = V1/2");
			Console.WriteLine("x = {0}/2",V1);
			Console.WriteLine("x = {0} m\n",brazoL);
			Console.WriteLine("Momento actuante: \nMu = Esfuerzo1u*At*brazo");
			Console.WriteLine("Mu = {0}*{1}*{2}",Esfuerzo1u,AtLf,brazoL);
			Console.WriteLine("Mu = {0} m2\n",MuL);
			Console.WriteLine("Factor k: \nk = MuL / (B*d*d)");
			Console.WriteLine("k = {0} / ({1}*{2}*{3})",MuL,B,d,d);
			Console.WriteLine("k = {0}\n",kL);
			Console.WriteLine("Factor m: \nm = fy / (0.85*fc)");
			Console.WriteLine("m = {0} / (0.85*{1})",fy,fc);
			Console.WriteLine("m = {0}\n",mL);
			
			Console.WriteLine("Cuantia requerida: \npr = (1 - raiz(1 - 2*m*k/(0.9*fy*1000)))/m");
			Console.WriteLine("pr = (1 - raiz(1 - 2*{0}*{1}/(0.9*{2}*1000)))/{3}",mL,kL,fy,mL);
			Console.WriteLine("pr = {0}\n",cuantiadL);
			
				AsrL = cuantiadL*B*d;
				AstL = 0.0018*B*H;
				
			Console.WriteLine("El area minima requerida es: \nAsr = pr*B*d");
			Console.WriteLine("Asr = {0}*{1}*{2}",cuantiadL,B,d);
			Console.WriteLine("Asr = {0} m2 \n",AsrL);
				
			Console.WriteLine("El area minima por retraccion y temperatura es: \nAst = 0.0018*B*H");
			Console.WriteLine("Ast = 0.0018*{0}*{1}",B,H);
			Console.WriteLine("Ast = {0} m2 \n",AstL);
			
				AssL = maximo2(AsrL,AstL);
			Console.WriteLine("Area de acero a suministrar: \nAss = max[ AsrL ; AstL ]");
			Console.WriteLine("Ass = max[ {0} ; {1} ]",AsrL,AstL);
			Console.WriteLine("Ass = {0} m2 \n",AssL);
			
				NbL = checked(Math.Ceiling(AssL/Areabz));
				
			Console.WriteLine("El numero de barras requeridas es: \nNb = Ass/Areabz");
			Console.WriteLine("Nb = {0}/{1}",AssL,Areabz);
			Console.WriteLine("Nb = {0} \n",NbL);
			
				SccL = checked((B - 2*rprima - diametrobz)/(NbL - 1));
				
			Console.WriteLine("Separacion centro a centro entre las barras: \nScc = (B - 2*rprima - diametrobz)/(NbB - 1)");
			Console.WriteLine("Scc = ({0} - 2*{1} - {2})/({3} - 1)",B,rprima, diametrobz,NbL);
			Console.WriteLine("Scc = {0} m \n",SccL);
			
				SlbL = checked(SccL - diametrobz);
				
			Console.WriteLine("Separacion libre entre las barras: \nSlb = Scc - diametrobz");
			Console.WriteLine("Slb = {0} - {1}",SccL,diametrobz);
			Console.WriteLine("Slb = {0} m \n",SlbL);
			
				Sminb = maximo2(diametrobz,maximo2(0.025, 1.33*TM));
			
			Console.WriteLine("Separacion minima entre las barras: \nSmin = max(diametrobz ; 0.025 ; 1.33*TM)");
			Console.WriteLine("Smin = max({0} ; 0.025 ; 1.33*{1})",diametrobz,TM);
			Console.WriteLine("Smin = {0} m\n",Sminb);
			
				if (SlbL<Sminb)
						{
							Console.WriteLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a L, EMPIECE DE "+
							"NUEVO AUMENTANDO EL DIAMETRO DE LA BARRA");
						}
						
						
				if (SlbL>0.45)
						{
							Console.WriteLine("OJO!!! Separación da mas que la maxima, entonces se"+
							" toma la maxima que es: 0.45m");
							//aqui en la zapata aislada podria ir que pregunte si desea cambiar 
							// el diametro o tomar la sep=0,45
							SlbL = 0.45;
							//Recalculamos el numero de barras con sep=0.45m
							NbL = checked(Math.Ceiling(((B - 2*rprima - diametrobz)/SlbL) + 1));
							Console.WriteLine("Recalculamos el numero de barras: \nNb = ((B - 2*rprima - diametrobz)/SlbL) + 1");
							Console.WriteLine("Nb = (({0} - 2*{1} - {2})/{3}) + 1",B,rprima,diametrobz,SlbL);
							Console.WriteLine("Nb = {0} \n",NbL);
							
						}
			
				AssrL = checked(NbL*Areabz);
			Console.WriteLine("El area suministrada es: \nAssr = Nb*Areabz");
			Console.WriteLine("Assr = {0}*{1}",NbL,Areabz);
			Console.WriteLine("Assr = {0} m2 \n",AssrL);
			
			if (AsrL<AstL)
				{
					cuantiaLsum = checked(AssrL/(B*H));
				Console.WriteLine("La cuantia suministrada es: \npsum = Assr/(B*H)");
				Console.WriteLine("psum = {0}/({1}*{2})",AssrL,B,H);
				Console.WriteLine("psum = {0} \n",cuantiaLsum);
				}
				
				
			if (AsrL>AstL)
				{
					cuantiaLsum = checked(AssrL/(B*d));
				Console.WriteLine("La cuantia suministrada es: \npsum = Assr/(B*d)");
				Console.WriteLine("psum = {0}/({1}*{2})",AssrL,B,d);
				Console.WriteLine("psum = {0} \n",cuantiaLsum);
				}
			
			
			Console.WriteLine("\t\t Longitud de Anclaje: \n");
				lddL = V1 - rprima;
			Console.WriteLine("Longitud disponible: \nldd = V1 - rprima");
			Console.WriteLine("ldd = {0} - {1}",V1,rprima);
			Console.WriteLine("ldd = {0} m\n",lddL);
			
			
			Console.WriteLine("Longitud de desarrollo requerida por la barra a traccion sin gancho:");
				cc1 = SccL/2;
				cc2 = r + diametrobz/2;
				cc = minimo2(cc1,cc2);
				relccdbz = cc / diametrobz;
			Console.WriteLine("Se tiene: c = min ( cc1 ; cc2 )");
			Console.WriteLine("c = min ( Sccl/2 ; r + diametrobz/2 ) = min ( {0}/2 ; {1} + {2}/2 )",SccL,r,diametrobz);
			Console.WriteLine("c = min ({0} ; {1}) = {2}",cc1,cc2,cc);
			Console.WriteLine("Se tiene la relacion: c/dbz = {0}/{1} = {2}\n",cc,diametrobz,relccdbz);
				if (relccdbz > 2.5)
					{
						relccdbz = 2.5;
						Console.WriteLine("Se toma la relacion: c/dbz = {0}\n",relccdbz);
					}
			
				factorAsL = AssL / AssrL;
				ldrL = (fy*diametrobz / (1.1*Math.Pow(fc,0.5)*relccdbz) )*factorAsL;
			Console.WriteLine("Calculamos un factor de reduccion por area de acero suministrada:");
			Console.WriteLine("factorAs = Ass / Assr = {0} / {1} = {2}\n",AssL,AssrL,factorAsL);
			
			Console.WriteLine("Calculamos la longitud requerida por la barra: \nldr = (fy*diametrobz / (1.1*raiz(fc)*relccdbz) )*factorAs");
			Console.WriteLine("ldr = (fy*diametrobz / (1.1*raiz(fc)*relccdbz) )*factorAs",fy,diametrobz,fc,relccdbz,factorAsL);
			Console.WriteLine("ldr = {0} m\n",ldrL);
			if (ldrL<0.3)
				{
					ldrL = 0.3;
					Console.WriteLine("Se toma ldr = {0} m, que es la minima por norma",ldrL);
				}
			
			if (ldrL>lddL)
				{
					Console.WriteLine(" !!!Ojo!!! no hay espacio suficiente para desarrollar las barras"+
					" emplee ganchos o disminuya el diametro de la barra.\n");
				}
				
			//longitud de corte de las barras redondeadas al cm
				lcorteL = Math.Round(L - 2*rprima,2,MidpointRounding.AwayFromZero);
			Console.WriteLine("La longitud de corte de las barras es: LC = L-2*rprima");
			Console.WriteLine("LC = {0}-2*{1}",L,rprima);
			Console.WriteLine("LC = {0} m \n",lcorteL);
			
			//diseño a flexion refuerzo paralelo a B:
			
			Console.WriteLine("\t\t Acero de refuerzo paralelo a 'B'. \n");
				AtBf = L*V2;
				brazoB = V2/2;
				MuB = Esfuerzo1u*AtBf*brazoB;
				kB = MuB / (L*d*d);
				mB = fy / (0.85*fc);
				cuantiadB = cuantiad(fc, fy, MuB, L, d);
			Console.WriteLine("Area tributaria: \nAt = L*V2");
			Console.WriteLine("At = {0}*{1}",L,V2);
			Console.WriteLine("At = {0} m2\n",AtBf);
			Console.WriteLine("Brazo de momento: \nx = V2/2");
			Console.WriteLine("x = {0}/2",V2);
			Console.WriteLine("x = {0} m\n",brazoB);
			Console.WriteLine("Momento actuante: \nMu = Esfuerzo1u*At*brazo");
			Console.WriteLine("Mu = {0}*{1}*{2}",Esfuerzo1u,AtBf,brazoB);
			Console.WriteLine("Mu = {0} m2\n",MuB);
			Console.WriteLine("Factor k: \nk = Mu / (L*d*d)");
			Console.WriteLine("k = {0} / ({1}*{2}*{3})",MuB,L,d,d);
			Console.WriteLine("k = {0}\n",kB);
			Console.WriteLine("Factor m: \nm = fy / (0.85*fc)");
			Console.WriteLine("m = {0} / (0.85*{1})",fy,fc);
			Console.WriteLine("m = {0}\n",mB);
			
			Console.WriteLine("Cuantia requerida: \npr = (1 - raiz(1 - 2*m*k/(0.9*fy*1000)))/m");
			Console.WriteLine("pr = (1 - raiz(1 - 2*{0}*{1}/(0.9*{2}*1000)))/{3}",mB,kB,fy,mB);
			Console.WriteLine("pr = {0}\n",cuantiadB);
			
				AsrB = cuantiadB*L*d;
				AstB = 0.0018*L*H;
				
			Console.WriteLine("El area minima requerida es: \nAsr = pr*L*d");
			Console.WriteLine("Asr = {0}*{1}*{2}",cuantiadB,L,d);
			Console.WriteLine("Asr = {0} m2 \n",AsrB);
				
			Console.WriteLine("El area minima por retraccion y temperatura es: \nAst = 0.0018*L*H");
			Console.WriteLine("Ast = 0.0018*{0}*{1}",L,H);
			Console.WriteLine("Ast = {0} m2 \n",AstB);
			
				AssB = maximo2(AsrB,AstB);
			Console.WriteLine("Area de acero a suministrar: \nAss = max[ AsrB ; Ast ]");
			Console.WriteLine("Ass = max[ {0} ; {1} ]",AsrB,AstB);
			Console.WriteLine("Ass = {0} m2 \n",AssB);
			
				NbB = checked(Math.Ceiling(AssB/Areabz));
				
			Console.WriteLine("El numero de barras requeridas es: \nNb = Ass/Areabz");
			Console.WriteLine("Nb = {0}/{1}",AssB,Areabz);
			Console.WriteLine("Nb = {0} \n",NbB);
			
				SccB = checked((L - 2*rprima - diametrobz)/(NbB - 1));
				
			Console.WriteLine("Separacion centro a centro entre las barras: \nScc = (L - 2*rprima - diametrobz)/(NbB - 1)");
			Console.WriteLine("Scc = ({0} - 2*{1} - {2})/({3} - 1)",L,rprima, diametrobz,NbB);
			Console.WriteLine("Scc = {0} m \n",SccB);
			
				SlbB = checked(SccB - diametrobz);
				
			Console.WriteLine("Separacion libre entre las barras: \nSlb = Scc - diametrobz");
			Console.WriteLine("Slb = {0} - {1}",SccB,diametrobz);
			Console.WriteLine("Slb = {0} m \n",SlbB);
			
				Sminb = maximo2(diametrobz,maximo2(0.025, 1.33*TM));
			
			Console.WriteLine("Separacion minima entre las barras: \nSmin = max(diametrobz ; 0.025 ; 1.33*TM)");
			Console.WriteLine("Smin = max({0} ; 0.025 ; 1.33*{1})",diametrobz,TM);
			Console.WriteLine("Smin = {0} m\n",Sminb);
			
				if (SlbB<Sminb)
						{
							Console.WriteLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a B, EMPIECE DE "+
							"NUEVO AUMENTANDO EL DIAMETRO DE LA BARRA");
						}
						
						
				if (SlbB>0.45)
						{
							Console.WriteLine("OJO!!! Separación da mas que la maxima, entonces se"+
							" toma la maxima que es: 0.45m");
							//aqui en la zapata aislada podria ir que pregunte si desea cambiar 
							// el diametro o tomar la sep=0,45
							SlbB = 0.45;
							//Recalculamos el numero de barras con sep=0.45m
							NbB = checked(Math.Ceiling(((L - 2*rprima - diametrobz)/SlbB) + 1));
							Console.WriteLine("Recalculamos el numero de barras: \nNb = ((L - 2*rprima - diametrobz)/SlbB) + 1");
							Console.WriteLine("Nb = (({0} - 2*{1} - {2})/{3}) + 1",L,rprima,diametrobz,SlbB);
							Console.WriteLine("Nb = {0} \n",NbB);
							
						}
			
				AssrB = checked(NbB*Areabz);
			Console.WriteLine("El area suministrada es: \nAssr = Nb*Areabz");
			Console.WriteLine("Assr = {0}*{1}",NbB,Areabz);
			Console.WriteLine("Assr = {0} m2 \n",AssrB);
			
			if (AsrB<AstB)
				{
					cuantiaBsum = checked(AssrB/(L*H));
				Console.WriteLine("La cuantia suministrada es: \npsum = Assr/(L*H)");
				Console.WriteLine("psum = {0}/({1}*{2})",AssrB,L,H);
				Console.WriteLine("psum = {0} \n",cuantiaBsum);
				}
				
				
			if (AsrB>AstB)
				{
					cuantiaBsum = checked(AssrB/(L*d));
				Console.WriteLine("La cuantia suministrada es: \npsum = Assr/(L*d)");
				Console.WriteLine("psum = {0}/({1}*{2})",AssrB,L,d);
				Console.WriteLine("psum = {0} \n",cuantiaBsum);
				}
			
			
			Console.WriteLine("\t\t Longitud de Anclaje: \n");
				lddB = V2 - rprima;
			Console.WriteLine("Longitud disponible: \nldd = V2 - rprima");
			Console.WriteLine("ldd = {0} - {1}",V2,rprima);
			Console.WriteLine("ldd = {0} m\n",lddB);
			
			Console.WriteLine("Longitud de desarrollo requerida por la barra a traccion sin gancho:");
				cc1 = SccB/2;
				cc2 = r + diametrobz/2;
				cc = minimo2(cc1,cc2);
				relccdbz = cc / diametrobz;
			Console.WriteLine("Se tiene: c = min ( cc1 ; cc2 )");
			Console.WriteLine("c = min ( SccB/2 ; r + diametrobz/2 ) = min ( {0}/2 ; {1} + {2}/2 )",SccB,r,diametrobz);
			Console.WriteLine("c = min ({0} ; {1}) = {2}",cc1,cc2,cc);
			Console.WriteLine("Se tiene la relacion: c/dbz = {0}/{1} = {2}\n",cc,diametrobz,relccdbz);
				if (relccdbz > 2.5)
					{
						relccdbz = 2.5;
						Console.WriteLine("Se toma la relacion: c/dbz = {0}\n",relccdbz);
					}
			
				factorAsB = AssB / AssrB;
				ldrB = (fy*diametrobz / (1.1*Math.Pow(fc,0.5)*relccdbz) )*factorAsB;
			Console.WriteLine("Calculamos un factor de reduccion por area de acero suministrada:");
			Console.WriteLine("factorAs = Ass / Assr = {0} / {1} = {2}\n",AssB,AssrB,factorAsB);
			
			Console.WriteLine("Calculamos la longitud requerida por la barra: \nldr = (fy*diametrobz / (1.1*raiz(fc)*relccdbz) )*factorAs");
			Console.WriteLine("ldr = (fy*diametrobz / (1.1*raiz(fc)*relccdbz) )*factorAs",fy,diametrobz,fc,relccdbz,factorAsB);
			Console.WriteLine("ldr = {0} m\n",ldrB);
			if (ldrB<0.3)
				{
					ldrB = 0.3;
					Console.WriteLine("Se toma ldr = {0} m, que es la minima por norma",ldrB);
				}
			
			if (ldrB>lddB)
				{
					Console.WriteLine(" !!!Ojo!!! no hay espacio suficiente para desarrollar las barras"+
					" emplee ganchos o disminuya el diametro de la barra.\n");
				}
				
			//longitud de corte de las barras redondeadas al cm
				lcorteB = Math.Round(B - 2*rprima,2,MidpointRounding.AwayFromZero);
			Console.WriteLine("La longitud de corte de las barras es: LC = B - 2*rprima");
			Console.WriteLine("LC = {0} - 2*{1}",B,rprima);
			Console.WriteLine("LC = {0} m \n",lcorteB);
			
			//calculo de las cantidades de obra
			//Cantidades de obra:
					
					vconcreto=H*B*L;
				Console.WriteLine("El volumen de concreto: VC = H*B*L");
				Console.WriteLine("VC = {0}*{1}*{2}",H,B,L);
				Console.WriteLine("VC = {0} m3 \n",vconcreto);
					vexcavacion=df*B*L;
				Console.WriteLine("El volumen de excavación: VE = Df*B*L");
				Console.WriteLine("VE = {0}*{1}*{2}",df,B,L);
				Console.WriteLine("VE = {0} m3 \n",vexcavacion);
			
					acerototal = NbB*Masabz*lcorteB + NbL*Masabz*lcorteL;
				Console.WriteLine("El acero total en Kg es: AR = NbB*densb*lcorteB + NbL*densb*lcorteL");
				Console.WriteLine("AR = {0}*{1}*{2} + {3}*{4}*{5}",NbB,Masabz,lcorteB,NbL,Masabz,lcorteL);
				Console.WriteLine("AR = {0} Kg \n",acerototal);
				//presupuesto
					
					costo = Math.Round(presupuesto(vconcreto, vexcavacion, acerototal),2,MidpointRounding.AwayFromZero);
				Console.WriteLine("Costo Directo Aproximado del cimiento: ${0} COP",costo);
				Console.WriteLine("No incluye: \nSolado \nAcero del muro \nConcreto del muro \nRelleno del material");
				Console.WriteLine("Precios del semestre 1 del año 2015");
	
			
			
		}
		
		//metodo zapata cuadrada:
		static void ZapataCuadrada()
		{
			L=checked(Math.Round((Math.Pow(areareq,0.5)),2,MidpointRounding.AwayFromZero));
			B=L;
			areafinal=L*L;
			Console.WriteLine("Lado de la zapata cuadrada:\nL = raiz(area)");
			Console.WriteLine("L = raiz({0})",areareq);
			Console.WriteLine("L = {0} m",L);
			Console.WriteLine("L = B = {0} m",B);
			Console.WriteLine("Area final = B*L");
			Console.WriteLine("Area final = {0}*{1}",B,L);
			Console.WriteLine("Area final = {0}m2 \n",areafinal);
			
				constructor.AppendLine("Zapata Cuadrada:");
				constructor.AppendLine("Lado de la zapata cuadrada:\nL = raiz(area)");
				constructor.AppendLine("L = raiz("+areareq+")");
				constructor.AppendLine("L = "+L+" m");
				constructor.AppendLine("L = B = "+B+" m");
				constructor.AppendLine("Area final = B*L");
				constructor.AppendLine("Area final = "+B+"*"+L+"");
				constructor.AppendLine("Area final = "+areafinal+" m2\n");
				
			
		}
		
		//metodo zapata Rectangular:
		static void ZapataRectangular()
		{
			Console.WriteLine("Elija [1] si desea definir el lado 'L' ó\n"+
			"Elija [2] si desea definir el lado 'B' \n");
			int.TryParse(Console.ReadLine(), out opcionlado);
			switch (opcionlado)
				{
					case 1:
						Console.WriteLine("Ingrese el lado 'L' en metros con dos decimales:");
							double.TryParse(Console.ReadLine(), out L);
							B=checked(Math.Round((areareq/L),2,MidpointRounding.AwayFromZero));
						Console.WriteLine("Lado B de la zapata es: B = Area/L");
						Console.WriteLine("B = {0}/{1}",areareq,L);
						Console.WriteLine("B = {0} m",B);
						Console.WriteLine("L = {0} m \n",L);
						
							constructor.AppendLine("Zapata Rectangular:");
							constructor.AppendLine("Lado B de la zapata es: B = Area/L");
							constructor.AppendLine("B = "+areareq+" / "+L+"");
							constructor.AppendLine("B = "+B+" m");
							constructor.AppendLine("L = "+L+" m\n");
							
						
					break;
					case 2:
						Console.WriteLine("Ingrese el lado 'B' en metros con dos decimales:");
							double.TryParse(Console.ReadLine(), out B);
							L=checked(Math.Round((areareq/B),2,MidpointRounding.AwayFromZero));
						Console.WriteLine("Lado L de la zapata es: L = Area/B");
						Console.WriteLine("L = {0}/{1}",areareq,B);
						Console.WriteLine("L = {0} m",L);
						Console.WriteLine("B = {0} m \n",B);
						
							constructor.AppendLine("Zapata Rectangular:");
							constructor.AppendLine("Lado L de la zapata es: L = Area/B");
							constructor.AppendLine("L = "+areareq+" / "+B+"");
							constructor.AppendLine("L = "+L+" m");
							constructor.AppendLine("B = "+B+" m\n");
						
					break;
					default:
						Console.WriteLine("Ingrese el lado 'L' en metros con dos decimales:");
							double.TryParse(Console.ReadLine(), out L);
							B=checked(Math.Round((areareq/L),2,MidpointRounding.AwayFromZero));
						Console.WriteLine("Lado B de la zapata es: B = Area/L");
						Console.WriteLine("B = {0}/{1}",areareq,L);
						Console.WriteLine("B = {0} m",B);
						Console.WriteLine("L = {0} m \n",L);
						
							constructor.AppendLine("Zapata Rectangular:");
							constructor.AppendLine("Lado B de la zapata es: B = Area/L");
							constructor.AppendLine("B = "+areareq+" / "+L+"");
							constructor.AppendLine("B = "+B+" m");
							constructor.AppendLine("L = "+L+" m\n");
					break;
				}
			areafinal=B*L;
			Console.WriteLine("Area final = B*L");
			Console.WriteLine("Area final = {0}*{1}",B,L);
			Console.WriteLine("Area final = {0}m2 \n",areafinal);
			
				constructor.AppendLine("Area final = B*L");
				constructor.AppendLine("Area final = "+B+"*"+L+"");
				constructor.AppendLine("Area final = "+areafinal+" m2\n");
				
			
		}
		//metodo para calcular el maximo entre dos numeros
		static double maximo2(double a, double b)
			{
				double max;
				if(a>b)
				{
					max=a;
				}
				else
				{
					max=b;
				}
				return max;
			}
		//metodo para calcular el minimo entre dos numeros
		static double minimo2(double a, double b)
			{
				double min;
				if(a>b)
				{
					min=b;
				}
				else
				{
					min=a;
				}
				return min;
			}
		//metodo para calcular la cuantia de acero	
		static double cuantiad(double fconcreto, double facero, double Momento, double anchobw, double peralted)
			{
				double cuantiaacero, fiM=0.9;
				
				cuantiaacero=checked(0.85*fconcreto/facero*(1-(Math.Pow(1-(2*Momento/(0.85*fiM*fconcreto*anchobw*peralted*peralted*1000)), 0.5))));
				
				return cuantiaacero;
			}
			
		//creamos el metodo que calcula el presupuesto y recibe tres parametros que son las cantidades de materiales	
		static double presupuesto(double Vconcreto, double Vexcavacion, double Acerototal)
			{
				//rendimientos
				double rexc, rA420, rcon, rzap, rmezcladora, rvibrador, costodirecto;
				rexc=Vconcreto*8/10;
				rA420=Acerototal*8/500;
				rcon=Vconcreto*8/16;
				rzap=Vconcreto*8/8;
				rmezcladora=rcon;
				rvibrador=rzap/2;
				costodirecto=rexc*cexc+rA420*cacero+rcon*ccon+rzap*czap+Vconcreto*con3000+Acerototal*A420+Hmenor+rmezcladora*mezcladora+rvibrador*vibrador;
				return costodirecto;
			}
		
		//metodo biseccion
		static double biseccion4(double xmin, double xmax, double c4, double c3, double c2, double c1, double c0)
			{
				double a, fa, b, fb, c, fC, error, n=-1, errorp=0.0001;
				a = xmin;
				b = xmax;
				do 
					{
						
						fa = c4*Math.Pow(a,4) + c3*Math.Pow(a,3) + c2*Math.Pow(a,2) + c1*a + c0;
						
						fb = c4*Math.Pow(b,4) + c3*Math.Pow(b,3) + c2*Math.Pow(b,2) + c1*b + c0;
						
						c = (a + b)/2;
						
						fC = c4*Math.Pow(c,4) + c3*Math.Pow(c,3) + c2*Math.Pow(c,2) + c1*c + c0;
						
						if (fa*fC > 0)
							{
								a=c;
							}
						if (fa*fC < 0)
							{
								b=c;
							}
						n+=1;
						
						error = (b - a)/Math.Pow(2,n);
						
						
					} while(error > errorp);
					
					return c;
			}
			
	}
}
