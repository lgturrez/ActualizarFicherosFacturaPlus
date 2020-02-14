# ActualizarFicherosFacturaPlus
Fix for dbf files with date 1920 set to 2020<br>
<br>
Actualiza las fechas de los ficheros de FacturaPlus si están entre 1920 y 1930.<br>
<br>
En el fichero zip está el ejecutable y el fichero de configuración
<br>
Propiedades a configurar en el fichero:<br>
&emsp;&emsp;carpetaInicial: Carpeta donde están los ficheros de contabilidad.<br>
&emsp;&emsp;&emsp;&emsp;C:\GrupoSP\FAE08R05\DBF20\ (DBFX, DONDE “X” ES EL NUMERO DE EMPRESA AFECTADO, por ejemplo 20)<br>
Por defecto, va a modificar los ficheros utilizando como clave el número de factura o el número de remesa para el fichero de remesas. Si necesitas que modifique algún fichero que tiene otra clave, deberás introducir una entrada indicando el nombre del fichero y su clave:<br>
&emsp;&emsp;<add key="Remesas.dbf_PK" value="NNUMREM"/><br>
<br>    
El botón Simular no modifica los ficheros, solo recorre las tablas e identifica las filas que se cambiarían.<br>
<br>
Pasos a seguir:<br>
  1.- Organizar ficheros en Facturaplus<br>
  2.- Crear una copia de seguridad de FacturaPlus y Contaplus<br>
  3.- Cerrar FacturaPlus y Contaplus<br>
  4.- Ejecutar ActualizarFicherosFacturaPlus.exe<br>
  5.- Comprobar que la ruta es la adecuada para la empresa que queremos actualizar. Si no lo es, pulsar en el icono ... y elegir la ruta<br>
  6.- Realizar una simulación :)<br>
  7.- Actualizar<br>
  8.- Abrir FacturaPlus y hacer el traspaso a Contaplus<br>
&emsp;&emsp;&emsp;&emsp;Importante: Si cambiamos la fecha de fin a mano no nos cogía los asientos, sospechamos que se produce el mismo efecto con el 1920<br>
  9.- Comprobar que todo está correcto.<br>
<br>
