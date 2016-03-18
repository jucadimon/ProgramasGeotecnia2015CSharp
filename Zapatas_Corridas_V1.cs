using System;

namespace Geotecnia_3
{
	public class ZapatasCorridas
	{
		//precios del APU de Zapata
		//se deben actualizar
		static double cexc=22292, cacero=12952, ccon=82171, czap=55419, con3000=260200, A420=2800, Hmenor=500, mezcladora=8500, vibrador=5000;
		static string eleccion="";
		
		public static void Main()
		{
			ZapataCorridaparaMuro();
			Console.ReadKey();
		}
	static void ZapataCorridaparaMuro()
			{
				ZapataCorridaparaMuroPrediseño();
				Console.ReadKey();
			}
	static void ZapataCorridaparaMuroPrediseño()
			{
				Console.WriteLine("\t\t\t INGENIERIA CIVIL \n");
				Console.WriteLine("\t\t 1.Zapata Corrida para Muro de Concreto \n");
				
				double PD=0, PL=0, P1=0, P1u=0;
				double fc=21, fy=420, dbm=0, dbz=0, rprima=0.075;
				double r=0, H=0, Hmin=0, ldc=0, FC=1, d1=0.15, d2=0, d3=0, d=0, Ddoblamiento=0;
				double df=0, rsuelo=0, rconcreto=24, TM=0;
				double EsfuerzoNeto=0, EsfuerzoAdmisible=100, Sobrecarga=0;
				double areareq=0, L=0, areafinal=0;
				double anchomuro=0, lvoladizo=0, rel=0;
				double Esfuerzo1u=0;
				double V1u=0, Atributaria=0, fiVc=0, ficortante=0.75, landa=1;
				double AstB=0, Asbz=0, NbB=0, AstBsum=0, cuantiaBsum=0;
				double SccB=0, SlbB=0, Sminb=0;
				double refuerzo=0, M1u=0;
				double AsrL, AsfL, AssL, AstL;
				double NbL, SccL, SlbL;
				double AssrL, cuantiaLsum;
				
						
				
				Console.WriteLine("Ingrese Carga Muerta en KN:");
				double.TryParse(Console.ReadLine(),out PD);
				Console.WriteLine("Ingrese Carga Viva en KN:");
				double.TryParse(Console.ReadLine(),out PL);
					P1=checked(PD+PL);
					P1u=checked(1.2*PD+1.6*PL);
				Console.WriteLine("\t\t\t Calculo de las cargas de diseño. \n");
				Console.WriteLine("Carga Muerta:"+PD+"KN");
				Console.WriteLine("Carga Viva:"+PL+"KN");
				Console.WriteLine("Carga de Servicio={0}+{1}",PD,PL);
				Console.WriteLine("Carga de Servicio:"+P1+"KN");
				Console.WriteLine("Carga Mayorada=1,2*{0}+1,6*{1}",PD,PL);
				Console.WriteLine("Carga Mayorada:"+P1u+"KN \n");
				
				Console.WriteLine("Ingrese f'c en MPa:");
				double.TryParse(Console.ReadLine(),out fc);
				Console.WriteLine("Ingrese Tamaño Maximo del agregado en centimetros:");
				double.TryParse(Console.ReadLine(),out TM);
				
				Console.WriteLine("¿Colocará estribos N4@10cm para el acero de la columna?");
				Console.WriteLine("Digite: si ó no");
				eleccion=Console.ReadLine();
					if (eleccion=="si")
						{
							FC=0.75;
						}
						
					if (eleccion=="no")
						{
							FC=1;
						}
						
				Console.WriteLine("FC={0} \n",FC);
				
				Console.WriteLine("Ingrese diámetro del refuerzo para muro en centimetros:");
					double.TryParse(Console.ReadLine(),out dbm);
				Console.WriteLine("Ingrese diámetro del refuerzo para la zapata en centimetros:");
					double.TryParse(Console.ReadLine(),out dbz);
				Console.WriteLine("Ingrese la profundidad de desplante en metros:");
					double.TryParse(Console.ReadLine(),out df);
				Console.WriteLine("Ingrese el Peso Especifico del suelo de relleno en KN/m3:");
					double.TryParse(Console.ReadLine(),out rsuelo);
				Console.WriteLine("Ingrese el Esfuerzo Admisible del suelo en KN/m2:");
					double.TryParse(Console.ReadLine(),out EsfuerzoAdmisible);
				
					r=checked(rprima+dbz/100);
					Hmin=checked(d1+r);
					ldc=maximo2(0.24*fy*dbm/100*FC/Math.Pow(fc, 0.5),0.043*dbm/100*fy*FC);
					Ddoblamiento=checked(6*dbm/100);
					d2=checked(ldc+Ddoblamiento/2+dbm/100+dbz/100);
					d3=checked(P1u/(250*Math.Pow(fc, 0.5)+2.8*EsfuerzoAdmisible));
					d=maximo2(d2,d3);
					H=Math.Round(maximo2(Hmin, d+r),2,MidpointRounding.AwayFromZero);
					d=H-r;
					
					Sobrecarga=checked(rsuelo*(df-H)+rconcreto*H);
					EsfuerzoNeto=checked(EsfuerzoAdmisible-Sobrecarga);
					areareq=checked(P1/EsfuerzoNeto);
					
					areafinal=areareq;
					L=Math.Round(areafinal,2,MidpointRounding.AwayFromZero);
				
				Console.WriteLine("\t\t\t Calculo del Espesor y Area del cimiento. \n");
				Console.WriteLine("El recubrimiento: r = r'+dbz");
				Console.WriteLine("r ={0}+{1}/100",rprima,dbz);
				Console.WriteLine("r = "+r+"m \n");
				Console.WriteLine("El espesor H minimo por norma es: Hmin = d1+r");
				Console.WriteLine("Hmin = {0}+{1}",d1,r);
				Console.WriteLine("Hmin = {0}m \n",Hmin);
				Console.WriteLine("La longitud de desarrollo a compresion \nldc = max(0.24*fy*dbm/100*FC/raiz(f'c) ó 0.043*dbm/100*fy*FC)");
				Console.WriteLine("ldc = max(0.24*{0}*{1}/100*{2}/raiz({3}) ó 0.043*{4}/100*{5}*{6})",fy,dbm,FC,fc,dbm,fy,FC);
				Console.WriteLine("ldc = {0}m \n",ldc);
				Console.WriteLine("El diámetro de doblamiento para la barra de columna es: Dd = 6*db");
				Console.WriteLine("Dd = 6*{0}/100",dbm);
				Console.WriteLine("Dd = {0}m \n",Ddoblamiento);
				Console.WriteLine("El peralte: d2 = ldc+Ddoblamiento/2+dbm/100+dbz/100");
				Console.WriteLine("d2 = {0}+{1}/2+{2}/100+{3}/100",ldc,Ddoblamiento,dbm,dbz);
				Console.WriteLine("d2 = {0}m \n",d2);
				Console.WriteLine("El peralte: d3 = P1u/(250*raiz(f'c)+2.8*EsfuerzoAdmisible)");
				Console.WriteLine("d3 = {0}/(250*raiz({1})+2.8*{2})",P1u,fc,EsfuerzoAdmisible);
				Console.WriteLine("d3 = {0}m \n",d3);
				Console.WriteLine("El peralte minimo es: d = max(d1,d2,d3)");
				Console.WriteLine("d = max({0},{1},{2})",d1,d2,d3);
				Console.WriteLine("d ="+d+"m \n");
				Console.WriteLine("El Espesor minimo es: H = max(Hmin,d+r)");
				Console.WriteLine("H = max({0},{1}+{2})",Hmin,d,r);
				Console.WriteLine("H = {0}m \n",H);
				Console.WriteLine("La sobrecarga es: qw = rsuelo*(df-H)+rconcreto*H");
				Console.WriteLine("qw = {0}*({1}-{2})+{3}*{4}",rsuelo,df,H,rconcreto,H);
				Console.WriteLine("qw = {0}KN/m2 \n",Sobrecarga);
				Console.WriteLine("El Esfuerzo neto es: EsfNeto = EsfuerzoAdmisible-Sobrecarga");
				Console.WriteLine("EsfNeto = {0}-{1}",EsfuerzoAdmisible,Sobrecarga);
				Console.WriteLine("EsfNeto = {0}KN/m2 \n",EsfuerzoNeto);
				Console.WriteLine("El Area minima de la zapata es: A = P1/EsfuerzoNeto");
				Console.WriteLine("A = {0}/{1}",P1,EsfuerzoNeto);
				Console.WriteLine("A = {0}m2 \n",areafinal);
				Console.WriteLine("El lado L de la zapata es: L = A/1m");
				Console.WriteLine("L = {0}/1m",areafinal);
				Console.WriteLine("L = {0}m \n",L);
			
			//voy aqui	
				Console.WriteLine("\t\t\t Chequeo de Rigidez. \n");
				Console.WriteLine("Ingrese ancho del muro en m:");
					double.TryParse(Console.ReadLine(),out anchomuro);
					lvoladizo=checked((L-anchomuro)/2);
					rel=checked(lvoladizo/H);
				Console.WriteLine("Longitud del voladizo sentido perpendicular al muro: lv = (L-anchomuro)/2");
				Console.WriteLine("lv = ({0}-{1})/2",L,anchomuro);
				Console.WriteLine("lv = {0}m \n",lvoladizo);
				Console.WriteLine("Relación entre el voladizo y el espesor H: relacion = lv/H");
				Console.WriteLine("lv/H = {0}/{1}",lvoladizo,H);
				Console.WriteLine("lv/H = {0}m \n",rel);
					 if (rel<=3&&rel>0)
						{
							Console.WriteLine("=> Cimiento rigido en condición Aceptable \n");
						}
						
						if (rel<=0)
						{
							Console.WriteLine("!!!OJO relacion lv/H es numero negativo");
						}
						
						if (rel>3)
						{
							Console.WriteLine("!!Cuidado no cumple Rigidez!! Se Volverá a calcular el espesor H");
							Console.WriteLine("Tambien se calculará el esfuerzo neto, el area y la dimension L \n");
							H=checked(Math.Round(lvoladizo/2.8,2,MidpointRounding.AwayFromZero));
							d=checked(H-r);
							Sobrecarga=checked(rsuelo*(df-H)+rconcreto*H);
							EsfuerzoNeto=checked(EsfuerzoAdmisible-Sobrecarga);
							areafinal=checked(P1/EsfuerzoNeto);					
							Console.WriteLine("H final = lvoladizo/2.8");
							Console.WriteLine("H final = {0}/2.8",lvoladizo);
							Console.WriteLine("H final para rigidez = {0}m \n",H);
							Console.WriteLine("d final = H nuevo-r");
							Console.WriteLine("d final = {0}-{1}",H,r);
							Console.WriteLine("d final para rigidez = {0}m \n",d);
							Console.WriteLine("La sobrecarga es: qw = rsuelo*(df-H)+rconcreto*H");
							Console.WriteLine("qw = {0}*({1}-{2})+{3}*{4}",rsuelo,df,H,rconcreto,H);
							Console.WriteLine("qw = {0}KN/m2 \n",Sobrecarga);
							Console.WriteLine("El Esfuerzo neto es: EsfNeto = EsfuerzoAdmisible-Sobrecarga");
							Console.WriteLine("EsfNeto = {0}-{1}",EsfuerzoAdmisible,Sobrecarga);
							Console.WriteLine("EsfNeto = {0}KN/m2 \n",EsfuerzoNeto);
							Console.WriteLine("El Area minima de la zapata es: A = P1/EsfuerzoNeto");
							Console.WriteLine("A = {0}/{1}",P1,EsfuerzoNeto);
							Console.WriteLine("A = {0}m2 \n",areafinal);
							Console.WriteLine("El lado L de la zapata es: L = A/1m");
							Console.WriteLine("L = {0}/1m",areafinal);
							Console.WriteLine("L = {0}m \n",L);
						}
				
				
				Console.WriteLine("\t\t\t Presion de contacto neta mayorada. \n");
					Esfuerzo1u=checked(P1u/areafinal);
				Console.WriteLine("El Esfuerzo (1u) = P1u/areafinal");
				Console.WriteLine("Esf(1u) = {0}/{1}",P1u,areafinal);
				Console.WriteLine("Esf(1u) = {0}KN/m2 \n",Esfuerzo1u);
				
				Console.WriteLine("\t\t\t Diseño a solicitaciones cortantes. \n");
				
				Console.WriteLine("\t\t\t Acción como viga. \n");
					Atributaria=checked(((L-anchomuro)/2)-d);
					V1u=checked(Esfuerzo1u*Atributaria);
					fiVc=checked(ficortante*0.17*landa*Math.Pow(fc, 0.5)*1*d*1000);
				Console.WriteLine("El Area tributaria es: Atrib = ((L-anchomuro)/2)-d");
				Console.WriteLine("Atrib = (({0}-{1})/2)-{2}",L,anchomuro,d);
				Console.WriteLine("Atrib = {0}m2 \n",Atributaria);
				Console.WriteLine("El Cortante actuante es: V1u = Esfuerzo1u*Atributaria");
				Console.WriteLine("V1u = {0}*{1}",Esfuerzo1u,Atributaria);
				Console.WriteLine("V1u = {0}KN \n",V1u);
				Console.WriteLine("El Cortante resistente es: fiVc = ficortante*0.17*landa*raiz(fc)*1*d*1000");
				Console.WriteLine("fiVc = {0}*0.17*{1}*raiz({2})*1*{3}*1000",ficortante,landa,fc,d);
				Console.WriteLine("fiVc = {0}KN \n",fiVc);
					if (fiVc>=V1u)
						{
							Console.WriteLine("fiVc > V1u \n{0} > {1}",fiVc,V1u);
							Console.WriteLine("=>Con el peralte d={0}m el concreto resiste los esfuerzos cortantes \n",d);
						}
						
					if (fiVc<V1u)
						{
							Console.WriteLine("fiVc < V1u \n{0} < {1}",fiVc,V1u);
							Console.WriteLine("!!El concreto no resiste las solicitaciones cortantes!!");
							Console.WriteLine("Se calcula un peralte d que cumpla, igualando las ecuaciones de esfuerzo cortante actuante y resistente, despejando a d.");
							d=checked((Esfuerzo1u*(L-anchomuro)/2)/(ficortante*0.17*landa*Math.Pow(fc, 0.5)*1*1000+Esfuerzo1u));
							H=checked(Math.Round(d+r,2,MidpointRounding.AwayFromZero));
							d=H-r;
							Console.WriteLine("d final = (Esfuerzo1u*(L-anchomuro)/2)/(ficortante*0.17*landa*Math.Pow(fc, 0.5)*1*1000+Esfuerzo1u)");
							Console.WriteLine("d final = ({0}*({1}-{2})/2)/({3}*0.17*{4}*raiz({5})*1*1000+{6})",Esfuerzo1u,L,anchomuro,ficortante,landa,fc,Esfuerzo1u);
							Console.WriteLine("d final para cortante = {0}m \n",d);
							Console.WriteLine("H final = d+r");
							Console.WriteLine("H final = {0}+{1}",d,r);
							Console.WriteLine("H final para cortante = {0}m \n",H);
						}
						
				
				Console.WriteLine("\t\t\t Diseño a flexion. \n");
				
				Console.WriteLine("\t\t\t Refuerzo paralelo a B. \n");
					Asbz=checked(3.1416/4*Math.Pow(dbz/100,2));
				Console.WriteLine("El area de la barra de refuerzo es: Asbz = 3.1416*(dbz/100)^2)/4;");
				Console.WriteLine("Asbz = 3.1416*({0}/100)^2)/4;",dbz);
				Console.WriteLine("Asbz = {0} m2 \n",Asbz);
					AstB=checked(0.0018*L*H);
				Console.WriteLine("El area por retraccion y temperatura es: Ast = 0.0018*L*H");
				Console.WriteLine("Ast = 0.0018*{0}*{1}",L,H);
				Console.WriteLine("Ast = {0} m2 \n",AstB);
					NbB=checked(Math.Ceiling(AstB/Asbz));
				Console.WriteLine("El numero de barras requeridas es: Nb = AstB/Asbz");
				Console.WriteLine("Nb = {0}/{1}",AstB,Asbz);
				Console.WriteLine("Nb = {0} \n",NbB);
					SccB=checked((L-2*rprima-dbz/100)/(NbB-1));
				Console.WriteLine("Separacion centro a centro entre las barras: Scc = (L-2*rprima-dbz/100)/(NbB-1)");
				Console.WriteLine("Scc = ({0}-2*{1}-{2}/100)/({3}-1)",L,rprima,dbz,NbB);
				Console.WriteLine("Scc = {0} m \n",SccB);	
					SlbB=checked(SccB-dbz/100);
				Console.WriteLine("Separacion libre entre las barras: Slb = SccB-dbz/100");
				Console.WriteLine("Slb = {0}-{1}/100",SccB,dbz);
				Console.WriteLine("Slb = {0} m \n",SlbB);
				Sminb=checked(maximo2(dbz/100, 1.33*TM/100));
					if (SlbB<Sminb)
						{
							Console.WriteLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a B, EMPIECE DENUEVO AUMENTANDO EL DIAMETRO DE LA BARRA");
							
						}
						
					if (SlbB>0.45)
						{
							Console.WriteLine("OJO!!! Separación da mas que la maxima, entonces se toma la maxima que es: 0,45m");
							SlbB=0.45;
							//Recalculamos el numero de barras
							NbB=checked(Math.Ceiling(((L-2*rprima-dbz/100)/SlbB)+1));
							Console.WriteLine("Recalculamos el numero de barras: Nb = ((L-2*rprima-dbz/100)/SlbB)+1");
							Console.WriteLine("Nb = (({0}-2*{1}-{2}/100)/{3})+1",L,rprima,dbz,SlbB);
							Console.WriteLine("Nb = {0} \n",NbB);
						}
				
					AstBsum=checked(NbB*Asbz);
				Console.WriteLine("El area suministrada es: AstBsum = NbB*Asbz");
				Console.WriteLine("AstBsum = {0}*{1}",NbB,Asbz);
				Console.WriteLine("AstBsum = {0} m2 \n",AstBsum);
					cuantiaBsum=checked(AstBsum/(L*H));
				Console.WriteLine("La cuantia suministrada es: cuantiaBsum = AstBsum/(L*H)");
				Console.WriteLine("cuantiaBsum = {0}/({1}*{2})",AstBsum,L,H);
				Console.WriteLine("cuantiaBsum = {0} \n",cuantiaBsum);
				//longitud de corte de las barras paralelas a B
						double lcorteB;
						lcorteB=1;
				Console.WriteLine("La longitud de corte de las barras es: {0} m \n",lcorteB);
				
				Console.WriteLine("\t\t\t Refuerzo paralelo a L (Principal). \n");
					M1u=checked(Esfuerzo1u*1*Math.Pow((L-anchomuro)/2,2)/2);
				Console.WriteLine("El Momento actuante es: Mu = Esfuerzo1u*1*((L-anchomuro)/2)^2/2");
				Console.WriteLine("Mu = {0}*1*(({1}-{2})/2)^2/2",Esfuerzo1u,L,anchomuro);
				Console.WriteLine("Mu = {0}KN*m \n",M1u);
					refuerzo=checked(cuantiad(fc,fy,M1u,1,d));
				Console.WriteLine("Cuantia requerida: cud = 0.85*fc/fy*(1-raiz(1-(2*M1u/(0.85*0.9*L*d^2*1000))))");
				Console.WriteLine("cud = 0.85*{0}/{1}*(1-raiz(1-(2*{2}/(0.85*0.9*{3}*{4}^2*1000))))",fc,fy,M1u,L,d);
				Console.WriteLine("cud = {0}\n",refuerzo);
					AsrL=checked(refuerzo*1*d);
				Console.WriteLine("El area minima requerida es: Asr = cud*B*d");
				Console.WriteLine("Asr = {0}*1*{1}",refuerzo,d);
				Console.WriteLine("Asr = {0} m2 \n",AsrL);
					AstL=checked(0.0018*1*H);
				Console.WriteLine("El area minima por retraccion y temperatura es: Ast = 0.0018*B*H");
				Console.WriteLine("Ast = 0.0018*1*{0}",H);
				Console.WriteLine("Ast = {0} m2 \n",AstL);
					AsfL=checked(0.0033*1*d);
				Console.WriteLine("El area minima por flexion es: Asf = 0.0033*B*d");
				Console.WriteLine("Asf = 0.0033*1*{0}",d);
				Console.WriteLine("Asf = {0} m2 \n",AsfL);
					AssL=checked(maximo2(maximo2(AsrL,AstL),AsfL));
				Console.WriteLine("Area de acero a suministrar: AssL = max[AsrL,AstL,AsfL]");
				Console.WriteLine("AssL = max[{0},{1},{2}]",AsrL,AstL,AsfL);
				Console.WriteLine("AssL = {0} m2 \n",AssL);
					NbL=checked(Math.Ceiling(AssL/Asbz));
				Console.WriteLine("El numero de barras requeridas es: Nb = AssL/Asbz");
				Console.WriteLine("Nb = {0}/{1}",AssL,Asbz);
				Console.WriteLine("Nb = {0} \n",NbL);
					SccL=checked((1-2*rprima-dbz/100)/(NbL-1));
				Console.WriteLine("Separacion centro a centro entre las barras: Scc = (B-2*rprima-dbz/100)/(NbB-1)");
				Console.WriteLine("Scc = (1-2*{0}-{1}/100)/({2}-1)",rprima,dbz,NbL);
				Console.WriteLine("Scc = {0} m \n",SccL);
					SlbL=checked(SccL-dbz/100);
				Console.WriteLine("Separacion libre entre las barras: Slb = SccL-dbz/100");
				Console.WriteLine("Slb = {0}-{1}/100",SccL,dbz);
				Console.WriteLine("Slb = {0} m \n",SlbL);
					if (SlbL<Sminb)
						{
							Console.WriteLine("OJO!!! NO CUMPLE SEPARACION MINIMA paralelo a L, EMPIECE DENUEVO AUMENTANDO EL DIAMETRO DE LA BARRA");
						}
						
						
					if (SlbL>0.45)
						{
							Console.WriteLine("OJO!!! Separación da mas que la maxima, entonces se toma la maxima que es: 0,45m");
							//cambiar aqui en la zapata aislada que pregunte si desea cambiar el diametro o tomar la sep=0,45
							SlbL=0.45;
							//Recalculamos el numero de barras
							NbL=checked(Math.Ceiling(((1-2*rprima-dbz/100)/SlbL)+1));
							Console.WriteLine("Recalculamos el numero de barras: Nb = ((1-2*rprima-dbz/100)/SlbL)+1");
							Console.WriteLine("Nb = ((1-2*{0}-{1}/100)/{2})+1",rprima,dbz,SlbL);
							Console.WriteLine("Nb = {0} \n",NbL);
						}
				
					AssrL=checked(NbL*Asbz);
				Console.WriteLine("El area suministrada es: AssrL = NbL*Asbz");
				Console.WriteLine("AssrL = {0}*{1}",NbL,Asbz);
				Console.WriteLine("AssrL = {0} m2 \n",AssrL);
					cuantiaLsum=checked(AssrL/(1*H));
				Console.WriteLine("La cuantia suministrada es: cuantiaLsum = AssrL/(B*H)");
				Console.WriteLine("cuantiaLsum = {0}/(1*{1})",AssrL,H);
				Console.WriteLine("cuantiaLsum = {0} \n",cuantiaLsum);
					double ldisponible, lreqtraccion, c;
					ldisponible=(L-anchomuro)/2-rprima;
				Console.WriteLine("Longitud Disponible para desarrollo: ldisp = (L-anchomuro)/2-rprima");
				Console.WriteLine("ldisp = ({0}-{1})/2-{2}",L,anchomuro,rprima);
				Console.WriteLine("ldisp = {0} m \n",ldisponible);
					lreqtraccion=longitudDesarrolloBarra(fc,fy,dbz,rprima,SccL);
					c=Math.Min(rprima+dbz/(100*2),SccL/2);
					if (lreqtraccion>ldisponible)
					{
						Console.WriteLine("OJO!! Se requiere mas espacio para desarrollar"+
						" las barras de refuerzo, pruebe cambiando el diametro de la barra"+
						" por un diametro menor o coloque gancho"); 
					}
					
				Console.WriteLine("Longitud de desarrollo requerida por la barra a traccion:");
				Console.WriteLine("c = Min[rprima+dbz/(100*2);SccL/2]");
				Console.WriteLine("c = Min[{0}+{1}/(100*2);{2}/2]",rprima,dbz,SccL);
				Console.WriteLine("c = {0} m \n",c);	
				Console.WriteLine("ldbt = MAX[fy*T*dbz/(1.1*landa*raiz(f'c)*(c*100/dbz));0.3]");
				Console.WriteLine("ldbt = MAX[{0}*0.8*{1}/(1.1*1*raiz({2})*({3}*100/{4}));0.3]",fy,dbz,fc,c,dbz);
				Console.WriteLine("ldbt = {0} m \n",lreqtraccion);
				Console.WriteLine("=>Longitud disponible > Longitud requerida");
				//longitud de corte de las barras
						double lcorteL;
						lcorteL=Math.Round(L-2*rprima,2,MidpointRounding.AwayFromZero);
				Console.WriteLine("La longitud de corte de las barras es: LC = L-2*rprima");
				Console.WriteLine("LC = {0}-2*{1}",L,rprima);
				Console.WriteLine("LC = {0} m \n",lcorteL);		
				
				///////////////////////////////////////////////////////////////////////////7
				//Cantidades de obra
					double vconcreto, vexcavacion, acerototal;
					vconcreto=H*1*L;
				Console.WriteLine("El volumen de concreto: VC = H*B*L");
				Console.WriteLine("VC = {0}*1*{1}",H,L);
				Console.WriteLine("VC = {0} m3 \n",vconcreto);
					vexcavacion=df*1*L;
				Console.WriteLine("El volumen de excavación: VE = Df*B*L");
				Console.WriteLine("VE = {0}*1*{1}",df,L);
				Console.WriteLine("VE = {0} m3 \n",vexcavacion);
				//Aqui falta ponerle la densidad lineal correcta para cada barra
					acerototal=NbB*1.8*lcorteB+NbL*1.8*lcorteL;
				Console.WriteLine("El acero total en Kg es: AR = NbB*densb*lcorteB+NbL*densb*lcorteL");
				Console.WriteLine("AR = {0}*1.6*{1}+{2}*2.2*{3}",NbB,lcorteB,NbL,lcorteL);
				Console.WriteLine("AR = {0} Kg \n",acerototal);
				//presupuesto
					double costo=0;
					costo=Math.Round(presupuesto(vconcreto, vexcavacion, acerototal),2,MidpointRounding.AwayFromZero);
				Console.WriteLine("Costo Directo Aproximado del cimiento por metro lineal de muro: ${0} COP",costo);
				Console.WriteLine("No incluye: \nSolado \nAcero del muro \nConcreto del muro \nRelleno del material");
				Console.WriteLine("Precios del semestre 1 del año 2015");
				
				
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
			
		static double cuantiad(double fconcreto, double facero, double Momento, double anchobw, double peralted)
			{
				double cuantiaacero, fiM=0.9;
				
				cuantiaacero=checked(0.85*fconcreto/facero*(1-(Math.Pow(1-(2*Momento/(0.85*fiM*fconcreto*anchobw*peralted*peralted*1000)), 0.5))));
				
				return cuantiaacero;
			}
			
		static double longitudDesarrolloBarra(double fconcreto, double facero, double diametroBarracm, double recubrimientoprima, double separacioncentrocentro)
			{
				double trincheT=1, trincheE=1, trincheS=1;
				double ldbb, ct, relacionctsobredbz;
				ct=Math.Min(recubrimientoprima+diametroBarracm/(100*2),separacioncentrocentro/2);
				relacionctsobredbz=ct/(diametroBarracm/100);
				if (relacionctsobredbz>2.5)
				{
					relacionctsobredbz=2.5;
				}
				
				if (diametroBarracm<1.91)
				{
					trincheS=0.8;
				}
				
				ldbb=Math.Max((facero*trincheT*trincheE*trincheS*diametroBarracm)/(100*1.1*1*Math.Pow(fconcreto,0.5)*relacionctsobredbz),0.3);
				return ldbb;
			}
		
	}
}
