using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Cimentacion_Combinada_Viga_Enlace
{
	class MainClass
	{
		//variables exportar:
		static StringBuilder constructor= new StringBuilder();
		static StringBuilder registro= new StringBuilder();
		static string fecha = "", nombrezapata = "";
		static int opcionc=5, opcionz=6;
		//variables del programa:
		static double PD1=250, PL1=50, P11=0, P11u=0, T1=1250, alfa1=30, FC=0.75, a1=0.3, b1=0.3;
		static double PD2=500, PL2=100, P12=0, P21u=0, T2=2000, alfa2=40, a2=0.3, b2=0.4;
		static double diametrobz, Areabz, Masabz, Ddoblamientoz;
		static double diametrobc, Areabc, Masabc, Ddoblamientoc;

		static double fc=21, fy=420, TM=0.0254, df=1.6, rsuelo=20, EsfuerzoAdmisible, l, r1=1.5, r2=1;

		static double r, rprima=0.075, Hmin1, ldc1, d11=0.15, d21, d31, d1, H1;
		static double Hmin2, ldc2, ldc, d12=0.15, d22, d32, d2, H2, d, H;

		static double c5=0, c4=0, c3=0, c2=0, c1=0, c0=0;

		static double rconcreto=24, Sobrecarga, EsfuerzoNeto, B1=1, L1=1, B2=1, L2=1, DR1, Area1, Area2;
		static double variable;
		static double V11, V21, rel11, rel21, V12, V22, rel12, rel22, voladizomax, relmax;
		static double DR1u, R11u, E11u, R21u, E21u;
		static double AtL1, V1uL1, fiVcL1, ficortante=0.75, landa=1, AtB1, V1uB1, fiVcB1;
		static double bo1, betac1, fiVc1p1, fiVc2p1, fiVc3p1, fiVcpmin1, Atp1, V1up1;
		static double AtL2, V1uL2, fiVcL2, AtB2, V1uB2, fiVcB2;
		static double bo2, betac2, fiVc1p2, fiVc2p2, fiVc3p2, fiVcpmin2, Atp2, V1up2;

		static double AtLf1, brazoL1, MuL1, kL1, m, cuantiadL1, AsrL1, AstL1, AssL1, NbL1, SccL1, SlbL1, Sminb;
		static double AssrL1, cuantiaLsum1, lddL1, cc1, cc2, cc, relccdbz, factorAsL1, ldrL1, lcorteL1;
		static double AtBf1, brazoB1, MuB1, kB1, cuantiadB1, AsrB1, AstB1, AssB1, NbB1, SccB1, SlbB1, AssrB1;
		static double cuantiaBsum1, lddB1, factorAsB1, ldrB1, lcorteB1;

		static double AtLf2, brazoL2, MuL2, kL2, cuantiadL2, AsrL2, AstL2, AssL2, NbL2, SccL2, SlbL2;
		static double AssrL2, cuantiaLsum2, lddL2, factorAsL2, ldrL2, lcorteL2;

		static double bw, lprima, hmin1, hmin2, hmin3, hmin, LM, Muv, pmin, prv, Asv, Nbv, Sccv, Slbv;

		
		public static void Main (string[] args)
		{
			//codigo control de acceso y seguimiento:
			fecha=DateTime.Now.ToString();
			registro.AppendLine("INGRESO: "+fecha+"\n");
			String reg = "";
			reg=registro.ToString();
			FileStream registroVE = new FileStream("/home/juan/Desktop/Reg_Viga_Enlace.txt", FileMode.Append);
			registroVE.Write(ASCIIEncoding.ASCII.GetBytes(reg), 0, reg.Length);
			registroVE.Close();
			//
			Console.WriteLine("Ingrese un nombre para identificar la cimentacion:");
			nombrezapata=Console.ReadLine();
			//metodos de calculo:
			VigaEnlace();
			//metodo exportar a txt los calculos
			Exportar();
			Console.WriteLine ("Fin");
		}
		static void VigaEnlace()
		{
			Console.WriteLine("\t\t\t INGENIERIA CIVIL \n");
			Console.WriteLine("\t\t\t 3.Cimentacion Combinada Viga de Enlace \n");
				constructor.AppendLine("CIMENTACION COMBINADA MEDIANTE VIGA DE ENLACE\n\n");
			constructor.AppendLine("Combinacion: "+nombrezapata+".\n");
			//Propiedades de suelo:
			Console.WriteLine("Ingrese el Esfuerzo Admisible del suelo en KN/m2:");
			double.TryParse(Console.ReadLine(),out EsfuerzoAdmisible);
			//columna 1 y 2:
			Console.WriteLine("Ingrese Carga Muerta Columna 1 en KN:");
			double.TryParse(Console.ReadLine(),out PD1);
			Console.WriteLine("Ingrese Carga Viva Columna 1 en KN:");
			double.TryParse(Console.ReadLine(),out PL1);
			Console.WriteLine("Ingrese Carga Muerta Columna 2 en KN:");
			double.TryParse(Console.ReadLine(),out PD2);
			Console.WriteLine("Ingrese Carga Viva Columna 2 en KN:");
			double.TryParse(Console.ReadLine(),out PL2);
			Console.WriteLine("Ingrese dimension 'a1' paralela a 'L1' de la columna 1 en metros:");
			double.TryParse(Console.ReadLine(), out a1);
			Console.WriteLine("Ingrese dimension 'b1' paralela a 'B1' de la columna 1 en metros:");
			double.TryParse(Console.ReadLine(), out b1);
			Console.WriteLine("Ingrese dimension 'a2' paralela a 'L2' de la columna 2 en metros:");
			double.TryParse(Console.ReadLine(), out a2);
			Console.WriteLine("Ingrese dimension 'b2' paralela a 'B2' de la columna 2 en metros:");
			double.TryParse(Console.ReadLine(), out b2);
			Console.WriteLine("Elija el diametro de refuerzo a usar para la columna:");
			for(int i=2;i<=11;i++)
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
				diametrobc=15.9/1000;
				Areabc=0.000199;
				Masabc=1.552;
				Ddoblamientoc=6*diametrobc;
				opcionc=5;
				Console.WriteLine("No seleccionó una opción válida, se tomará la #5 :P");
				break;	
			}
			Console.WriteLine("Elija el diametro de refuerzo a usar para la zapata:");
			for(int i=2;i<=11;i++)
			{
				Console.WriteLine("{0} ==> Para #{1}",i,i);
			}
			int.TryParse(Console.ReadLine(), out opcionz);

			//Distancia libre entre columnas:
			Console.WriteLine("Ingrese distancia libre entre columnas 'l' en (m):");
			double.TryParse(Console.ReadLine(),out l);
			Console.WriteLine("Ingrese la relación B1/L1:");
			double.TryParse(Console.ReadLine(),out r1);
			Console.WriteLine("Ingrese la relación B2/L2:");
			double.TryParse(Console.ReadLine(),out r2);
			//
			constructor.AppendLine("DATOS INICIALES:\n\n");
			constructor.AppendLine("f'c = "+fc+" KN/m2, fy = "+fy+" KN/m2, r' = "+rprima+" m, "+
			                       "df = "+df+" m, r suelo = "+rsuelo+" KN/m3, r concreto = "+rconcreto+" KN/m3, "+
			                       "Eadm suelo = "+EsfuerzoAdmisible+" KN/m2.\n");
			constructor.AppendLine("Cargas en las columnas:\nPD1 = "+PD1+" KN, PL1 = "+PL1+" KN, "+
			                       "PD2 = "+PD2+" KN, PL2 = "+PL2+" KN.\nCon dimensiones de columnas:\n"+
			                       "a1 = "+a1+" m; b1 = "+b1+" m, a2 = "+a2+" m; b2 = "+b2+" m.\n");
			constructor.AppendLine("Separacion de eje a eje de las columnas: l = "+l+" m.\n");
			constructor.AppendLine("Refuerzo para columna: N"+opcionc+"");
			constructor.AppendLine("Diametro: "+diametrobc+" m");
			constructor.AppendLine("Area: "+Areabc+" m2");
			constructor.AppendLine("Masa: "+Masabc+" Kg/mL");
			constructor.AppendLine("Diametro de Doblamiento: "+Ddoblamientoc+" m\n");

			constructor.AppendLine("Empleando estribos N4 cada 10 cm, con FC = "+FC+".\n");
			constructor.AppendLine("Columna de Lindero: T = "+T1+", alfa = "+alfa1+".\n"+
			                       "Columna de Interior: T = "+T2+", alfa = "+alfa2+".\n");
			constructor.AppendLine("Tomando las relaciones: B1/L1 = "+r1+" y B2/L2 = "+r2+". Para "+
			                       "calcular las respectivas areas: A1 y A2.\n");
			//calculamos desde aqui:
			constructor.AppendLine("\nRESULTADOS:\n\n");

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
				break;	
			}

		Calculos_Refuerzo:

			constructor.AppendLine("Refuerzo para zapata: N"+opcionz+"");
			constructor.AppendLine("Diametro: "+diametrobz+" m");
			constructor.AppendLine("Area: "+Areabz+" m2");
			constructor.AppendLine("Masa: "+Masabz+" Kg/mL");
			constructor.AppendLine("Diametro de Doblamiento: "+Ddoblamientoz+" m\n");

			P11  = PD1 + PL1;
			P11u = 1.2*PD1 + 1.6*PL1;
			P12 = PD2 + PL2;
			P21u = 1.2*PD2 + 1.6*PL2;
			r = rprima + diametrobz;
			Hmin1 = d11 + r;
			Hmin2 = d12 + r;
			ldc1 = 0.24*fy*diametrobc*FC/Math.Pow(fc,0.5);
			ldc2 = 0.043*diametrobc*fy*FC;
			ldc = maximo2(ldc1, ldc2);
			d21 = ldc + Ddoblamientoc/2 + diametrobc + diametrobz;
			d22 = d21;
			d31 = Math.Pow(P11u/(T1*Math.Pow(fc, 0.5)), 0.5);
			d32 = Math.Pow(P21u/(T2*Math.Pow(fc, 0.5)), 0.5);
			d1 = maximo2(d11, maximo2(d21, d31));
			d2 = maximo2(d12, maximo2(d22, d32));
			H1 = d1 + r;
			H2 = d2 + r;
			d = maximo2(d1, d2);
			H = maximo2((Math.Round((d + r),2,MidpointRounding.AwayFromZero)),Hmin1);

			constructor.AppendLine("\nCalculo de las cargas de Diseño:\n\n");
			constructor.AppendLine("\nZapata de Lindero.\n\n");
			constructor.AppendLine("Carga de Servicio:\n");
			constructor.AppendLine("P11 = PD1 + PL1");
			constructor.AppendLine("P11 = "+PD1+" + "+PL1+"");
			constructor.AppendLine("P11 = "+P11+" KN.\n");
			constructor.AppendLine("Carga de Mayorada:\n");
			constructor.AppendLine("P1(1u) = 1.2 * PD1 + 1.6 * PL1");
			constructor.AppendLine("P1(1u) = 1.2 * "+PD1+" + 1.6 * "+PL1+"");
			constructor.AppendLine("P1(1u) = "+P11u+" KN.\n");
			constructor.AppendLine("\nZapata de Interior.\n\n");
			constructor.AppendLine("Carga de Servicio:\n");
			constructor.AppendLine("P12 = PD2 + PL2");
			constructor.AppendLine("P12 = "+PD2+" + "+PL2+"");
			constructor.AppendLine("P12 = "+P12+" KN.\n");
			constructor.AppendLine("Carga de Mayorada:\n");
			constructor.AppendLine("P2(1u) = 1.2 * PD2 + 1.6 * PL2");
			constructor.AppendLine("P2(1u) = 1.2 * "+PD2+" + 1.6 * "+PL2+"");
			constructor.AppendLine("P2(1u) = "+P21u+" KN.\n");

			constructor.AppendLine("\nCalculo de los Espesores y Areas de los cimientos:\n\n");
			constructor.AppendLine("El recubrimiento: \nr = r' + dbz");
			constructor.AppendLine("r = "+rprima+" + "+diametrobz+"");
			constructor.AppendLine("r = "+r+" m.\n");
			constructor.AppendLine("El espesor H minimo por norma es: \nHmin1 = Hmin2 = d1 + r");
			constructor.AppendLine("Hmin1 = Hmin2 = "+d11+" + "+r+"");
			constructor.AppendLine("Hmin1 = Hmin2 = "+Hmin1+" m.\n");
			constructor.AppendLine("Usando el mismo refuerzo en ambas columnas se tiene:\n");
			constructor.AppendLine("ldc = max( ldc1 o ldc2 )");
			constructor.AppendLine("ldc = max( 0.24*"+fy+"*"+diametrobc+"*"+FC+"/raiz("+fc+") o 0.043*"+diametrobc+"*"+fy+"*"+FC+")");
			constructor.AppendLine("ldc = max( "+ldc1+" o "+ldc2+" )");
			constructor.AppendLine("ldc = "+ldc+" m.\n");
			constructor.AppendLine("El peralte columna 1 : \nd21 = d22 = ldc + Ddoblamientoc/2 + dbc + dbz");
			constructor.AppendLine("d21 = d22 = "+ldc+" + "+Ddoblamientoc+"/2 + "+diametrobc+" + "+diametrobz+"");
			constructor.AppendLine("d21 = d22 = "+d21+" m.\n");
			constructor.AppendLine("El peralte columna 2 : \nd31 = raiz(P11u / (T*raiz(f'c)))");
			constructor.AppendLine("d31 = raiz("+P11u+" / ("+T1+"*raiz("+fc+")))");
			constructor.AppendLine("d31 = "+d31+" m\n");
			constructor.AppendLine("El peralte: \nd32 = raiz(P21u / (T*raiz(f'c)))");
			constructor.AppendLine("d32 = raiz("+P21u+" / ("+T2+"*raiz("+fc+")))");
			constructor.AppendLine("d32 = "+d32+" m\n");
			constructor.AppendLine("El peralte minimo columna 1 es: \nd1 = max(d11,d21,d31)");
			constructor.AppendLine("d1 = max("+d11+" ; "+d21+" ; "+d31+")");
			constructor.AppendLine("d1 = "+d1+"m \n");
			constructor.AppendLine("El peralte minimo columna 2 es: \nd2 = max(d12,d22,d32)");
			constructor.AppendLine("d2 = max("+d12+" ; "+d22+" ; "+d32+")");
			constructor.AppendLine("d2 = "+d2+"m \n");
			constructor.AppendLine("H1 = d1 + r");
			constructor.AppendLine("H1 = "+d1+" + "+r+"");
			constructor.AppendLine("H1 = "+H1+" m.\n");
			constructor.AppendLine("H2 = d2 + r");
			constructor.AppendLine("H2 = "+d2+" + "+r+"");
			constructor.AppendLine("H2 = "+H2+" m.\n");
			constructor.AppendLine("El peralte minimo para ambas zapatas es: \nd = max( d1 , d2 )");
			constructor.AppendLine("d = max( "+d1+" ; "+d2+" )");
			constructor.AppendLine("d = "+d+"m \n");
			constructor.AppendLine("El Espesor minimo para ambas zapatas es: \nH = max(Hmin1 ; d + r)");
			constructor.AppendLine("H = max("+Hmin1+" ; "+d+" + "+r+")");
			constructor.AppendLine("H = max("+Hmin2+" ; "+H+")");
			constructor.AppendLine("H = "+H+" m.\n");


		calculos_H:

				d = H - r;
			constructor.AppendLine("Recalculamos: d = H-r");
			constructor.AppendLine("d = "+H+" - "+r+"");
			constructor.AppendLine("d = "+d+" m.\n");

			Sobrecarga = checked(rsuelo*(df-H)+rconcreto*H);
			EsfuerzoNeto = checked(EsfuerzoAdmisible-Sobrecarga);

			constructor.AppendLine("La sobrecarga es: \nqw = rsuelo*(df - H) + rconcreto*H");
			constructor.AppendLine("qw = "+rsuelo+"*("+df+" - "+H+") + "+rconcreto+"*"+H+"");
			constructor.AppendLine("qw = "+Sobrecarga+" KN/m2.\n");
			constructor.AppendLine("El Esfuerzo neto es: \nEsfNeto = EsfuerzoAdmisible - Sobrecarga");
			constructor.AppendLine("EsfNeto = "+EsfuerzoAdmisible+" - "+Sobrecarga+"");
			constructor.AppendLine("EsfNeto = "+EsfuerzoNeto+" KN/m2.\n");
			constructor.AppendLine("Emplearemos el mismo esfuerzo neto para hallar las areas de ambas zapatas.\n");

			constructor.AppendLine("\nDimensiones de la zapata de lindero.\n\n");
			constructor.AppendLine("Usando la relacion B1/L1 = "+r1+", para calcular el area de la zapata de "+
			                       "lindero, se tienen que las ecuaciones de equilibrio en terminos de L1, es:\n");
			//calcular la raiz por metodo
			c3 = -r1*EsfuerzoNeto;
			c2 = (r1*EsfuerzoNeto)*( 2*l + a1 );
			c1 = 0;
			c0 = -2*l*P11;
			L1 = nr5(3.5,c5,c4,c3,c2,c1,c0);
			L1 = Math.Round(L1,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("-r1*EsfNeto*(L1)^3 + (r1*EsfNeto)*( 2*l + a1 )*(L1)^2 - 2*l*P11 = 0\n");
			constructor.AppendLine("c3 = -r1*EsfNeto, c2 = (r1*EsfNeto)*( 2*l + a1 ), c1 = 0, c0 = -2*l*P11.\n");
			constructor.AppendLine("Se tiene:\nc3 = -"+r1+"*"+EsfuerzoNeto+".\nc2 = ("+r1+"*"+EsfuerzoNeto+")*( 2*"+l+" + "+a1+" ).\n"+
			                       "c1 = 0.\nc0 = -2*"+l+"*"+P11+".\n");
			constructor.AppendLine("c3 = "+c3+", c2 = "+c2+", c1 = 0, c0 = "+c0+".\n");
			constructor.AppendLine("Utilizando el metodo Newton-Raphson para hallar las raices del polinomio:\n"+
			                       "Se tiene que:\nL1 = "+L1+" m.");
			//
			B1 = r1*L1;
			B1 = Math.Round(B1,2,MidpointRounding.AwayFromZero);
			Area1 = B1*L1;
			DR1 = Area1*EsfuerzoNeto - P11;
			constructor.AppendLine("B1 = r1*L1\nB1 = "+r1+" * "+L1+"\nB1 = "+B1+" m.\n");
			constructor.AppendLine("Area = B1*L1\nArea = "+B1+" * "+L1+"\nArea = "+Area1+" m.\n");
			constructor.AppendLine("DR requerido:\nDR1 = Area*EsfuerzoNeto - P11");
			constructor.AppendLine("DR1 = "+Area1+" * "+EsfuerzoNeto+" - "+P11+"\nDR1 = "+DR1+" KN.\n");


			constructor.AppendLine("\nDimensiones de la zapata de Interior.\n\n");
			Area2 = (P12 - DR1 ) / EsfuerzoNeto;
			constructor.AppendLine("Calculamos el area requerida:\nArea = (P12 - DR1 ) / EsfuerzoNeto");
			constructor.AppendLine("Area = ("+P12+" - "+DR1+" ) / "+EsfuerzoNeto+"\nArea = "+Area2+" m2.\n");
			L2 = Math.Pow((Area2 / r2),0.5);
			L2 = Math.Round(L2,2,MidpointRounding.AwayFromZero);
			B2 = r2 * L2;
			B2 = Math.Round(B2,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("Calculamos las dimensiones B2 y L2:\nL2 = raiz(Area / r2)\nL2 = raiz("+Area2+" / "+r2+" )");
			constructor.AppendLine("L2 = "+L2+" m.\nSe tiene que:\nB2 = r2 * L2 = "+r2+" * "+L2+" = "+B2+"");
			Area2 = B2*L2;
			constructor.AppendLine("Recalculamos el area:\nArea = B2 * L2\nArea = "+B2+" * "+L2+"\nArea = "+Area2+" m2.\n");

			Console.WriteLine("B1 = {0}, L1 = {1}, B2 = {2}, L2 = {3}",B1,L1,B2,L2);
			//Chequeo Rigidez
			V11 = (L1 - a1);
			rel11 = V11 / H;
			V21 = (B1 - b1)/2;
			rel21 = V21 / H;
			V12 = (L2 - a2)/2;
			rel12 = V12 / H;
			V22 = (B2 - b2)/2;
			rel22 = V22 / H;
			constructor.AppendLine("\nChequeos de Rigidez:\n\n");
			//zapata de lindero
			constructor.AppendLine("\nZapata de Lindero.\n\n");
			constructor.AppendLine("El voladizo 'V1' es la distancia paralela a 'L'");
			constructor.AppendLine("V1(1) = (L1 - a1)");
			constructor.AppendLine("V1(1) = ("+L1+" - "+a1+")");
			constructor.AppendLine("V1(1) = "+V11+" m\n");
			constructor.AppendLine("Relacion entre el voladizo y el espesor H: \nrelacion = V1(1)/H");
			constructor.AppendLine("V1(1)/H = "+V11+" / "+H+"");
			constructor.AppendLine("V1(1)/H = "+rel11+"\n");
			constructor.AppendLine("El voladizo 'V2' es la distancia paralela a 'B'");
			constructor.AppendLine("V2(1) = (B1 - b1)/2");
			constructor.AppendLine("V2(1) = ("+B1+" - "+b1+")/2");
			constructor.AppendLine("V2(1) = "+V21+" m\n");
			constructor.AppendLine("Relacion entre el voladizo y el espesor H: \nrelacion = V2(1)/H");
			constructor.AppendLine("V2(1)/H = "+V21+" / "+H+"");
			constructor.AppendLine("V2(1)/H = "+rel21+"\n");
			//zapata de interior
			constructor.AppendLine("\nZapata de Interior.\n\n");
			constructor.AppendLine("El voladizo 'V1' es la distancia paralela a 'L'");
			constructor.AppendLine("V1(2) = (L2 - a2)/2");
			constructor.AppendLine("V1(2) = ("+L2+" - "+a2+")/2");
			constructor.AppendLine("V1(2) = "+V12+" m\n");
			constructor.AppendLine("Relacion entre el voladizo y el espesor H: \nrelacion = V1(1)/H");
			constructor.AppendLine("V1(2)/H = "+V12+" / "+H+"");
			constructor.AppendLine("V1(2)/H = "+rel12+"\n");
			constructor.AppendLine("El voladizo 'V2' es la distancia paralela a 'B'");
			constructor.AppendLine("V2(2) = (B2 - b2)/2");
			constructor.AppendLine("V2(2) = ("+B2+" - "+b2+")/2");
			constructor.AppendLine("V2(2) = "+V22+" m\n");
			constructor.AppendLine("Relacion entre el voladizo y el espesor H: \nrelacion = V2(1)/H");
			constructor.AppendLine("V2(2)/H = "+V22+" / "+H+"");
			constructor.AppendLine("V2(2)/H = "+rel22+"\n");
			voladizomax = maximo2(V11,maximo2(V21,maximo2(V12,V22)));
			relmax = voladizomax / H;
			if (relmax>3)
			{
				Console.WriteLine("No cumple rigidez, se rediseña el espesor");
				H = checked(Math.Round(voladizomax/2.8,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("No se cumple la Rigidez!! Se Volvera a calcular el espesor H.");
				constructor.AppendLine("Tambien se calculara el esfuerzo neto, el area y las dimensiones.\n");
				constructor.AppendLine("H para rigidez = voladizomaximo/2.8");
				constructor.AppendLine("H = "+voladizomax+" / 2.8");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			constructor.AppendLine(" ==> Cimiento rigido en condicion aceptable.\n");
			//Calculos de las presiones netas de contacto
			DR1u = P11u * V11/ (l - V11);
			R11u = P11u + DR1u;
			E11u = R11u / Area1;
			R21u = P21u - DR1u;
			E21u = R21u / Area2;
			constructor.AppendLine("\nDeterminacion de la presion de contacto neta mayorada:\n\n");

			constructor.AppendLine("\nZapata de Lindero.\n\n");
			constructor.AppendLine("DR(1u) = P1(1u) * V1(1) / (l - V1(1) )");
			constructor.AppendLine("DR(1u) = "+P11u+" * "+V11+" / ("+l+" - "+V11+")");
			constructor.AppendLine("DR(1u) = "+DR1u+" KN.\n");
			constructor.AppendLine("La resultante ultima es:");
			constructor.AppendLine("R1(1u) = P1(1u) + DR(1u)");
			constructor.AppendLine("R1(1u) = "+P11u+" + "+DR1u+"");
			constructor.AppendLine("R1(1u) = "+R11u+" KN.\n");
			constructor.AppendLine("El esfuerzo ultimo es:");
			constructor.AppendLine("E1(1u) = R1(1u) / Area1");
			constructor.AppendLine("E1(1u) = "+R11u+" / "+Area1+"");
			constructor.AppendLine("E1(1u) = "+E11u+" KN/m2.\n");
			constructor.AppendLine("\nZapata de Interior.\n\n");
			constructor.AppendLine("La resultante ultima es:");
			constructor.AppendLine("R2(1u) = P2(1u) - DR1u");
			constructor.AppendLine("R2(1u) = "+P21u+" - "+DR1u+"");
			constructor.AppendLine("R2(1u) = "+R21u+" KN.\n");
			constructor.AppendLine("El esfuerzo ultimo es:");
			constructor.AppendLine("E2(1u) = R2(1u) / Area2");
			constructor.AppendLine("E2(1u) = "+R21u+" / "+Area2+"");
			constructor.AppendLine("E2(1u) = "+E21u+" KN/m2.\n");

			//Diseño de las zapatas, cortante y momento.
			constructor.AppendLine("\n\nDISENO DE LAS ZAPATAS.\n\n");
			constructor.AppendLine("\n\nDISENO DE LAS ZAPATAS A CORTANTE.\n\n");

			constructor.AppendLine("\nZapata de Lindero.\n\n");
			constructor.AppendLine("Accion como viga:\n");

			constructor.AppendLine("Diseno y revision lado paralelo a 'L1' con el 'V1(1)':");
			AtL1 = checked(B1*(V11-d));
			V1uL1 = checked(E11u * AtL1);
			fiVcL1 = checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*B1*d*1000);
			constructor.AppendLine("El Area tributaria es: \nAtrib = B1 * ( V1(1) - d )");
			constructor.AppendLine("Atrib = "+B1+"*("+V11+" - "+d+")");
			constructor.AppendLine("Atrib = "+AtL1+" m2\n");
			constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u * Atributaria");
			constructor.AppendLine("V1u = "+E11u+" * "+AtL1+"");
			constructor.AppendLine("V1u = "+V1uL1+" KN\n");
			constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante * 0.17 * landa * raiz(fc) * B1 * d * 1000");
			constructor.AppendLine("fiVc = "+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * "+B1+" * "+d+" * 1000");
			constructor.AppendLine("fiVc = "+fiVcL1+" KN\n");

			if (fiVcL1>=V1uL1)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVcL1+" > "+V1uL1+"");
				constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto "+
				                       "resiste los esfuerzos cortantes \n");
			}

			if (fiVcL1<V1uL1)
			{
				d = checked(E11u*V11 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + E11u));
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVcL1+" < "+V1uL1+"");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
				                       "cortante actuante y resistente, despejando a d.\n");
				constructor.AppendLine("'d' para accion como viga paralelo a 'L1': \nd = E11u * V11 / ((ficortante * 0.17 * landa * raiz(fc) * 1000) + E11u)");
				constructor.AppendLine("d = "+E11u+" * "+V11+" / (("+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * 1000) + "+E11u+")");
				constructor.AppendLine("d = "+d+" m\n");
				constructor.AppendLine("H para accion como viga paralelo a 'L1': \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			constructor.AppendLine("Diseno y revision lado paralelo a 'B1' con el 'V2(1)':");
			AtB1 = checked(L1*(V21-d));
			V1uB1 = checked(E11u * AtB1);
			fiVcB1 = checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*L1*d*1000);
			constructor.AppendLine("El Area tributaria es: \nAtrib = L1 * ( V2(1) - d)");
			constructor.AppendLine("Atrib = "+L1+" * ("+V21+" - "+d+")");
			constructor.AppendLine("Atrib = "+AtB1+" m2\n");
			constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u*Atributaria");
			constructor.AppendLine("V1u = "+E11u+" * "+AtB1+"");
			constructor.AppendLine("V1u = "+V1uB1+" KN\n");
			constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante * 0.17 * landa * raiz(fc) * L1 * d * 1000");
			constructor.AppendLine("fiVc = "+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * "+L1+" * "+d+" * 1000");
			constructor.AppendLine("fiVc = "+fiVcB1+" KN\n");

			if (fiVcB1>=V1uB1)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVcB1+" > "+V1uB1+"");
				constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto "+
				                       "resiste los esfuerzos cortantes \n");
			}

			if (fiVcB1<V1uB1)
			{
				d = checked(E11u*V21 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + E11u));
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVcB1+" < "+V1uB1+"");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
				                       "cortante actuante y resistente, despejando a d.\n");
				constructor.AppendLine("'d' para accion como viga paralelo a 'L1': \nd = E11u * V11 / ((ficortante * 0.17 * landa * raiz(fc) * 1000) + E11u)");
				constructor.AppendLine("d = "+E11u+" * "+V21+" / (("+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * 1000) + "+E11u+")");
				constructor.AppendLine("d = "+d+" m\n");
				constructor.AppendLine("H para accion como viga paralelo a 'L1': \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			constructor.AppendLine("\nAccion en dos direcciones o punzonamiento:\n");
			bo1 = 2 * a1 + b1 + 2 * d;
			betac1 = maximo2(a1,b1)/minimo2(a1,b1);
			//fiVc1p
			fiVc1p1 = ficortante*0.17*Math.Pow(fc,0.5)*(1 + 2/betac1)*bo1*d*1000;
			//fiVc2p
			fiVc2p1 = ficortante*0.17*Math.Pow(fc,0.5)*(1 + alfa1*d/(2 * bo1))*bo1*d*1000;
			//fiVc3p
			fiVc3p1 = ficortante*0.33*Math.Pow(fc,0.5)*bo1*d*1000;
			//fiVcp minimo:
			fiVcpmin1 = minimo2(fiVc3p1,minimo2(fiVc1p1,fiVc2p1));
			//Area tributaria:
			Atp1 = ( a1 + d/2 )*( b1 + d );
			V1up1 = P11u - E11u * Atp1;
			constructor.AppendLine("Calculo del perimetro critico: \nbo = 2 * a1 + b1 + 2 * d");
			constructor.AppendLine("bo = 2 * "+a1+" + "+b1+" + 2 * "+d+"");
			constructor.AppendLine("bo = "+bo1+" m.\n");
			constructor.AppendLine("Calculo del factor betac: \nbetac = Llargo Columna / Lcorto Columna");
			constructor.AppendLine("betac = "+maximo2(a1,b1)+" / "+minimo2(a1,b1)+"");
			constructor.AppendLine("betac = "+betac1+"\n");
			constructor.AppendLine("Calculo del primer fiVc:");
			constructor.AppendLine("fiVc1 = ficortante * 0.17 * raiz(fc) * (1 + 2/betac) * bo * d * 1000");
			constructor.AppendLine("fiVc1 = "+ficortante+" * 0.17 * raiz("+fc+") * (1 + 2/"+betac1+") * "+bo1+" * "+d+" * 1000");
			constructor.AppendLine("fiVc1 = "+fiVc1p1+" KN.\n");
			constructor.AppendLine("Calculo del segundo fiVc:");
			constructor.AppendLine("fiVc2 = ficortante * 0.17 * raiz(fc) * (1 + alfa * d / (2 * bo)) * bo * d * 1000");
			constructor.AppendLine("fiVc2 = "+ficortante+" * 0.17 * raiz("+fc+") * (1 + "+alfa1+" * "+d+" / (2 * "+bo1+")) * "+bo1+" * "+d+" * 1000");
			constructor.AppendLine("fiVc2 = "+fiVc2p1+" KN.\n");
			constructor.AppendLine("Calculo del tercer fiVc:");
			constructor.AppendLine("fiVc3 = ficortante * 0.33 * raiz(fc) * bo * d * 1000");
			constructor.AppendLine("fiVc3 = "+ficortante+" * 0.33 * raiz("+fc+") * "+bo1+" * "+d+" * 1000");
			constructor.AppendLine("fiVc3 = "+fiVc3p1+" KN.\n");
			constructor.AppendLine("Se escoge el menor de los tres fiVc: "+fiVcpmin1+" KN.\n");
			constructor.AppendLine("Calculo del area tributaria: \nAtp = ( a1 + d/2 )*( b1 + d )");
			constructor.AppendLine("Atp = ( "+a1+" + "+d+"/2 )*( "+b1+" + "+d+" )");
			constructor.AppendLine("Atp = "+Atp1+" m2\n");
			constructor.AppendLine("Calculo del cortante actuante: \nV1u = P11u - E11u * Atp");
			constructor.AppendLine("V1u = "+P11u+" - "+E11u+" * "+Atp1+"");
			constructor.AppendLine("V1u = "+V1up1+" KN\n");

			if (fiVcpmin1>=V1up1)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple para punzonamiento!\n==> "+fiVcpmin1+" > "+V1up1+"");
				constructor.AppendLine(" ==>Con el peralte d = "+d+" m el concreto resiste los esfuerzos "
				                       +"cortantes de punzonamiento.\n");
			}

			if (fiVcpmin1<V1up1)
			{
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n==> fiVcpmin1 < V1up1");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo "+
				                       "cortante actuante y resistente, despejando a d.\n");
				c2 = ficortante * 0.33 * Math.Pow(fc, 0.5) * 2000 + E11u/2;
				c1 = ficortante * 0.33 * Math.Pow(fc, 0.5) * 2000 * (a1 + b1/2) + E11u * (a1 + b1/2);
				c0 = E11u * a1 * b1 - P11u;
				d = nr5(1,0,0,0,c2,c1,c0);
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("Se emplea el metodo de Newton-raphson para hallar la raiz del polinomio");
				constructor.AppendLine("Se tienen: \nc2 = ficortante * 0.33 * raiz(f'c) * 2000 + E11u/2");
				constructor.AppendLine("c2 = "+ficortante+" * 0.33 * raiz("+fc+") * 2000 + "+E11u+"/2");
				constructor.AppendLine("c2 = "+c2+"");
				constructor.AppendLine("c1 = ficortante * 0.33 * raiz(f'c) * 2000 * (a1 + b1/2) + E11u * (a1 + b1/2)");
				constructor.AppendLine("c1 = "+ficortante+" * 0.33 * raiz("+fc+") * 2000 * ("+a1+" + "+b1+"/2) + "+E11u+" * ("+a1+" + "+b1+"/2)");
				constructor.AppendLine("c1 = "+c1+"");
				constructor.AppendLine("c0 = E11u * a1 * b1 - P11u");
				constructor.AppendLine("c0 = "+E11u+" * "+a1+" * "+b1+" - "+P11u+"");
				constructor.AppendLine("c0 = "+c0+"");
				constructor.AppendLine("'d' requerido para punzonamiento: d = "+d+"");
				constructor.AppendLine("H para punzonamiento: \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}


			constructor.AppendLine("\nZapata de Interior.\n\n");

			constructor.AppendLine("Accion como viga:\n");
			constructor.AppendLine("Diseno y revision lado paralelo a 'L2' con el 'V1(2)':");
			AtL2 = checked(B2*(V12-d));
			V1uL2 = checked(E21u * AtL2);
			fiVcL2 = checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*B2*d*1000);
			constructor.AppendLine("El Area tributaria es: \nAtrib = B2 * ( V1(2) - d )");
			constructor.AppendLine("Atrib = "+B2+"*("+V12+" - "+d+")");
			constructor.AppendLine("Atrib = "+AtL2+" m2\n");
			constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u * Atributaria");
			constructor.AppendLine("V1u = "+E21u+" * "+AtL2+"");
			constructor.AppendLine("V1u = "+V1uL2+" KN\n");
			constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante * 0.17 * landa * raiz(fc) * B2 * d * 1000");
			constructor.AppendLine("fiVc = "+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * "+B2+" * "+d+" * 1000");
			constructor.AppendLine("fiVc = "+fiVcL2+" KN\n");

			if (fiVcL2>=V1uL2)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVcL2+" > "+V1uL2+"");
				constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto "+
				                       "resiste los esfuerzos cortantes \n");
			}

			if (fiVcL2<V1uL2)
			{
				d = checked(E21u*V12 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + E21u));
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVcL2+" < "+V1uL2+"");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
				                       "cortante actuante y resistente, despejando a d.\n");
				constructor.AppendLine("'d' para accion como viga paralelo a 'L2': \nd = E21u * V12 / ((ficortante * 0.17 * landa * raiz(fc) * 1000) + E21u)");
				constructor.AppendLine("d = "+E21u+" * "+V12+" / (("+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * 1000) + "+E21u+")");
				constructor.AppendLine("d = "+d+" m\n");
				constructor.AppendLine("H para accion como viga paralelo a 'L2': \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			constructor.AppendLine("Diseno y revision lado paralelo a 'B2' con el 'V2(2)':");
			AtB2 = checked(L2*(V22-d));
			V1uB2 = checked(E21u * AtB2);
			fiVcB2 = checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*L2*d*1000);
			constructor.AppendLine("El Area tributaria es: \nAtrib = L2 * ( V2(2) - d)");
			constructor.AppendLine("Atrib = "+L2+" * ("+V22+" - "+d+")");
			constructor.AppendLine("Atrib = "+AtB2+" m2\n");
			constructor.AppendLine("El Cortante actuante es: \nV1u = Esfuerzo1u*Atributaria");
			constructor.AppendLine("V1u = "+E21u+" * "+AtB2+"");
			constructor.AppendLine("V1u = "+V1uB2+" KN\n");
			constructor.AppendLine("El Cortante resistente es: \nfiVc = ficortante * 0.17 * landa * raiz(fc) * L2 * d * 1000");
			constructor.AppendLine("fiVc = "+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * "+L2+" * "+d+" * 1000");
			constructor.AppendLine("fiVc = "+fiVcB2+" KN\n");

			if (fiVcB2>=V1uB2)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple!\n"+fiVcB2+" > "+V1uB2+"");
				constructor.AppendLine("==>Con el peralte d = "+d+" m, el concreto "+
				                       "resiste los esfuerzos cortantes \n");
			}

			if (fiVcB2<V1uB2)
			{

				d = checked(E21u*V22 / ((ficortante*0.17*landa*Math.Pow(fc, 0.5)*1000) + E21u));
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n"+fiVcB2+" < "+V1uB2+"");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo"+
				                       "cortante actuante y resistente, despejando a d.\n");
				constructor.AppendLine("'d' para accion como viga paralelo a 'L2': \nd = E21u * V22 / ((ficortante * 0.17 * landa * raiz(fc) * 1000) + E21u)");
				constructor.AppendLine("d = "+E21u+" * "+V22+" / (("+ficortante+" * 0.17 * "+landa+" * raiz("+fc+") * 1000) + "+E21u+")");
				constructor.AppendLine("d = "+d+" m\n");
				constructor.AppendLine("H para accion como viga paralelo a 'L2': \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			//punzonamiento:
			constructor.AppendLine("\nAccion en dos direcciones o punzonamiento:\n");
			bo2 = 2 * ( a2 + b2 ) + 4 * d;
			betac2 = maximo2(a2,b2)/minimo2(a2,b2);
			//fiVc1p
			fiVc1p2 = ficortante*0.17*Math.Pow(fc,0.5)*(1 + 2/betac2)*bo2*d*1000;
			//fiVc2p
			fiVc2p2 = ficortante*0.17*Math.Pow(fc,0.5)*(1 + alfa2*d/(2 * bo2))*bo2*d*1000;
			//fiVc3p
			fiVc3p2 = ficortante*0.33*Math.Pow(fc,0.5)*bo2*d*1000;
			//fiVcp minimo:
			fiVcpmin2 = minimo2(fiVc3p2,minimo2(fiVc1p2,fiVc2p2));
			//Area tributaria:
			Atp2 = ( a2 + d )*( b2 + d );
			V1up2 = P21u - E21u * Atp2;
			constructor.AppendLine("Calculo del perimetro critico: \nbo = 2 * ( a2 + b2 ) + 4 * d");
			constructor.AppendLine("bo = 2 * ( "+a2+" + "+b2+" ) + 4 * "+d+"");
			constructor.AppendLine("bo = "+bo2+" m.\n");
			constructor.AppendLine("Calculo del factor betac: \nbetac = Llargo Columna / Lcorto Columna");
			constructor.AppendLine("betac = "+maximo2(a2,b2)+" / "+minimo2(a2,b2)+"");
			constructor.AppendLine("betac = "+betac2+"\n");
			constructor.AppendLine("Calculo del primer fiVc:");
			constructor.AppendLine("fiVc1 = ficortante * 0.17 * raiz(fc) * (1 + 2/betac) * bo * d * 1000");
			constructor.AppendLine("fiVc1 = "+ficortante+" * 0.17 * raiz("+fc+") * (1 + 2/"+betac2+") * "+bo2+" * "+d+" * 1000");
			constructor.AppendLine("fiVc1 = "+fiVc1p2+" KN.\n");
			constructor.AppendLine("Calculo del segundo fiVc:");
			constructor.AppendLine("fiVc2 = ficortante * 0.17 * raiz(fc) * (1 + alfa * d / (2 * bo)) * bo * d * 1000");
			constructor.AppendLine("fiVc2 = "+ficortante+" * 0.17 * raiz("+fc+") * (1 + "+alfa2+" * "+d+" / (2 * "+bo2+")) * "+bo2+" * "+d+" * 1000");
			constructor.AppendLine("fiVc2 = "+fiVc2p2+" KN.\n");
			constructor.AppendLine("Calculo del tercer fiVc:");
			constructor.AppendLine("fiVc3 = ficortante * 0.33 * raiz(fc) * bo * d * 1000");
			constructor.AppendLine("fiVc3 = "+ficortante+" * 0.33 * raiz("+fc+") * "+bo2+" * "+d+" * 1000");
			constructor.AppendLine("fiVc3 = "+fiVc3p2+" KN.\n");
			constructor.AppendLine("Se escoge el menor de los tres fiVc: "+fiVcpmin2+" KN.\n");
			constructor.AppendLine("Calculo del area tributaria: \nAtp = ( a2 + d )*( b2 + d )");
			constructor.AppendLine("Atp = ( "+a2+" + "+d+" )*( "+b2+" + "+d+" )");
			constructor.AppendLine("Atp = "+Atp2+" m2\n");
			constructor.AppendLine("Calculo del cortante actuante: \nV1u = P21u - E21u * Atp");
			constructor.AppendLine("V1u = "+P21u+" - "+E21u+" * "+Atp2+"");
			constructor.AppendLine("V1u = "+V1up2+" KN\n");

			if (fiVcpmin2>=V1up2)
			{
				constructor.AppendLine("fiVc > V1u ==>!Cumple para punzonamiento!\n==> "+fiVcpmin2+" > "+V1up2+"");
				constructor.AppendLine(" ==>Con el peralte d = "+d+" m el concreto resiste los esfuerzos "
				                       +"cortantes de punzonamiento.\n");
			}

			if (fiVcpmin2<V1up2)
			{
				constructor.AppendLine("fiVc < V1u ==>!No Cumple!\n==> fiVcpmin1 < V1up1");
				constructor.AppendLine("!!El concreto no resiste las solicitaciones cortantes!!");
				constructor.AppendLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo "+
				                       "cortante actuante y resistente, despejando a d.\n");
				c2 = ficortante * 0.33 * Math.Pow(fc, 0.5) * 4000 + E21u;
				c1 = ficortante * 0.33 * Math.Pow(fc, 0.5) * 2000 * (a2 + b2) + E21u * (a2 + b2);
				c0 = E21u * a2 * b2 - P21u;
				d = nr5(1,0,0,0,c2,c1,c0);
				H = checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
				constructor.AppendLine("Se emplea el metodo de Newton-raphson para hallar la raiz del polinomio");
				constructor.AppendLine("Se tienen: \nc2 = ficortante * 0.33 * raiz(f'c) * 4000 + E21u");
				constructor.AppendLine("c2 = "+ficortante+" * 0.33 * raiz("+fc+") * 4000 + "+E21u+"");
				constructor.AppendLine("c2 = "+c2+"");
				constructor.AppendLine("c1 = ficortante * 0.33 * raiz(f'c) * 2000 * (a2 + b2) + E21u * (a2 + b2)");
				constructor.AppendLine("c1 = "+ficortante+" * 0.33 * raiz("+fc+") * 2000 * ("+a2+" + "+b2+") + "+E21u+" * ("+a2+" + "+b2+")");
				constructor.AppendLine("c1 = "+c1+"");
				constructor.AppendLine("c0 = E21u * a2 * b2 - P21u");
				constructor.AppendLine("c0 = "+E21u+" * "+a2+" * "+b2+" - "+P21u+"");
				constructor.AppendLine("c0 = "+c0+"");
				constructor.AppendLine("'d' requerido para punzonamiento: d = "+d+"");
				constructor.AppendLine("H para punzonamiento: \nH = d + r");
				constructor.AppendLine("H = "+d+" + "+r+"");
				constructor.AppendLine("H = "+H+" m\n");
				goto calculos_H;
			}

			//diseño a flexion de las zapatas:
			constructor.AppendLine("\n\nDISENO DE LAS ZAPATAS A MOMENTO.\n\n");
			constructor.AppendLine("\nZapata de Lindero.\n\n");
			constructor.AppendLine("Acero de refuerzo paralelo a 'L'.\n");
			AtLf1 = B1 * V11;
			constructor.AppendLine("Area tributaria: \nAtL = B1 * V1(1)");
			constructor.AppendLine("AtL = "+B1+" * "+V11+"");
			constructor.AppendLine("AtL = "+AtLf1+" m2.\n");
			brazoL1 = V11 / 2;
			constructor.AppendLine("Brazo de momento: \nx = V1(1) / 2");
			constructor.AppendLine("x = "+V11+" / 2");
			constructor.AppendLine("x = "+brazoL1+" m.\n");
			MuL1 = E11u * AtLf1 * brazoL1;
			constructor.AppendLine("Momento actuante: \nMu = E11u * AtL * x");
			constructor.AppendLine("Mu = "+E11u+" * "+AtLf1+" * "+brazoL1+"");
			constructor.AppendLine("Mu = "+MuL1+" KN-m.\n");
			kL1 = MuL1 / (B1 * d * d);
			constructor.AppendLine("Factor k: \nk = Mu / (B1 * d * d)");
			constructor.AppendLine("k = "+MuL1+" / ("+B1+" * "+d+" * "+d+")");
			constructor.AppendLine("k = "+kL1+"\n");
			//esto se omite de aqui en adelante para los otras zapatas:
			m = fy / (0.85*fc);
			constructor.AppendLine("Factor m: \nm = fy / (0.85*fc)");
			constructor.AppendLine("m = "+fy+" / (0.85*"+fc+")");
			constructor.AppendLine("m = "+m+"\n");
			//

			cuantiadL1 = cuantiad(fc, fy, MuL1, B1, d);
			constructor.AppendLine("Cuantia requerida: \npr = (1 - raiz(1 - 2*m*k/(0.9*fy*1000)))/m");
			constructor.AppendLine("pr = (1 - raiz(1 - 2 * "+m+" * "+kL1+" / (0.9 * "+fy+" * 1000))) / "+m+" ");
			constructor.AppendLine("pr = "+cuantiadL1+"\n");
			AsrL1 = cuantiadL1 * B1 * d;
			constructor.AppendLine("El area minima requerida es: \nAsr = pr * B1 * d");
			constructor.AppendLine("Asr = "+cuantiadL1+" * "+B1+" * "+d+"");
			constructor.AppendLine("Asr = "+AsrL1+" m2.\n");
			AstL1 = 0.0018 * B1 * H;
			constructor.AppendLine("El area minima por retraccion y temperatura es: \nAst = 0.0018 * B1 * H");
			constructor.AppendLine("Ast = 0.0018 * "+B1+" * "+H+"");
			constructor.AppendLine("Ast = "+AstL1+" m2.\n");
			AssL1 = maximo2(AsrL1,AstL1);
			constructor.AppendLine("Area de acero a suministrar: \nAss = max[ AsrL ; AstL ]");
			constructor.AppendLine("Ass = max[ "+AsrL1+" ; "+AstL1+" ]");
			constructor.AppendLine("Ass = "+AssL1+" m2.\n");
			NbL1 = checked(Math.Ceiling(AssL1 / Areabz));
			constructor.AppendLine("El numero de barras requeridas es: \nNb = Ass / Areabz");
			constructor.AppendLine("Nb = "+AssL1+" / "+Areabz+"");
			constructor.AppendLine("Nb = "+NbL1+"\n");
			SccL1 = checked((B1 - 2*rprima - diametrobz)/(NbL1 - 1));
			constructor.AppendLine("Separacion centro a centro entre las barras: \nScc = (B1 - 2*r' - diametrobz)/(NbL1 - 1)");
			constructor.AppendLine("Scc = ("+B1+" - 2*"+rprima+" - "+diametrobz+")/("+NbL1+" - 1)");
			constructor.AppendLine("Scc = "+SccL1+" m.\n");
			SlbL1 = checked(SccL1 - diametrobz);
			constructor.AppendLine("Separacion libre entre las barras: \nSlb = Scc - diametrobz");
			constructor.AppendLine("Slb = "+SccL1+" - "+diametrobz+"");
			constructor.AppendLine("Slb = "+SlbL1+" m.\n");

			//esto se omite de aqui en adelante para los otras zapatas:
			Sminb = maximo2(diametrobz,maximo2(0.025, 1.33*TM));
			constructor.AppendLine("Separacion minima entre las barras: \nSmin = MAX( diametrobz ; 0.025 ; 1.33*TM )");
			constructor.AppendLine("Smin = max( "+diametrobz+" ; 0.025 ; 1.33*"+TM+")");
			constructor.AppendLine("Smin = "+Sminb+" m.\n");
			//

			if (SlbL1<Sminb)
			{
				constructor.AppendLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a L, EMPIECE DE "+
				                  "NUEVO AUMENTANDO EL DIAMETRO DE LA BARRA\n");
			}

			if (SlbL1>0.45)
			{
				constructor.AppendLine("OJO!!! Separación da mas que la maxima, entonces se"+
				                  " toma la maxima que es: 0.45m");
				SlbL1 = 0.45;
				//Recalculamos el numero de barras con separacion = 0.45m
				NbL1 = checked(Math.Ceiling(((B1 - 2*rprima - diametrobz)/SlbL1) + 1));
				constructor.AppendLine("Recalculamos el numero de barras: \nNb = ((B1 - 2*rprima - diametrobz)/SlbL1) + 1");
				constructor.AppendLine("Nb = (("+B1+" - 2*"+rprima+" - "+diametrobz+") / "+SlbL1+" ) + 1");
				constructor.AppendLine("Nb = "+NbL1+"\n");
			}

			if (SlbL1<=0.45 && SlbL1>Sminb)
			{
				constructor.AppendLine(" ==> Cumple las separaciones: "+Sminb+" < "+SlbL1+" < 0.45\n\n");
			}

			AssrL1 = checked(NbL1 * Areabz);
			constructor.AppendLine("El area suministrada es: \nAssr = Nb * Areabz");
			constructor.AppendLine("Assr = "+NbL1+" * "+Areabz+"");
			constructor.AppendLine("Assr = "+AssrL1+" m2.\n");
			if (AsrL1<AstL1)
			{
				cuantiaLsum1 = checked(AssrL1 / (B1 * H));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (B1 * H)");
				constructor.AppendLine("psum = "+AssrL1+" / ("+B1+" * "+H+"");
				constructor.AppendLine("psum = "+cuantiaLsum1+"\n");
			}

			if (AsrL1>AstL1)
			{
				cuantiaLsum1 = checked(AssrL1 /(B1 * d));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (B1 * d)");
				constructor.AppendLine("psum = "+AssrL1+" / ("+B1+" * "+d+")");
				constructor.AppendLine("psum = "+cuantiaLsum1+"\n");
			}

			constructor.AppendLine("\nLongitud de Anclaje: \n");
			lddL1 = V11 - rprima;
			constructor.AppendLine("Longitud disponible: \nldd = V1(1) - rprima");
			constructor.AppendLine("ldd = "+V11+" - "+rprima+"");
			constructor.AppendLine("ldd = "+lddL1+" m.\n");
			constructor.AppendLine("Longitud de desarrollo requerida por la barra a traccion sin gancho:");
			cc1 = SccL1 / 2;
			cc2 = r + diametrobz/2;
			cc = minimo2(cc1,cc2);
			relccdbz = cc / diametrobz;
			constructor.AppendLine("Se tiene: c = min ( cc1 ; cc2 )");
			constructor.AppendLine("c = min ( Sccl1/2 ; r + diametrobz/2 ) = min ( "+SccL1+"/2 ; "+r+" + "+diametrobz+"/2 )");
			constructor.AppendLine("c = min ("+cc1+" ; "+cc2+") = "+cc+"");
			constructor.AppendLine("Se tiene la relacion: c/dbz = "+cc+" / "+diametrobz+" = "+relccdbz+"\n");
			if (relccdbz > 2.5)
			{
				relccdbz = 2.5;
				constructor.AppendLine("Se toma la relacion: c/dbz = "+relccdbz+"\n");
			}

			factorAsL1 = AssL1 / AssrL1;
			ldrL1 = (fy * diametrobz / (1.1*Math.Pow(fc,0.5) * relccdbz) ) * factorAsL1;
			constructor.AppendLine("Calculamos un factor de reduccion por area de acero suministrada:");
			constructor.AppendLine("factorAs = Ass / Assr = "+AssL1+" / "+AssrL1+" = "+factorAsL1+"\n");

			constructor.AppendLine("Calculamos la longitud requerida por la barra: \nldr = (fy * diametrobz / (1.1*raiz(fc) * relccdbz) ) * factorAs");
			constructor.AppendLine("ldr = ( "+fy+" * "+diametrobz+" / (1.1 * raiz("+fc+") * "+relccdbz+" ) ) * "+factorAsL1+"");
			constructor.AppendLine("ldr = "+ldrL1+" m.\n");
			if (ldrL1<0.3)
			{
				ldrL1 = 0.3;
				constructor.AppendLine("Se toma ldr = "+ldrL1+" m, que es la minima por norma");
			}

			if (ldrL1>lddL1)
			{
				constructor.AppendLine("!!!Ojo!!! no hay espacio suficiente para desarrollar las barras"+
				                  " emplee ganchos o disminuya el diametro de la barra.\n");
			}

			//longitud de corte de las barras redondeadas al cm
			lcorteL1 = Math.Round(L1 - 2*rprima,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("La longitud de corte de las barras es: \nLC = L1 - 2*rprima");
			constructor.AppendLine("LC = "+L1+" - 2*"+rprima+"");
			constructor.AppendLine("LC = "+lcorteL1+" m.\n");

			constructor.AppendLine("\nAcero de refuerzo paralelo a 'B'.\n");
			AtBf1 = L1 * V21;
			constructor.AppendLine("Area tributaria: \nAtB = L1 * V2(1)");
			constructor.AppendLine("AtB = "+L1+" * "+V21+"");
			constructor.AppendLine("AtB = "+AtBf1+" m2.\n");
			brazoB1 = V21 / 2;
			constructor.AppendLine("Brazo de momento: \nx = V2(1) / 2");
			constructor.AppendLine("x = "+V21+" / 2");
			constructor.AppendLine("x = "+brazoB1+" m.\n");
			MuB1 = E21u * AtBf1 * brazoB1;
			constructor.AppendLine("Momento actuante: \nMu = E21u * AtB * x");
			constructor.AppendLine("Mu = "+E21u+" * "+AtBf1+" * "+brazoB1+"");
			constructor.AppendLine("Mu = "+MuB1+" KN-m.\n");
			kB1 = MuB1 / (L1 * d * d);
			constructor.AppendLine("Factor k: \nk = Mu / (L1 * d * d)");
			constructor.AppendLine("k = "+MuB1+" / ("+L1+" * "+d+" * "+d+")");
			constructor.AppendLine("k = "+kB1+"\n");
			//esto se omite de aqui en adelante para los otras zapatas:
			m = fy / (0.85*fc);
			constructor.AppendLine("Factor m: \nm = fy / (0.85*fc)");
			constructor.AppendLine("m = "+fy+" / (0.85*"+fc+")");
			constructor.AppendLine("m = "+m+"\n");
			//

			cuantiadB1 = cuantiad(fc, fy, MuB1, L1, d);
			constructor.AppendLine("Cuantia requerida: \npr = (1 - raiz(1 - 2*m*k/(0.9*fy*1000)))/m");
			constructor.AppendLine("pr = (1 - raiz(1 - 2 * "+m+" * "+kB1+" / (0.9 * "+fy+" * 1000))) / "+m+" ");
			constructor.AppendLine("pr = "+cuantiadB1+"\n");
			AsrB1 = cuantiadB1 * L1 * d;
			constructor.AppendLine("El area minima requerida es: \nAsr = pr * L1 * d");
			constructor.AppendLine("Asr = "+cuantiadB1+" * "+L1+" * "+d+"");
			constructor.AppendLine("Asr = "+AsrB1+" m2.\n");
			AstB1 = 0.0018 * L1 * H;
			constructor.AppendLine("El area minima por retraccion y temperatura es: \nAst = 0.0018 * L1 * H");
			constructor.AppendLine("Ast = 0.0018 * "+L1+" * "+H+"");
			constructor.AppendLine("Ast = "+AstB1+" m2.\n");
			AssB1 = maximo2(AsrB1,AstB1);
			constructor.AppendLine("Area de acero a suministrar: \nAss = max[ AsrL ; AstL ]");
			constructor.AppendLine("Ass = max[ "+AsrB1+" ; "+AstB1+" ]");
			constructor.AppendLine("Ass = "+AssB1+" m2.\n");
			NbB1 = checked(Math.Ceiling(AssB1 / Areabz));
			constructor.AppendLine("El numero de barras requeridas es: \nNb = Ass / Areabz");
			constructor.AppendLine("Nb = "+AssB1+" / "+Areabz+"");
			constructor.AppendLine("Nb = "+NbB1+"\n");
			SccB1 = checked((L1 - 2*rprima - diametrobz)/(NbB1 - 1));
			constructor.AppendLine("Separacion centro a centro entre las barras: \nScc = (L1 - 2*r' - diametrobz)/(NbB1 - 1)");
			constructor.AppendLine("Scc = ("+L1+" - 2*"+rprima+" - "+diametrobz+")/("+NbB1+" - 1)");
			constructor.AppendLine("Scc = "+SccB1+" m.\n");
			SlbB1 = checked(SccB1 - diametrobz);
			constructor.AppendLine("Separacion libre entre las barras: \nSlb = Scc - diametrobz");
			constructor.AppendLine("Slb = "+SccB1+" - "+diametrobz+"");
			constructor.AppendLine("Slb = "+SlbB1+" m.\n");

			//esto se omite de aqui en adelante para los otras zapatas:
			Sminb = maximo2(diametrobz,maximo2(0.025, 1.33*TM));
			constructor.AppendLine("Separacion minima entre las barras: \nSmin = MAX( diametrobz ; 0.025 ; 1.33*TM )");
			constructor.AppendLine("Smin = max( "+diametrobz+" ; 0.025 ; 1.33*"+TM+")");
			constructor.AppendLine("Smin = "+Sminb+" m.\n");
			//

			if (SlbB1<Sminb)
			{
				constructor.AppendLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a L, EMPIECE DE "+
				                       "NUEVO AUMENTANDO EL DIAMETRO DE LA BARRA\n");
			}

			if (SlbB1>0.45)
			{
				constructor.AppendLine("OJO!!! Separación da mas que la maxima, entonces se"+
				                       " toma la maxima que es: 0.45m");
				SlbB1 = 0.45;
				//Recalculamos el numero de barras con separacion = 0.45m
				NbB1 = checked(Math.Ceiling(((L1 - 2*rprima - diametrobz)/SlbB1) + 1));
				constructor.AppendLine("Recalculamos el numero de barras: \nNb = ((L1 - 2*rprima - diametrobz)/SlbB1) + 1");
				constructor.AppendLine("Nb = (("+L1+" - 2*"+rprima+" - "+diametrobz+") / "+SlbB1+" ) + 1");
				constructor.AppendLine("Nb = "+NbB1+"\n");
			}

			if (SlbB1<=0.45 && SlbB1>Sminb)
			{
				constructor.AppendLine(" ==> Cumple las separaciones: "+Sminb+" < "+SlbB1+" < 0.45\n\n");
			}

			AssrB1 = checked(NbB1 * Areabz);
			constructor.AppendLine("El area suministrada es: \nAssr = Nb * Areabz");
			constructor.AppendLine("Assr = "+NbB1+" * "+Areabz+"");
			constructor.AppendLine("Assr = "+AssrB1+" m2.\n");
			if (AsrB1<AstB1)
			{
				cuantiaBsum1 = checked(AssrB1 / (L1 * H));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (L1 * H)");
				constructor.AppendLine("psum = "+AssrB1+" / ("+L1+" * "+H+"");
				constructor.AppendLine("psum = "+cuantiaBsum1+"\n");
			}

			if (AsrB1>AstB1)
			{
				cuantiaBsum1 = checked(AssrB1 /(L1 * d));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (L1 * d)");
				constructor.AppendLine("psum = "+AssrB1+" / ("+L1+" * "+d+")");
				constructor.AppendLine("psum = "+cuantiaBsum1+"\n");
			}

			constructor.AppendLine("\nLongitud de Anclaje: \n");
			lddB1 = V21 - rprima;
			constructor.AppendLine("Longitud disponible: \nldd = V2(1) - rprima");
			constructor.AppendLine("ldd = "+V21+" - "+rprima+"");
			constructor.AppendLine("ldd = "+lddB1+" m.\n");
			constructor.AppendLine("Longitud de desarrollo requerida por la barra a traccion sin gancho:");
			cc1 = SccB1 / 2;
			cc2 = r + diametrobz/2;
			cc = minimo2(cc1,cc2);
			relccdbz = cc / diametrobz;
			constructor.AppendLine("Se tiene: c = min ( cc1 ; cc2 )");
			constructor.AppendLine("c = min ( Sccl1/2 ; r + diametrobz/2 ) = min ( "+SccB1+"/2 ; "+r+" + "+diametrobz+"/2 )");
			constructor.AppendLine("c = min ("+cc1+" ; "+cc2+") = "+cc+"");
			constructor.AppendLine("Se tiene la relacion: c/dbz = "+cc+" / "+diametrobz+" = "+relccdbz+"\n");
			if (relccdbz > 2.5)
			{
				relccdbz = 2.5;
				constructor.AppendLine("Se toma la relacion: c/dbz = "+relccdbz+"\n");
			}

			factorAsB1 = AssB1 / AssrB1;
			ldrB1 = (fy * diametrobz / (1.1*Math.Pow(fc,0.5) * relccdbz) ) * factorAsB1;
			constructor.AppendLine("Calculamos un factor de reduccion por area de acero suministrada:");
			constructor.AppendLine("factorAs = Ass / Assr = "+AssB1+" / "+AssrB1+" = "+factorAsB1+"\n");

			constructor.AppendLine("Calculamos la longitud requerida por la barra: \nldr = (fy * diametrobz / (1.1*raiz(fc) * relccdbz) ) * factorAs");
			constructor.AppendLine("ldr = ( "+fy+" * "+diametrobz+" / (1.1 * raiz("+fc+") * "+relccdbz+" ) ) * "+factorAsB1+"");
			constructor.AppendLine("ldr = "+ldrB1+" m.\n");
			if (ldrB1<0.3)
			{
				ldrB1 = 0.3;
				constructor.AppendLine("Se toma ldr = "+ldrB1+" m, que es la minima por norma");
			}

			if (ldrB1>lddB1)
			{
				constructor.AppendLine("!!!Ojo!!! no hay espacio suficiente para desarrollar las barras"+
				                       " emplee ganchos o disminuya el diametro de la barra.\n");
			}

			//longitud de corte de las barras redondeadas al cm
			lcorteB1 = Math.Round(B1 - 2*rprima,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("La longitud de corte de las barras es: \nLC = B1 - 2*rprima");
			constructor.AppendLine("LC = "+B1+" - 2*"+rprima+"");
			constructor.AppendLine("LC = "+lcorteB1+" m.\n");
			//

			constructor.AppendLine("\nZapata de Interior.\n\n");

			AtLf2 = B2 * V12;
			constructor.AppendLine("Area tributaria: \nAtL = B2 * V1(2)");
			constructor.AppendLine("AtL = "+B2+" * "+V12+"");
			constructor.AppendLine("AtL = "+AtLf2+" m2.\n");
			brazoL2 = V12 / 2;
			constructor.AppendLine("Brazo de momento: \nx = V1(2) / 2");
			constructor.AppendLine("x = "+V12+" / 2");
			constructor.AppendLine("x = "+brazoL2+" m.\n");
			MuL2 = E21u * AtLf2 * brazoL2;
			constructor.AppendLine("Momento actuante: \nMu = E21u * AtL * x");
			constructor.AppendLine("Mu = "+E21u+" * "+AtLf2+" * "+brazoL2+"");
			constructor.AppendLine("Mu = "+MuL2+" KN-m.\n");
			kL2 = MuL2 / (B2 * d * d);
			constructor.AppendLine("Factor k: \nk = Mu / (B1 * d * d)");
			constructor.AppendLine("k = "+MuL2+" / ("+B2+" * "+d+" * "+d+")");
			constructor.AppendLine("k = "+kL2+"\n");
			//esto se omite de aqui en adelante para los otras zapatas:
			m = fy / (0.85*fc);
			constructor.AppendLine("Factor m: \nm = fy / (0.85*fc)");
			constructor.AppendLine("m = "+fy+" / (0.85*"+fc+")");
			constructor.AppendLine("m = "+m+"\n");
			//

			cuantiadL2 = cuantiad(fc, fy, MuL2, B2, d);
			constructor.AppendLine("Cuantia requerida: \npr = (1 - raiz(1 - 2*m*k/(0.9*fy*1000)))/m");
			constructor.AppendLine("pr = (1 - raiz(1 - 2 * "+m+" * "+kL2+" / (0.9 * "+fy+" * 1000))) / "+m+" ");
			constructor.AppendLine("pr = "+cuantiadL2+"\n");
			AsrL2 = cuantiadL2 * B2 * d;
			constructor.AppendLine("El area minima requerida es: \nAsr = pr * B2 * d");
			constructor.AppendLine("Asr = "+cuantiadL2+" * "+B2+" * "+d+"");
			constructor.AppendLine("Asr = "+AsrL2+" m2.\n");
			AstL2 = 0.0018 * B2 * H;
			constructor.AppendLine("El area minima por retraccion y temperatura es: \nAst = 0.0018 * B2 * H");
			constructor.AppendLine("Ast = 0.0018 * "+B2+" * "+H+"");
			constructor.AppendLine("Ast = "+AstL2+" m2.\n");
			AssL2 = maximo2(AsrL2,AstL2);
			constructor.AppendLine("Area de acero a suministrar: \nAss = max[ AsrL ; AstL ]");
			constructor.AppendLine("Ass = max[ "+AsrL2+" ; "+AstL2+" ]");
			constructor.AppendLine("Ass = "+AssL2+" m2.\n");
			NbL2 = checked(Math.Ceiling(AssL2 / Areabz));
			constructor.AppendLine("El numero de barras requeridas es: \nNb = Ass / Areabz");
			constructor.AppendLine("Nb = "+AssL2+" / "+Areabz+"");
			constructor.AppendLine("Nb = "+NbL2+"\n");
			SccL2 = checked((B2 - 2*rprima - diametrobz)/(NbL2 - 1));
			constructor.AppendLine("Separacion centro a centro entre las barras: \nScc = (B2 - 2*r' - diametrobz)/(NbL2 - 1)");
			constructor.AppendLine("Scc = ("+B2+" - 2*"+rprima+" - "+diametrobz+")/("+NbL2+" - 1)");
			constructor.AppendLine("Scc = "+SccL2+" m.\n");
			SlbL2 = checked(SccL2 - diametrobz);
			constructor.AppendLine("Separacion libre entre las barras: \nSlb = Scc - diametrobz");
			constructor.AppendLine("Slb = "+SccL2+" - "+diametrobz+"");
			constructor.AppendLine("Slb = "+SlbL2+" m.\n");

			//esto se omite de aqui en adelante para los otras zapatas:
			Sminb = maximo2(diametrobz,maximo2(0.025, 1.33*TM));
			constructor.AppendLine("Separacion minima entre las barras: \nSmin = MAX( diametrobz ; 0.025 ; 1.33*TM )");
			constructor.AppendLine("Smin = max( "+diametrobz+" ; 0.025 ; 1.33*"+TM+")");
			constructor.AppendLine("Smin = "+Sminb+" m.\n");
			//

			if (SlbL2<Sminb)
			{
				constructor.AppendLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a L, EMPIECE DE "+
				                       "NUEVO AUMENTANDO EL DIAMETRO DE LA BARRA\n");
			}


			if (SlbL2>0.45)
			{
				constructor.AppendLine("OJO!!! Separación da mas que la maxima, entonces se"+
				                       " toma la maxima que es: 0.45m");
				SlbL2 = 0.45;
				//Recalculamos el numero de barras con separacion = 0.45m
				NbL2 = checked(Math.Ceiling(((B2 - 2*rprima - diametrobz)/SlbL2) + 1));
				constructor.AppendLine("Recalculamos el numero de barras: \nNb = ((B2 - 2*rprima - diametrobz)/SlbL2) + 1");
				constructor.AppendLine("Nb = (("+B2+" - 2*"+rprima+" - "+diametrobz+") / "+SlbL2+" ) + 1");
				constructor.AppendLine("Nb = "+NbL2+"\n");
			}

			if (SlbL2<=0.45 && SlbL2>Sminb)
			{
				constructor.AppendLine(" ==> Cumple las separaciones: "+Sminb+" < "+SlbL2+" < 0.45\n\n");
			}

			AssrL2 = checked(NbL2 * Areabz);
			constructor.AppendLine("El area suministrada es: \nAssr = Nb * Areabz");
			constructor.AppendLine("Assr = "+NbL2+" * "+Areabz+"");
			constructor.AppendLine("Assr = "+AssrL2+" m2.\n");
			if (AsrL2<AstL2)
			{
				cuantiaLsum2 = checked(AssrL2 / (B2 * H));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (B2 * H)");
				constructor.AppendLine("psum = "+AssrL2+" / ("+B2+" * "+H+"");
				constructor.AppendLine("psum = "+cuantiaLsum2+"\n");
			}

			if (AsrL2>AstL2)
			{
				cuantiaLsum2 = checked(AssrL2 /(B2 * d));
				constructor.AppendLine("La cuantia suministrada es: \npsum = Assr / (B2 * d)");
				constructor.AppendLine("psum = "+AssrL2+" / ("+B2+" * "+d+")");
				constructor.AppendLine("psum = "+cuantiaLsum2+"\n");
			}

			constructor.AppendLine("\nLongitud de Anclaje: \n");
			lddL2 = V12 - rprima;
			constructor.AppendLine("Longitud disponible: \nldd = V1(2) - rprima");
			constructor.AppendLine("ldd = "+V12+" - "+rprima+"");
			constructor.AppendLine("ldd = "+lddL2+" m.\n");
			constructor.AppendLine("Longitud de desarrollo requerida por la barra a traccion sin gancho:");
			cc1 = SccL2 / 2;
			cc2 = r + diametrobz/2;
			cc = minimo2(cc1,cc2);
			relccdbz = cc / diametrobz;
			constructor.AppendLine("Se tiene: c = min ( cc1 ; cc2 )");
			constructor.AppendLine("c = min ( Sccl2/2 ; r + diametrobz/2 ) = min ( "+SccL2+"/2 ; "+r+" + "+diametrobz+"/2 )");
			constructor.AppendLine("c = min ("+cc1+" ; "+cc2+") = "+cc+"");
			constructor.AppendLine("Se tiene la relacion: c/dbz = "+cc+" / "+diametrobz+" = "+relccdbz+"\n");
			if (relccdbz > 2.5)
			{
				relccdbz = 2.5;
				constructor.AppendLine("Se toma la relacion: c/dbz = "+relccdbz+"\n");
			}

			factorAsL2 = AssL2 / AssrL2;
			ldrL2 = (fy * diametrobz / (1.1*Math.Pow(fc,0.5) * relccdbz) ) * factorAsL2;
			constructor.AppendLine("Calculamos un factor de reduccion por area de acero suministrada:");
			constructor.AppendLine("factorAs = Ass / Assr = "+AssL2+" / "+AssrL2+" = "+factorAsL2+"\n");

			constructor.AppendLine("Calculamos la longitud requerida por la barra: \nldr = (fy * diametrobz / (1.1*raiz(fc) * relccdbz) ) * factorAs");
			constructor.AppendLine("ldr = ( "+fy+" * "+diametrobz+" / (1.1 * raiz("+fc+") * "+relccdbz+" ) ) * "+factorAsL2+"");
			constructor.AppendLine("ldr = "+ldrL2+" m.\n");
			if (ldrL2<0.3)
			{
				ldrL2 = 0.3;
				constructor.AppendLine("Se toma ldr = "+ldrL2+" m, que es la minima por norma");
			}

			if (ldrL2>lddL2)
			{
				constructor.AppendLine("!!!Ojo!!! no hay espacio suficiente para desarrollar las barras"+
				                       " emplee ganchos o disminuya el diametro de la barra.\n");
			}

			//longitud de corte de las barras redondeadas al cm
			lcorteL2 = Math.Round(L2 - 2*rprima,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("La longitud de corte de las barras es: \nLC = L2 - 2*rprima");
			constructor.AppendLine("LC = "+L2+" - 2*"+rprima+"");
			constructor.AppendLine("LC = "+lcorteL2+" m.\n");
			constructor.AppendLine("Como la zapata de interior es cuadrada, "+
			                       "se tiene el mismo refuerzo en ambas direcciones.\n\n");

			/////Diseño de la viga de enlace:

			constructor.AppendLine("\n\nDISENO DE LA VIGA DE ENLACE.\n\n");
			constructor.AppendLine("\nDiseño a Flexion.\n\n");
			bw = b1 ;
			constructor.AppendLine("Se toma un ancho efectivo de la viga minimo, igual a al ancho de la "+
			                       "columna de lindero:\nbw = b1 = "+bw+" m.\n");
			lprima = Math.Round(l - (L1 - a1/2) - L2/2,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("Distancia libre entre zapatas:\nlprima = l - (L1 - a1/2) - L2/2");
			constructor.AppendLine("lprima = "+l+" - ("+L1+" - "+a1+"/2) - "+L2+"/2");
			constructor.AppendLine("lprima = "+lprima+" m.\n");

			hmin1 = Math.Round(l / 16,2,MidpointRounding.AwayFromZero);
			hmin2 = Math.Round(lprima / 7,2,MidpointRounding.AwayFromZero);
			hmin3 = H;
			hmin = maximo2(hmin1,maximo2(hmin2,hmin3));
			constructor.AppendLine("Guia:\nhmin1 = l / 16 = "+l+" / 16 = "+hmin1+" m.\n");
			constructor.AppendLine("hmin2 = lprima / 7 = "+lprima+" / 7 = "+hmin2+" m.\n");
			constructor.AppendLine("hmin3 = H = "+hmin3+" m.\n");
			constructor.AppendLine("Escogemos el maximo de los tres: MAX [ hmin1 ; hmin2 ; hmin3 ]");
			constructor.AppendLine("hv = MAX [ "+hmin1+" ; "+hmin2+" ; "+hmin3+" ]");
			constructor.AppendLine("hv = "+hmin+" m.\n");


			hmin = maximo2(hmin, bw*2+0.05);
			constructor.AppendLine("Se toma:\nhv = "+hmin+" m.\n");


			LM = l - V11 - a1/2;
			constructor.AppendLine("Calculamos la distancia: LM = l - V1(1) - a1/2");
			constructor.AppendLine("LM = "+l+" - "+V11+" - "+a1+"/2");
			constructor.AppendLine("LM = "+LM+" m.\n");
			Muv = DR1u * LM;
			constructor.AppendLine("Momento Actuante:\nMuv = DR(1u) * LM");
			constructor.AppendLine("Muv = "+DR1u+" * "+LM+"");
			constructor.AppendLine("Muv = "+Muv+" KN-m.\n");
			pmin = 0.25 * Math.Pow(fc,0.5) / fy; 
			constructor.AppendLine("Cuantia de minima:\npmin = 0.25 * raiz(f'c) / fy");
			constructor.AppendLine("pmin = 0.25 * raiz("+fc+") / "+fy+"");
			constructor.AppendLine("pmin = "+pmin+"\n");
			double psum, dv;
			dv = hmin - (32.5/1000)/2 - 0.075;

			hmin = Math.Round(dv + (32.5/1000)/2 + 0.075,2,MidpointRounding.AwayFromZero);
			constructor.AppendLine("Tenemos que: hv = "+hmin+" m.\n");
			prv = cuantiad(fc, fy, Muv, bw, dv);
			psum = maximo2(pmin, prv);
			constructor.AppendLine("Cuantia de diseño:\nprv = "+prv+"\n");
			Asv =  maximo2(pmin, prv) * bw * dv;
			constructor.AppendLine("Area de acero a sumunistrar:\nAsv = MAX ( pmin , prv) * bw * dv");
			constructor.AppendLine("Asv = MAX ( "+pmin+" , "+prv+" ) * "+bw+" * "+dv+"");
			constructor.AppendLine("Asv = ( "+psum+" ) * "+bw+" * "+dv+"");
			constructor.AppendLine("Asv = "+Asv+" m2.\n");

			Nbv = Math.Ceiling( Asv / 0.000819 );
			constructor.AppendLine("Numero de barras:\nNbv = Asv / AreabN10");
			constructor.AppendLine("Nbv = "+Asv+" / 0.000819");
			constructor.AppendLine("Nbv = "+Nbv+"\n");
			Sccv = ( bw - 2*rprima - 2*0.0095 - 32.5/1000 ) / ( Nbv - 1 );
			Slbv = Sccv - 32.5/1000;
			constructor.AppendLine("Separacion centro a centro con refuerzo igual al de columna y empleando "+
			                       "estribos N3:\nSccv = ( bw - 2*rprima - 2*0.0095 - diametrobN10 ) / ( Nbv - 1 )");
			constructor.AppendLine("Sccv = ( "+bw+" - 2*"+rprima+" - 2*0.0095 - "+32.5/1000+" ) / ( "+Nbv+" - 1 )");
			constructor.AppendLine("Sccv = "+Sccv+" m.\n");
			constructor.AppendLine("Separacion libre:\nSlbv = Sccv - diametrobN10");
			constructor.AppendLine("Slbv = "+Sccv+" - "+32.5/1000+"");
			constructor.AppendLine("Slbv = "+Slbv+" m.\n");

			if (Slbv<Sminb)
			{
				constructor.AppendLine("!!!! No cumple Separacion !!!!");
				//aqui hay que ver como hacer para que cambie el reuferzo
			}


			constructor.AppendLine("\nDiseño a Cortante.\n\n");
			constructor.AppendLine("");
			constructor.AppendLine("RESUMEN FINAL:");
			constructor.AppendLine("H = "+H+", AsrB1="+AsrB1+",AssB1= "+AssB1+", NbB1="+NbB1+", cuantiaBsum1="+cuantiaBsum1+", AsrL1="+AsrL1+", AssL1="+AssL1+", NbL1="+NbL1+", cuantiaLsum1="+cuantiaLsum1+"");
			constructor.AppendLine("zapata de interior: AsrL2="+AsrL2+", AssL2="+AssL2+", NbL2="+NbL2+", cuantiaLsum2="+cuantiaLsum2+" ");
			constructor.AppendLine("viga enlace: hv="+hmin+", bw="+bw+", Asv="+Asv+", psum="+psum+", Nbv ="+Nbv+" ");

		}
		//
		static void Exportar()
		{
			String cadena = "";
			cadena=constructor.ToString();
			FileStream fs = new FileStream("/home/juan/Desktop/combinaciones_p1_300/Cimentacion:"+nombrezapata+".txt", FileMode.Append);
			fs.Write(ASCIIEncoding.ASCII.GetBytes(cadena), 0, cadena.Length);
			fs.Close();
			Console.WriteLine("Exportacion correcta!");
		}
		//metodo para calcular el maximo entre dos numeros:
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
		//metodo newton polinomio grado 5:
		static double nr5(double x0, double c5, double c4, double c3, double c2, double c1, double c0)
		{
			double a, fa, dfa, x1, error, errorp=0.01;

			do  {

				a = x0;
				fa = c5*Math.Pow(a,5) + c4*Math.Pow(a,4) + c3*Math.Pow(a,3) + c2*Math.Pow(a,2) + c1*Math.Pow(a,1) + c0;
				dfa = 5*c5*Math.Pow(a,4) + 4*c4*Math.Pow(a,3) + 3*c3*Math.Pow(a,2) + 2*c2*Math.Pow(a,1) + 1*c1;
				x1 = x0 - (fa/dfa);
				error = Math.Abs(100*(x1-x0)/x1);
				x0 = x1;

			} while (error > errorp);
			return x1;
		}
		//metodo para calcular el minimo entre dos numeros:
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
		//

		//
	}
}
